using System;
using UnityEngine;

namespace Neodroid.Utilities.Unsorted {
  [RequireComponent(typeof(Joint))]
  public class JointFix : MonoBehaviour {
    JointDrive[] _angular_x_drive;
    Rigidbody[] _connected_bodies;
    float[] _force_break_limits;
    Vector3 _initial_local_position;
    Quaternion _initial_local_rotation;
    Type[] _joint_types;

    Joint[] _joints;
    JointLimits[] _limits;
    Vector3 _local_position_on_disable;

    Quaternion _local_rotation_on_disable;
    Quaternion[] _target_rotations;
    float[] _torque_break_limits;

    bool _was_disabled;
    SoftJointLimit[] _x_ang_high_limits;
    SoftJointLimit[] _x_ang_low_limits;
    ConfigurableJointMotion[] _x_ang_motion;
    ConfigurableJointMotion[] _x_motion;
    ConfigurableJointMotion[] _y_ang_motion;
    ConfigurableJointMotion[] _y_motion;
    ConfigurableJointMotion[] _z_ang_motion;
    ConfigurableJointMotion[] _z_motion;

    void Awake() { this.Setup(); }

    void Setup() {
      this._initial_local_rotation = this.transform.localRotation;
      this._initial_local_position = this.transform.localPosition;
      this._joints = this.GetComponents<Joint>();
      this._connected_bodies = new Rigidbody[this._joints.Length];
      this._joint_types = new Type[this._joints.Length];
      this._x_ang_low_limits = new SoftJointLimit[this._joints.Length];
      this._x_ang_high_limits = new SoftJointLimit[this._joints.Length];
      this._limits = new JointLimits[this._joints.Length];
      this._force_break_limits = new float[this._joints.Length];
      this._torque_break_limits = new float[this._joints.Length];
      this._x_motion = new ConfigurableJointMotion[this._joints.Length];
      this._y_motion = new ConfigurableJointMotion[this._joints.Length];
      this._z_motion = new ConfigurableJointMotion[this._joints.Length];
      this._x_ang_motion = new ConfigurableJointMotion[this._joints.Length];
      this._y_ang_motion = new ConfigurableJointMotion[this._joints.Length];
      this._z_ang_motion = new ConfigurableJointMotion[this._joints.Length];
      this._angular_x_drive = new JointDrive[this._joints.Length];
      this._target_rotations = new Quaternion[this._joints.Length];

      for (var i = 0; i < this._joints.Length; i++) {
        this._connected_bodies[i] = this._joints[i].connectedBody;
        this._joint_types[i] = this._joints[i].GetType();
        this._force_break_limits[i] = this._joints[i].breakForce;
        this._torque_break_limits[i] = this._joints[i].breakTorque;
        if (this._joints[i] is HingeJoint) {
          this._limits[i] = ((HingeJoint)this._joints[i]).limits;
        } else if (this._joints[i] is ConfigurableJoint) {
          this._x_ang_low_limits[i] = ((ConfigurableJoint)this._joints[i]).lowAngularXLimit;
          this._x_ang_high_limits[i] = ((ConfigurableJoint)this._joints[i]).highAngularXLimit;
          this._x_motion[i] = ((ConfigurableJoint)this._joints[i]).xMotion;
          this._y_motion[i] = ((ConfigurableJoint)this._joints[i]).yMotion;
          this._z_motion[i] = ((ConfigurableJoint)this._joints[i]).zMotion;
          this._x_ang_motion[i] = ((ConfigurableJoint)this._joints[i]).angularXMotion;
          this._y_ang_motion[i] = ((ConfigurableJoint)this._joints[i]).angularYMotion;
          this._z_ang_motion[i] = ((ConfigurableJoint)this._joints[i]).angularZMotion;
          this._angular_x_drive[i] = ((ConfigurableJoint)this._joints[i]).angularXDrive;
          this._target_rotations[i] = ((ConfigurableJoint)this._joints[i]).targetRotation;
        }
      }
    }

    void OnDisable() {
      this._local_rotation_on_disable = this.transform.localRotation;
      this.transform.localRotation = this._initial_local_rotation;

      this._local_position_on_disable = this.transform.localPosition;
      this.transform.localPosition = this._initial_local_position;

      this._was_disabled = true;
    }

    void Update() {
      if (this._was_disabled) {
        this._was_disabled = false;
        this.transform.localRotation = this._local_rotation_on_disable;
        this.transform.localPosition = this._local_position_on_disable;
      }
    }

    /// <summary>
    ///
    /// </summary>
    public void Reset() {
      if (this._joints == null) {
        this.Setup();
      }

      this.transform.localRotation = this._initial_local_rotation;
      this.transform.localPosition = this._initial_local_position;
      for (var i = 0; i < this._joints.Length; i++) {
        if (this._joints[i] == null) {
          this._joints[i] = (Joint)this.gameObject.AddComponent(this._joint_types[i]);
          this._joints[i].connectedBody = this._connected_bodies[i];
        }

        this._joints[i].breakForce = this._force_break_limits[i];
        this._joints[i].breakTorque = this._torque_break_limits[i];

        if (this._joints[i] is HingeJoint) {
          ((HingeJoint)this._joints[i]).limits = this._limits[i];
        } else if (this._joints[i] is SpringJoint) {
          //((SpringJoint)this._joints[i]).anchor = this.anchor[i];
        } else if (this._joints[i] is ConfigurableJoint) {
          ((ConfigurableJoint)this._joints[i]).lowAngularXLimit = this._x_ang_low_limits[i];
          ((ConfigurableJoint)this._joints[i]).highAngularXLimit = this._x_ang_high_limits[i];
          ((ConfigurableJoint)this._joints[i]).xMotion = this._x_motion[i];
          ((ConfigurableJoint)this._joints[i]).yMotion = this._y_motion[i];
          ((ConfigurableJoint)this._joints[i]).zMotion = this._z_motion[i];
          ((ConfigurableJoint)this._joints[i]).angularXMotion = this._x_ang_motion[i];
          ((ConfigurableJoint)this._joints[i]).angularYMotion = this._y_ang_motion[i];
          ((ConfigurableJoint)this._joints[i]).angularZMotion = this._z_ang_motion[i];
          ((ConfigurableJoint)this._joints[i]).angularXDrive = this._angular_x_drive[i];
          ((ConfigurableJoint)this._joints[i]).targetRotation = this._target_rotations[i];
        }
      }
    }
  }
}
