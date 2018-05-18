﻿#if UNITY_EDITOR
using droid.Neodroid.Utilities.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace droid.Neodroid.Editor.ScriptableObjects {
  public static class CreatePlayerMotions {
    [MenuItem(EditorScriptableObjectMenuPath._ScriptableObjectMenuPath + "PlayerMotions")]
    public static void CreatePlayerMotionsAsset() {
      var asset = ScriptableObject.CreateInstance<PlayerMotions>();

      AssetDatabase.CreateAsset(asset, "Assets/NewPlayerMotions.asset");
      AssetDatabase.SaveAssets();

      EditorUtility.FocusProjectWindow();

      Selection.activeObject = asset;
    }
  }
}
#endif