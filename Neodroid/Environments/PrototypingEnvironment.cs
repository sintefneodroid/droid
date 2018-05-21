using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using droid.Neodroid.Prototyping.Actors;
using droid.Neodroid.Prototyping.Configurables;
using droid.Neodroid.Prototyping.Displayers;
using droid.Neodroid.Prototyping.Evaluation;
using droid.Neodroid.Prototyping.Internals;
using droid.Neodroid.Prototyping.Observers;
using droid.Neodroid.Utilities.BoundingBoxes;
using droid.Neodroid.Utilities.Enums;
using droid.Neodroid.Utilities.Interfaces;
using droid.Neodroid.Utilities.Messaging.Messages;
using droid.Neodroid.Utilities.Unsorted;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;

namespace droid.Neodroid.Environments {
  /// <inheritdoc cref="NeodroidEnvironment" />
  /// <summary>
  /// Environment to be used with the prototyping components.
  /// </summary>
  [AddComponentMenu("Neodroid/Environments/PrototypingEnviroment")]
  public class PrototypingEnvironment : NeodroidEnvironment,
                                        IHasRegister<Actor>,
                                        IHasRegister<Observer>,
                                        IHasRegister<ConfigurableGameObject>,
                                        IHasRegister<Resetable>,
                                        IHasRegister<Displayer>,
                                        IHasRegister<EnvironmentListener> {
    /// <summary>
    ///
    /// </summary>
    public event Action PreStepEvent;

    /// <summary>
    ///
    /// </summary>
    public event Action StepEvent;

    /// <summary>
    ///
    /// </summary>
    public event Action PostStepEvent;

    /// <summary>
    ///
    /// </summary>
    Object _react_lock = new Object();

    int _reset_i;

    [SerializeField] bool _ordered_observables;
    [SerializeField] bool _keep_unobservables;
    
    WaitForFixedUpdate _wait_for_fixed_update =  new WaitForFixedUpdate();
    List<float> _observables = new List<float>();
    List<MotorMotion> _sample_motions = new List<MotorMotion>();

    #region UnityCallbacks

    #endregion

    #region NeodroidCallbacks

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      if (!this._objective_function) {
        this._objective_function = this.GetComponent<ObjectiveFunction>();
      }

      if (!this.PlayableArea) {
        this.PlayableArea = this.GetComponent<BoundingBox>();
      }

      this.SaveInitialPoses();
      this.SaveInitialAnimations();
      this.StartCoroutine(this.SaveInitialBodiesIe());
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Clear() {
      this.Displayers = new Dictionary<string, Displayer>();
      this.Configurables = new Dictionary<string, ConfigurableGameObject>();
      this.Actors = new Dictionary<string, Actor>();
      this.Observers = new Dictionary<string, Observer>();
      this.Resetables = new Dictionary<string, Resetable>();
      this.Listeners = new Dictionary<string, EnvironmentListener>();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PostStep() {
      this.PostStepEvent?.Invoke();
      if (this._Configure) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Configuring");
        }
        #endif
        this._Configure = false;
        this.Configure();
      }

      if (!this._Simulation_Manager.IsSyncingEnvironments) {
        this._Describe = false;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override Reaction SampleReaction() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"Sampling a reaction for environment {this.Identifier}");
      }
      #endif

      this._sample_motions.Clear();

      foreach (var actor in this.Actors) {
        foreach (var motor in actor.Value.Motors) {
          var strength = Random.Range(
              (int)motor.Value.MotionValueSpace._Min_Value,
              (int)(motor.Value.MotionValueSpace._Max_Value + 1));
          this._sample_motions.Add(new MotorMotion(actor.Key, motor.Key, strength));
        }
      }

      if (this._Terminated) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("SampleReaction resetting environment");
        }
        #endif

        var reset_reaction =
            new ReactionParameters(false, false, true, episode_count : true) {
                IsExternal = false
            };
        return new Reaction(reset_reaction, this.Identifier);
      }

      var rp = new ReactionParameters(true, true, episode_count : true) {
          IsExternal = false
      };
      return new Reaction(
          rp,
          this._sample_motions.ToArray(),
          null,
          null,
          null,
          "",
          this.Identifier);
    }

    /// <summary>
    ///
    /// </summary>
    protected void UpdateObserversData() {
      foreach (var obs in this.Observers.Values) {
        if (obs) {
          obs.UpdateObservation();
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    protected void UpdateConfigurableValues() {
      foreach (var con in this.Configurables.Values) {
        if (con) {
          con.UpdateCurrentConfiguration();
        }
      }
    }

    #endregion

    #region Fields

    /// <summary>
    ///
    /// </summary>
    [Header("References", order = 20)]
    [SerializeField]
    ObjectiveFunction _objective_function;

    /// <summary>
    ///
    /// </summary>
    [Header("General", order = 30)]
    [SerializeField]
    Transform _coordinate_reference_point;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    bool _track_only_children = true;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    CoordinateSystem _coordinate_system = CoordinateSystem.Local_coordinates_;

    /// <summary>
    ///
    /// </summary>
    [Header("(Optional)", order = 80)]
    [SerializeField]
    BoundingBox _playable_area;

    #endregion

    #region PrivateMembers

    /// <summary>
    ///
    /// </summary>
    Vector3[] _reset_positions;

    /// <summary>
    ///
    /// </summary>
    Quaternion[] _reset_rotations;

    /// <summary>
    ///
    /// </summary>
    GameObject[] _tracked_game_objects;

    /// <summary>
    ///
    /// </summary>
    Vector3[] _reset_velocities;

    /// <summary>
    ///
    /// </summary>
    Vector3[] _reset_angulars;

    /// <summary>
    ///
    /// </summary>
    Rigidbody[] _bodies;

    /// <summary>
    ///
    /// </summary>
    Transform[] _poses;

    /// <summary>
    ///
    /// </summary>
    Pose[] _received_poses;

    /// <summary>
    ///
    /// </summary>
    Body[] _received_bodies;

    /// <summary>
    ///
    /// </summary>
    Configuration[] _received_configurations;





    /// <summary>
    ///
    /// </summary>
    Animation[] _animations;

    /// <summary>
    ///
    /// </summary>
    float[] _reset_animation_times;

    #endregion

    #region PublicMethods

    #region Getters

    /// <summary>
    ///
    /// </summary>
    public Dictionary<string, Displayer> Displayers { get; set; } = new Dictionary<string, Displayer>();

    /// <summary>
    ///
    /// </summary>
    public Dictionary<string, ConfigurableGameObject> Configurables { get; set; } =
      new Dictionary<string, ConfigurableGameObject>();

    /// <summary>
    ///
    /// </summary>
    public Dictionary<string, Actor> Actors { get; set; } = new Dictionary<string, Actor>();

    /// <summary>
    ///
    /// </summary>
    public Dictionary<string, Observer> Observers { get; set; } = new Dictionary<string, Observer>();

    /// <summary>
    ///
    /// </summary>
    public Dictionary<string, Resetable> Resetables { get; set; } =
      new Dictionary<string, Resetable>();

    /// <summary>
    ///
    /// </summary>
    public Dictionary<string, EnvironmentListener> Listeners { get; set; } =
      new Dictionary<string, EnvironmentListener>();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "PrototypingEnvironment"; } }

    /// <summary>
    ///
    /// </summary>
    public ObjectiveFunction ObjectiveFunction {
      get { return this._objective_function; }
      set { this._objective_function = value; }
    }

    /// <summary>
    ///
    /// </summary>
    public BoundingBox PlayableArea {
      get { return this._playable_area; }
      set { this._playable_area = value; }
    }

    /// <summary>
    ///
    /// </summary>
    public Transform CoordinateReferencePoint {
      get { return this._coordinate_reference_point; }
      set { this._coordinate_reference_point = value; }
    }

    /// <summary>
    ///
    /// </summary>
    public CoordinateSystem CoordinateSystem {
      get { return this._coordinate_system; }
      set { this._coordinate_system = value; }
    }


    #endregion

    /// <summary>
    /// Termination of an episode, can be supplied with a reason for various purposes debugging or clarification for a learner.
    /// </summary>
    /// <param name="reason"></param>
    public void Terminate(string reason = "None") {
      lock (this._react_lock) {
        if (this._Terminable) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.LogWarning($"Environment {this.Identifier} as terminated because {reason}");
          }
          #endif

          this._Terminated = true;
          this._Termination_Reason = reason;
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public override EnvironmentState ReactAndCollectState(
        Reaction reaction) {
      lock (this._react_lock) {
        this._Terminable = reaction.Parameters.Terminable;

        if (reaction.Parameters.IsExternal) {
          this._received_configurations = reaction.Configurations;
          if (!this._Describe) {
            this._Describe = reaction.Parameters.Describe;
          }

          this._Configure = reaction.Parameters.Configure;
          if (this._Configure && reaction.Unobservables != null) {
            this._received_poses = reaction.Unobservables.Poses;
            this._received_bodies = reaction.Unobservables.Bodies;
          }
        }

        this.SendToDisplayers(reaction);

        if (reaction.Parameters.Reset) {
          this.Terminate(
              $"{(reaction.Parameters.IsExternal ? "External" : "Internal")} reaction caused a reset");
          this._Resetting = true;
        } else if (reaction.Parameters.Step) {
          this.Step(reaction);
        }

        return this.CollectState();
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public override void React(Reaction reaction) {
      lock (this._react_lock) {
        this._Terminable = reaction.Parameters.Terminable;

        if (reaction.Parameters.IsExternal) {
          this._received_configurations = reaction.Configurations;

          if (!this._Describe) {
            this._Describe = reaction.Parameters.Describe;
          }

          this._Configure = reaction.Parameters.Configure;
          if (this._Configure && reaction.Unobservables != null) {
            this._received_poses = reaction.Unobservables.Poses;
            this._received_bodies = reaction.Unobservables.Bodies;
          }
        }

        this.SendToDisplayers(reaction);

        if (reaction.Parameters.Reset) {
          this.Terminate(
              $"{(reaction.Parameters.IsExternal ? "External" : "Internal")} reaction caused a reset");
          this._Resetting = true;
        } else if (reaction.Parameters.Step) {
          this.Step(reaction);
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Tick() {
      if (this._Resetting) {
        if (this._reset_i >= this._Simulation_Manager.Configuration.ResetIterations) {
          this._Resetting = false;
          this._reset_i = 0;
          this.UpdateConfigurableValues();
          this.UpdateObserversData();
        } else {
          this.Reset();
          this._reset_i += 1;
        }
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Reset {this._reset_i}/{this._Simulation_Manager.Configuration.ResetIterations}");
        }
        #endif
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Tick");
      }
      #endif
    }

    #region Registration

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public void Register(Displayer displayer) { this.Register(displayer, displayer.Identifier); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="identifier"></param>
    public void Register(Displayer obj, string identifier) {
      if (!this.Displayers.ContainsKey(identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} has registered displayer {identifier}");
        }
        #endif
        this.Displayers.Add(identifier, obj);
      } else {
        Debug.LogWarning(
            $"WARNING! Please check for duplicates, Environment {this.name} already has displayer {identifier} registered");
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="actor"></param>
    public void Register(Actor actor) { this.Register(actor, actor.Identifier); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="identifier"></param>
    public void Register(Actor actor, string identifier) {
      if (!this.Actors.ContainsKey(identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} has registered actor {identifier}");
        }
        #endif

        this.Actors.Add(identifier, actor);
      } else {
        Debug.LogWarning(
            $"WARNING! Please check for duplicates, Environment {this.name} already has actor {identifier} registered");
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="observer"></param>
    public void Register(Observer observer) { this.Register(observer, observer.Identifier); }

    /// <inheritdoc />
    ///     /// <summary>
    /// </summary>
    /// <param name="observer"></param>
    /// <param name="identifier"></param>
    public void Register(Observer observer, string identifier) {
      if (!this.Observers.ContainsKey(identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} has registered observer {identifier}");
        }
        #endif

        this.Observers.Add(identifier, observer);
      } else {
        Debug.LogWarning(
            $"WARNING! Please check for duplicates, Environment {this.name} already has observer {identifier} registered");
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="configurable"></param>
    public void Register(ConfigurableGameObject configurable) {
      this.Register(configurable, configurable.Identifier);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="configurable"></param>
    /// <param name="identifier"></param>
    public void Register(ConfigurableGameObject configurable, string identifier) {
      if (!this.Configurables.ContainsKey(identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} has registered configurable {identifier}");
        }
        #endif

        this.Configurables.Add(identifier, configurable);
      } else {
        Debug.LogWarning(
            $"WARNING! Please check for duplicates, Environment {this.name} already has configurable {identifier} registered");
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="resetable"></param>
    /// <param name="identifier"></param>
    public void Register(Resetable resetable, string identifier) {
      if (!this.Resetables.ContainsKey(identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} has registered resetable {identifier}");
        }
        #endif
        this.Resetables.Add(identifier, resetable);
      } else {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(
              $"WARNING! Please check for duplicates, Environment {this.name} already has resetable {identifier} registered");
        }
        #endif
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="resetable"></param>
    public void Register(Resetable resetable) {
      this.Register(resetable, resetable.Identifier);
    }

    public void Register(EnvironmentListener environment_listener) {
      this.Register(environment_listener, environment_listener.Identifier);
    }

    public void Register(EnvironmentListener environment_listener, string identifier) {
      if (!this.Listeners.ContainsKey(identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} has registered listener {identifier}");
        }
        #endif
        this.Listeners.Add(identifier, environment_listener);
      } else {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(
              $"WARNING! Please check for duplicates, Environment {this.name} already has listener {identifier} registered");
        }
        #endif
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="identifier"></param>
    public void UnRegisterActor(string identifier) {
      if (this.Actors.ContainsKey(identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} unregistered actor {identifier}");
        }
        #endif
        this.Actors.Remove(identifier);
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="actor"></param>
    public void UnRegister(Actor actor) { this.UnRegisterActor(actor.Identifier); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="identifier"></param>
    public void UnRegisterObserver(string identifier) {
      if (this.Observers.ContainsKey(identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} unregistered observer {identifier}");
        }
        #endif
        this.Observers.Remove(identifier);
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="observer"></param>
    public void UnRegister(Observer observer) { this.UnRegisterObserver(observer.Identifier); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="identifier"></param>
    public void UnRegisterConfigurable(string identifier) {
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
    ///
    /// </summary>
    /// <param name="configurable"></param>
    public void UnRegister(Configurable configurable) {
      this.UnRegisterConfigurable(configurable.Identifier);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="displayer"></param>
    public void UnRegister(Displayer displayer) { this.UnRegisterDisplayer(displayer.Identifier); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="identifier"></param>
    public void UnRegisterDisplayer(string identifier) {
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
    ///
    /// </summary>
    /// <param name="resetable"></param>
    public void UnRegister(Resetable resetable) {
      this.UnRegisterDisplayer(resetable.Identifier);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="identifier"></param>
    public void UnRegisterResetable(string identifier) {
      if (this.Resetables.ContainsKey(identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} unregistered resetable {identifier}");
        }
        #endif
        this.Resetables.Remove(identifier);
      }
    }

    ///  <summary>
    ///
    ///  </summary>
    /// <param name="environment_listener"></param>
    public void UnRegister(EnvironmentListener environment_listener) {
      this.UnRegisterListener(environment_listener.Identifier);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="identifier"></param>
    public void UnRegisterListener(string identifier) {
      if (this.Listeners.ContainsKey(identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} unregistered listener {identifier}");
        }
        #endif
        this.Listeners.Remove(identifier);
      }
    }

    #endregion

    #region Transformations

    /// <summary>
    ///
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Vector3 TransformPosition(Vector3 position) {
      if (this._coordinate_system == CoordinateSystem.Relative_to_reference_point_) {
        if (this._coordinate_reference_point) {
          return this._coordinate_reference_point.transform.InverseTransformPoint(position);
        }

        return position;
      }

      if (this._coordinate_system == CoordinateSystem.Local_coordinates_) {
        return position - this.transform.position;
      }

      return position;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Vector3 InverseTransformPosition(Vector3 position) {
      if (this._coordinate_system == CoordinateSystem.Relative_to_reference_point_) {
        if (this._coordinate_reference_point) {
          return this._coordinate_reference_point.transform.TransformPoint(position);
        }

        return position;
      }

      if (this._coordinate_system == CoordinateSystem.Local_coordinates_) {
        return position - this.transform.position;
      }

      return position;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public Vector3 TransformDirection(Vector3 direction) {
      if (this._coordinate_system == CoordinateSystem.Relative_to_reference_point_) {
        if (this._coordinate_reference_point) {
          return this._coordinate_reference_point.transform.InverseTransformDirection(direction);
        }

        return direction;
      }

      if (this._coordinate_system == CoordinateSystem.Local_coordinates_) {
        return this.transform.InverseTransformDirection(direction);
      }

      return direction;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public Vector3 InverseTransformDirection(Vector3 direction) {
      if (this._coordinate_system == CoordinateSystem.Relative_to_reference_point_) {
        if (this._coordinate_reference_point) {
          return this._coordinate_reference_point.transform.TransformDirection(direction);
        }

        return direction;
      }

      if (this._coordinate_system == CoordinateSystem.Local_coordinates_) {
        return this.transform.InverseTransformDirection(direction);
      }

      return direction;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="quaternion"></param>
    /// <returns></returns>
    public Quaternion TransformRotation(Quaternion quaternion) {
      if (this._coordinate_system == CoordinateSystem.Relative_to_reference_point_) {
        if (this._coordinate_reference_point) {
          return Quaternion.Inverse(this._coordinate_reference_point.rotation) * quaternion;
        }

        //Quaternion.Euler(this._coordinate_reference_point.transform.TransformDirection(quaternion.forward));
      }

      return quaternion;
    }

    #endregion

    #endregion

    #region PrivateMethods

    /// <summary>
    ///
    /// </summary>
    void SaveInitialPoses() {
      var ignored_layer = LayerMask.NameToLayer("IgnoredByNeodroid");
      if (this._track_only_children) {
        this._tracked_game_objects =
            NeodroidUtilities.RecursiveChildGameObjectsExceptLayer(this.transform, ignored_layer);
      } else {
        this._tracked_game_objects = NeodroidUtilities.FindAllGameObjectsExceptLayer(ignored_layer);
      }

      var length = this._tracked_game_objects.Length;
      
      this._reset_positions = new Vector3[length];
      this._reset_rotations = new Quaternion[length];
      this._poses = new Transform[length];
      for (var i = 0; i < length; i++) {
        var go = this._tracked_game_objects[i];
        var transf = go.transform;
        this._reset_positions[i] = transf.position;
        this._reset_rotations[i] = transf.rotation;
        this._poses[i] = transf;
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
    ///
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
      */ //Should be equalvalent to the line below, but kept as a reference in case of confusion

      this._bodies = this._tracked_game_objects.Where(go => go != null)
          .Select(go => go.GetComponent<Rigidbody>()).Where(body => body).ToArray();

      this._reset_velocities = new Vector3[this._bodies.Length];
      this._reset_angulars = new Vector3[this._bodies.Length];
      for (var i = 0; i < this._bodies.Length; i++) {
        this._reset_velocities[i] = this._bodies[i].velocity;
        this._reset_angulars[i] = this._bodies[i].angularVelocity;
      }
    }

    /// <summary>
    ///
    /// </summary>
    void SaveInitialAnimations() {
      this._animations = this._tracked_game_objects.Where(go => go != null)
          .Select(go => go.GetComponent<Animation>()).Where(anim => anim).ToArray();
      this._reset_animation_times = new float[this._animations.Length];
      for (var i = 0; i < this._animations.Length; i++) {
        this._reset_animation_times[i] =
            this._animations[i].CrossFadeQueued(this._animations[i].clip.name)
                .time; //TODO: IS NOT USED AS RIGHT NOW and should use animations clips instead the legacy "clip.name".
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    IEnumerator SaveInitialBodiesIe() {
      yield return this._wait_for_fixed_update;
      this.SaveInitialBodies();
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <returns></returns>
    public override EnvironmentState CollectState() {
      lock (this._react_lock) {
        foreach (var a in this.Actors.Values) {
          foreach (var m in a.Motors.Values) {
            this._Energy_Spent += m.GetEnergySpend();
          }
        }

        var signal = 0f;
        //if (!this._Terminated) {
        if (this._objective_function) {
          signal = this._objective_function.Evaluate();
        }
        //}

        EnvironmentDescription description = null;
        if (this._Describe) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log("Describing Environment");
          }
          #endif
          var threshold = 0f;
          if (this._objective_function) {
            threshold = this._objective_function.SolvedThreshold;
          }

          description = new EnvironmentDescription(
              this.EpisodeLength,
              this.Actors,
              this.Configurables,
              threshold);
        }

        this._observables.Clear();
        if (this._ordered_observables) {
          foreach (var item in this.Observers.OrderBy(i => i.Key)) {
            if (item.Value != null) {
              if (item.Value.FloatEnumerable != null) {
                this._observables.AddRange(item.Value.FloatEnumerable);
              } else {
                #if NEODROID_DEBUG
                if (this.Debugging) {
                  Debug.Log($"Observer with key {item.Key} has a null FloatEnumerable value");
                }
                #endif
              }
            } else {
              #if NEODROID_DEBUG
              if (this.Debugging) {
                Debug.Log($"Observer with key {item.Key} has a null value");
              }
              #endif
            }
          }
        } else {
          foreach (var item in this.Observers) {
            if (item.Value != null) {
              if (item.Value.FloatEnumerable != null) {
                this._observables.AddRange(item.Value.FloatEnumerable);
              } else {
                #if NEODROID_DEBUG
                if (this.Debugging) {
                  Debug.Log($"Observer with key {item.Key} has a null FloatEnumerable value");
                }
                #endif
              }
            } else {
              #if NEODROID_DEBUG
              if (this.Debugging) {
                Debug.Log($"Observer with key {item.Key} has a null value");
              }
              #endif
            }
          }
        }

        var time = Time.time - this._Lastest_Reset_Time;
        var obs = this._observables.ToArray();
        if (this._keep_unobservables) {
          return new EnvironmentState(
              this.Identifier,
              this._Energy_Spent,
              this.Observers,
              this.CurrentFrameNumber,
              time,
              signal,
              this._Terminated,
              ref obs,
              ref this._bodies,
              ref this._poses,
              this.TerminationReason,
              description);
        }

        return new EnvironmentState(
            this.Identifier,
            this._Energy_Spent,
            this.Observers,
            this.CurrentFrameNumber,
            time,
            signal,
            this._Terminated,
            ref obs,
            this.TerminationReason,
            description);
      }
    }

    /// <summary>
    ///
    /// </summary>
    protected void Reset() {
      this._Lastest_Reset_Time = Time.time;
      this.CurrentFrameNumber = 0;
      if (this._objective_function) {
        this._objective_function.Reset();
      }

        this.SetEnvironmentPoses(this._tracked_game_objects, this._reset_positions, this._reset_rotations);
        this.SetEnvironmentBodies(this._bodies, this._reset_velocities, this._reset_angulars);

      this.ResetRegisteredObjects();
      this.Configure();

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"Reset called on environment {this.Identifier}");
      }
      #endif

      this._Terminated = false;
    }

    /// <summary>
    ///
    /// </summary>
    protected void Configure() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"Configure was called");
      }
      #endif

      if (this._received_poses != null) {
        var positions = new Vector3[this._received_poses.Length];
        var rotations = new Quaternion[this._received_poses.Length];
        for (var i = 0; i < this._received_poses.Length; i++) {
          positions[i] = this._received_poses[i].position;
          rotations[i] = this._received_poses[i].rotation;
        }

        this.SetEnvironmentPoses(this._tracked_game_objects, positions, rotations);
      }

      if (this._received_bodies != null) {
        var vels = new Vector3[this._received_bodies.Length];
        var angs = new Vector3[this._received_bodies.Length];
        for (var i = 0; i < this._received_bodies.Length; i++) {
          vels[i] = this._received_bodies[i].Velocity;
          angs[i] = this._received_bodies[i].AngularVelocity;
        }

        this.SetEnvironmentBodies(this._bodies, vels, angs);
      }

      if (this._received_configurations != null) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Configuration length: {this._received_configurations.Length}");
        }
        #endif
        foreach (var configuration in this._received_configurations) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log($"Configuring configurable with the specified name: " + configuration.ConfigurableName);
          }
          #endif
          if (this.Configurables.ContainsKey(configuration.ConfigurableName)) {
            this.Configurables[configuration.ConfigurableName].ApplyConfiguration(configuration);
          } else {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log(
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

      this.UpdateConfigurableValues();
      this.UpdateObserversData();
    }

    /// <summary>
    ///
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

    /// <summary>
    ///
    /// </summary>
    /// <param name="reaction"></param>
    void SendToMotors(Reaction reaction) {
      if (reaction.Motions != null && reaction.Motions.Length > 0) {
        foreach (var motion in reaction.Motions) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log("Applying " + motion + " To " + this.name + "'s actors");
          }
          #endif
          var motion_actor_name = motion.ActorName;
          if (this.Actors.ContainsKey(motion_actor_name) && this.Actors[motion_actor_name] != null) {
            this.Actors[motion_actor_name].ApplyMotion(motion);
          } else {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log("Could find not actor with the specified name: " + motion_actor_name);
            }
            #endif
          }
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="reaction"></param>
    void Step(Reaction reaction) {
      lock (this._react_lock) {
        this.PreStepEvent?.Invoke();

        /*#if NEODROID_DEBUG
 if (this.Debugging) {
          Debug.Log($"Step! CurrentFrameNumber: {this.CurrentFrameNumber}");
        }                    #endif*/
        if (reaction.Parameters.EpisodeCount) {
          this.CurrentFrameNumber++;
        } else {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.LogWarning("Step did not count towards CurrentFrameNumber");
          }
          #endif
        }

        this.SendToMotors(reaction);

        this.StepEvent?.Invoke();

        if (this.EpisodeLength > 0 && this.CurrentFrameNumber >= this.EpisodeLength) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log($"Maximum episode length reached, Length {this.CurrentFrameNumber}");
          }
          #endif

          this.Terminate("Maximum episode length reached");
        }

        this.UpdateObserversData();
      }
    }

    /// <summary>
    ///
    /// </summary>
    protected void ResetRegisteredObjects() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Resetting registed objects");
      }
      #endif

      foreach (var resetable in this.Resetables.Values) {
        if (resetable != null) {
          resetable.Reset();
        }
      }

      foreach (var actor in this.Actors.Values) {
        if (actor) {
          actor.Reset();
        }
      }

      foreach (var observer in this.Observers.Values) {
        if (observer) {
          observer.Reset();
        }
      }
    }

    #region EnvironmentStateSetters

    /// <summary>
    ///
    /// </summary>
    /// <param name="child_game_objects"></param>
    /// <param name="positions"></param>
    /// <param name="rotations"></param>
    void SetEnvironmentPoses(GameObject[] child_game_objects, Vector3[] positions, Quaternion[] rotations) {
      if (this._Simulation_Manager) {
        for (var iterations = 0;
             iterations < this._Simulation_Manager.Configuration.ResetIterations;
             iterations++) {
          for (var i = 0; i < child_game_objects.Length; i++) {
            if (child_game_objects[i] != null && i < positions.Length && i < rotations.Length) {
              var rigid_body = child_game_objects[i].GetComponent<Rigidbody>();
              if (rigid_body) {
                rigid_body.Sleep();
              }

              child_game_objects[i].transform.position = positions[i];
              child_game_objects[i].transform.rotation = rotations[i];
              if (rigid_body) {
                rigid_body.WakeUp();
              }

              var joint_fix = child_game_objects[i].GetComponent<JointFix>();
              if (joint_fix) {
                joint_fix.Reset();
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
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="bodies"></param>
    /// <param name="velocities"></param>
    /// <param name="angulars"></param>
    void SetEnvironmentBodies(Rigidbody[] bodies, Vector3[] velocities, Vector3[] angulars) {
      if (bodies != null && bodies.Length > 0) {
        for (var i = 0; i < bodies.Length; i++) {
          if (i < bodies.Length && bodies[i] != null && i < velocities.Length && i < angulars.Length) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log(
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

    #endregion
  }
}
