﻿#if UNITY_EDITOR
using droid.Editor.Windows;
using droid.Runtime.Utilities.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace droid.Editor.ScriptableObjects {
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
