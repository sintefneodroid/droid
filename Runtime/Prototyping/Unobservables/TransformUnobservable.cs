using UnityEngine;

namespace droid.Runtime.Prototyping.Unobservables {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ObservablesComponentMenuPath._ComponentMenuPath
                    + "Transform"
                    + ObservablesComponentMenuPath._Postfix)]
  public class TransformUnobservable : Unobservable {
    /// <summary>
    /// </summary>
    Vector3 _original_position;

    /// <summary>
    /// </summary>
    Quaternion _original_rotation;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "Transform"; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PrototypingReset() {
      this.transform.position = this._original_position;
      this.transform.rotation = this._original_rotation;
    }

    /// <summary>
    /// </summary>
    public override void Setup() {
      this._original_position = this.transform.position;
      this._original_rotation = this.transform.rotation;
    }
  }
}
