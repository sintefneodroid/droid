using droid.Editor.Windows;
using droid.Runtime.ScriptableObjects;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

namespace droid.Editor.ScriptableObjects {
  /// <summary>
  /// 
  /// </summary>
  public static class CreatePlayerMotions {
    /// <summary>
    ///
    /// </summary>
    [MenuItem(EditorScriptableObjectMenuPath._ScriptableObjectMenuPath + "PlayerMotions")]
    public static void CreatePlayerMotionsAsset() {
      var asset = ScriptableObject.CreateInstance<PlayerMotions>();

      AssetDatabase.CreateAsset(asset, EditorWindowMenuPath._NewAssetPath + "Assets/NewPlayerMotions.asset");
      AssetDatabase.SaveAssets();

      EditorUtility.FocusProjectWindow();

      Selection.activeObject = asset;
    }
  }
}
#endif
