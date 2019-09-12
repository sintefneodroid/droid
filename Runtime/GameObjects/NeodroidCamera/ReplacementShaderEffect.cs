using UnityEngine;

namespace droid.Runtime.GameObjects.NeodroidCamera {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public class ReplacementShaderEffect : MonoBehaviour {
    [SerializeField] string _replace_render_type = "";

    [SerializeField] Shader _replacement_shader = null;

    void Start() {
      if (this._replacement_shader != null) {
        this.GetComponent<Camera>().SetReplacementShader(this._replacement_shader, this._replace_render_type);
      }
    }

    void OnEnable() {
      if (this._replacement_shader != null) {
        this.GetComponent<Camera>().SetReplacementShader(this._replacement_shader, this._replace_render_type);
      }
    }

    void OnDisable() { this.GetComponent<Camera>().ResetReplacementShader(); }
  }
}
