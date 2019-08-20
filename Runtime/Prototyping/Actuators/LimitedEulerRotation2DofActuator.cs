using System;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using droid.Runtime.Utilities;
using UnityEngine;
using Object = System.Object;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace droid.Runtime.Prototyping.Actuators {
  /// <inheritdoc />
  [AddComponentMenu(ActuatorComponentMenuPath._ComponentMenuPath
                    + "LimitedEulerRotationActuator2Dof"
                    + ActuatorComponentMenuPath._Postfix)]
  public class LimitedEulerRotation2DofActuator : Actuator {
    /// <summary>
    ///
    /// </summary>
    public override string PrototypingTypeName { get { return "EulerRotation"; } }

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    protected Space1 limits = Space1.MinusOneOne * 0.25f;

    [SerializeField] string _x;
    [SerializeField] string _z;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this._x = this.Identifier + "RotX_";

      this._z = this.Identifier + "RotZ_";

      this.Parent =
          NeodroidRegistrationUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent,
                                                          this,
                                                          this._x);
      this.Parent =
          NeodroidRegistrationUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent,
                                                          this,
                                                          this._z);
    }

    /// <summary>
    ///
    /// </summary>
    protected override void UnRegisterComponent() {
      this.Parent?.UnRegister(this, this._x);
      this.Parent?.UnRegister(this, this._z);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    /// <exception cref="T:System.ArgumentOutOfRangeException"></exception>
    protected override void InnerApplyMotion(IMotion motion) {
      var m = 2f * Mathf.Clamp(motion.Strength, -1f, 1f);
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Inner Applying " + m + " To " + this.name);
      }
      #endif
      if (motion.ActuatorName == this._x) {
        if (this.transform.rotation.x < this.limits.MaxValue && m > 0f
            || this.transform.rotation.x > this.limits.MinValue && m < 0f) {
          this.transform.Rotate(new Vector3(1, 0, 0), m);
        }
      } else if (motion.ActuatorName == this._z) {
        if (this.transform.rotation.z < this.limits.MaxValue && m > 0f
            || this.transform.rotation.z > this.limits.MinValue && m < 0f) {
          this.transform.Rotate(new Vector3(0, 0, 1), m);
        }
      }
    }

    public override String[] InnerMotionNames { get; }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected() {
      if (this.enabled) {
        var position = this.transform.position;

        Handles.DrawWireArc(this.transform.position, this.transform.right, -this.transform.forward, 180, 2);

        Handles.DrawWireArc(this.transform.position, this.transform.forward, -this.transform.right, 180, 2);
      }
    }

    #endif
  }
}
