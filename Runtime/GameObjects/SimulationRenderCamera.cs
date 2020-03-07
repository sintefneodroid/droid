using UnityEngine;

namespace droid.Runtime.GameObjects {
  [RequireComponent(requiredComponent : typeof(Camera))]
  public class SimulationRenderCamera : MonoBehaviour {
    Camera _camera;

    void Awake() { this._camera = this.GetComponent<Camera>(); }

    internal void DisableCamera() {
      if (this._camera.enabled) {
        this._camera.enabled = false;
      }
    }

    internal void EnableCamera() {
      if (!this._camera.enabled) {
        this._camera.enabled = true;
      }
    }

    internal void Render() { this._camera.Render(); }
  }
}
