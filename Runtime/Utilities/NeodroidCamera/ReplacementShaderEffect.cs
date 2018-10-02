using UnityEngine;

namespace Neodroid.Runtime.Utilities.NeodroidCamera {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public class ReplacementShaderEffect : MonoBehaviour {
    //[SerializeField] Color _color = Color.gray;
    //[SerializeField] Color _outline_color = Color.magenta;
    //[SerializeField, Range(0, 1)] float _outline_width_factor = 0.3f;
    [SerializeField] string _replace_render_type = "";
    [SerializeField] Shader _replacement_shader;

    void OnValidate() { this.Setup(); }

    void OnEnable() {
      if (this._replacement_shader != null) {
        this.GetComponent<Camera>().SetReplacementShader(this._replacement_shader, this._replace_render_type);
      }
    }

    void Setup() {
      //Shader.SetGlobalColor("_SegmentationColor", this._color);
      //Shader.SetGlobalColor ("_OutlineColor", this._outline_color);
      //Shader.SetGlobalFloat("_OutlineWidthFactor", this._outline_width_factor);
    }

    //void Start() { this.Setup(); }

    void OnDisable() { this.GetComponent<Camera>().ResetReplacementShader(); }

    void OnPreRender() { }
  }
}
