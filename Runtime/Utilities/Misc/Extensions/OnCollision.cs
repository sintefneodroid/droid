using UnityEngine;

namespace Neodroid.Runtime.Utilities.Misc.Extensions {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [RequireComponent(typeof(Collider))]
  public class ChildCollisionPublisher : MonoBehaviour {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    public delegate void OnCollisionDelegate(Collision collision);

    OnCollisionDelegate _collision_delegate;

    /// <summary>
    /// 
    /// </summary>
    public OnCollisionDelegate CollisionDelegate {
      set { this._collision_delegate = value; }
    }

    void OnCollisionEnter(Collision collision) { this._collision_delegate?.Invoke(collision); }
  }
}