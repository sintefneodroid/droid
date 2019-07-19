using UnityEngine;

namespace droid.Runtime.Utilities.Orientation {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public class PointInDirection : MonoBehaviour {
    /// <summary>
    /// </summary>
    [SerializeField]
    Vector3 _direction = Vector3.down;

    /// <summary>
    /// </summary>
    void Update() { this.transform.rotation = Quaternion.LookRotation(this._direction); }
  }
}
