using System.Collections.Generic;
using droid.Runtime.Environments.Prototyping;
using droid.Runtime.GameObjects;
using droid.Runtime.GameObjects.BoundingBoxes.Experimental;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities;
using UnityEditor;
using UnityEngine;

namespace droid.Runtime.Prototyping.Actors {
  /// <inheritdoc cref="PrototypingGameObject" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ActorComponentMenuPath._ComponentMenuPath + "Base" + ActorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  public class Actor : PrototypingGameObject,
                       IActor {
    /// <summary>
    /// </summary>
    [SerializeField]
    Bounds _bounds;

    /// <summary>
    /// </summary>
    [SerializeField]
    bool _draw_bounds = false;

    /// <summary>
    /// </summary>
    public Bounds ActorBounds {
      get {
        var col = this.GetComponent<BoxCollider>();
        this._bounds = new Bounds(this.transform.position, Vector3.zero); // position and size

        if (col) {
          this._bounds.Encapsulate(col.bounds);
        }

        foreach (var child_col in this.GetComponentsInChildren<Collider>()) {
          if (child_col != col) {
            this._bounds.Encapsulate(child_col.bounds);
          }
        }

        return this._bounds;
      }
    }

    SortedDictionary<string, IActuator> IActor.Actuators { get { return this._Actuators; } }

    public Transform Transform { get { return this.transform; } }

    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    public virtual void ApplyMotion(IMotion motion) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"Applying {motion} To {this.name}'s Actuators");
      }
      #endif

      var motion_actuator_name = motion.ActuatorName;
      if (this._Actuators.ContainsKey(motion_actuator_name)
          && this._Actuators[motion_actuator_name] != null) {
        this._Actuators[motion_actuator_name].ApplyMotion(motion);
      } else {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Could find not Actuator with the specified name: {motion_actuator_name} on actor {this.name}");
        }
        #endif
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PrototypingReset() {
      if (this._Actuators != null) {
        foreach (var actuator in this._Actuators.Values) {
          actuator?.PrototypingReset();
        }
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="actuator"></param>
    /// <param name="identifier"></param>
    public void UnRegister(IActuator actuator, string identifier) {
      if (this._Actuators != null) {
        if (this._Actuators.ContainsKey(identifier)) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log($"Actor {this.name} unregistered Actuator {identifier}");
          }
          #endif

          this._Actuators.Remove(identifier);
        }
      }
    }



    /// <summary>
    /// </summary>
    /// <param name="actuator"></param>
    public void UnRegister(IActuator actuator) { this.UnRegister(actuator, actuator.Identifier); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() {
      #if UNITY_EDITOR
      if (!Application.isPlaying) {
        var manager_script = MonoScript.FromMonoBehaviour(this);
        if (MonoImporter.GetExecutionOrder(manager_script) != _script_execution_order) {
          MonoImporter.SetExecutionOrder(manager_script,
                                         _script_execution_order); // Ensures that PreStep is called first, before all other scripts.
          Debug.LogWarning("Execution Order changed, you will need to press play again to make everything function correctly!");
          EditorApplication.isPlaying = false;
          //TODO: UnityEngine.Experimental.LowLevel.PlayerLoop.SetPlayerLoop(new UnityEngine.Experimental.LowLevel.PlayerLoopSystem());
        }
      }
      #endif
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Clear() { this._Actuators.Clear(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment = NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment, this);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() { this._environment?.UnRegister(this); }

    /// <summary>
    /// </summary>
    void Update() {
      if (this._draw_bounds) {
        var corners =
            Corners.ExtractCorners(this.ActorBounds.center, this.ActorBounds.extents, this.transform);

        Corners.DrawBox(corners[0],
                        corners[1],
                        corners[2],
                        corners[3],
                        corners[4],
                        corners[5],
                        corners[6],
                        corners[7],
                        Color.gray);
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="actuator"></param>
    /// <param name="identifier"></param>
    public void RegisterActuator(IActuator actuator, string identifier) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Actor " + this.name + " has Actuator " + identifier);
      }
      #endif

      if (!this._Actuators.ContainsKey(identifier)) {
        this._Actuators.Add(identifier, actuator);
      } else {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"A Actuator with the identifier {identifier} is already registered");
        }
        #endif
      }
    }

    #region Fields

    /// <summary>
    /// </summary>
    [Header("References", order = 99)]
    [SerializeField]
    ActorisedPrototypingEnvironment _environment;

    /// <summary>
    /// </summary>
    [Header("General", order = 101)]
    [SerializeField]
    protected SortedDictionary<string, IActuator> _Actuators = new SortedDictionary<string, IActuator>();

    #if UNITY_EDITOR
    const int _script_execution_order = -10;
    #endif

    #endregion

    #region Getters

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "Actor"; } }

    /// <summary>
    /// </summary>
    /// <param name="actuator"></param>
    public void Register(IActuator actuator) { this.RegisterActuator(actuator, actuator.Identifier); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="actuator"></param>
    /// <param name="identifier"></param>
    public void Register(IActuator actuator, string identifier) {
      this.RegisterActuator(actuator, identifier);
    }

    /// <summary>
    /// </summary>
    public SortedDictionary<string, IActuator> Actuators { get { return this._Actuators; } }

    /// <summary>
    /// </summary>
    public ActorisedPrototypingEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    #endregion
  }
}
