using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables {
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "QuaternionTransform"
                    + ConfigurableComponentMenuPath._Postfix)]
  public class QuaternionTransformConfigurable : Configurable,
                                                 IHasQuaternionTransform {
    [Header("Specific", order = 102)]
    [SerializeField]
    Vector3 _position;

    [SerializeField] Quaternion _rotation;

    [SerializeField] string _pos_x = "pos_x";
    [SerializeField] string _pos_y = "pos_y";
    [SerializeField] string _pos_z = "pos_z";

    [SerializeField] string _rot_w = "row_w";
    [SerializeField] string _rot_x = "rot_x";
    [SerializeField] string _rot_y = "rot_y";
    [SerializeField] string _rot_z = "rot_z";

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "QuaternionTransformConfigurable"; } }

    /// <summary>
    /// </summary>
    public Quaternion Rotation { get { return this._rotation; } }

    /// <summary>
    /// </summary>
    public Vector3 Position { get { return this._position; } }

    /// <summary>
    ///
    /// </summary>
    protected override void PreSetup() {
      this._pos_x = this.Identifier + "pos_x";
      this._pos_y = this.Identifier + "pos_y";
      this._pos_z = this.Identifier + "pos_z";

      this._rot_x = this.Identifier + "rot_x";
      this._rot_y = this.Identifier + "rot_y";
      this._rot_z = this.Identifier + "rot_z";
      this._rot_w = this.Identifier + "row_w";
    }

    void Reset() {
      var transform1 = this.transform;
      transform1.position = this._position;
      transform1.rotation = this._rotation;
    }

    public override ISpace ConfigurableValueSpace { get; }

    public override void ApplyConfiguration(IConfigurableConfiguration obj) {
      if (obj.ConfigurableName == this._pos_x) {
        this._position.x = obj.ConfigurableValue;
      } else if (obj.ConfigurableName == this._pos_y) {
        this._position.y = obj.ConfigurableValue;
      } else if (obj.ConfigurableName == this._pos_z) {
        this._position.z = obj.ConfigurableValue;
      } else if (obj.ConfigurableName == this._rot_x) {
        this._rotation.x = obj.ConfigurableValue;
      } else if (obj.ConfigurableName == this._rot_y) {
        this._rotation.y = obj.ConfigurableValue;
      } else if (obj.ConfigurableName == this._rot_z) {
        this._rotation.z = obj.ConfigurableValue;
      } else if (obj.ConfigurableName == this._rot_w) {
        this._rotation.w = obj.ConfigurableValue;
      }

      this.Reset();
    }
  }
}
