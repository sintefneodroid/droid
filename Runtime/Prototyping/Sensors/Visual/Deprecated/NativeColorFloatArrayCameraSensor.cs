using System;
using System.Collections.Generic;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using droid.Runtime.Managers;
using droid.Runtime.Structs.Space;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Visual.Deprecated {
  /// <summary>
  ///
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "NativeColorArrayCamera"
                    + SensorComponentMenuPath._Postfix)]
  public class NativeColorFloatArrayCameraSensor : Sensor,
                                                   IHasFloatArray {
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
    public float[] ObservationArray { get; private set; }

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
        this.ObservationArray = new float[target_texture.width * target_texture.height * 4];
      } else {
        #if NEODROID_DEBUG
        Debug.LogWarning("Texture not available!");
        #endif
        this._texture = new Texture2D(NeodroidConstants._Default_Width,
                                      NeodroidConstants._Default_Height,
                                      NeodroidConstants._Default_TextureFormat,
                                      false);
        this.ObservationArray = new float[this._texture.width * this._texture.height * 4];
      }
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void OnPostRender() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        this._grab = true;
      }
      #endif
      if (this._camera.targetTexture) {
        this.UpdateArray();
      }
      #if NEODROID_DEBUG
      if (this.Debugging) {
        //Graphics.DrawTexture(new Rect(new Vector2(0, 0), new Vector2(256, 256)), this._texture);
      }
      #endif
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void UpdateArray() {
      if (!this._grab) {
        return;
      }

      this._grab = false;

      if (this._camera) {
        var current_render_texture = RenderTexture.active;
        RenderTexture.active = this._camera.targetTexture;

        if (this._texture
            && this._camera.targetTexture.width == this._texture.width
            && this._camera.targetTexture.height == this._texture.height) {
          this._texture.ReadPixels(new Rect(0,
                                            0,
                                            this._texture.width,
                                            this._texture.height),
                                   0,
                                   0);
          //this._texture.Apply();
        } else {
          #if NEODROID_DEBUG
          Debug.LogWarning("Texture not available!");
          #endif
          this._texture = new Texture2D(this._camera.targetTexture.width,
                                        this._camera.targetTexture.height,
                                        NeodroidConstants._Default_TextureFormat,
                                        false);
          this.ObservationArray = new float[this._texture.width * this._texture.height * 4];
        }

        //Texture2D texCopy = new Texture2D(_texture.width, _texture.height, _texture.format, _texture.mipmapCount > 1);
        //texCopy.LoadRawTextureData(_texture.GetRawTextureData());
        //texCopy.Apply();
        //var a = texCopy.GetRawTextureData<Color>();
        var a = this._texture.GetRawTextureData<Color>();

        #if NEODROID_DEBUG
        var min = a[0];
        var max = a[0];

        #endif

        var i = 0;
/*
        foreach (var b in a) {
          this.flat_float_array[i] = b.r;
          this.flat_float_array[i + 1] = b.g;
          this.flat_float_array[i + 2] = b.b;
          this.flat_float_array[i + 3] = b.a;
          i += 4;
        }
*/
        for (var index = 0; index < a.Length; index++) {
          var b = a[index];
          //i = index*4;
          this.ObservationArray[i] = b.r;
          this.ObservationArray[i + 1] = b.g;
          this.ObservationArray[i + 2] = b.b;
          this.ObservationArray[i + 3] = b.a;
          i += 4;

          #if NEODROID_DEBUG
          if (this.Debugging) {
            max[0] = Mathf.Max(b[0], max[0]);
            min[0] = Mathf.Min(b[0], min[0]);
            max[1] = Mathf.Max(b[1], max[1]);
            min[1] = Mathf.Min(b[1], min[1]);
            max[2] = Mathf.Max(b[2], max[2]);
            min[2] = Mathf.Min(b[2], min[2]);
            max[3] = Mathf.Max(b[3], max[3]);
            min[3] = Mathf.Min(b[3], min[3]);
          }
          #endif
        }

        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"len(a):{a.Length}, min:{min}, max:{max}");
        }
        #endif

        RenderTexture.active = current_render_texture;
      } else {
        Debug.LogWarning($"No camera found on {this}");
      }
    }

    /// <summary>
    ///
    /// </summary>
    public override String PrototypingTypeName { get { return ""; } }

    public override IEnumerable<float> FloatEnumerable {
      get { return null; } //this.ObservationArray; }
    }

    /// <summary>
    ///
    /// </summary>
    public override void UpdateObservation() {
      this._grab = true;
      if (this._manager?.SimulatorConfiguration?.SimulationType != SimulationType.Frame_dependent_) {
        if (Application.isPlaying) {
          this._camera.Render();
        }

        this.UpdateArray();
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      var rep = $"Float Array (Length: {this.ObservationArray.Length}), "
                + $"Sample [{Mathf.Clamp01(this.ObservationArray[0])}.."
                + $"{Mathf.Clamp01(this.ObservationArray[this.ObservationArray.Length - 1])}]";

      return rep;
    }
  }
}
