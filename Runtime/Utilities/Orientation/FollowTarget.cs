using UnityEngine;

namespace droid.Runtime.Utilities.Orientation {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public class FollowTarget : MonoBehaviour {
    /// <summary>
    /// </summary>
    public Vector3 _Offset = new Vector3(0f, 7.5f, 0f);

    /// <summary>
    /// </summary>
    public Transform target;

    void LateUpdate() {
      if (this.target) {
        this.transform.position = this.target.position + this._Offset;
      }
    }
  }
}
