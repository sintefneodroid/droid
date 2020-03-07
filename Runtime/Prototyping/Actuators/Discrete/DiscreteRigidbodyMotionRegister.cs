using System;
using UnityEngine;

namespace droid.Runtime.Prototyping.Actuators.Discrete {
  /// <inheritdoc />
  /// <summary>
  /// A register of functionality to performed on Rigidbody
  /// es
  /// </summary>
  [RequireComponent(requiredComponent : typeof(Rigidbody))]
  public class DiscreteRigidbodyMotionRegister : MonoBehaviour {
    [SerializeField] float force_unit = 1;
    [SerializeField] float torque_unit = 90;

    /// <summary>
    /// </summary>
    [SerializeField]
    protected Space _Relative_To = Space.Self;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    protected ForceMode _force_mode = ForceMode.Acceleration;

    Rigidbody _rb;

    void Awake() { this._rb = this.GetComponent<Rigidbody>(); }

    void AddForce(Vector3 vec) {
      if (this._rb) {
        switch (this._Relative_To) {
          case Space.World:
            this._rb.AddForce(force : vec, mode : this._force_mode);
            break;
          case Space.Self:
            this._rb.AddRelativeForce(force : vec, mode : this._force_mode);
            break;
          default: throw new ArgumentOutOfRangeException();
        }
      }
    }

    void AddTorque(Vector3 vec) {
      if (this._rb) {
        switch (this._Relative_To) {
          case Space.World:
            this._rb.AddTorque(torque : vec, mode : this._force_mode);
            break;
          case Space.Self:
            this._rb.AddRelativeTorque(torque : vec, mode : this._force_mode);
            break;
          default: throw new ArgumentOutOfRangeException();
        }
      }
    }

    /// <summary>
    /// Self explanatory
    /// </summary>
    public void AddUnitForceForward() { this.AddForce(vec : Vector3.forward * this.force_unit); }

    /// <summary>
    /// Self explanatory
    /// </summary>
    public void AddUnitForceBackward() { this.AddForce(vec : Vector3.back * this.force_unit); }

    /// <summary>
    /// Self explanatory
    /// </summary>
    public void AddUnitForceLeft() { this.AddForce(vec : Vector3.left * this.force_unit); }

    /// <summary>
    /// Self explanatory
    /// </summary>
    public void AddUnitForceRight() { this.AddForce(vec : Vector3.right * this.force_unit); }

    /// <summary>
    /// Self explanatory
    /// </summary>
    public void AddUnitForceUp() { this.AddForce(vec : Vector3.up * this.force_unit); }

    /// <summary>
    /// Self explanatory
    /// </summary>
    public void AddUnitForceDown() { this.AddForce(vec : Vector3.down * this.force_unit); }

    /// <summary>
    /// Self explanatory
    /// </summary>
    public void AddUnitTorqueYClockWise() { this.AddTorque(vec : Vector3.up * this.torque_unit); }

    /// <summary>
    /// Self explanatory
    /// </summary>
    public void AddUnitTorqueYAntiClockWise() { this.AddTorque(vec : Vector3.up * -this.torque_unit); }

    /// <summary>
    /// Self explanatory
    /// </summary>
    public void AddUnitTorqueXClockWise() { this.AddTorque(vec : Vector3.left * this.torque_unit); }

    /// <summary>
    /// Self explanatory
    /// </summary>
    public void AddUnitTorqueXAntiClockWise() { this.AddTorque(vec : Vector3.left * -this.torque_unit); }

    /// <summary>
    /// Self explanatory
    /// </summary>
    public void AddUnitTorqueZClockWise() { this.AddTorque(vec : Vector3.forward * -this.torque_unit); }

    /// <summary>
    /// Self explanatory
    /// </summary>
    public void AddUnitTorqueZAntiClockWise() { this.AddTorque(vec : Vector3.forward * this.torque_unit); }
  }
}
