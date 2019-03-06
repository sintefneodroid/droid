using System.Collections.Generic;
using droid.Runtime.Environments;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities.BoundingBoxes.Experimental;
using droid.Runtime.Utilities.GameObjects;
using droid.Runtime.Utilities.Misc;
using UnityEditor;
using UnityEngine;

namespace droid.Runtime.Prototyping.Actors {
  /// <inheritdoc cref="PrototypingGameObject" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ActorComponentMenuPath._ComponentMenuPath + "Vanilla" + ActorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  public class Actor : PrototypingGameObject,
                       IHasRegister<IMotor>,
                       IActor
      //IResetable
  {
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

    Dictionary<string, IMotor> IActor.Motors { get { return this._Motors; } }

    public Transform Transform { get { return this.transform; } }

    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    public virtual void ApplyMotion(IMotorMotion motion) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"Applying {motion} To {this.name}'s motors");
      }
      #endif

      var motion_motor_name = motion.MotorName;
      if (this._Motors.ContainsKey(motion_motor_name) && this._Motors[motion_motor_name] != null) {
        this._Motors[motion_motor_name].ApplyMotion(motion);
      } else {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Could find not motor with the specified name: {motion_motor_name} on actor {this.name}");
        }
        #endif
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public virtual void EnvironmentReset() {
      if (this._Motors != null) {
        foreach (var motor in this._Motors.Values) {
          motor?.EnvironmentReset();
        }
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="identifier"></param>
    public void UnRegister(IMotor motor, string identifier) {
      if (this._Motors != null) {
        if (this._Motors.ContainsKey(identifier)) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log($"Actor {this.name} unregistered motor {identifier}");
          }
          #endif

          this._Motors.Remove(identifier);
        }
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="motor"></param>
    public void UnRegister(IMotor motor) { this.UnRegister(motor, motor.Identifier); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Setup() {
      #if UNITY_EDITOR
      if (!Application.isPlaying) {
        var manager_script = MonoScript.FromMonoBehaviour(this);
        if (MonoImporter.GetExecutionOrder(manager_script) != _script_execution_order) {
          MonoImporter.SetExecutionOrder(
              manager_script,
              _script_execution_order); // Ensures that PreStep is called first, before all other scripts.
          Debug.LogWarning(
              "Execution Order changed, you will need to press play again to make everything function correctly!");
          EditorApplication.isPlaying = false;
          //TODO: UnityEngine.Experimental.LowLevel.PlayerLoop.SetPlayerLoop(new UnityEngine.Experimental.LowLevel.PlayerLoopSystem());
        }
      }
      #endif
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Clear() { this._Motors.Clear(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          this);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() { this._environment?.UnRegister(this); }

    /// <summary>
    /// </summary>
    void Update() {
      if (this._draw_bounds) {
        var corners = Corners.ExtractCorners(
            this.ActorBounds.center,
            this.ActorBounds.extents,
            this.transform);

        Corners.DrawBox(
            corners[0],
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
    /// <param name="motor"></param>
    /// <param name="identifier"></param>
    public void RegisterMotor(IMotor motor, string identifier) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Actor " + this.name + " has motor " + identifier);
      }
      #endif

      if (!this._Motors.ContainsKey(identifier)) {
        this._Motors.Add(identifier, motor);
      } else {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"A motor with the identifier {identifier} is already registered");
        }
        #endif
      }
    }

    #region Fields

    /// <summary>
    /// </summary>
    [Header("References", order = 99)]
    [SerializeField]
    IPrototypingEnvironment _environment;

    /// <summary>
    /// </summary>
    [Header("General", order = 101)]
    [SerializeField]
    protected Dictionary<string, IMotor> _Motors = new Dictionary<string, IMotor>();

    #if UNITY_EDITOR
    const int _script_execution_order = -10;
    #endif

    #endregion

    #region Getters

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "Actor"; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motor"></param>
    public void Register(IMotor motor) { this.RegisterMotor(motor, motor.Identifier); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motor"></param>
    /// <param name="identifier"></param>
    public void Register(IMotor motor, string identifier) { this.RegisterMotor(motor, identifier); }

    /// <summary>
    /// </summary>
    public Dictionary<string, IMotor> Motors { get { return this._Motors; } }

    /// <summary>
    /// </summary>
    public IPrototypingEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    #endregion
  }
}
