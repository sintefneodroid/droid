#if UNITY_EDITOR
using droid.Neodroid.Utilities.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace droid.Neodroid.Editor.ScriptableObjects {
  public static class CreateCurriculum {
    [MenuItem(EditorScriptableObjectMenuPath._ScriptableObjectMenuPath + "Curriculum")]
    public static void CreateCurriculumAsset() {
      var asset = ScriptableObject.CreateInstance<Curriculum>();

      AssetDatabase.CreateAsset(asset, "Assets/NewCurriculum.asset");
      AssetDatabase.SaveAssets();

      EditorUtility.FocusProjectWindow();

      Selection.activeObject = asset;
    }
  }
}
#endif
