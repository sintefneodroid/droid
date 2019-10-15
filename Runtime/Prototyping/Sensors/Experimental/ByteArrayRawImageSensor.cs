using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

namespace droid.Runtime.Prototyping.Sensors.Experimental {
  /// <summary>
  ///
  /// </summary>
  public class ByteArrayRawImageSensor : Sensor,
                                         IHasByteArray {
    [SerializeField] RawImage raw_image;
    [SerializeField] Texture2D texture2D;
    [SerializeField] WebCamTexture webcam_texture;
    Byte[] _bytes = {};

    /// <summary>
    ///
    /// </summary>
    public override void PreSetup() {
      if (this.raw_image == null) {
        this.raw_image = this.GetComponent<RawImage>();
      }

      this.ReInitCamera();
    }

    void ReInitCamera() {
      if (!Application.isPlaying) {
        return;
      }

      if (this.webcam_texture == null) {
        this.webcam_texture = new WebCamTexture();
      } else {
        this.webcam_texture.Stop();
        this.webcam_texture = new WebCamTexture();
      }

      if (this.webcam_texture) {
        this.raw_image.texture = this.webcam_texture;
        this.raw_image.material.mainTexture = this.webcam_texture;
        this.webcam_texture.Play();

        this.texture2D = new Texture2D(this.webcam_texture.width,
                                       this.webcam_texture.height,
                                       GraphicsFormat.R8G8B8A8_UNorm,
                                       0,
                                       TextureCreationFlags.None);
      }
    }

    /// <summary>
    ///
    /// </summary>
    public override IEnumerable<Single> FloatEnumerable { get { return new List<Single>(); } }

    /// <summary>
    ///
    /// </summary>
    public override void UpdateObservation() {
      if (this.webcam_texture && this.webcam_texture.isPlaying) {
        this.texture2D.SetPixels(this.webcam_texture.GetPixels());
        this.texture2D.Apply();
        //this.texture2D.UpdateExternalTexture(this.webcam_texture.GetNativeTexturePtr());
        this.Bytes = this.texture2D.GetRawTextureData();
      } else {
        this.ReInitCamera();
      }
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
        switch (this.texture2D.graphicsFormat) {
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

        return new[] {this.texture2D.height, this.texture2D.width, channels};
      }
    }

    /// <summary>
    ///
    /// </summary>
    public String ArrayEncoding {
      get {
        var s = "Unknown";

        var texture = this.texture2D;
        switch (texture.graphicsFormat) {
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
  }
}
