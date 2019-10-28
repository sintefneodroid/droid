using System;
using droid.Runtime.Environments.Prototyping;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.Prototyping.Actuators {
  /// <inheritdoc cref="Actuator" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ActuatorComponentMenuPath._ComponentMenuPath
                    + "TargetRigidbody"
                    + ActuatorComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Rigidbody))]
  public class TargetRigidbodyActuator : Actuator
                                          {
    string _movement;
    AbstractSpatialPrototypingEnvironment _parent_environment;

    /// <summary>
    /// </summary>
    [SerializeField]
    protected Rigidbody _Rigidbody;

    string _turn;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "TargetRigidbody"; } }

    /// <summary>
    ///
    /// </summary>
    public Single MovementSpeed { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Single RotationSpeed { get; set; }

    /// <summary>
    ///
    /// </summary>
    public override void Tick() { this.OnStep(); }


    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() {
      this._Rigidbody = this.GetComponent<Rigidbody>();

      this._movement = this.Identifier + "Movement_";
      this._turn = this.Identifier + "Turn_";
    }

    public override string[] InnerMotionNames => new[] {this._movement, this._turn};

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.Parent =
          NeodroidRegistrationUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent,
                                                          (Actuator)this,
                                                          this._movement);
      this.Parent =
          NeodroidRegistrationUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent,
                                                          (Actuator)this,
                                                          this._turn);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(IMotion motion) {
      if (motion.ActuatorName == this._movement) {
        this.ApplyMovement(motion.Strength);
      } else if (motion.ActuatorName == this._turn) {
        this.ApplyRotation(motion.Strength);
      }
    }

    void ApplyRotation(float rotation_change = 0f) { this.RotationSpeed = rotation_change; }

    void ApplyMovement(float movement_change = 0f) { this.MovementSpeed = movement_change; }

    void OnStep() {
      this._Rigidbody.velocity = Vector3.zero;
      this._Rigidbody.angularVelocity = Vector3.zero;

      // Move
      var movement = this.MovementSpeed * Time.deltaTime * this.transform.forward;
      this._Rigidbody.MovePosition(this._Rigidbody.position + movement);

      // Turn
      var turn = this.RotationSpeed * Time.deltaTime;
      var turn_rotation = Quaternion.Euler(0f, turn, 0f);
      this._Rigidbody.MoveRotation(this._Rigidbody.rotation * turn_rotation);
    }
  }
}
