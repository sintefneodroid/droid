#if UNITY_EDITOR
using droid.Neodroid.Editor.Windows;
using droid.Neodroid.Utilities.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace droid.Neodroid.Editor.ScriptableObjects {
  public static class CreateNeodroidTask {
    [MenuItem(EditorScriptableObjectMenuPath._ScriptableObjectMenuPath + "NeodroidTask")]
    public static void CreateNeodroidTaskAsset() {
      var asset = ScriptableObject.CreateInstance<NeodroidTask>();

      AssetDatabase.CreateAsset(asset, EditorWindowMenuPath._NewAssetPath + "Assets/NewNeodroidTask.asset");
      AssetDatabase.SaveAssets();

      EditorUtility.FocusProjectWindow();

      Selection.activeObject = asset;
    }
  }
}
#endif
