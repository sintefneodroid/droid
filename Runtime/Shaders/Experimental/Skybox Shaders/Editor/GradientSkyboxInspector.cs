#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace droid.Runtime.Shaders.Experimental.Skybox_Shaders.Editor {
  public class GradientSkyboxInspector : MaterialEditor {
    public override void OnInspectorGUI() {
      this.serializedObject.Update();

      if (this.isVisible) {
        EditorGUI.BeginChangeCheck();

        this.ColorProperty(GetMaterialProperty(this.targets, "_Color2"), "Top Color");
        this.ColorProperty(GetMaterialProperty(this.targets, "_Color1"), "Bottom Color");
        this.FloatProperty(GetMaterialProperty(this.targets, "_Intensity"), "Intensity");
        this.FloatProperty(GetMaterialProperty(this.targets, "_Exponent"), "Exponent");

        var dp = GetMaterialProperty(this.targets, "_UpVectorPitch");
        var dy = GetMaterialProperty(this.targets, "_UpVectorYaw");

        if (dp.hasMixedValue || dy.hasMixedValue) {
          EditorGUILayout.HelpBox("Editing angles is disabled because they have mixed values.",
                                  MessageType.Warning);
        } else {
          this.FloatProperty(dp, "Pitch");
          this.FloatProperty(dy, "Yaw");
        }

        if (EditorGUI.EndChangeCheck()) {
          var rp = dp.floatValue * Mathf.Deg2Rad;
          var ry = dy.floatValue * Mathf.Deg2Rad;

          var up_vector = new Vector4(Mathf.Sin(rp) * Mathf.Sin(ry),
                                      Mathf.Cos(rp),
                                      Mathf.Sin(rp) * Mathf.Cos(ry),
                                      0.0f);
          GetMaterialProperty(this.targets, "_UpVector").vectorValue = up_vector;

          this.PropertiesChanged();
        }
      }
    }
  }
}
#endif
