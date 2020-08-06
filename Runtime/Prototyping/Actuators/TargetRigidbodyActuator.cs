using System;
using droid.Runtime.Environments.Prototyping;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.Prototyping.Actuators {
  /// <inheritdoc cref="Actuator" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(menuName : ActuatorComponentMenuPath._ComponentMenuPath
                               + "TargetRigidbody"
                               + ActuatorComponentMenuPath._Postfix)]
  [RequireComponent(requiredComponent : typeof(Rigidbody))]
  public class TargetRigidbodyActuator : Actuator {
    string _movement;
    AbstractPrototypingEnvironment _parent_environment;

    /// <summary>
    /// </summary>
    [SerializeField]
    protected Rigidbody _Rigidbody;

    string _turn;

    /// <summary>
    ///
    /// </summary>
    public float MovementSpeed { get; set; }

    /// <summary>
    ///
    /// </summary>
    public float RotationSpeed { get; set; }

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
      this.Parent = NeodroidRegistrationUtilities.RegisterComponent(r : (IHasRegister<IActuator>)this.Parent,
                                                                    c : (Actuator)this,
                                                                    identifier : this._movement);
      this.Parent = NeodroidRegistrationUtilities.RegisterComponent(r : (IHasRegister<IActuator>)this.Parent,
                                                                    c : (Actuator)this,
                                                                    identifier : this._turn);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(IMotion motion) {
      if (motion.ActuatorName == this._movement) {
        this.ApplyMovement(movement_change : motion.Strength);
      } else if (motion.ActuatorName == this._turn) {
        this.ApplyRotation(rotation_change : motion.Strength);
      }
    }

    void ApplyRotation(float rotation_change = 0f) { this.RotationSpeed = rotation_change; }

    void ApplyMovement(float movement_change = 0f) { this.MovementSpeed = movement_change; }

    void OnStep() {
      this._Rigidbody.velocity = Vector3.zero;
      this._Rigidbody.angularVelocity = Vector3.zero;

      // Move
      var movement = this.MovementSpeed * Time.deltaTime * this.transform.forward;
      this._Rigidbody.MovePosition(position : this._Rigidbody.position + movement);

      // Turn
      var turn = this.RotationSpeed * Time.deltaTime;
      var turn_rotation = Quaternion.Euler(0f, y : turn, 0f);
      this._Rigidbody.MoveRotation(rot : this._Rigidbody.rotation * turn_rotation);
    }
  }
}
