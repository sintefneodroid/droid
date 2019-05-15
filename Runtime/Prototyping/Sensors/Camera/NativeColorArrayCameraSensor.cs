using System;
using System.Collections.Generic;
using System.Linq;
using droid.Runtime.Interfaces;
using droid.Runtime.Managers;
using droid.Runtime.Utilities.Enums;
using droid.Runtime.Utilities.Misc;
using droid.Runtime.Utilities.Structs;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace droid.Runtime.Prototyping.Sensors.Camera {
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "NativeColorArrayCamera"
                    + SensorComponentMenuPath._Postfix)]
  public class NativeColorArrayCameraSensor : Sensor,
                                              IHasArray {
    [Header("Observation", order = 103)] float[] flat_float_array;

    [Header("Specific", order = 102)]
    [SerializeField]
    UnityEngine.Camera _camera = null;

    bool _grab = true;

    IManager _manager = null;

    [SerializeField] Texture2D _texture = null;

    /// <summary>
    ///
    /// </summary>
    public float[] ObservationArray {
      get { return this.flat_float_array; }
      private set { this.flat_float_array = value; }
    }

    /// <summary>
    ///
    /// </summary>
    public Space1[] ObservationSpace { get { return new[] {Space1.ZeroOne}; } }

    protected override void PreSetup() {
      if(this._manager==null) {
        this._manager = FindObjectOfType<NeodroidManager>();
      }

      if (this._camera == null) {
        this._camera = this.GetComponent<UnityEngine.Camera>();
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

      if (this._camera) {
        var current_render_texture = RenderTexture.active;
        RenderTexture.active = this._camera.targetTexture;

        if (this._texture
            && this._camera.targetTexture.width == this._texture.width
            && this._camera.targetTexture.height == this._texture.height) {
          this._texture.ReadPixels(new Rect(0, 0, this._texture.width, this._texture.height), 0, 0);
          this._texture.Apply();
        } else {
          #if NEODROID_DEBUG
          Debug.LogWarning("Texture not available!");
          #endif
          this._texture = new Texture2D(this._camera.targetTexture.width,
                                        this._camera.targetTexture.height,
                                        NeodroidConstants._Default_TextureFormat,
                                        false);
          this.flat_float_array = new float[this._texture.width * this._texture.height * 4];
        }

        var a = this._texture.GetRawTextureData<Color>();

        //var min = a[0];
        //var max = a[0];

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
          this.flat_float_array[i] = b.r;
          this.flat_float_array[i + 1] = b.g;
          this.flat_float_array[i + 2] = b.b;
          this.flat_float_array[i + 3] = b.a;
          i += 4;
        }

        //Debug.Log($"min:{min}, max:{max}");



        RenderTexture.active = current_render_texture;
      } else {
        Debug.LogWarning($"No camera found on {this}");
      }
    }

    public override String PrototypingTypeName { get { return ""; } }

    public override IEnumerable<float> FloatEnumerable {
      get { return null; } //this.ObservationArray; }
    }

    public override void UpdateObservation() {
      this._grab = true;
      if (this._manager?.SimulatorConfiguration?.SimulationType != SimulationType.Frame_dependent_) {
        #if NEODROID_DEBUG

        #endif
        Debug.Log($"{this._manager?.SimulatorConfiguration?.SimulationType}");
        if (Application.isPlaying) {
          this._camera.Render();
        }

        this.UpdateArray();
      }
    }

    public override string ToString() {
      var rep = $"Very Large Float Array (Length: {this.ObservationArray.Length}), "
                + $"Sample [{Mathf.Clamp01(this.flat_float_array[0])}.."
                + $"{Mathf.Clamp01(this.flat_float_array[this.flat_float_array.Length - 1])}]";

      return rep;
    }
  }
}
