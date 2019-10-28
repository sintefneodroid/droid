using droid.Runtime.Interfaces;
using droid.Runtime.Utilities;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace droid.Runtime.Prototyping.Actuators {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ActuatorComponentMenuPath._ComponentMenuPath
                    + "EulerTransform3Dof"
                    + ActuatorComponentMenuPath._Postfix)]
  public class EulerTransform3DofActuator : Actuator {
    /// <summary>
    /// </summary>
    [SerializeField]
    protected string _Layer_Mask = "Obstructions";

    /// <summary>
    /// </summary>
    [SerializeField]
    protected bool _No_Collisions = true;

    /// <summary>
    /// </summary>
    [SerializeField]
    protected Space _Relative_To = Space.Self;

    /// <summary>
    /// </summary>
    [SerializeField]
    protected bool _angular_Actuators;

    /// <summary>
    /// </summary>
    [SerializeField]
    protected bool _Use_Mask = true;

    /// <summary>
    ///   XAxisIdentifier
    /// </summary>
    string _x;

    /// <summary>
    ///   YAxisIdentifier
    /// </summary>
    string _y;

    /// <summary>
    ///   ZAxisIdentifier
    /// </summary>
    string _z;

    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "EulerTransform"; } }

    /// <summary>
    /// </summary>
    public override void Setup() {
      if (!this._angular_Actuators) {
        this._x = this.Identifier + "X_";
        this._y = this.Identifier + "Y_";
        this._z = this.Identifier + "Z_";
      } else {
        this._x = this.Identifier + "RotX_";
        this._y = this.Identifier + "RotY_";
        this._z = this.Identifier + "RotZ_";
      }
    }

    public override string[] InnerMotionNames => new[] {this._x, this._y, this._z};

    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.Parent =
          NeodroidRegistrationUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent,
                                                          this,
                                                          this._x);
      this.Parent =
          NeodroidRegistrationUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent,
                                                          this,
                                                          this._y);
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
      this.Parent?.UnRegister(this, this._y);
      this.Parent?.UnRegister(this, this._z);
    }

    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(IMotion motion) {
      var layer_mask = 1 << LayerMask.NameToLayer(this._Layer_Mask);
      if (!this._angular_Actuators) {
        if (motion.ActuatorName == this._x) {
          var vec = Vector3.right * motion.Strength;
          if (this._No_Collisions) {
            if (!Physics.Raycast(this.transform.position,
                                 vec,
                                 Mathf.Abs(motion.Strength),
                                 layer_mask)) {
              this.transform.Translate(vec, this._Relative_To);
            }
          } else {
            this.transform.Translate(vec, this._Relative_To);
          }
        } else if (motion.ActuatorName == this._y) {
          var vec = -Vector3.up * motion.Strength;
          if (this._No_Collisions) {
            if (!Physics.Raycast(this.transform.position,
                                 vec,
                                 Mathf.Abs(motion.Strength),
                                 layer_mask)) {
              this.transform.Translate(vec, this._Relative_To);
            }
          } else {
            this.transform.Translate(vec, this._Relative_To);
          }
        } else if (motion.ActuatorName == this._z) {
          var vec = -Vector3.forward * motion.Strength;
          if (this._No_Collisions) {
            if (!Physics.Raycast(this.transform.position,
                                 vec,
                                 Mathf.Abs(motion.Strength),
                                 layer_mask)) {
              this.transform.Translate(vec, this._Relative_To);
            }
          } else {
            this.transform.Translate(vec, this._Relative_To);
          }
        }
      } else {
        if (motion.ActuatorName == this._x) {
          this.transform.Rotate(Vector3.right, motion.Strength, this._Relative_To);
        } else if (motion.ActuatorName == this._y) {
          this.transform.Rotate(Vector3.up, motion.Strength, this._Relative_To);
        } else if (motion.ActuatorName == this._z) {
          this.transform.Rotate(Vector3.forward, motion.Strength, this._Relative_To);
        }
      }
    }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected() {
      if (this.enabled) {
        var position = this.transform.position;
        if (this._angular_Actuators) {
          Handles.DrawWireArc(this.transform.position,
                              this.transform.right,
                              -this.transform.forward,
                              180,
                              2);
          Handles.DrawWireArc(this.transform.position,
                              this.transform.up,
                              -this.transform.right,
                              180,
                              2);
          Handles.DrawWireArc(this.transform.position,
                              this.transform.forward,
                              -this.transform.right,
                              180,
                              2);
        } else {
          Debug.DrawLine(position, position + Vector3.up * 2, Color.green);
          Debug.DrawLine(position, position + Vector3.forward * 2, Color.green);
          Debug.DrawLine(position, position + Vector3.right * 2, Color.green);
        }
      }
    }
    #endif
  }
}
