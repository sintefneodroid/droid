using System;
using System.Collections;
using System.Collections.Generic;
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
  [RequireComponent(requiredComponent : typeof(Transform))]
  public abstract class AbstractPrototypingEnvironment : NeodroidEnvironment,
                                                         IAbstractPrototypingEnvironment {
    #region Fields

    #endregion

    #region ProtectedMembers

    /// <summary>
    /// </summary>
    protected object _Reaction_Lock = new object();

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

    [SerializeField] bool keep_last_configuration = false;

    #endregion

    #region Events

    /// <inheritdoc />
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

    /// <inheritdoc cref="Environment" />
    /// <summary>
    /// </summary>
    public event Action PostTickEvent;

    /// <inheritdoc cref="Environment" />
    /// <summary>
    /// </summary>
    public event Action PreTickEvent;

    #endregion

    #region Private Methods

    /// <inheritdoc />
    /// <summary>
    ///   Termination of an episode, can be supplied with a reason for various purposes debugging or clarification
    ///   for a learner.
    /// </summary>
    /// <param name="reason"></param>
    public void Terminate(string reason = "None") {
      if (this.Terminable) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.LogWarning(message : $"Environment {this.Identifier} as terminated because {reason}");
        }
        #endif

        this.Terminated = true;
        this.LastTerminationReason = reason;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Tick() {
      this.PreTickEvent?.Invoke();

      if (this.IsResetting) {
        this.PrototypingReset();

        this.LoopConfigurables();
        this.LoopListeners();
        this.LoopSensors();

        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("Reset");
        }
        #endif

        this.IsResetting = false;
      } else {
        this.LoopConfigurables(update : this.UpdateObservationsWithEveryTick);
        this.LoopListeners();
        this.LoopSensors(update : this.UpdateObservationsWithEveryTick);
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Tick");
      }
      #endif

      this.PostTickEvent?.Invoke();
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

    /// <summary>
    ///
    /// </summary>
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
      var ignored_layer = LayerMask.NameToLayer(layerName : $"IgnoredByNeodroid");
      if (this.TrackOnlyChildren) {
        this._tracked_game_objects =
            NeodroidSceneUtilities.RecursiveChildGameObjectsExceptLayer(parent : this.transform,
                                                                        layer : ignored_layer);
      } else {
        this._tracked_game_objects =
            NeodroidSceneUtilities.FindAllGameObjectsExceptLayer(layer : ignored_layer);
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
            Debug.Log(message : $"Added a JointFix component to {maybe_joint_fix.name}");
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

      var list = new List<Rigidbody>();
      for (var index = 0; index < this._tracked_game_objects.Length; index++) {
        var go = this._tracked_game_objects[index];
        if (go != null) {
          var body = go.GetComponent<Rigidbody>();
          if (body) {
            list.Add(item : body);
          }
        }
      }

      this._Tracked_Rigid_Bodies = list.ToArray();

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
      var list = new List<Animation>();
      for (var index = 0; index < this._tracked_game_objects.Length; index++) {
        var go = this._tracked_game_objects[index];
        if (go != null) {
          var anim = go.GetComponent<Animation>();
          if (anim) {
            list.Add(item : anim);
          }
        }
      }

      this._animations = list.ToArray();
      this._reset_animation_times = new float[this._animations.Length];
      for (var i = 0; i < this._animations.Length; i++) {
        if (this._animations[i]) {
          if (this._animations[i].clip) {
            this._reset_animation_times[i] =
                this._animations[i].CrossFadeQueued(animation : this._animations[i].clip.name)
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
        this.PlayableArea = this.GetComponent<NeodroidBoundingBox>();
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("PreSetup");
      }
      #endif

      this.SaveInitialPoses();
      this.SaveInitialAnimations();
      this.StartCoroutine(routine : this.SaveInitialBodiesIe());
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    IEnumerator SaveInitialBodiesIe() {
      yield return this._Wait_For_Fixed_Update;
      this.SaveInitialBodies();
    }

    /// <summary>
    /// </summary>
    /// <param name="reaction"></param>
    void StepInner(Reaction reaction) {
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

      this.SendToActors(reaction : reaction);

      this.StepEvent?.Invoke();
      this.LoopListeners();

      this.LoopSensors();
    }

    public override void Reset() { }

    public override void Configure(Reaction reaction) { }

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
    public override void Step(Reaction reaction) {
      this.LastReaction = reaction;
      this.Terminable = reaction.Parameters.Terminable;

      this._last_configuration = reaction.Configurations;

      this.ProvideFullDescription = reaction.Parameters.Describe;

      if (reaction.Parameters.Configure && reaction.Unobservables != null) {
        this._received_poses = reaction.Unobservables.Poses;
        this._received_bodies = reaction.Unobservables.Bodies;
      }

      this.SendToDisplayers(reaction : reaction);

      if (reaction.Parameters.ReactionType == ReactionTypeEnum.Reset_) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(message : $"Resetting environment({this.Identifier})");
        }
        #endif
        //this.Terminate($"Reaction caused a reset");
        this.IsResetting = true;
      } else if (reaction.Parameters.ReactionType == ReactionTypeEnum.Step_) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(message : $"Stepping in environment({this.Identifier})");
        }
        #endif
        this.StepInner(reaction : reaction);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PostStep() {
      this.PostStepEvent?.Invoke();
      if (this.ShouldConfigure) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("Configuring");
        }
        #endif
        this.ShouldConfigure = false;
        this.Reconfigure();
      }

      if (!this.keep_last_configuration) {
        this._last_configuration = null;
      }

      //this.LastReaction = null;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PrototypingReset() {
      this.LastResetTime = Time.realtimeSinceStartup;
      this.StepI = 0;
      this.ObjectiveFunction?.PrototypingReset();

      SetEnvironmentTransforms(child_game_objects : ref this._tracked_game_objects,
                               positions : ref this._reset_positions,
                               rotations : ref this._reset_rotations,
                               scales : ref this._reset_scales);

      this.SetBodies(bodies : ref this._Tracked_Rigid_Bodies,
                     velocities : ref this._reset_velocities,
                     angulars : ref this._reset_angulars);

      this.ResetRegisteredObjects();
      this.Reconfigure();

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log(message : $"Reset called on environment {this.Identifier}");
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

        SetEnvironmentTransforms(child_game_objects : ref this._tracked_game_objects,
                                 positions : ref positions,
                                 rotations : ref rotations,
                                 scales : ref scales);
      }

      if (this._received_bodies != null) {
        var velocities = new Vector3[this._received_bodies.Length];
        var angulars = new Vector3[this._received_bodies.Length];
        for (var i = 0; i < this._received_bodies.Length; i++) {
          velocities[i] = this._received_bodies[i].Velocity;
          angulars[i] = this._received_bodies[i].AngularVelocity;
        }

        this.SetBodies(bodies : ref this._Tracked_Rigid_Bodies,
                       velocities : ref velocities,
                       angulars : ref angulars);
      }

      if (this._last_configuration != null) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(message : $"Configuration length: {this._last_configuration.Length}");
        }
        #endif
        for (var index = 0; index < this._last_configuration.Length; index++) {
          var configuration = this._last_configuration[index];
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log(message : "Configuring configurable with the specified name: "
                                + configuration.ConfigurableName);
          }
          #endif
          if (this.Configurables.ContainsKey(key : configuration.ConfigurableName)) {
            this.Configurables[key : configuration.ConfigurableName]
                .ApplyConfiguration(configuration : configuration);
          } else {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log(message :
                        $"Could find not configurable with the specified name: {configuration.ConfigurableName}");
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
        for (var index = 0; index < reaction.Displayables.Length; index++) {
          var displayable = reaction.Displayables[index];
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log(message : "Applying " + displayable + " To " + this.name + "'s displayers");
          }
          #endif
          var displayable_name = displayable.DisplayableName;
          if (this.Displayers.ContainsKey(key : displayable_name)
              && this.Displayers[key : displayable_name] != null) {
            var v = displayable.DisplayableValue;
            this.Displayers[key : displayable_name].Display(v);
          } else {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log(message : "Could find not displayer with the specified name: " + displayable_name);
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
    /// <param name="scales"></param>
    static void SetEnvironmentTransforms(ref GameObject[] child_game_objects,
                                         ref Vector3[] positions,
                                         ref Quaternion[] rotations,
                                         ref Vector3[] scales) {
      if (child_game_objects != null) {
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
    }

    /// <summary>
    /// </summary>
    /// <param name="bodies"></param>
    /// <param name="velocities"></param>
    /// <param name="angulars"></param>
    void SetBodies(ref Rigidbody[] bodies, ref Vector3[] velocities, ref Vector3[] angulars) {
      if (bodies != null && bodies.Length > 0) {
        for (var i = 0; i < bodies.Length; i++) {
          if (i < bodies.Length && bodies[i] != null && i < velocities.Length && i < angulars.Length) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log(message :
                        $"Setting {bodies[i].name}, velocity to {velocities[i]} and angular velocity to {angulars[i]}");
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

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public Vector3 TransformPoint(Vector3 point) {
      switch (this.CoordinateSpaceEnum) {
        case CoordinateSpaceEnum.Environment_ when this.CoordinateReferencePoint:
          return this.CoordinateReferencePoint.transform.InverseTransformPoint(position : point);
        case CoordinateSpaceEnum.Local_:
          return this.transform.InverseTransformPoint(position : point);
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
      switch (this.CoordinateSpaceEnum) {
        case CoordinateSpaceEnum.Environment_ when this.CoordinateReferencePoint:
          point = this.CoordinateReferencePoint.transform.InverseTransformPoint(position : point);
          break;
        case CoordinateSpaceEnum.Local_:
          //point = point - this.transform.position;
          point = this.transform.InverseTransformPoint(position : point);
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

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public Vector3 InverseTransformPoint(Vector3 point) {
      switch (this.CoordinateSpaceEnum) {
        case CoordinateSpaceEnum.Environment_ when this.CoordinateReferencePoint:
          return this.CoordinateReferencePoint.transform.TransformPoint(position : point);
        case CoordinateSpaceEnum.Local_:
          //return point - this.transform.position;
          return this.transform.TransformPoint(position : point);
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
      switch (this.CoordinateSpaceEnum) {
        case CoordinateSpaceEnum.Environment_ when this.CoordinateReferencePoint:
          point = this.CoordinateReferencePoint.transform.TransformPoint(position : point);
          break;
        case CoordinateSpaceEnum.Local_:
          //point = point - this.transform.position;
          point = this.transform.TransformPoint(position : point);
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

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public Vector3 TransformDirection(Vector3 direction) {
      switch (this.CoordinateSpaceEnum) {
        case CoordinateSpaceEnum.Environment_ when this.CoordinateReferencePoint:
          return this.CoordinateReferencePoint.transform.InverseTransformDirection(direction : direction);
        case CoordinateSpaceEnum.Local_:
          return this.transform.InverseTransformDirection(direction : direction);
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
      switch (this.CoordinateSpaceEnum) {
        case CoordinateSpaceEnum.Environment_ when this.CoordinateReferencePoint:
          direction =
              this.CoordinateReferencePoint.transform.InverseTransformDirection(direction : direction);
          break;
        case CoordinateSpaceEnum.Local_:
          direction = this.transform.InverseTransformDirection(direction : direction);
          break;
        case CoordinateSpaceEnum.Global_:
        default:
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log("Defaulting TransformDirection");
          }
          #endif
          break;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public Vector3 InverseTransformDirection(Vector3 direction) {
      switch (this.CoordinateSpaceEnum) {
        case CoordinateSpaceEnum.Environment_ when this.CoordinateReferencePoint:
          return this.CoordinateReferencePoint.transform.TransformDirection(direction : direction);
        case CoordinateSpaceEnum.Local_:
          return this.transform.TransformDirection(direction : direction);

        default:
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log("Defaulting InverseTransformDirection");
          }
          #endif
          return direction;
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="direction"></param>
    public void InverseTransformDirection(ref Vector3 direction) {
      switch (this.CoordinateSpaceEnum) {
        case CoordinateSpaceEnum.Environment_ when this.CoordinateReferencePoint:
          direction = this.CoordinateReferencePoint.transform.TransformDirection(direction : direction);
          break;
        case CoordinateSpaceEnum.Local_:
          direction = this.transform.TransformDirection(direction : direction);
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

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="quaternion"></param>
    /// <returns></returns>
    public Quaternion TransformRotation(Quaternion quaternion) {
      if (this.CoordinateSpaceEnum == CoordinateSpaceEnum.Environment_) {
        if (this.CoordinateReferencePoint) {
          return Quaternion.Inverse(rotation : this.CoordinateReferencePoint.rotation) * quaternion;
        }

        //Quaternion.Euler(this._coordinate_reference_point.transform.TransformDirection(quaternion.forward));
      } else if (this.CoordinateSpaceEnum == CoordinateSpaceEnum.Local_) {
        if (this.CoordinateReferencePoint) {
          return Quaternion.Inverse(rotation : this.Transform.rotation) * quaternion;
        }
      }

      return quaternion;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="quaternion"></param>
    public void TransformRotation(ref Quaternion quaternion) {
      if (this.CoordinateSpaceEnum == CoordinateSpaceEnum.Environment_) {
        if (this.CoordinateReferencePoint) {
          quaternion = Quaternion.Inverse(rotation : this.CoordinateReferencePoint.rotation) * quaternion;
        }

        //Quaternion.Euler(this._coordinate_reference_point.transform.TransformDirection(quaternion.forward));
      } else if (this.CoordinateSpaceEnum == CoordinateSpaceEnum.Local_) {
        if (this.CoordinateReferencePoint) {
          quaternion = Quaternion.Inverse(rotation : this.Transform.rotation) * quaternion;
        }
      }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <param name="quaternion"></param>
    ///  <returns></returns>
    public Quaternion InverseTransformRotation(Quaternion quaternion) {
      if (this.CoordinateSpaceEnum == CoordinateSpaceEnum.Environment_) {
        if (this.CoordinateReferencePoint) {
          return this.CoordinateReferencePoint.rotation * quaternion;
        }

        //Quaternion.Euler(this._coordinate_reference_point.transform.TransformDirection(quaternion.forward));
      } else if (this.CoordinateSpaceEnum == CoordinateSpaceEnum.Local_) {
        if (this.CoordinateReferencePoint) {
          return this.Transform.rotation * quaternion;
        }
      }

      return quaternion;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="quaternion"></param>
    public void InverseTransformRotation(ref Quaternion quaternion) {
      if (this.CoordinateSpaceEnum == CoordinateSpaceEnum.Environment_) {
        if (this.CoordinateReferencePoint) {
          quaternion = this.CoordinateReferencePoint.rotation * quaternion;
        } else if (this.CoordinateSpaceEnum == CoordinateSpaceEnum.Local_) {
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
    public EpisodicObjective ObjectiveFunction { get; set; }

    /// <summary>
    /// </summary>
    public Transform Transform { get { return this.transform; } }

    /// <summary>
    /// </summary>
    [field : Header("(Optional)", order = 80)]
    [field : SerializeField]
    public NeodroidBoundingBox PlayableArea { get; set; }

    /// <summary>
    /// </summary>
    [field : Header("General", order = 30)]
    [field : SerializeField]
    public Transform CoordinateReferencePoint { get; set; }

    /// <summary>
    /// </summary>
    [field : SerializeField]
    public CoordinateSpaceEnum CoordinateSpaceEnum { get; set; } = CoordinateSpaceEnum.Local_;

    /// <summary>
    /// </summary>
    protected Rigidbody[] TrackedRigidBodies { get { return this._Tracked_Rigid_Bodies; } }

    /// <summary>
    /// </summary>
    [field : SerializeField]
    protected bool TrackOnlyChildren { get; set; } = true;

    /// <summary>
    ///
    /// </summary>
    [field : SerializeField]
    protected bool UpdateObservationsWithEveryTick { get; set; } = true;

    #endregion

    #region Registration

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public void Register(IDisplayer displayer) {
      this.Register(obj : displayer, identifier : displayer.Identifier);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="identifier"></param>
    public void Register(IDisplayer obj, string identifier) {
      if (!this.Displayers.ContainsKey(key : identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(message : $"Environment {this.name} has registered displayer {identifier}");
        }
        #endif
        this.Displayers.Add(key : identifier, value : obj);
      } else {
        Debug.LogWarning(message :
                         $"WARNING! Please check for duplicates, Environment {this.name} already has displayer {identifier} registered");
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="sensor"></param>
    public void Register(ISensor sensor) { this.Register(sensor : sensor, identifier : sensor.Identifier); }

    /// <inheritdoc />
    /// ///
    /// <summary>
    /// </summary>
    /// <param name="sensor"></param>
    /// <param name="identifier"></param>
    public void Register(ISensor sensor, string identifier) {
      if (!this.Sensors.ContainsKey(key : identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(message : $"Environment {this.name} has registered sensor {identifier}");
        }
        #endif

        this.Sensors.Add(key : identifier, value : sensor);
      } else {
        Debug.LogWarning(message :
                         $"WARNING! Please check for duplicates, Environment {this.name} already has observer {identifier} registered");
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="configurable"></param>
    public void Register(IConfigurable configurable) {
      this.Register(configurable : configurable, identifier : configurable.Identifier);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="configurable"></param>
    /// <param name="identifier"></param>
    public void Register(IConfigurable configurable, string identifier) {
      if (!this.Configurables.ContainsKey(key : identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(message : $"Environment {this.name} has registered configurable {identifier}");
        }
        #endif

        this.Configurables.Add(key : identifier, value : configurable);
      } else {
        Debug.LogWarning(message :
                         $"WARNING! Please check for duplicates, Environment {this.name} already has configurable {identifier} registered");
      }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <param name="environment_listener"></param>
    public void Register(IUnobservable environment_listener) {
      this.Register(environment_listener : environment_listener,
                    identifier : environment_listener.Identifier);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="environment_listener"></param>
    /// <param name="identifier"></param>
    public void Register(IUnobservable environment_listener, string identifier) {
      if (!this.Listeners.ContainsKey(key : identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(message : $"Environment {this.name} has registered resetable {identifier}");
        }
        #endif
        this.Listeners.Add(key : identifier, value : environment_listener);
      } else {
        Debug.LogWarning(message :
                         $"WARNING! Please check for duplicates, Environment {this.name} already has resetable {identifier} registered");
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="sensor"></param>
    public void UnRegister(ISensor sensor) { this.UnRegister(t : sensor, identifier : sensor.Identifier); }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <param name="t"></param>
    ///  <param name="identifier"></param>
    public void UnRegister(ISensor t, string identifier) {
      if (this.Sensors.ContainsKey(key : identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(message : $"Environment {this.name} unregistered sensor {identifier}");
        }
        #endif
        this.Sensors.Remove(key : identifier);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="configurable"></param>
    public void UnRegister(IConfigurable configurable) {
      this.UnRegister(t : configurable, identifier : configurable.Identifier);
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <param name="t"></param>
    ///  <param name="identifier"></param>
    public void UnRegister(IConfigurable t, string identifier) {
      if (this.Configurables.ContainsKey(key : identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(message : $"Environment {this.name} unregistered configurable {identifier}");
        }
        #endif
        this.Configurables.Remove(key : identifier);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="displayer"></param>
    public void UnRegister(IDisplayer displayer) {
      this.UnRegister(t : displayer, identifier : displayer.Identifier);
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <param name="t"></param>
    ///  <param name="identifier"></param>
    public void UnRegister(IDisplayer t, string identifier) {
      if (this.Displayers.ContainsKey(key : identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(message : $"Environment {this.name} unregistered configurable {identifier}");
        }
        #endif
        this.Displayers.Remove(key : identifier);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="environment_listener"></param>
    public void UnRegister(IUnobservable environment_listener) {
      this.UnRegister(t : environment_listener, identifier : environment_listener.Identifier);
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <param name="t"></param>
    ///  <param name="identifier"></param>
    public void UnRegister(IUnobservable t, string identifier) {
      if (this.Listeners.ContainsKey(key : identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(message : $"Environment {this.name} unregistered resetable {identifier}");
        }
        #endif
        this.Listeners.Remove(key : identifier);
      }
    }

    #endregion

    /// <summary>
    ///
    /// </summary>
    protected abstract void InnerResetRegisteredObjects();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
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
