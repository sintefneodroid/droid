using UnityEngine;

namespace droid.Runtime.GameObjects.ChildSensors {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class ChildCollider2DSensor : ChildColliderSensor<Collider2D, Collision2D> {
    void OnTriggerEnter2D(Collider2D other) {
      this._on_trigger_enter_delegate?.Invoke(this.gameObject, other);
    }

    void OnTriggerStay2D(Collider2D other) { this._on_trigger_stay_delegate?.Invoke(this.gameObject, other); }
    void OnTriggerExit2D(Collider2D other) { this._on_trigger_exit_delegate?.Invoke(this.gameObject, other); }

    void OnCollisionEnter2D(Collision2D collision) {
      this._on_collision_enter_delegate?.Invoke(this.gameObject, collision);
    }

    void OnCollisionStay2D(Collision2D collision) {
      this._on_collision_stay_delegate?.Invoke(this.gameObject, collision);
    }

    void OnCollisionExit2D(Collision2D collision) {
      this._on_collision_exit_delegate?.Invoke(this.gameObject, collision);
    }
  }
}
