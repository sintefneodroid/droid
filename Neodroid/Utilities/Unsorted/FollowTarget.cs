using UnityEngine;

namespace Neodroid.Utilities.Unsorted {
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
    public Transform _Target;

    void LateUpdate() {
      if (this._Target) {
        this.transform.position = this._Target.position + this._Offset;
      }
    }
  }
}
