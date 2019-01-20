#if UNITY_EDITOR
using droid.Editor.Windows;
using droid.Runtime.Utilities.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace droid.Editor.ScriptableObjects {
  public static class CreateCurriculum {
    [MenuItem(EditorScriptableObjectMenuPath._ScriptableObjectMenuPath + "Curriculum")]
    public static void CreateCurriculumAsset() {
      var asset = ScriptableObject.CreateInstance<Curriculum>();

      AssetDatabase.CreateAsset(asset, EditorWindowMenuPath._NewAssetPath + "Assets/NewCurriculum.asset");
      AssetDatabase.SaveAssets();

      EditorUtility.FocusProjectWindow();

      Selection.activeObject = asset;
    }
  }
}
#endif
