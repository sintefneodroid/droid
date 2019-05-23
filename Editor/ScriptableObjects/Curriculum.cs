
using droid.Editor.Windows;
using droid.Runtime.Utilities.ScriptableObjects.Unused;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

namespace droid.Editor.ScriptableObjects {
  /// <summary>
  /// 
  /// </summary>
  public static class CreateCurriculum {
    /// <summary>
    /// 
    /// </summary>
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
