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
  [AddComponentMenu(menuName : ActuatorComponentMenuPath._ComponentMenuPath
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

    /// <inheritdoc />
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

    /// <summary>
    ///
    /// </summary>
    public override string[] InnerMotionNames => new[] {this._x, this._y, this._z};

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.Parent =
          NeodroidRegistrationUtilities.RegisterComponent(r : (IHasRegister<IActuator>)this.Parent,
                                                          c : this,
                                                          identifier : this._x);
      this.Parent =
          NeodroidRegistrationUtilities.RegisterComponent(r : (IHasRegister<IActuator>)this.Parent,
                                                          c : this,
                                                          identifier : this._y);
      this.Parent =
          NeodroidRegistrationUtilities.RegisterComponent(r : (IHasRegister<IActuator>)this.Parent,
                                                          c : this,
                                                          identifier : this._z);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      this.Parent?.UnRegister(t : this, obj : this._x);
      this.Parent?.UnRegister(t : this, obj : this._y);
      this.Parent?.UnRegister(t : this, obj : this._z);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(IMotion motion) {
      var layer_mask = 1 << LayerMask.NameToLayer(layerName : this._Layer_Mask);
      if (!this._angular_Actuators) {
        if (motion.ActuatorName == this._x) {
          var vec = Vector3.right * motion.Strength;
          if (this._No_Collisions) {
            if (!Physics.Raycast(origin : this.transform.position,
                                 direction : vec,
                                 maxDistance : Mathf.Abs(f : motion.Strength),
                                 layerMask : layer_mask)) {
              this.transform.Translate(translation : vec, relativeTo : this._Relative_To);
            }
          } else {
            this.transform.Translate(translation : vec, relativeTo : this._Relative_To);
          }
        } else if (motion.ActuatorName == this._y) {
          var vec = -Vector3.up * motion.Strength;
          if (this._No_Collisions) {
            if (!Physics.Raycast(origin : this.transform.position,
                                 direction : vec,
                                 maxDistance : Mathf.Abs(f : motion.Strength),
                                 layerMask : layer_mask)) {
              this.transform.Translate(translation : vec, relativeTo : this._Relative_To);
            }
          } else {
            this.transform.Translate(translation : vec, relativeTo : this._Relative_To);
          }
        } else if (motion.ActuatorName == this._z) {
          var vec = -Vector3.forward * motion.Strength;
          if (this._No_Collisions) {
            if (!Physics.Raycast(origin : this.transform.position,
                                 direction : vec,
                                 maxDistance : Mathf.Abs(f : motion.Strength),
                                 layerMask : layer_mask)) {
              this.transform.Translate(translation : vec, relativeTo : this._Relative_To);
            }
          } else {
            this.transform.Translate(translation : vec, relativeTo : this._Relative_To);
          }
        }
      } else {
        if (motion.ActuatorName == this._x) {
          this.transform.Rotate(axis : Vector3.right,
                                angle : motion.Strength,
                                relativeTo : this._Relative_To);
        } else if (motion.ActuatorName == this._y) {
          this.transform.Rotate(axis : Vector3.up, angle : motion.Strength, relativeTo : this._Relative_To);
        } else if (motion.ActuatorName == this._z) {
          this.transform.Rotate(axis : Vector3.forward,
                                angle : motion.Strength,
                                relativeTo : this._Relative_To);
        }
      }
    }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected() {
      if (this.enabled) {
        var position = this.transform.position;
        if (this._angular_Actuators) {
          Handles.DrawWireArc(center : this.transform.position,
                              normal : this.transform.right,
                              @from : -this.transform.forward,
                              180,
                              2);
          Handles.DrawWireArc(center : this.transform.position,
                              normal : this.transform.up,
                              @from : -this.transform.right,
                              180,
                              2);
          Handles.DrawWireArc(center : this.transform.position,
                              normal : this.transform.forward,
                              @from : -this.transform.right,
                              180,
                              2);
        } else {
          Debug.DrawLine(start : position, end : position + Vector3.up * 2, color : Color.green);
          Debug.DrawLine(start : position, end : position + Vector3.forward * 2, color : Color.green);
          Debug.DrawLine(start : position, end : position + Vector3.right * 2, color : Color.green);
        }
      }
    }
    #endif
  }
}
