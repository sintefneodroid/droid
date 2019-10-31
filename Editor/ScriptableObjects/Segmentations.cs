using droid.Editor.Windows;
using UnityEngine;
#if UNITY_EDITOR
using droid.Runtime.ScriptableObjects;
using UnityEditor;

namespace droid.Editor.ScriptableObjects {
  public static class CreateSegmentations {
    [MenuItem(EditorScriptableObjectMenuPath._ScriptableObjectMenuPath + "Segmentations")]
    public static void CreateSegmentationsAsset() {
      var asset = ScriptableObject.CreateInstance<Segmentation>();

      AssetDatabase.CreateAsset(asset, EditorWindowMenuPath._NewAssetPath + "Assets/NewSegmentations.asset");
      AssetDatabase.SaveAssets();

      EditorUtility.FocusProjectWindow();

      Selection.activeObject = asset;
    }
  }
}
#endif
