using System;
using droid.Runtime.Interfaces;
using droid.Runtime.Managers;
using droid.Runtime.Utilities.Enums;
using UnityEngine;

namespace droid.Runtime.Prototyping.Observers.Camera {
  /// <summary>
  /// </summary>
  enum ImageFormat {
    /// <summary>
    /// </summary>
    Jpg_,

    /// <summary>
    /// </summary>
    Png_,

    /// <summary>
    /// </summary>
    Exr_
  }

  /// <inheritdoc cref="Observer" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      ObserverComponentMenuPath._ComponentMenuPath + "Camera" + ObserverComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [RequireComponent(typeof(UnityEngine.Camera))]
  public class CameraObserver : Observer,
                                IHasByteArray {
    /// <summary>
    /// </summary>
    [Header("Observation", order = 103)]
    //[SerializeField]
    byte[] _bytes = { };

    /// <summary>
    /// </summary>
    [Header("Specific", order = 102)]
    [SerializeField]
    protected UnityEngine.Camera _Camera;

    [SerializeField] Int32 default_2d_texture_width = 256;
    [SerializeField] Int32 default_2d_texture_height = 256;
    [SerializeField] Int32 default_depth = 24;
    [SerializeField] RenderTextureFormat default_format = RenderTextureFormat.Default;
    [SerializeField] RenderTextureReadWrite default_read_write = RenderTextureReadWrite.Default;


    /// <summary>
    /// </summary>
    protected bool _Grab = true;

    /// <summary>
    /// </summary>
    [SerializeField]
    ImageFormat _image_format = ImageFormat.Jpg_;

    /// <summary>
    /// </summary>
    [SerializeField]
    [Range(0, 100)]
    int _jpeg_quality = 75;

    /// <summary>
    /// </summary>
    protected IManager _Manager;

    /// <summary>
    /// </summary>
    Texture2D _texture;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public byte[] Bytes { get { return this._bytes; } private set { this._bytes = value; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this._Manager = FindObjectOfType<NeodroidManager>();
      this._Camera = this.GetComponent<UnityEngine.Camera>();
      if (this._Camera) {
        var target_texture = this._Camera.targetTexture;
        if (!target_texture) {
          Debug.LogWarning("No targetTexture defaulting to a texture of size (256, 256)");
          this._texture = new Texture2D(this.default_2d_texture_width, this.default_2d_texture_height);
        } else {
          var texture_format_str = target_texture.format.ToString();
          if (Enum.TryParse(texture_format_str, out TextureFormat texture_format)) {
            this._texture = new Texture2D(
                target_texture.width,
                target_texture.height,
                texture_format,
                target_texture.useMipMap,
                !target_texture.sRGB);
          } else {
            Debug.LogWarning(
                $"Texture format {texture_format_str} is not a valid TextureFormat for Texture2D$");
          }
        }
      }

      if (this._Manager?.SimulatorConfiguration != null) {
        if (this._Manager.SimulatorConfiguration.SimulationType != SimulationType.Frame_dependent_) {
          Debug.LogWarning(
              "WARNING! Camera Observations may be out of sync with other observation data, because simulation configuration is not frame dependent");
        }
      }
    }

    /// <summary>
    /// </summary>
    protected virtual void OnPostRender() {
      if (this._Manager?.SimulatorConfiguration?.SimulationType == SimulationType.Frame_dependent_) {
        this.UpdateBytes();
      }
    }

    /// <summary>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    protected void UpdateBytes() {
      if (!this._Grab) {
        return;
      }

      this._Grab = false;

      if (this._Camera) {
        var current_render_texture = RenderTexture.active;
        var temporary_render_texture = this._Camera.targetTexture;

        if (temporary_render_texture) {
          RenderTexture.active = temporary_render_texture;
          if (this._Manager?.SimulatorConfiguration?.SimulationType != SimulationType.Frame_dependent_) {
            this._Camera.Render();
          }
          this._texture.ReadPixels(new Rect(0, 0, this._texture.width, this._texture.height), 0, 0);
          this._texture.Apply();
        } else {
          var old_rec = this._Camera.rect;
          this._Camera.rect = new Rect(0f, 0f, 1f, 1f);

          var temp_rt = RenderTexture.GetTemporary(
              this._texture.width,
              this._texture.height,
              this.default_depth,
              this.default_format,
              this.default_read_write);

          /*if (width != this._texture.width || height != this._texture.height) {
            texture2D.Resize(width, height);
          }*/

          RenderTexture.active = temp_rt; // render to offscreen texture (readonly from CPU side)

          if (this._Manager?.SimulatorConfiguration?.SimulationType != SimulationType.Frame_dependent_) {
            this._Camera.Render();
          }

          this._texture.ReadPixels(new Rect(0, 0, this._texture.width, this._texture.height), 0, 0);
          this._texture.Apply();

          this._Camera.rect = old_rec;
          RenderTexture.ReleaseTemporary(temp_rt);
        }

        switch (this._image_format) {
          case ImageFormat.Jpg_:
            this.Bytes = this._texture.EncodeToJPG(this._jpeg_quality);
            break;
          case ImageFormat.Png_:
            this.Bytes = this._texture.EncodeToPNG();
            break;
          case ImageFormat.Exr_:
            this.Bytes = this._texture.EncodeToEXR();
            break;
          default: throw new ArgumentOutOfRangeException();
        }

        RenderTexture.active = current_render_texture;
      } else {
        Debug.LogWarning($"No camera found on {this}");
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      this._Grab = true;
      if (this._Manager?.SimulatorConfiguration?.SimulationType != SimulationType.Frame_dependent_) {
        this.UpdateBytes();
      }
    }
  }
}
