using UnityEngine;

namespace droid.Runtime.Prototyping.EnvironmentListener {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(EnvironmentListenerComponentMenuPath._ComponentMenuPath
                    + "Transform"
                    + EnvironmentListenerComponentMenuPath._Postfix)]
  public class TransformEnvironmentListener : EnvironmentListener {
    /// <summary>
    /// </summary>
    Vector3 _original_position;

    /// <summary>
    /// </summary>
    Quaternion _original_rotation;



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
