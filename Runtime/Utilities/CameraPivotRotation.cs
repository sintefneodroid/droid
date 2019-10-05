using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Utilities {
  /// <summary>
  ///
  /// </summary>
  public class CameraPivotRotation : MonoBehaviour {
    /// <summary>
    ///
    /// </summary>
    public float rotateSpeed = 1f;

    /// <summary>
    ///
    /// </summary>
    public float scrollSpeed = 200f;

    /// <summary>
    ///
    /// </summary>
    public Transform pivot;

    /// <summary>
    ///
    /// </summary>
    public SphericalSpace _sphericalSpace;

    void Start() {
      this._sphericalSpace = SphericalSpace.FromCartesian(this.transform.position,
                                                          3f,
                                                          10f,
                                                          0f,
                                                          Mathf.PI * 2f,
                                                          0f,
                                                          Mathf.PI / 4f);
      // Initialize position
      this.transform.position = this._sphericalSpace.ToCartesian() + this.pivot.position;
    }

    void Update() {
      var kh = Input.GetAxis("Horizontal");
      var kv = Input.GetAxis("Vertical");

      var any_mouse_button = Input.GetMouseButton(0) | Input.GetMouseButton(1) | Input.GetMouseButton(2);
      var mh = any_mouse_button ? Input.GetAxis("Mouse X") : 0f;
      var mv = any_mouse_button ? Input.GetAxis("Mouse Y") : 0f;

      var h = kh * kh > mh * mh ? kh : mh;
      var v = kv * kv > mv * mv ? kv : mv;

      if (h * h > .1f || v * v > .1f) {
        this.transform.position =
            this._sphericalSpace.Rotate(h * this.rotateSpeed * Time.deltaTime,
                                        v * this.rotateSpeed * Time.deltaTime).ToCartesian()
            + this.pivot.position;
      }

      var sw = -Input.GetAxis("Mouse ScrollWheel");
      if (sw * sw > Mathf.Epsilon) {
        this.transform.position =
            this._sphericalSpace.TranslateRadius(sw * Time.deltaTime * this.scrollSpeed).ToCartesian()
            + this.pivot.position;
      }

      this.transform.LookAt(this.pivot.position);
    }
  }
}
