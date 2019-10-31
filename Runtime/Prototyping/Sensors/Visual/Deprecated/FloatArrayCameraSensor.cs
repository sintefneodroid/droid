using System.Collections.Generic;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using droid.Runtime.Managers;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Visual.Deprecated {
  /// <summary>
  ///
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "FloatArrayCamera"
                    + SensorComponentMenuPath._Postfix)]
  public class FloatArrayCameraSensor : Sensor,
                                        IHasFloatArray {
    [SerializeField] bool _black_white = false;

    [Header("Specific", order = 102)]
    [SerializeField]
    Camera _camera = null;

    bool _grab = true;

    IManager _manager = null;

    [SerializeField] Texture2D _texture = null;

    /// <summary>
    ///
    /// </summary>
    [field : Header("Observation", order = 103)]
    public float[] ObservationArray { get; private set; } = null;

    /// <summary>
    ///
    /// </summary>
    public Space1[] ObservationSpace { get { return new[] {Space1.ZeroOne}; } }

    /// <summary>
    ///
    /// </summary>
    public override void PreSetup() {
      if (this._manager == null) {
        this._manager = FindObjectOfType<AbstractNeodroidManager>();
      }

      if (this._camera == null) {
        this._camera = this.GetComponent<Camera>();
      }

      var target_texture = this._camera.targetTexture;
      if (target_texture) {
        this._texture = new Texture2D(target_texture.width, target_texture.height);
        if (this._black_white) {
          this.ObservationArray = new float[this._texture.width * this._texture.height * 1]; // *1 for clarity
        } else {
          this.ObservationArray = new float[this._texture.width * this._texture.height * 3];
        }
      } else {
        this.ObservationArray = new float[0];
      }
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void OnPostRender() {
      if (this._camera.targetTexture) {
        this.UpdateArray();
      }
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void UpdateArray() {
      if (!this._grab) {
        return;
      }

      this._grab = false;

      var current_render_texture = RenderTexture.active;
      var target_texture = this._camera.targetTexture;
      RenderTexture.active = target_texture;

      this._texture.ReadPixels(new Rect(0,
                                        0,
                                        target_texture.width,
                                        target_texture.height),
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
    }

    /// <summary>
    ///
    /// </summary>
    public override IEnumerable<float> FloatEnumerable { get { return this.ObservationArray; } }

    /// <summary>
    ///
    /// </summary>
    public override void UpdateObservation() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        if (this._manager?.SimulatorConfiguration != null) {
          if (this._manager.SimulatorConfiguration.SimulationType != SimulationType.Frame_dependent_) {
            Debug.LogWarning("WARNING! Camera Observations may be out of sync other data");
          }
        }
      }
      #endif

      this._grab = true;
      var manager = this._manager;
      if (manager != null
          && manager.SimulatorConfiguration.SimulationType != SimulationType.Frame_dependent_) {
        this.UpdateArray();
      }
    }
  }
}
