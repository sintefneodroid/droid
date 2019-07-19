using UnityEngine;

namespace droid.Runtime.GameObjects.ChildSensors {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class ChildCollider3DSensor : ChildColliderSensor<Collider, Collision> {
    void OnTriggerEnter(Collider other) { this._on_trigger_enter_delegate?.Invoke(this.gameObject, other); }
    void OnTriggerStay(Collider other) { this._on_trigger_stay_delegate?.Invoke(this.gameObject, other); }
    void OnTriggerExit(Collider other) { this._on_trigger_exit_delegate?.Invoke(this.gameObject, other); }

    void OnCollisionEnter(Collision collision) {
      this._on_collision_enter_delegate?.Invoke(this.gameObject, collision);
    }

    void OnCollisionStay(Collision collision) {
      this._on_collision_stay_delegate?.Invoke(this.gameObject, collision);
    }

    void OnCollisionExit(Collision collision) {
      this._on_collision_exit_delegate?.Invoke(this.gameObject, collision);
    }
  }
}
