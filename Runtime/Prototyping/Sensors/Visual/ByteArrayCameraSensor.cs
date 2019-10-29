using System;
using System.Collections.Generic;
using droid.Runtime.Enums;
using droid.Runtime.GameObjects.NeodroidCamera.Experimental;
using droid.Runtime.Interfaces;
using droid.Runtime.Managers;
using droid.Runtime.Utilities;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace droid.Runtime.Prototyping.Sensors.Visual {
  /// <summary>
  ///
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "ByteArrayCamera"
                    + SensorComponentMenuPath._Postfix)]
  public class ByteArrayCameraSensor : Sensor,
                                       IHasByteArray {
    const TextureCreationFlags _flags = TextureCreationFlags.None;
    bool _grab = true;
    IManager _manager = null;
    Texture2D _texture = null;
    RenderTexture _rt;

    [Header("Specific", order = 102)]
    [SerializeField]
    Camera _camera = null;

    Byte[] _bytes = { };
    [SerializeField] Boolean linear_space;
    [SerializeField] Camera disable_camera_when_unused;

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

      if (this._texture) {
        UnityHelpers.Destroy(this._texture);
      }

      if (this._manager?.SimulatorConfiguration?.SimulationType != SimulationType.Frame_dependent_) {
        if (this.disable_camera_when_unused) {
          this._camera.enabled = false;
        }
      }

      var target_texture = this._camera.targetTexture;
      if (!target_texture) {
        #if NEODROID_DEBUG
        Debug.LogWarning($"RenderTexture target not available on {this.Identifier} not available, allocating a default!");
        #endif
        this._rt = new RenderTexture(NeodroidConstants._Default_Width,
                                     NeodroidConstants._Default_Height,
                                     0,
                                     RenderTextureFormat.ARGBFloat) {
                                                                        filterMode = FilterMode.Point,
                                                                        name = $"rt_{this.Identifier}",
                                                                        enableRandomWrite = true
                                                                    };
        this._rt.Create();
        this._camera.targetTexture = this._rt;
        this._texture = new Texture2D(NeodroidConstants._Default_Width,
                                      NeodroidConstants._Default_Height,
                                      NeodroidConstants._Default_TextureFormat,
                                      false,
                                      this.linear_space);
      } else {
        this._texture = new Texture2D(target_texture.width,
                                      target_texture.height,
                                      target_texture.graphicsFormat,
                                      _flags);
      }
    }

    void OnDestroy() {
      if (this._rt) {
        this._rt.Release();
      }
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void OnPostRender() {
      if (this._manager?.SimulatorConfiguration?.SimulationType == SimulationType.Frame_dependent_) {
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
      #if NEODROID_DEBUG
      if (this.Debugging) {
        this._grab = true;
      }
      #endif

      if (!this._grab && this._camera.targetTexture) {
        return;
      }

      this._grab = false;

      if (this._camera) {
        var current_render_texture = RenderTexture.active;
        var texture = this._camera.targetTexture;
        RenderTexture.active = texture;

        this._texture.ReadPixels(new Rect(0,
                                          0,
                                          this._texture.width,
                                          this._texture.height),
                                 0,
                                 0,
                                 recalculateMipMaps : false);
        this.Bytes = this._texture.GetRawTextureData();
        RenderTexture.active = current_render_texture;
      } else {
        Debug.LogWarning($"No camera found on {this}");
      }
    }

    /// <summary>
    ///
    /// </summary>
    public override String PrototypingTypeName { get { return ""; } }

    /// <summary>
    ///
    /// </summary>
    public override IEnumerable<float> FloatEnumerable { get { return new float[] { }; } }

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
      if (this.Bytes != null) {
        var rep = $"Byte Array (Length: {this.Bytes.Length}), ";
        if (this.Bytes.Length > 1) {
          rep += $"Sample [{this.Bytes[0]}.." + $"{this.Bytes[this.Bytes.Length - 1]}]";
        }

        return rep;
      }

      return "No data";
    }

    /// <summary>
    ///
    /// </summary>
    public Byte[] Bytes {
      get { return this._bytes; }
      private set {
        if (value != null) {
          this._bytes = value;
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    public Int32[] Shape {
      get {
        int channels;
        switch (this._texture.graphicsFormat) {
          case GraphicsFormat.R8_UNorm:
          case GraphicsFormat.R16_SFloat:
          case GraphicsFormat.R32_SFloat:
            channels = 1;
            break;
          //case GraphicsFormat.R32G32B32A32_SFloat:
          //case GraphicsFormat.B8G8R8A8_UNorm:
          //case GraphicsFormat.R16G16B16A16_SFloat:
          default:
            channels = 4;
            break;
        }

        return new[] {this._texture.width, this._texture.height, channels};
      }
    }

    /// <summary>
    ///
    /// </summary>
    public String ArrayEncoding {
      get {
        string s;
        switch (this._texture.graphicsFormat) {
          case GraphicsFormat.R8G8B8A8_UNorm:
          case GraphicsFormat.R8_UInt:
            s = "UINT8";
            break;
          case GraphicsFormat.R16_SFloat:
          case GraphicsFormat.R16G16B16A16_SFloat:
            s = "FLOAT16";
            break;
          case GraphicsFormat.R32_SFloat:
          case GraphicsFormat.R32G32B32A32_SFloat:
            s = "FLOAT32";
            break;
          default:
            s = "Unknown";
            break;
        }

        return s;
      }
    }
  }
}
