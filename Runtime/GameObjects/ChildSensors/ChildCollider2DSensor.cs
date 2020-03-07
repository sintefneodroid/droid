using UnityEngine;

namespace droid.Runtime.GameObjects.ChildSensors {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class ChildCollider2DSensor : ChildColliderSensor<Collider2D, Collision2D> {
    void OnTriggerEnter2D(Collider2D other) {
      this._on_trigger_enter_delegate?.Invoke(child_sensor_game_object : this.gameObject, collider : other);
    }

    void OnTriggerStay2D(Collider2D other) {
      this._on_trigger_stay_delegate?.Invoke(child_sensor_game_object : this.gameObject, collider : other);
    }

    void OnTriggerExit2D(Collider2D other) {
      this._on_trigger_exit_delegate?.Invoke(child_sensor_game_object : this.gameObject, collider : other);
    }

    void OnCollisionEnter2D(Collision2D collision) {
      this._on_collision_enter_delegate?.Invoke(child_sensor_game_object : this.gameObject,
                                                collision : collision);
    }

    void OnCollisionStay2D(Collision2D collision) {
      this._on_collision_stay_delegate?.Invoke(child_sensor_game_object : this.gameObject,
                                               collision : collision);
    }

    void OnCollisionExit2D(Collision2D collision) {
      this._on_collision_exit_delegate?.Invoke(child_sensor_game_object : this.gameObject,
                                               collision : collision);
    }
  }
}
