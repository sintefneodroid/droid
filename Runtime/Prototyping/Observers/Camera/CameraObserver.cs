using System;
using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Managers;
using Neodroid.Runtime.Utilities.ScriptableObjects;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Observers.Camera {
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
          const Int32 default_width = 256;
          const Int32 default_height = default_width;
          this._texture = new Texture2D(default_width, default_height);
        } else {
          var texture_format_str = target_texture.format.ToString();
          TextureFormat texture_format;
          if (Enum.TryParse(texture_format_str, out texture_format)) {
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
    protected virtual void OnPostRender() { this.UpdateBytes(); }

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
        RenderTexture.active = this._Camera.targetTexture;

        this._texture.ReadPixels(new Rect(0, 0, this._texture.width, this._texture.height), 0, 0);
        this._texture.Apply();

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
        this._Camera.Render();
        this.UpdateBytes();
      }
    }
  }
}