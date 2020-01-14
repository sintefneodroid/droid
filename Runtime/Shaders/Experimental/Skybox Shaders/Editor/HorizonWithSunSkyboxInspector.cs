#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace droid.Runtime.Shaders.Experimental.Skybox_Shaders.Editor {
  public class HorizonWithSunSkyboxInspector : MaterialEditor {
    public override void OnInspectorGUI() {
      this.serializedObject.Update();

      if (this.isVisible) {
        EditorGUI.BeginChangeCheck();

        GUILayout.Label("Background Parameters");

        EditorGUILayout.Space();

        this.ColorProperty(GetMaterialProperty(mats : this.targets, "_SkyColor1"), "Top Color");
        this.FloatProperty(GetMaterialProperty(mats : this.targets, "_SkyExponent1"), "Exponential Factor");

        EditorGUILayout.Space();

        this.ColorProperty(GetMaterialProperty(mats : this.targets, "_SkyColor2"), "Horizon Color");

        EditorGUILayout.Space();

        this.ColorProperty(GetMaterialProperty(mats : this.targets, "_SkyColor3"), "Bottom Color");
        this.FloatProperty(GetMaterialProperty(mats : this.targets, "_SkyExponent2"), "Exponential Factor");

        EditorGUILayout.Space();

        this.FloatProperty(GetMaterialProperty(mats : this.targets, "_SkyIntensity"), "Intensity");

        EditorGUILayout.Space();

        GUILayout.Label("Sun Parameters");

        EditorGUILayout.Space();

        this.ColorProperty(GetMaterialProperty(mats : this.targets, "_SunColor"), "Color");
        this.FloatProperty(GetMaterialProperty(mats : this.targets, "_SunIntensity"), "Intensity");

        EditorGUILayout.Space();

        this.FloatProperty(GetMaterialProperty(mats : this.targets, "_SunAlpha"), "Alpha");
        this.FloatProperty(GetMaterialProperty(mats : this.targets, "_SunBeta"), "Beta");

        EditorGUILayout.Space();

        var az = GetMaterialProperty(mats : this.targets, "_SunAzimuth");
        var al = GetMaterialProperty(mats : this.targets, "_SunAltitude");

        if (az.hasMixedValue || al.hasMixedValue) {
          EditorGUILayout.HelpBox("Editing angles is disabled because they have mixed values.",
                                  type : MessageType.Warning);
        } else {
          this.FloatProperty(prop : az, "Azimuth");
          this.FloatProperty(prop : al, "Altitude");
        }

        if (EditorGUI.EndChangeCheck()) {
          var raz = az.floatValue * Mathf.Deg2Rad;
          var ral = al.floatValue * Mathf.Deg2Rad;

          var up_vector = new Vector4(Mathf.Cos(f : ral) * Mathf.Sin(f : raz),
                                      Mathf.Sin(f : ral),
                                      Mathf.Cos(f : ral) * Mathf.Cos(f : raz),
                                      0.0f);
          GetMaterialProperty(mats : this.targets, "_SunVector").vectorValue = up_vector;

          this.PropertiesChanged();
        }
      }
    }
  }
}
#endif
