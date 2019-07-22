using droid.Runtime.Utilities.Orientation;
using UnityEngine;

namespace droid.Runtime.Utilities.Extensions {
  /// <summary>
  ///
  /// </summary>
  public class Obstruction : MonoBehaviour,
                             IMotionTracker {
    Vector3 _last_recorded_move;
    Quaternion _last_recorded_rotation;
    Vector3 _previous_position;
    Quaternion _previous_rotation;

    public bool IsInMotion() {
      return this.transform.position != this._previous_position
             || this.transform.rotation != this._previous_rotation;
    }

    public bool IsInMotion(float sensitivity) {
      var distance_moved = Vector3.Distance(this.transform.position, this._last_recorded_move);
      var angle_rotated = Quaternion.Angle(this.transform.rotation, this._last_recorded_rotation);
      if (distance_moved > sensitivity || angle_rotated > sensitivity) {
        this.UpdateLastRecordedTranform();
        return true;
      }

      return false;
    }

    void UpdatePreviousTranform() {
      this._previous_position = this.transform.position;
      this._previous_rotation = this.transform.rotation;
    }

    void UpdateLastRecordedTranform() {
      this._last_recorded_move = this.transform.position;
      this._last_recorded_rotation = this.transform.rotation;
    }

    void Start() {
      this.UpdatePreviousTranform();
      this.UpdateLastRecordedTranform();
    }

    void Update() { this.UpdatePreviousTranform(); }
  }
}
