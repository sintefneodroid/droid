#if UNITY_EDITOR
using droid.Editor.Windows;
using droid.Runtime.Utilities.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace droid.Editor.ScriptableObjects {
  public static class CreatePlayerMotions {
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
