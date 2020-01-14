#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace droid.Runtime.Shaders.Experimental.Skybox_Shaders.Editor {
  public class GradientSkyboxInspector : MaterialEditor {
    public override void OnInspectorGUI() {
      this.serializedObject.Update();

      if (this.isVisible) {
        EditorGUI.BeginChangeCheck();

        this.ColorProperty(GetMaterialProperty(mats : this.targets, "_Color2"), "Top Color");
        this.ColorProperty(GetMaterialProperty(mats : this.targets, "_Color1"), "Bottom Color");
        this.FloatProperty(GetMaterialProperty(mats : this.targets, "_Intensity"), "Intensity");
        this.FloatProperty(GetMaterialProperty(mats : this.targets, "_Exponent"), "Exponent");

        var dp = GetMaterialProperty(mats : this.targets, "_UpVectorPitch");
        var dy = GetMaterialProperty(mats : this.targets, "_UpVectorYaw");

        if (dp.hasMixedValue || dy.hasMixedValue) {
          EditorGUILayout.HelpBox("Editing angles is disabled because they have mixed values.",
                                  type : MessageType.Warning);
        } else {
          this.FloatProperty(prop : dp, "Pitch");
          this.FloatProperty(prop : dy, "Yaw");
        }

        if (EditorGUI.EndChangeCheck()) {
          var rp = dp.floatValue * Mathf.Deg2Rad;
          var ry = dy.floatValue * Mathf.Deg2Rad;

          var up_vector = new Vector4(Mathf.Sin(f : rp) * Mathf.Sin(f : ry),
                                      Mathf.Cos(f : rp),
                                      Mathf.Sin(f : rp) * Mathf.Cos(f : ry),
                                      0.0f);
          GetMaterialProperty(mats : this.targets, "_UpVector").vectorValue = up_vector;

          this.PropertiesChanged();
        }
      }
    }
  }
}
#endif
