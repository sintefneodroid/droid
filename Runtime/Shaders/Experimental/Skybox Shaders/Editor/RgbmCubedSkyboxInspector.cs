#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace droid.Runtime.Shaders.Experimental.Skybox_Shaders.Editor {
  public class RgbmCubedSkyboxInspector : MaterialEditor {
    public override void OnInspectorGUI() {
      base.OnInspectorGUI();

      if (this.isVisible) {
        var material = this.target as Material;

        var use_linear = false;
        foreach (var keyword in material.shaderKeywords) {
          if (keyword == "USE_LINEAR") {
            use_linear = true;
            break;
          }
        }

        EditorGUI.BeginChangeCheck();

        use_linear = EditorGUILayout.Toggle("Linear Space Lighting", use_linear);

        if (EditorGUI.EndChangeCheck()) {
          if (use_linear) {
            material.EnableKeyword("USE_LINEAR");
            material.DisableKeyword("USE_GAMMA");
          } else {
            material.DisableKeyword("USE_LINEAR");
            material.EnableKeyword("USE_GAMMA");
          }

          EditorUtility.SetDirty(this.target);
        }
      }
    }
  }
}
#endif
