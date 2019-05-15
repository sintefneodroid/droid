using System;
using UnityEngine;

namespace droid.Runtime.Utilities.NeodroidCamera {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public class ReplacementShaderEffect : MonoBehaviour {
    //[SerializeField] Color _color = Color.gray;
    //[SerializeField] Color _outline_color = Color.magenta;
    //[SerializeField, Range(0, 1)] float _outline_width_factor = 0.3f;
    [SerializeField] string _replace_render_type = "";

    [SerializeField] Shader _replacement_shader = null;
    //[SerializeField, Range(0, 1)] int _use_right;
    [SerializeField, Range(0.001f, 1000f)] float _scalar = 0.01f;
    static readonly Int32 _scalar1 = Shader.PropertyToID("_Scalar");

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
      //Shader.SetGlobalFloat("_UseRight", this._use_right);
      Shader.SetGlobalFloat(_scalar1, this._scalar);
    }

    void OnDisable() { this.GetComponent<Camera>().ResetReplacementShader(); }

    void OnPreRender() { }
  }
}
