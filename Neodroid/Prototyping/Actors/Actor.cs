using System.Collections.Generic;
using Neodroid.Environments;
using Neodroid.Prototyping.Motors;
using Neodroid.Utilities;
using Neodroid.Utilities.BoundingBoxes;
using Neodroid.Utilities.GameObjects;
using Neodroid.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Prototyping.Actors {
  /// <inheritdoc cref="PrototypingGameObject" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ActorComponentMenuPath._ComponentMenuPath + "Vanilla" + ActorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  public class Actor : PrototypingGameObject,
                       IHasRegister<Motor> {
    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    Bounds _bounds;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    bool _draw_bounds;

    /// <summary>
    ///
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

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Setup() { }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Clear() { this._Motors = new Dictionary<string, Motor>(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment = Utilities.Unsorted.NeodroidUtilities.MaybeRegisterComponent(this.ParentEnvironment, this);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this._environment != null) {
        this._environment.UnRegister(this);
      }
    }

    /// <summary>
    ///
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
    ///
    /// </summary>
    /// <param name="motion"></param>
    public virtual void ApplyMotion(Utilities.Messaging.Messages.MotorMotion motion) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Applying " + motion + " To " + this.name + "'s motors");
      }
      #endif

      var motion_motor_name = motion.MotorName;
      if (this._Motors.ContainsKey(motion_motor_name) && this._Motors[motion_motor_name] != null) {
        this._Motors[motion_motor_name].ApplyMotion(motion);
      } else {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("Could find not motor with the specified name: " + motion_motor_name);
        }
        #endif
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="motor"></param>
    /// <param name="identifier"></param>
    public void RegisterMotor(Motor motor, string identifier) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Actor " + this.name + " has motor " + identifier);
      }
      #endif

      if (this._Motors == null) {
        this._Motors = new Dictionary<string, Motor>();
      }

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

    /// <summary>
    ///
    /// </summary>
    /// <param name="identifier"></param>
    public void UnRegisterMotor(string identifier) {
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
    ///
    /// </summary>
    /// <param name="motor"></param>
    public void UnRegister(Motor motor) { this.UnRegisterMotor(motor.Identifier); }

    /// <summary>
    ///
    /// </summary>
    public virtual void Reset() {
      if (this._Motors != null) {
        foreach (var motor in this._Motors.Values) {
          if (motor != null) {
            motor.Reset();
          }
        }
      }
    }

    #region Fields

    /// <summary>
    ///
    /// </summary>
    [Header("References", order = 99)]
    [SerializeField]
    PrototypingEnvironment _environment;

    /// <summary>
    ///
    /// </summary>
    [Header("General", order = 101)]
    [SerializeField]
    protected Dictionary<string, Motor> _Motors;

    #endregion

    #region Getters

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingType { get { return "Actor"; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motor"></param>
    public void Register(Motor motor) { this.RegisterMotor(motor, motor.Identifier); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motor"></param>
    /// <param name="identifier"></param>
    public void Register(Motor motor, string identifier) { this.RegisterMotor(motor, identifier); }

    /// <summary>
    ///
    /// </summary>
    public Dictionary<string, Motor> Motors { get { return this._Motors; } }

    /// <summary>
    ///
    /// </summary>
    public PrototypingEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    #endregion
  }
}
