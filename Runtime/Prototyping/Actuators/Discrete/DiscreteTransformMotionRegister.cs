using UnityEngine;

namespace droid.Runtime.Prototyping.Actuators.Discrete {
  /// <summary>
  /// A register of functionality to performed on transforms
  /// </summary>
  public class DiscreteTransformMotionRegister : MonoBehaviour {
    [SerializeField] float translation_unit = 1;
    [SerializeField] float rotation_unit = 90;

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

    void CheckCollisionTranslate(Vector3 vec) {
      var layer_mask = 1 << LayerMask.NameToLayer(layerName : this._Layer_Mask);
      if (this._No_Collisions) {
        if (!Physics.Raycast(origin : this.transform.position,
                             this.transform.TransformDirection(direction : vec.normalized),
                             Mathf.Abs(f : vec.magnitude),
                             layerMask : layer_mask)) {
          this.transform.Translate(translation : vec, relativeTo : this._Relative_To);
        }
      } else {
        this.transform.Translate(translation : vec, relativeTo : this._Relative_To);
      }
    }

    /// <summary>
    /// Self explanatory
    /// </summary>
    public void TranslateUnitForward() { this.CheckCollisionTranslate(Vector3.forward * this.translation_unit); }

    /// <summary>
    /// Self explanatory
    /// </summary>
    public void TranslateUnitBackward() { this.CheckCollisionTranslate(Vector3.back * this.translation_unit); }

    /// <summary>
    /// Self explanatory
    /// </summary>
    public void TranslateUnitLeft() { this.CheckCollisionTranslate(Vector3.left * this.translation_unit); }

    /// <summary>
    /// Self explanatory
    /// </summary>
    public void TranslateUnitRight() { this.CheckCollisionTranslate(Vector3.right * this.translation_unit); }

    /// <summary>
    /// Self explanatory
    /// </summary>
    public void TranslateUnitUp() { this.CheckCollisionTranslate(Vector3.up * this.translation_unit); }

    /// <summary>
    /// Self explanatory
    /// </summary>
    public void TranslateUnitDown() { this.CheckCollisionTranslate(Vector3.down * this.translation_unit); }

    /// <summary>
    /// Self explanatory
    /// </summary>
    public void RotateUnitYClockWise() { this.transform.Rotate(axis : Vector3.up, angle : this.rotation_unit); }

    /// <summary>
    /// Self explanatory
    /// </summary>
    public void RotateUnitYAntiClockWise() { this.transform.Rotate(axis : Vector3.up, angle : -this.rotation_unit); }

    /// <summary>
    /// Self explanatory
    /// </summary>
    public void RotateUnitXClockWise() { this.transform.Rotate(axis : Vector3.left, angle : this.rotation_unit); }

    /// <summary>
    /// Self explanatory
    /// </summary>
    public void RotateUnitXAntiClockWise() { this.transform.Rotate(axis : Vector3.left, angle : -this.rotation_unit); }

    /// <summary>
    /// Self explanatory
    /// </summary>
    public void RotateUnitZClockWise() { this.transform.Rotate(axis : Vector3.forward, angle : -this.rotation_unit); }

    /// <summary>
    /// Self explanatory
    /// </summary>
    public void RotateUnitZAntiClockWise() { this.transform.Rotate(axis : Vector3.forward, angle : this.rotation_unit); }
  }
}
