using System;
using Neodroid.Managers;
using Neodroid.Utilities.Interfaces;
using Neodroid.Utilities.ScriptableObjects;
using UnityEngine;

namespace Neodroid.Prototyping.Observers.Camera {
  /// <summary>
  ///
  /// </summary>
  enum ImageFormat {
    /// <summary>
    ///
    /// </summary>
    Jpg_,

    /// <summary>
    ///
    /// </summary>
    Png_,

    /// <summary>
    ///
    /// </summary>
    Exr_
  }

  /// <inheritdoc cref="Observer" />
  ///  <summary>
  ///  </summary>
  [AddComponentMenu(
       ObserverComponentMenuPath._ComponentMenuPath + "Camera" + ObserverComponentMenuPath._Postfix),
   ExecuteInEditMode, RequireComponent(typeof(UnityEngine.Camera))]
  public class CameraObserver : Observer,
                                IHasByteArray {
    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    ImageFormat _image_format = ImageFormat.Jpg_;

    /// <summary>
    ///
    /// </summary>
    [SerializeField, Range(0, 100)]
    int _jpeg_quality = 75;

    /// <summary>
    ///
    /// </summary>
    [Header("Observation", order = 103)]
    //[SerializeField]
    byte[] _bytes = { };

    /// <summary>
    ///
    /// </summary>
    [Header("Specific", order = 102), SerializeField]
    UnityEngine.Camera _camera;

    /// <summary>
    ///
    /// </summary>
    bool _grab = true;

    /// <summary>
    ///
    /// </summary>
    NeodroidManager _manager;

    /// <summary>
    ///
    /// </summary>
    Texture2D _texture;

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override string PrototypingTypeName { get { return "Camera"; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public byte[] Bytes { get { return this._bytes; } private set { this._bytes = value; } }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    protected override void PreSetup() {
      this._manager = FindObjectOfType<NeodroidManager>();
      this._camera = this.GetComponent<UnityEngine.Camera>();
      if (this._camera) {
        var target_texture = this._camera.targetTexture;
        if (!target_texture) {
          Debug.LogWarning("No targetTexture defaulting to a texture of size (256, 256)");
          this._texture = new Texture2D(256, 256);
        } else {
          var texture_format_str = target_texture.format.ToString();
          TextureFormat o;
          if (Enum.TryParse(texture_format_str, out o)) {
            this._texture = new Texture2D(
                target_texture.width,
                target_texture.height,
                o,
                target_texture.useMipMap,
                !target_texture.sRGB);
          } else {
            Debug.LogWarning(
                $"Texture format {texture_format_str} is not a valid textureformat for Texture2D$");
          }
        }
      }

      if (this._manager) {
        if (this._manager.Configuration) {
          if (this._manager.Configuration.SimulationType != SimulationType.Frame_dependent_) {
            Debug.LogWarning(
                "WARNING! Camera Observations may be out of sync with other observation data, because simulation configuration is not frame dependent");
          }
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void OnPostRender() { this.UpdateBytes(); }

    // ReSharper disable once Unity.RedundantEventFunction
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Update() {
      //Dont assign anything the floatenumerable
    }

    /// <summary>
    ///
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    protected virtual void UpdateBytes() {
      if (!this._grab) {
        return;
      }

      this._grab = false;

      if (this._camera) {
        var current_render_texture = RenderTexture.active;
        RenderTexture.active = this._camera.targetTexture;

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
    ///  <summary>
    ///  </summary>
    public override void UpdateObservation() {
      this._grab = true;
      if (this._manager.Configuration.SimulationType != SimulationType.Frame_dependent_) {
        this._camera.Render();
        this.UpdateBytes();
      }
    }
  }
}
