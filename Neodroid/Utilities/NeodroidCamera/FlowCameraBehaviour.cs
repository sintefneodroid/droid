using UnityEngine;

namespace Neodroid.Utilities.NeodroidCamera {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  [RequireComponent(typeof(Camera))]
  public class FlowCameraBehaviour : MonoBehaviour {
    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    Color _background_color = Color.white;

    [SerializeField] [Range(0, 1)] float _blending = 0.5f;

    [SerializeField] [Range(0, 100)] float _overlay_amplitude = 60;

    /// <summary>
    ///
    /// </summary>
    Material _material;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    Shader _shader;

    /// <summary>
    ///
    /// </summary>
    void Awake() {
      this.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth | DepthTextureMode.MotionVectors;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    void OnRenderImage(RenderTexture source, RenderTexture destination) {
      if (this._material == null) {
        this._material = new Material(this._shader) {hideFlags = HideFlags.DontSave};
      }

      this._material.SetColor("_BackgroundColor", this._background_color);
      this._material.SetFloat("_Blending", this._blending);
      this._material.SetFloat("_Amplitude", this._overlay_amplitude);
      Graphics.Blit(source, destination, this._material);
    }

    /// <summary>
    ///
    /// </summary>
    void OnDestroy() {
      if (this._material != null) {
        if (Application.isPlaying) {
          Destroy(this._material);
        } else {
          DestroyImmediate(this._material);
        }
      }
    }
  }
}
