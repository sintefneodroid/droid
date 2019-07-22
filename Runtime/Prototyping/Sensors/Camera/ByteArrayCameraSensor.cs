using System;
using System.Collections.Generic;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using droid.Runtime.Managers;
using droid.Runtime.Utilities;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace droid.Runtime.Prototyping.Sensors.Camera {
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "ByteArrayCamera"
                    + SensorComponentMenuPath._Postfix)]
  public class ByteArrayCameraSensor : Sensor,
                                       IHasByteArray {
    [Header("Specific", order = 102)]
    [SerializeField]
    UnityEngine.Camera _camera = null;

    bool _grab = true;

    //[SerializeField] bool linear_space;
    IManager _manager = null;

    [SerializeField] Texture2D _texture = null;

    const TextureCreationFlags _flags = TextureCreationFlags.None;

    /// <summary>
    /// 
    /// </summary>
    protected override void PreSetup() {
      if (this._manager == null) {
        this._manager = FindObjectOfType<AbstractNeodroidManager>();
      }

      if (this._camera == null) {
        this._camera = this.GetComponent<UnityEngine.Camera>();
      }

      var target_texture = this._camera.targetTexture;
      if (!target_texture) {
        #if NEODROID_DEBUG
        Debug.LogWarning("Texture not available!");
        #endif
        this._texture = new Texture2D(NeodroidConstants._Default_Width,
                                      NeodroidConstants._Default_Height,
                                      NeodroidConstants._Default_TextureFormat,
                                      false
                                      //,this.linear_space
                                     );
      } else {
        this._texture = new Texture2D(target_texture.width,
                                      target_texture.height,
                                      target_texture.graphicsFormat,
                                      _flags);
      }

      /*this._post_material = new Material( Shader.Find("Neodroid/Gamma") );
    this._post_material.SetFloat("_gamma", this.gamma);
    Graphics.Blit(source, destination, this._post_material);*/
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
        var texture = this._camera.targetTexture;
        //texture.GetNativeTexturePtr()
        RenderTexture.active = texture;

        this._texture.ReadPixels(new Rect(0, 0, this._texture.width, this._texture.height),
                                 0,
                                 0,
                                 recalculateMipMaps : false);

        //this._texture.Apply();

        this.Bytes = this._texture.GetRawTextureData();
        //Debug.Log($"{this.Identifier}:{this.Bytes.Length}");

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
      var rep = $"Byte Array (Length: {this.Bytes.Length}), "
                + $"Sample [{this.Bytes[0]}.."
                + $"{this.Bytes[this.Bytes.Length - 1]}]";

      return rep;
    }

    /// <summary>
    ///
    /// </summary>
    public Byte[] Bytes { get; private set; }

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
