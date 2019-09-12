using UnityEngine;

namespace droid.Runtime.GameObjects.NeodroidCamera {
  /// <summary>
  ///
  /// </summary>
  [RequireComponent(typeof(LineRenderer))]
  [ExecuteInEditMode]
  public class LineProject : MonoBehaviour {
    LineRenderer _line_renderer = null;
    [SerializeField] Vector3 _direction = Vector3.down;
    [SerializeField] float _length = 30f;

    Vector3 _old_pos = Vector3.zero;

    void Awake() {
      this._line_renderer = this.GetComponent<LineRenderer>();
      this.Project();
    }

    void OnEnable() { this.Project(); }

    void Update() {
      if (Application.isPlaying) {
        if (this.transform.position != this._old_pos) {
          this.Project();
        }
      }
    }

    void Project() {
      var position = this.transform.position;
      if (Physics.Raycast(position,
                          this._direction,
                          out var ray,
                          this._length)) {
        this._line_renderer.SetPositions(new[] {position, ray.point});
      }
    }
  }
}
