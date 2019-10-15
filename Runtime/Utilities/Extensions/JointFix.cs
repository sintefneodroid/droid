using System;
using UnityEngine;

namespace droid.Runtime.Utilities.Extensions {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [RequireComponent(typeof(Joint))]
  public class JointFix : MonoBehaviour {
    JointDrive[] _angular_x_drive;
    Rigidbody[] _connected_bodies;
    bool[] _enable_processings;
    float[] _force_break_limits;
    Vector3 _initial_local_position;
    Quaternion _initial_local_rotation;
    Type[] _joint_types;

    Joint[] _joints;
    JointLimits[] _limits;
    SoftJointLimitSpring[] _linear_limit_springs;
    SoftJointLimit[] _linear_limits;
    Vector3 _local_position_on_disable;

    Quaternion _local_rotation_on_disable;
    JointDrive[] _slerp_drives;
    Vector3[] _target_angulars;
    Vector3[] _target_positions;
    Quaternion[] _target_rotations;
    Vector3[] _target_velocities;
    float[] _torque_break_limits;

    [SerializeField] bool counting = false;
    [SerializeField] int resetAfterFrames = 500;
    int _frames_counted;

    bool _was_disabled;
    SoftJointLimit[] _x_ang_high_limits;
    SoftJointLimit[] _x_ang_low_limits;
    ConfigurableJointMotion[] _x_ang_motion;
    JointDrive[] _x_drives;
    ConfigurableJointMotion[] _x_motion;
    ConfigurableJointMotion[] _y_ang_motion;
    JointDrive[] _y_drives;
    ConfigurableJointMotion[] _y_motion;
    ConfigurableJointMotion[] _z_ang_motion;
    JointDrive[] _z_drives;
    ConfigurableJointMotion[] _z_motion;

    void Awake() { this.Setup(); }

    void Setup() {
      this._initial_local_rotation = this.transform.localRotation;
      this._initial_local_position = this.transform.localPosition;
      this._joints = this.GetComponents<Joint>();
      var length = this._joints.Length;
      this._connected_bodies = new Rigidbody[length];
      this._joint_types = new Type[length];
      this._x_ang_low_limits = new SoftJointLimit[length];
      this._x_ang_high_limits = new SoftJointLimit[length];
      this._limits = new JointLimits[length];
      this._force_break_limits = new float[length];
      this._torque_break_limits = new float[length];
      this._x_motion = new ConfigurableJointMotion[length];
      this._y_motion = new ConfigurableJointMotion[length];
      this._z_motion = new ConfigurableJointMotion[length];
      this._x_ang_motion = new ConfigurableJointMotion[length];
      this._y_ang_motion = new ConfigurableJointMotion[length];
      this._z_ang_motion = new ConfigurableJointMotion[length];
      this._angular_x_drive = new JointDrive[length];
      this._target_rotations = new Quaternion[length];
      this._target_angulars = new Vector3[length];
      this._target_positions = new Vector3[length];
      this._target_velocities = new Vector3[length];
      this._x_drives = new JointDrive[length];
      this._y_drives = new JointDrive[length];
      this._z_drives = new JointDrive[length];
      this._linear_limits = new SoftJointLimit[length];
      this._linear_limit_springs = new SoftJointLimitSpring[length];
      this._slerp_drives = new JointDrive[length];
      this._enable_processings = new bool[length];

      for (var i = 0; i < length; i++) {
        this._connected_bodies[i] = this._joints[i].connectedBody;
        this._joint_types[i] = this._joints[i].GetType();
        this._force_break_limits[i] = this._joints[i].breakForce;
        this._torque_break_limits[i] = this._joints[i].breakTorque;
        this._enable_processings[i] = this._joints[i].enablePreprocessing;
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
          this._linear_limits[i] = ((ConfigurableJoint)this._joints[i]).linearLimit;
          this._slerp_drives[i] = ((ConfigurableJoint)this._joints[i]).slerpDrive;
          this._linear_limit_springs[i] = ((ConfigurableJoint)this._joints[i]).linearLimitSpring;
          this._x_drives[i] = ((ConfigurableJoint)this._joints[i]).xDrive;
          this._y_drives[i] = ((ConfigurableJoint)this._joints[i]).yDrive;
          this._z_drives[i] = ((ConfigurableJoint)this._joints[i]).zDrive;
          this._target_positions[i] = ((ConfigurableJoint)this._joints[i]).targetPosition;
          this._target_velocities[i] = ((ConfigurableJoint)this._joints[i]).targetVelocity;
          this._target_angulars[i] = ((ConfigurableJoint)this._joints[i]).targetAngularVelocity;
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
      if (this._frames_counted >= this.resetAfterFrames) {
        this.JointReset();
        this._frames_counted = 0;
      }

      if (this.counting) {
        this._frames_counted++;
      }

      if (this._was_disabled) {
        this._was_disabled = false;
        this.transform.localRotation = this._local_rotation_on_disable;
        this.transform.localPosition = this._local_position_on_disable;
      }
    }

    /// <summary>
    /// </summary>
    public void JointReset() {
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
        this._joints[i].enablePreprocessing = this._enable_processings[i];

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
          ((ConfigurableJoint)this._joints[i]).linearLimit = this._linear_limits[i];
          ((ConfigurableJoint)this._joints[i]).linearLimitSpring = this._linear_limit_springs[i];
          ((ConfigurableJoint)this._joints[i]).xDrive = this._x_drives[i];
          ((ConfigurableJoint)this._joints[i]).yDrive = this._y_drives[i];
          ((ConfigurableJoint)this._joints[i]).zDrive = this._z_drives[i];
          ((ConfigurableJoint)this._joints[i]).targetPosition = this._target_positions[i];
          ((ConfigurableJoint)this._joints[i]).targetVelocity = this._target_velocities[i];
          ((ConfigurableJoint)this._joints[i]).targetAngularVelocity = this._target_angulars[i];
          ((ConfigurableJoint)this._joints[i]).slerpDrive = this._slerp_drives[i];
        }
      }
    }
  }
}
