using droid.Editor.Windows;
using UnityEngine;
#if UNITY_EDITOR
using droid.Runtime.ScriptableObjects.Deprecated;
using UnityEditor;

namespace droid.Editor.ScriptableObjects {
  /// <summary>
  /// 
  /// </summary>
  public static class CreateNeodroidTask {
    /// <summary>
    /// 
    /// </summary>
    [MenuItem(itemName : EditorScriptableObjectMenuPath._ScriptableObjectMenuPath + "NeodroidTask")]
    public static void CreateNeodroidTaskAsset() {
      var asset = ScriptableObject.CreateInstance<NeodroidTask>();

      AssetDatabase.CreateAsset(asset : asset,
                                path : EditorWindowMenuPath._NewAssetPath + "Assets/NewNeodroidTask.asset");
      AssetDatabase.SaveAssets();

      EditorUtility.FocusProjectWindow();

      Selection.activeObject = asset;
    }
  }
}
#endif
