using System;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.GameObjects.NeodroidCamera.Experimental.Camera360 {
  public class CreateStereoCubemaps : MonoBehaviour {
    public RenderTexture cubemapLeftEye;
    public RenderTexture cubemapRightEye;
    public RenderTexture cubemapEquirect;
    public Texture2D _texture;
    public bool renderStereo = true;
    public float stereoSeparation = 0.064f;

    Camera _cam;

    void Start() {
      this._cam = this.GetComponent<Camera>();

      if (this._cam == null) {
        this._cam = this.GetComponentInParent<Camera>();
      }

      if (this._cam) {
        var target_texture = this._cam.targetTexture;
        if (!target_texture) {
          Debug.LogWarning($"No targetTexture defaulting to a texture of size ({NeodroidConstants._Default_Width}, {NeodroidConstants._Default_Height})");

          this._texture = new Texture2D(NeodroidConstants._Default_Width, NeodroidConstants._Default_Height);
        } else {
          var texture_format_str = target_texture.format.ToString();
          if (Enum.TryParse(texture_format_str, out TextureFormat texture_format)) {
            this._texture = new Texture2D(target_texture.width,
                                          target_texture.height,
                                          texture_format,
                                          target_texture.useMipMap,
                                          !target_texture.sRGB);
          }
        }
      }
    }

    void LateUpdate() {
      if (this._cam == null) {
        Debug.Log("stereo 360 capture node has no camera or parent camera");
      }

      if (this.renderStereo) {
        this._cam.stereoSeparation = this.stereoSeparation;
        this._cam.RenderToCubemap(this.cubemapLeftEye, 63, Camera.MonoOrStereoscopicEye.Left);
        this._cam.RenderToCubemap(this.cubemapRightEye, 63, Camera.MonoOrStereoscopicEye.Right);
      } else {
        this._cam.RenderToCubemap(this.cubemapLeftEye, 63, Camera.MonoOrStereoscopicEye.Mono);
      }

      //optional: convert cubemaps to equirect
      if (this.cubemapEquirect != null) {
        if (this.renderStereo) {
          this.cubemapLeftEye.ConvertToEquirect(this.cubemapEquirect, Camera.MonoOrStereoscopicEye.Left);
          this.cubemapRightEye.ConvertToEquirect(this.cubemapEquirect, Camera.MonoOrStereoscopicEye.Right);
        } else {
          this.cubemapLeftEye.ConvertToEquirect(this.cubemapEquirect);
        }
      }
    }
  }
}
