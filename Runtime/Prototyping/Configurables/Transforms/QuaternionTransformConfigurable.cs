using System;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables.Transforms {
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "QuaternionTransform"
                    + ConfigurableComponentMenuPath._Postfix)]
  public class QuaternionTransformConfigurable : Configurable,
                                                 IHasQuaternionTransform {
    [Header("Specific", order = 102)]
    [SerializeField]
    Vector3 _position;

    [SerializeField] Quaternion _rotation;

     string _pos_x = "pos_x";
    string _pos_y = "pos_y";
    string _pos_z = "pos_z";

    string _rot_w = "row_w";
    string _rot_x = "rot_x";
    string _rot_y = "rot_y";
    string _rot_z = "rot_z";

    /// <summary>
    /// </summary>
    public Quaternion Rotation { get { return this._rotation; } }

    public Space1 PositionSpace { get; } //TODO: Implement
    public Space1 RotationSpace { get; } //TODO: Implement

    /// <summary>
    /// </summary>
    public Vector3 Position { get { return this._position; } }

    /// <summary>
    ///
    /// </summary>
    public override void PreSetup() {
      //TODO: use envs bound extent if available for space

      this._pos_x = this.Identifier + "pos_x";
      this._pos_y = this.Identifier + "pos_y";
      this._pos_z = this.Identifier + "pos_z";

      this._rot_x = this.Identifier + "rot_x";
      this._rot_y = this.Identifier + "rot_y";
      this._rot_z = this.Identifier + "rot_z";
      this._rot_w = this.Identifier + "row_w";
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() { throw new NotImplementedException();}


    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() { throw new NotImplementedException();}

    public override void PrototypingReset() {
      base.PrototypingReset();
      var transform1 = this.transform;
      transform1.position = this._position;
      transform1.rotation = this._rotation;
    }

    public override ISamplable ConfigurableValueSpace { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration obj) {
      //TODO: Denormalize configuration if space is marked as normalised

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

      this.PrototypingReset();
    }
  }
}
