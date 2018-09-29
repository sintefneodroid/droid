using UnityEngine;

namespace Neodroid.Runtime.Utilities.NeodroidCamera {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public class ReplacementShaderEffect : MonoBehaviour {
    [SerializeField] Color _color = Color.black;
    [SerializeField] string _replace_render_type = "";
    [SerializeField] Shader _replacement_shader;

    void OnValidate() {
      Shader.SetGlobalColor("_OverDrawColor", this._color);
      Shader.SetGlobalColor("_SegmentationColor", this._color);
      //Shader.SetGlobalColor ("_Color", _color);
    }

    void OnEnable() {
      if (this._replacement_shader != null) {
        this.GetComponent<Camera>().SetReplacementShader(this._replacement_shader, this._replace_render_type);
      }
    }

    void OnDisable() { this.GetComponent<Camera>().ResetReplacementShader(); }

    void OnPreRender() { }
  }
}
