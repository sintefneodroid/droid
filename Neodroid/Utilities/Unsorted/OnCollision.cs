using UnityEngine;

namespace Neodroid.Utilities.Unsorted {
  [RequireComponent(typeof(Collider))]
  public class ChildCollisionPublisher : MonoBehaviour {
    public delegate void OnCollisionDelegate(Collision collision);

    OnCollisionDelegate _collision_delegate;

    public OnCollisionDelegate CollisionDelegate { set { this._collision_delegate = value; } }

    void OnCollisionEnter(Collision collision) { this._collision_delegate(collision); }
  }
}
