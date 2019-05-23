
using droid.Editor.Windows;
using droid.Runtime.Utilities.ScriptableObjects.Unused;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

namespace droid.Editor.ScriptableObjects {
  /// <summary>
  /// 
  /// </summary>
  public static class CreateNeodroidTask {
    /// <summary>
    /// 
    /// </summary>
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
