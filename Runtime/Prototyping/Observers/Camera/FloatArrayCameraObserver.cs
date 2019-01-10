using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Utilities.Enums;
using Neodroid.Runtime.Utilities.ScriptableObjects;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Observers.Camera {
  [AddComponentMenu(
      ObserverComponentMenuPath._ComponentMenuPath + "FloatArrayCamera" + ObserverComponentMenuPath._Postfix)]
  public class FloatArrayCameraObserver : Observer,
                                          IHasArray {
    [Header("Observation", order = 103)]
    //[SerializeField]
    float[] _array = { };

    [SerializeField] bool _black_white;

    [Header("Specific", order = 102)]
    [SerializeField]
    UnityEngine.Camera _camera;

    bool _grab = true;

    IManager _manager;

    [SerializeField] Texture2D _texture;

    public float[] ObservationArray { get { return this._array; } private set { this._array = value; } }

    protected override void PreSetup() {
      //this._manager = FindObjectOfType<NeodroidManager>();
      this._camera = this.GetComponent<UnityEngine.Camera>();
      if (this._camera.targetTexture) {
        this._texture = new Texture2D(this._camera.targetTexture.width, this._camera.targetTexture.height);
        if (this._black_white) {
          this.ObservationArray = new float[this._texture.width * this._texture.height * 1]; // *1 for clarity
        } else {
          this.ObservationArray = new float[this._texture.width * this._texture.height * 3];
        }
      } else {
        this.ObservationArray = new float[0];
      }

      this.FloatEnumerable = this.ObservationArray;
    }

    protected virtual void OnPostRender() {
      if (this._camera.targetTexture) {
        this.UpdateArray();
      }
    }

    protected virtual void UpdateArray() {
      if (!this._grab) {
        return;
      }

      this._grab = false;

      var current_render_texture = RenderTexture.active;
      RenderTexture.active = this._camera.targetTexture;

      this._texture.ReadPixels(
          new Rect(0, 0, this._camera.targetTexture.width, this._camera.targetTexture.height),
          0,
          0);
      this._texture.Apply();

      if (!this._black_white) {
        for (var w = 0; w < this._texture.width; w++) {
          for (var h = 0; h < this._texture.height; h++) {
            var c = this._texture.GetPixel(w, h);
            this.ObservationArray[this._texture.width * w + h * 3] = c.r;
            this.ObservationArray[this._texture.width * w + h * 3 + 1] = c.g;
            this.ObservationArray[this._texture.width * w + h * 3 + 2] = c.b;
          }
        }
      } else {
        for (var w = 0; w < this._texture.width; w++) {
          for (var h = 0; h < this._texture.height; h++) {
            var c = this._texture.GetPixel(w, h);
            this.ObservationArray[this._texture.width * w + h] = (c.r + c.g + c.b) / 3;
          }
        }
      }

      RenderTexture.active = current_render_texture;
      this.FloatEnumerable = this.ObservationArray;
    }

    public override void UpdateObservation() {
      if (this._manager?.SimulatorConfiguration != null) {
        if (this._manager.SimulatorConfiguration.SimulationType != SimulationType.Frame_dependent_) {
          Debug.LogWarning("WARNING! Camera Observations may be out of sync other data");
        }
      }

      this._grab = true;
      var manager = this._manager;
      if (manager != null
          && manager.SimulatorConfiguration.SimulationType != SimulationType.Frame_dependent_) {
        this._camera.Render();

        this.UpdateArray();
      }
    }
  }
}