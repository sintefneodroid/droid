using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using droid.Runtime.Enums;
using droid.Runtime.GameObjects;
using droid.Runtime.GameObjects.BoundingBoxes;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Prototyping.ObjectiveFunctions;
using droid.Runtime.Utilities;
using droid.Runtime.Utilities.Extensions;
using UnityEngine;
using Object = System.Object;

namespace droid.Runtime.Environments.Prototyping {
  /// <inheritdoc cref="PrototypingGameObject" />
  /// <summary>
  /// </summary>
  [RequireComponent(typeof(Transform))]
  public abstract class AbstractSpatialPrototypingEnvironment : NeodroidEnvironment,
                                                                ISpatialPrototypingEnvironment {
    #region Fields

    #endregion

    #region ProtectedMembers

    /// <summary>
    /// </summary>
    protected Object _Reaction_Lock = new Object();

    /// <summary>
    ///
    /// </summary>
    protected WaitForFixedUpdate _Wait_For_Fixed_Update = new WaitForFixedUpdate();

    /// <summary>
    ///
    /// </summary>
    protected List<float> _Observables = new List<float>();

    /// <summary>
    /// </summary>
    protected Rigidbody[] _Tracked_Rigid_Bodies;

    /// <summary>
    /// </summary>
    protected Transform[] _Poses;

    #endregion

    #region PrivateMembers

    /// <summary>
    /// </summary>
    Vector3[] _reset_positions;

    /// <summary>
    /// </summary>
    Quaternion[] _reset_rotations;

    Vector3[] _reset_scales;

    /// <summary>
    /// </summary>
    GameObject[] _tracked_game_objects;

    /// <summary>
    /// </summary>
    Vector3[] _reset_velocities;

    /// <summary>
    /// </summary>
    Vector3[] _reset_angulars;

    /// <summary>
    /// </summary>
    Pose[] _received_poses;

    /// <summary>
    /// </summary>
    Body[] _received_bodies;

    /// <summary>
    /// </summary>
    Configuration[] _last_configuration;

    /// <summary>
    /// </summary>
    Animation[] _animations;

    /// <summary>
    /// </summary>
    float[] _reset_animation_times;

    #endregion

    #region Events

    /// <summary>
    /// </summary>
    public event Action PreStepEvent;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public event Action StepEvent;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public event Action PostStepEvent;

    #endregion

    #region Private Methods

    /// <summary>
    ///   Termination of an episode, can be supplied with a reason for various purposes debugging or clarification
    ///   for a learner.
    /// </summary>
    /// <param name="reason"></param>
    public void Terminate(string reason = "None") {
      lock (this._Reaction_Lock) {
        if (this.Terminable) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.LogWarning($"Environment {this.Identifier} as terminated because {reason}");
          }
          #endif

          this.Terminated = true;
          this.LastTerminationReason = reason;
        }
      }
    }

    /// <summary>
    /// </summary>
    public override void Tick() {
      if (this.IsResetting) {

        this.PrototypingReset();

        this.LoopConfigurables();
        this.LoopSensors();

        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("Reset");
        }
        #endif

        this.IsResetting = false;
      } else {
        this.LoopConfigurables(this.UpdateObservationsWithEveryTick);
        this.LoopSensors(this.UpdateObservationsWithEveryTick);
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Tick");
      }
      #endif

      this.LoopListeners();
    }

    /// <summary>
    /// </summary>
    protected void LoopSensors(bool update = true) {
      foreach (var s in this.Sensors.Values) {
        s?.Tick();
        if (update) {
          s?.UpdateObservation();
        }
      }
    }

    protected void LoopListeners() {
      foreach (var obs in this.Listeners.Values) {
        obs?.Tick();
      }
    }

    /// <summary>
    /// </summary>
    protected void LoopConfigurables(bool update = true) {
      foreach (var con in this.Configurables.Values) {
        con?.Tick();
        if (update) {
          con?.UpdateCurrentConfiguration();
        }
      }
    }

    /// <summary>
    /// </summary>
    void SaveInitialPoses() {
      var ignored_layer = LayerMask.NameToLayer($"IgnoredByNeodroid");
      if (this.TrackOnlyChildren) {
        this._tracked_game_objects =
            NeodroidSceneUtilities.RecursiveChildGameObjectsExceptLayer(this.transform, ignored_layer);
      } else {
        this._tracked_game_objects = NeodroidSceneUtilities.FindAllGameObjectsExceptLayer(ignored_layer);
      }

      var length = this._tracked_game_objects.Length;

      this._reset_positions = new Vector3[length];
      this._reset_rotations = new Quaternion[length];
      this._reset_scales = new Vector3[length];
      this._Poses = new Transform[length];
      for (var i = 0; i < length; i++) {
        var go = this._tracked_game_objects[i];
        var trans = go.transform;
        this._reset_positions[i] = trans.position;
        this._reset_rotations[i] = trans.rotation;
        this._reset_scales[i] = trans.localScale;
        this._Poses[i] = trans;
        var maybe_joint = go.GetComponent<Joint>();
        if (maybe_joint != null) {
          var maybe_joint_fix = maybe_joint.GetComponent<JointFix>();
          if (maybe_joint_fix == null) {
            // ReSharper disable once RedundantAssignment
            maybe_joint_fix = maybe_joint.gameObject.AddComponent<JointFix>();
          }
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log($"Added a JointFix component to {maybe_joint_fix.name}");
          }
          #endif
        }
      }
    }

    /// <summary>
    /// </summary>
    void SaveInitialBodies() {
      /*var body_list = new List<Rigidbody>();
      foreach (var go in this._tracked_game_objects) {
        if (go != null) {
          var body = go.GetComponent<Rigidbody>();
          if (body)
            body_list.Add(body);
        }
      }
            this._bodies = body_list.ToArray();
      */ //Should be equivalent to the line below, but kept as a reference in case of confusion

      this._Tracked_Rigid_Bodies = this._tracked_game_objects.Where(go => go != null)
                                       .Select(go => go.GetComponent<Rigidbody>()).Where(body => body)
                                       .ToArray();

      this._reset_velocities = new Vector3[this._Tracked_Rigid_Bodies.Length];
      this._reset_angulars = new Vector3[this._Tracked_Rigid_Bodies.Length];
      for (var i = 0; i < this._Tracked_Rigid_Bodies.Length; i++) {
        this._reset_velocities[i] = this._Tracked_Rigid_Bodies[i].velocity;
        this._reset_angulars[i] = this._Tracked_Rigid_Bodies[i].angularVelocity;
      }
    }

    /// <summary>
    /// </summary>
    void SaveInitialAnimations() {
      this._animations = this._tracked_game_objects.Where(go => go != null)
                             .Select(go => go.GetComponent<Animation>()).Where(anim => anim).ToArray();
      this._reset_animation_times = new float[this._animations.Length];
      for (var i = 0; i < this._animations.Length; i++) {
        if (this._animations[i]) {
          if (this._animations[i].clip) {
            this._reset_animation_times[i] =
                this._animations[i].CrossFadeQueued(this._animations[i].clip.name)
                    .time; //TODO: IS NOT USED AS RIGHT NOW and should use animations clips instead the legacy "clip.name".
            //TODO: DOES NOT WORK
          }
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      if (this.ObjectiveFunction == null) {
        this.ObjectiveFunction = this.GetComponent<EpisodicObjective>();
      }

      if (!this.PlayableArea) {
        this.PlayableArea = this.GetComponent<BoundingBox>();
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("PreSetup");
      }
      #endif

      if (this._tracked_game_objects == null || this._tracked_game_objects.Length == 0) {
        this.SaveInitialPoses();
        this.SaveInitialAnimations();
        this.StartCoroutine(this.SaveInitialBodiesIe());
      }
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    IEnumerator SaveInitialBodiesIe() {
      yield return this._Wait_For_Fixed_Update;
      this.SaveInitialBodies();
      this.RemotePostSetup();
    }

    /// <summary>
    /// </summary>
    /// <param name="reaction"></param>
    void Step(Reaction reaction) {
      lock (this._Reaction_Lock) {
        this.PreStepEvent?.Invoke();

        if (reaction.Parameters.EpisodeCount) {
          this.StepI++;
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.LogWarning("Step did count");
          }
          #endif
        } else {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.LogWarning("Step did not count");
          }
          #endif
        }

        this.SendToActors(reaction);

        this.StepEvent?.Invoke();

        this.LoopSensors();
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="reaction"></param>
    protected abstract void SendToActors(Reaction reaction);

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public override void React(Reaction reaction) {
      this.LastReaction = reaction;
      this.Terminable = reaction.Parameters.Terminable;

      this._last_configuration = reaction.Configurations;

      this.ReplyWithDescriptionThisStep = reaction.Parameters.Describe;

      if (reaction.Parameters.Configure && reaction.Unobservables != null) {
        this._received_poses = reaction.Unobservables.Poses;
        this._received_bodies = reaction.Unobservables.Bodies;
      }

      this.SendToDisplayers(reaction);

      if (reaction.Parameters.StepResetObserveEnu == StepResetObserve.Reset_) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Resetting environment({this.Identifier})");
        }
        #endif
        this.Terminate($"Reaction caused a reset");
        this.IsResetting = true;
      } else if (reaction.Parameters.StepResetObserveEnu == StepResetObserve.Step_) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Stepping in environment({this.Identifier})");
        }
        #endif
        this.Step(reaction);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PostStep() {
      this.PostStepEvent?.Invoke();
      if (this.Configure) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("Configuring");
        }
        #endif
        this.Configure = false;
        this.Reconfigure();
      }

      this.ReplyWithDescriptionThisStep = false;
    }

    /// <summary>
    /// </summary>
    public override void PrototypingReset() {
      this.LastResetTime = Time.realtimeSinceStartup;
      this.StepI = 0;
      this.ObjectiveFunction?.PrototypingReset();

      SetEnvironmentTransforms(ref this._tracked_game_objects,
                               ref this._reset_positions,
                               ref this._reset_rotations,
                               ref this._reset_scales);

      this.SetBodies(ref this._Tracked_Rigid_Bodies, ref this._reset_velocities, ref this._reset_angulars);

      this.ResetRegisteredObjects();
      this.Reconfigure();

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"Reset called on environment {this.Identifier}");
      }
      #endif

      this.Terminated = false;
    }

    /// <summary>
    /// </summary>
    protected void ResetRegisteredObjects() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Resetting registered objects");
      }
      #endif

      foreach (var configurable in this.Configurables.Values) {
        configurable?.PrototypingReset();
      }

      foreach (var resetable in this.Listeners.Values) {
        resetable?.PrototypingReset();
      }

      this.InnerResetRegisteredObjects();

      foreach (var sensor in this.Sensors.Values) {
        sensor?.PrototypingReset();
      }
    }

    /// <summary>
    /// </summary>
    protected void Reconfigure() {
      if (this._received_poses != null) {
        var positions = new Vector3[this._received_poses.Length];
        var rotations = new Quaternion[this._received_poses.Length];
        var scales = new Vector3[this._received_poses.Length];
        for (var i = 0; i < this._received_poses.Length; i++) {
          positions[i] = this._received_poses[i].position;
          rotations[i] = this._received_poses[i].rotation;
          scales[i] = this._reset_scales[i]; //TODO: this._received_poses[i].scale;
        }

        SetEnvironmentTransforms(ref this._tracked_game_objects,
                                 ref positions,
                                 ref rotations,
                                 ref scales);
      }

      if (this._received_bodies != null) {
        var velocities = new Vector3[this._received_bodies.Length];
        var angulars = new Vector3[this._received_bodies.Length];
        for (var i = 0; i < this._received_bodies.Length; i++) {
          velocities[i] = this._received_bodies[i].Velocity;
          angulars[i] = this._received_bodies[i].AngularVelocity;
        }

        this.SetBodies(ref this._Tracked_Rigid_Bodies, ref velocities, ref angulars);
      }

      if (this._last_configuration != null) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Configuration length: {this._last_configuration.Length}");
        }
        #endif
        foreach (var configuration in this._last_configuration) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log("Configuring configurable with the specified name: " + configuration.ConfigurableName);
          }
          #endif
          if (this.Configurables.ContainsKey(configuration.ConfigurableName)) {
            this.Configurables[configuration.ConfigurableName].ApplyConfiguration(configuration);
          } else {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log($"Could find not configurable with the specified name: {configuration.ConfigurableName}");
            }
            #endif
          }
        }
      } else {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("Has no received_configurations");
        }
        #endif
      }

      this.LoopConfigurables();
      this.LoopSensors();
    }

    /// <summary>
    /// </summary>
    /// <param name="reaction"></param>
    void SendToDisplayers(Reaction reaction) {
      if (reaction.Displayables != null && reaction.Displayables.Length > 0) {
        foreach (var displayable in reaction.Displayables) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log("Applying " + displayable + " To " + this.name + "'s displayers");
          }
          #endif
          var displayable_name = displayable.DisplayableName;
          if (this.Displayers.ContainsKey(displayable_name) && this.Displayers[displayable_name] != null) {
            var v = displayable.DisplayableValue;
            this.Displayers[displayable_name].Display(v);
          } else {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log("Could find not displayer with the specified name: " + displayable_name);
            }
            #endif
          }
        }
      }
    }

    #endregion

    #region EnvironmentStateSetters

    /// <summary>
    /// </summary>
    /// <param name="child_game_objects"></param>
    /// <param name="positions"></param>
    /// <param name="rotations"></param>
    static void SetEnvironmentTransforms(ref GameObject[] child_game_objects,
                                         ref Vector3[] positions,
                                         ref Quaternion[] rotations,
                                         ref Vector3[] scales) {
      for (var i = 0; i < child_game_objects.Length; i++) {
        if (child_game_objects[i] != null && i < positions.Length && i < rotations.Length) {
          var rigid_body = child_game_objects[i].GetComponent<Rigidbody>();
          if (rigid_body) {
            rigid_body.Sleep();
          }

          child_game_objects[i].transform.position = positions[i];
          child_game_objects[i].transform.rotation = rotations[i];
          child_game_objects[i].transform.localScale = scales[i];
          if (rigid_body) {
            rigid_body.WakeUp();
          }

          var joint_fix = child_game_objects[i].GetComponent<JointFix>();
          if (joint_fix) {
            joint_fix.JointReset();
          }

          var anim = child_game_objects[i].GetComponent<Animation>();
          if (anim) {
            anim.Rewind();
            anim.Play();
            anim.Sample();
            anim.Stop();
          }
        }
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="bodies"></param>
    /// <param name="velocities"></param>
    /// <param name="angulars"></param>
    /// <param name="debugging"></param>
    void SetBodies(ref Rigidbody[] bodies, ref Vector3[] velocities, ref Vector3[] angulars) {
      if (bodies != null && bodies.Length > 0) {
        for (var i = 0; i < bodies.Length; i++) {
          if (i < bodies.Length && bodies[i] != null && i < velocities.Length && i < angulars.Length) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log($"Setting {bodies[i].name}, velocity to {velocities[i]} and angular velocity to {angulars[i]}");
            }

            #endif

            bodies[i].Sleep();
            bodies[i].velocity = velocities[i];
            bodies[i].angularVelocity = angulars[i];
            bodies[i].WakeUp();
          }
        }
      }
    }

    #endregion

    #region Transformations

    /// <summary>
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public Vector3 TransformPoint(Vector3 point) {
      switch (this.CoordinateSpace) {
        case CoordinateSpace.Environment_ when this.CoordinateReferencePoint:
          return this.CoordinateReferencePoint.transform.InverseTransformPoint(point);
        case CoordinateSpace.Local_:
          return this.transform.InverseTransformPoint(point);
        //return point - this.transform.position;
        default:
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log("Defaulting TransformPoint");
          }
          #endif
          return point;
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="point"></param>
    public void TransformPoint(ref Vector3 point) {
      switch (this.CoordinateSpace) {
        case CoordinateSpace.Environment_ when this.CoordinateReferencePoint:
          point = this.CoordinateReferencePoint.transform.InverseTransformPoint(point);
          break;
        case CoordinateSpace.Local_:
          //point = point - this.transform.position;
          point = this.transform.InverseTransformPoint(point);
          break;
        default:
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log("Defaulting TransformPoint");
          }
          #endif
          break;
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public Vector3 InverseTransformPoint(Vector3 point) {
      switch (this.CoordinateSpace) {
        case CoordinateSpace.Environment_ when this.CoordinateReferencePoint:
          return this.CoordinateReferencePoint.transform.TransformPoint(point);
        case CoordinateSpace.Local_:
          //return point - this.transform.position;
          return this.transform.TransformPoint(point);
        default:
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log("Defaulting InverseTransformPoint");
          }
          #endif
          return point;
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="point"></param>
    public void InverseTransformPoint(ref Vector3 point) {
      switch (this.CoordinateSpace) {
        case CoordinateSpace.Environment_ when this.CoordinateReferencePoint:
          point = this.CoordinateReferencePoint.transform.TransformPoint(point);
          break;
        case CoordinateSpace.Local_:
          //point = point - this.transform.position;
          point = this.transform.TransformPoint(point);
          break;
        default:
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log("Defaulting InverseTransformPoint");
          }
          #endif
          break;
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public Vector3 TransformDirection(Vector3 direction) {
      switch (this.CoordinateSpace) {
        case CoordinateSpace.Environment_ when this.CoordinateReferencePoint:
          return this.CoordinateReferencePoint.transform.InverseTransformDirection(direction);
        case CoordinateSpace.Local_:
          return this.transform.InverseTransformDirection(direction);
        default:
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log("Defaulting TransformDirection");
          }
          #endif
          return direction;
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="direction"></param>
    public void TransformDirection(ref Vector3 direction) {
      switch (this.CoordinateSpace) {
        case CoordinateSpace.Environment_ when this.CoordinateReferencePoint:
          direction = this.CoordinateReferencePoint.transform.InverseTransformDirection(direction);
          break;
        case CoordinateSpace.Local_:
          direction = this.transform.InverseTransformDirection(direction);
          break;
        case CoordinateSpace.Global_:
        default:
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log("Defaulting TransformDirection");
          }
          #endif
          break;
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public Vector3 InverseTransformDirection(Vector3 direction) {
      switch (this.CoordinateSpace) {
        case CoordinateSpace.Environment_ when this.CoordinateReferencePoint:
          return this.CoordinateReferencePoint.transform.TransformDirection(direction);
        case CoordinateSpace.Local_:
          return this.transform.TransformDirection(direction);

        default:
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log("Defaulting InverseTransformDirection");
          }
          #endif
          return direction;
      }
    }

    public void InverseTransformDirection(ref Vector3 direction) {
      switch (this.CoordinateSpace) {
        case CoordinateSpace.Environment_ when this.CoordinateReferencePoint:
          direction = this.CoordinateReferencePoint.transform.TransformDirection(direction);
          break;
        case CoordinateSpace.Local_:
          direction = this.transform.TransformDirection(direction);
          break;
        default:
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log("Defaulting InverseTransformDirection");
          }
          #endif
          break;
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="quaternion"></param>
    /// <returns></returns>
    public Quaternion TransformRotation(Quaternion quaternion) {
      if (this.CoordinateSpace == CoordinateSpace.Environment_) {
        if (this.CoordinateReferencePoint) {
          return Quaternion.Inverse(this.CoordinateReferencePoint.rotation) * quaternion;
        }

        //Quaternion.Euler(this._coordinate_reference_point.transform.TransformDirection(quaternion.forward));
      } else if (this.CoordinateSpace == CoordinateSpace.Local_) {
        if (this.CoordinateReferencePoint) {
          return Quaternion.Inverse(this.Transform.rotation) * quaternion;
        }
      }

      return quaternion;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="quaternion"></param>
    public void TransformRotation(ref Quaternion quaternion) {
      if (this.CoordinateSpace == CoordinateSpace.Environment_) {
        if (this.CoordinateReferencePoint) {
          quaternion = Quaternion.Inverse(this.CoordinateReferencePoint.rotation) * quaternion;
        }

        //Quaternion.Euler(this._coordinate_reference_point.transform.TransformDirection(quaternion.forward));
      } else if (this.CoordinateSpace == CoordinateSpace.Local_) {
        if (this.CoordinateReferencePoint) {
          quaternion = Quaternion.Inverse(this.Transform.rotation) * quaternion;
        }
      }
    }

    public Quaternion InverseTransformRotation(Quaternion quaternion) {
      if (this.CoordinateSpace == CoordinateSpace.Environment_) {
        if (this.CoordinateReferencePoint) {
          return this.CoordinateReferencePoint.rotation * quaternion;
        }

        //Quaternion.Euler(this._coordinate_reference_point.transform.TransformDirection(quaternion.forward));
      } else if (this.CoordinateSpace == CoordinateSpace.Local_) {
        if (this.CoordinateReferencePoint) {
          return this.Transform.rotation * quaternion;
        }
      }

      return quaternion;
    }

    public void InverseTransformRotation(ref Quaternion quaternion) {
      if (this.CoordinateSpace == CoordinateSpace.Environment_) {
        if (this.CoordinateReferencePoint) {
          quaternion = this.CoordinateReferencePoint.rotation * quaternion;
        } else if (this.CoordinateSpace == CoordinateSpace.Local_) {
          if (this.CoordinateReferencePoint) {
            quaternion = this.Transform.rotation * quaternion;
          }
        }

        //Quaternion.Euler(this._coordinate_reference_point.transform.TransformDirection(quaternion.forward));
      }
    }

    #endregion

    #region Getters

    /// <summary>
    /// </summary>
    public SortedDictionary<string, IDisplayer> Displayers { get; } =
      new SortedDictionary<string, IDisplayer>();

    /// <summary>
    /// </summary>
    public SortedDictionary<string, IConfigurable> Configurables { get; } =
      new SortedDictionary<string, IConfigurable>();

    /// <summary>
    /// </summary>
    public SortedDictionary<string, ISensor> Sensors { get; } = new SortedDictionary<string, ISensor>();

    /// <summary>
    /// </summary>
    public SortedDictionary<string, IUnobservable> Listeners { get; } =
      new SortedDictionary<string, IUnobservable>();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "PrototypingEnvironment"; } }

    /// <summary>
    /// </summary>
    [field : Header("References", order = 20)]
    [field : SerializeField]
    public IEpisodicObjectiveFunction ObjectiveFunction { get; set; }

    /// <summary>
    /// </summary>
    public Transform Transform { get { return this.transform; } }

    /// <summary>
    /// </summary>
    [field : Header("(Optional)", order = 80)]
    [field : SerializeField]
    public BoundingBox PlayableArea { get; set; }

    /// <summary>
    /// </summary>
    [field : Header("General", order = 30)]
    [field : SerializeField]
    public Transform CoordinateReferencePoint { get; set; }

    /// <summary>
    /// </summary>
    [field : SerializeField]
    public CoordinateSpace CoordinateSpace { get; set; } = CoordinateSpace.Local_;

    /// <summary>
    /// </summary>
    protected Rigidbody[] TrackedRigidBodies { get { return this._Tracked_Rigid_Bodies; } }

    /// <summary>
    /// </summary>
    [field : SerializeField]
    protected Boolean TrackOnlyChildren { get; set; } = true;

    /// <summary>
    ///
    /// </summary>
    [field : SerializeField]
    protected Boolean UpdateObservationsWithEveryTick { get; set; } = true;

    #endregion

    #region Registration

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public void Register(IDisplayer displayer) { this.Register(displayer, displayer.Identifier); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="identifier"></param>
    public void Register(IDisplayer obj, string identifier) {
      if (!this.Displayers.ContainsKey(identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} has registered displayer {identifier}");
        }
        #endif
        this.Displayers.Add(identifier, obj);
      } else {
        Debug.LogWarning($"WARNING! Please check for duplicates, Environment {this.name} already has displayer {identifier} registered");
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="sensor"></param>
    public void Register(ISensor sensor) { this.Register(sensor, sensor.Identifier); }

    /// <inheritdoc />
    /// ///
    /// <summary>
    /// </summary>
    /// <param name="sensor"></param>
    /// <param name="identifier"></param>
    public void Register(ISensor sensor, string identifier) {
      if (!this.Sensors.ContainsKey(identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} has registered sensor {identifier}");
        }
        #endif

        this.Sensors.Add(identifier, sensor);
      } else {
        Debug.LogWarning($"WARNING! Please check for duplicates, Environment {this.name} already has observer {identifier} registered");
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="configurable"></param>
    public void Register(IConfigurable configurable) { this.Register(configurable, configurable.Identifier); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="configurable"></param>
    /// <param name="identifier"></param>
    public void Register(IConfigurable configurable, string identifier) {
      if (!this.Configurables.ContainsKey(identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} has registered configurable {identifier}");
        }
        #endif

        this.Configurables.Add(identifier, configurable);
      } else {
        Debug.LogWarning($"WARNING! Please check for duplicates, Environment {this.name} already has configurable {identifier} registered");
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="environment_listener"></param>
    public void Register(IUnobservable environment_listener) {
      this.Register(environment_listener, environment_listener.Identifier);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="environment_listener"></param>
    /// <param name="identifier"></param>
    public void Register(IUnobservable environment_listener, string identifier) {
      if (!this.Listeners.ContainsKey(identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} has registered resetable {identifier}");
        }
        #endif
        this.Listeners.Add(identifier, environment_listener);
      } else {
        Debug.LogWarning($"WARNING! Please check for duplicates, Environment {this.name} already has resetable {identifier} registered");
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="sensor"></param>
    public void UnRegister(ISensor sensor) { this.UnRegister(sensor, sensor.Identifier); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="t"></param>
    /// <param name="identifier"></param>
    public void UnRegister(ISensor t, string identifier) {
      if (this.Sensors.ContainsKey(identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} unregistered sensor {identifier}");
        }
        #endif
        this.Sensors.Remove(identifier);
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="configurable"></param>
    public void UnRegister(IConfigurable configurable) {
      this.UnRegister(configurable, configurable.Identifier);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="t"></param>
    /// <param name="identifier"></param>
    public void UnRegister(IConfigurable t, string identifier) {
      if (this.Configurables.ContainsKey(identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} unregistered configurable {identifier}");
        }
        #endif
        this.Configurables.Remove(identifier);
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="displayer"></param>
    public void UnRegister(IDisplayer displayer) { this.UnRegister(displayer, displayer.Identifier); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="t"></param>
    /// <param name="identifier"></param>
    public void UnRegister(IDisplayer t, string identifier) {
      if (this.Displayers.ContainsKey(identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} unregistered configurable {identifier}");
        }
        #endif
        this.Displayers.Remove(identifier);
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="environment_listener"></param>
    public void UnRegister(IUnobservable environment_listener) {
      this.UnRegister(environment_listener, environment_listener.Identifier);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="t"></param>
    /// <param name="identifier"></param>
    public void UnRegister(IUnobservable t, string identifier) {
      if (this.Listeners.ContainsKey(identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} unregistered resetable {identifier}");
        }
        #endif
        this.Listeners.Remove(identifier);
      }
    }

    #endregion

    /// <summary>
    ///
    /// </summary>
    protected abstract void InnerResetRegisteredObjects();

    public override String ToString() {
      var e = " - ";

      e += this.Identifier;
      e += ", Sensors: ";
      e += this.Sensors.Count;
      e += ", Objective: ";
      e += this.ObjectiveFunction != null ? this.ObjectiveFunction.Identifier : "None";

      return e;
    }
  }
}
