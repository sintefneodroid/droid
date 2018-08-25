using System;
using Neodroid.Environments;
using Neodroid.Prototyping.Internals;
using Neodroid.Utilities.Interfaces;
using Neodroid.Utilities.Messaging.Messages;
using Neodroid.Utilities.Unsorted;
using UnityEngine;
using Object = System.Object;

namespace Neodroid.Prototyping.Motors {
  /// <inheritdoc cref="Motor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
       MotorComponentMenuPath._ComponentMenuPath + "TargetRigidbody" + MotorComponentMenuPath._Postfix),
   RequireComponent(typeof(Rigidbody))]
  public class TargetRigidbodyMotor : Motor,
                                      IEnvironmentListener {
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    protected Rigidbody _Rigidbody;

    string _movement;
    string _turn;
    Single _movement_speed;
    Single _rotation_speed;
    PrototypingEnvironment _parent_environment;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "TargetRigidbody"; } }

    public Single MovementSpeed { get { return this._movement_speed; } }

    public Single RotationSpeed { get { return this._rotation_speed; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Setup() {
      this._Rigidbody = this.GetComponent<Rigidbody>();

      this._movement = this.Identifier + "Movement_";
      this._turn = this.Identifier + "Turn_";
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentActor,
          (Motor)this,
          this._movement);
      this.ParentActor =
          NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._turn);

      this._parent_environment = NeodroidUtilities.MaybeRegisterComponent(this._parent_environment, this);

      if (this._parent_environment) {
        this._parent_environment.PreStepEvent += this.PreStep;
        this._parent_environment.StepEvent += this.Step;
        this._parent_environment.PostStepEvent += this.PostStep;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(MotorMotion motion) {
      if (motion.MotorName == this._movement) {
        this.ApplyMovement(motion.Strength);
      } else if (motion.MotorName == this._turn) {
        this.ApplyRotation(motion.Strength);
      }
    }

    void ApplyRotation(float rotation_change = 0f) { this._rotation_speed = rotation_change; }

    void ApplyMovement(float movement_change = 0f) { this._movement_speed = movement_change; }

    void OnStep() {
      this._Rigidbody.velocity = Vector3.zero;
      this._Rigidbody.angularVelocity = Vector3.zero;
      
      // Move
      var movement = this.transform.forward * this._movement_speed * Time.deltaTime;
      this._Rigidbody.MovePosition(this._Rigidbody.position + movement);

      // Turn
      var turn = this._rotation_speed * Time.deltaTime;
      var turn_rotation = Quaternion.Euler(0f, turn, 0f);
      this._Rigidbody.MoveRotation(this._Rigidbody.rotation * turn_rotation);
    }

    public void PreStep() { }

    public void Step() { this.OnStep(); }

    public void PostStep() { }
  }
}
