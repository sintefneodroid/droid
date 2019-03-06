using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Managers;
using droid.Runtime.Utilities.Enums;
using droid.Runtime.Utilities.Misc;
using UnityEngine;
using UnityEngine.Serialization;

namespace droid.Runtime.Prototyping.Observers.Camera
{
  /// <summary>
  /// </summary>
  enum ImageFormat
  {
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
    IHasByteArray
  {
    /// <summary>
    /// </summary>
    [Header("Observation", order = 103)]
    //[SerializeField]
    byte[] _bytes = { };

    /// <summary>
    /// </summary>
    [Header("Specific", order = 102)] [SerializeField]
    protected UnityEngine.Camera _Camera;

    /// <summary>
    /// </summary>
    protected bool _Grab = true;

    /// <summary>
    /// </summary>
    [SerializeField] ImageFormat imageFormat = ImageFormat.Jpg_;

    /// <summary>
    /// </summary>
    [SerializeField] [Range(0, 100)] int jpegQuality = 75;



    /// <summary>
    /// </summary>
    protected IManager _Manager=null;

    /// <summary>
    /// </summary>
    Texture2D _texture=null;

    [SerializeField] bool disable_encoding=false;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public byte[] Bytes
    {
      get { return this._bytes; }
      private set { this._bytes = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup()
    {
      this._Manager = FindObjectOfType<NeodroidManager>();
      this._Camera = this.GetComponent<UnityEngine.Camera>();
      if (this._Camera)
      {
        var target_texture = this._Camera.targetTexture;
        if (!target_texture)
        {
          Debug.LogWarning(
            $"No targetTexture defaulting to a texture of size ({NeodroidConstants._Default_Width}, {NeodroidConstants._Default_Height})");

          this._texture = new Texture2D(NeodroidConstants._Default_Width, NeodroidConstants._Default_Height);
        }
        else
        {
          var texture_format_str = target_texture.format.ToString();
          if (Enum.TryParse(texture_format_str, out TextureFormat texture_format))
          {
            this._texture = new Texture2D(
              target_texture.width,
              target_texture.height,
              texture_format,
              target_texture.useMipMap,
              !target_texture.sRGB);
          }
          else
          {
#if NEODROID_DEBUG
            Debug.LogWarning(
                $"Texture format {texture_format_str} is not a valid TextureFormat for Texture2D$");
#endif
          }
        }
      }

      if (this._Manager?.SimulatorConfiguration != null)
      {
        if (this._Manager.SimulatorConfiguration.SimulationType != SimulationType.Frame_dependent_ &&
            Application.isEditor)
        {
#if NEODROID_DEBUG
          Debug.Log(
              "Notice that camera observations may be out of sync with other observation data, because simulation configuration is not frame dependent");
#endif
        }
      }
    }

    /// <summary>
    /// </summary>
    protected virtual void OnPostRender()
    {
      this.UpdateBytes();
    }

    /// <summary>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    protected void UpdateBytes()
    {
      if (!this._Grab)
      {
        return;
      }

      this._Grab = false;

      if (this._Camera)
      {
        var current_render_texture = RenderTexture.active;
        RenderTexture.active = this._Camera.targetTexture;

        if (this._texture)
        {
          this._texture.ReadPixels(new Rect(0, 0, this._texture.width, this._texture.height), 0, 0);
          this._texture.Apply();
        }
        else
        {
#if NEODROID_DEBUG
          Debug.LogWarning("Texture not available!");
#endif
          this._texture = new Texture2D(NeodroidConstants._Default_Width, NeodroidConstants._Default_Height);
        }


        if (!this.disable_encoding)
        {
          switch (this.imageFormat)
          {
            case ImageFormat.Jpg_:
              this.Bytes = this._texture.EncodeToJPG(this.jpegQuality);
              break;
            case ImageFormat.Png_:
              this.Bytes = this._texture.EncodeToPNG();
              break;
            case ImageFormat.Exr_:
              this.Bytes = this._texture.EncodeToEXR();
              break;
            default: throw new ArgumentOutOfRangeException();
          }
        }

        RenderTexture.active = current_render_texture;
      }
      else
      {
        Debug.LogWarning($"No camera found on {this}");
      }
    }

    public override IEnumerable<float> FloatEnumerable
    {
      get { return new List<float>(); }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation()
    {
      this._Grab = true;
      if (this._Manager?.SimulatorConfiguration?.SimulationType != SimulationType.Frame_dependent_)
      {
        if(Application.isPlaying)
        {
          if (UnityEngine.Camera.current)
          {

            this._Camera.Render();
          }
        }

        this.UpdateBytes();
      }
    }
  }
}