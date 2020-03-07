using UnityEngine;

namespace droid.Runtime.GameObjects.NeodroidCamera.Experimental {
  /// <summary>
  ///
  /// </summary>
  public class CreateStereoCubemaps : MonoBehaviour {
    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    RenderTexture cubemapLeftEye;

    [SerializeField] RenderTexture cubemapRightEye;
    [SerializeField] RenderTexture cubemapEquirect;
    [SerializeField] bool renderStereo = false;
    [SerializeField] float stereoSeparation = 0.064f;

    [SerializeField] Camera _cam;

    void Start() {
      this._cam = this.GetComponent<Camera>();

      if (this._cam == null) {
        this._cam = this.GetComponentInParent<Camera>();
      }
    }

    void LateUpdate() {
      if (this._cam == null) {
        Debug.Log("stereo 360 capture node has no camera or parent camera");
      }

      if (this.renderStereo) {
        this._cam.stereoSeparation = this.stereoSeparation;
        this._cam.RenderToCubemap(cubemap : this.cubemapLeftEye,
                                  63,
                                  stereoEye : Camera.MonoOrStereoscopicEye.Left);
        this._cam.RenderToCubemap(cubemap : this.cubemapRightEye,
                                  63,
                                  stereoEye : Camera.MonoOrStereoscopicEye.Right);
      } else {
        this._cam.RenderToCubemap(cubemap : this.cubemapLeftEye,
                                  63,
                                  stereoEye : Camera.MonoOrStereoscopicEye.Mono);
      }

      //optional: convert cubemaps to equirect
      if (this.cubemapEquirect != null) {
        if (this.renderStereo) {
          this.cubemapLeftEye.ConvertToEquirect(equirect : this.cubemapEquirect,
                                                eye : Camera.MonoOrStereoscopicEye.Left);
          this.cubemapRightEye.ConvertToEquirect(equirect : this.cubemapEquirect,
                                                 eye : Camera.MonoOrStereoscopicEye.Right);
        } else {
          this.cubemapLeftEye.ConvertToEquirect(equirect : this.cubemapEquirect);
        }
      }
    }
  }
}
