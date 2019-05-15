using System;
using droid.Runtime.Utilities.Misc;
using UnityEngine;

namespace droid.Runtime.Utilities.NeodroidCamera.Experimental.Camera360 {
  public class CreateStereoCubemaps : MonoBehaviour {
    public RenderTexture cubemapLeftEye;
    public RenderTexture cubemapRightEye;
    public RenderTexture cubemapEquirect;
    public Texture2D _texture;
    public bool renderStereo = true;
    public float stereoSeparation = 0.064f;

    Camera cam;

    void Start() {
      this.cam = this.GetComponent<Camera>();

      if (this.cam == null) {
        this.cam = this.GetComponentInParent<Camera>();
      }

      if (this.cam) {
        var target_texture = this.cam.targetTexture;
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
      if (this.cam == null) {
        Debug.Log("stereo 360 capture node has no camera or parent camera");
      }

      if (this.renderStereo) {
        this.cam.stereoSeparation = this.stereoSeparation;
        this.cam.RenderToCubemap(this.cubemapLeftEye, 63, Camera.MonoOrStereoscopicEye.Left);
        this.cam.RenderToCubemap(this.cubemapRightEye, 63, Camera.MonoOrStereoscopicEye.Right);
      } else {
        this.cam.RenderToCubemap(this.cubemapLeftEye, 63, Camera.MonoOrStereoscopicEye.Mono);
      }

      //optional: convert cubemaps to equirect
      if (this.cubemapEquirect != null) {
        if (this.renderStereo) {
          this.cubemapLeftEye.ConvertToEquirect(this.cubemapEquirect, Camera.MonoOrStereoscopicEye.Left);
          this.cubemapRightEye.ConvertToEquirect(this.cubemapEquirect, Camera.MonoOrStereoscopicEye.Right);
        } else {
          this.cubemapLeftEye.ConvertToEquirect(this.cubemapEquirect, Camera.MonoOrStereoscopicEye.Mono);
        }
      }
    }
  }
}
