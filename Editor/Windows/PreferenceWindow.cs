#if UNITY_EDITOR
using System.Linq;
using Neodroid.Editor.ScriptableObjects;
using Neodroid.Runtime;
using TMPro;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;

namespace Neodroid.Editor.Windows {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class PreferenceWindow : MonoBehaviour {
    static bool _preferences_loaded;

    /// <summary>
    ///
    /// </summary>
    public static bool _EnableNeodroidDebug;

    const string _debug_pref_key = "EnableNeodroidDebug";

    /// <summary>
    ///
    /// </summary>
    [PreferenceItem("Neodroid")]
    public static void PreferencesGui() {
      EditorGUILayout.HelpBox($"Version {NeodroidInfo._Version}", MessageType.Info);

      if (!_preferences_loaded) {
        _EnableNeodroidDebug = EditorPrefs.GetBool(_debug_pref_key, false);
        _preferences_loaded = true;
      }

      _EnableNeodroidDebug = EditorGUILayout.Toggle(_debug_pref_key, _EnableNeodroidDebug);

      EditorGUILayout.HelpBox("Enter import path of Neodroid!", MessageType.Info);

      NeodroidInfo._ImportLocation = EditorGUILayout.TextField(NeodroidInfo._ImportLocation);

      if (GUI.changed) {
        if (_EnableNeodroidDebug) {
          DefineSymbolsFunctionality.AddDebugDefineSymbol();
        } else {
          DefineSymbolsFunctionality.RemoveDebugDefineSymbol();
        }

        EditorPrefs.SetBool(_debug_pref_key, _EnableNeodroidDebug);
      }
    }

    /*[SettingsProvider]
    static SettingsProvider CreateProjectSettingsProvider()
    {
      var provider = new AssetSettingsProvider("Project/Physics", "ProjectSettings/DynamicsManager.asset")
      {
        icon = EditorGUIUtility.IconContent("Profiler.Physics").image as Texture2D
      };
      SettingsProvider.GetSearchKeywordsFromSerializedObject(provider.CreateEditor().serializedObject, provider.keywords);
      return provider;
    }*/

    /*
     #if UNITY_2018_3_OR_NEWER
    [SettingsProvider]
    static SettingsProvider CreateNeodroidSettingsProvider() {
      var provider = new AssetSettingsProvider("Project/Neodroid", () => NeodroidSettings.Instance);
      provider.PopulateSearchKeywordsFromGUIContentProperties<Styles>();
      return provider;
    }
    #endif
    
    */
  }

  /// <inheritdoc />
  /// <summary>
  /// Adds the given define symbols to PlayerSettings define symbols.
  /// Just add your own define symbols to the Symbols property at the below.
  /// </summary>
  [InitializeOnLoad]
  public class DefineSymbolsController : UnityEditor.Editor {
    /// <summary>
    /// Add define symbols as soon as Unity gets done compiling.
    /// </summary>
    static DefineSymbolsController() { DefineSymbolsFunctionality.AddDefineSymbols(); }
  }

  public static class DefineSymbolsFunctionality {
    /// <summary>
    /// Symbols that will be added to the editor
    /// </summary>
    public static readonly string[] _Symbols = {"NEODROID", "NEODROID_EXISTS"};

    /// <summary>
    ///  Debug symbols that will be added to the editor
    /// </summary>
    public static readonly string[] _Debug_Symbols = {"NEODROID_DEBUG"};

    /// <summary>
    /// 
    /// </summary>
    public static void AddDefineSymbols() {
      var defines_string =
          PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
      var all_defines = defines_string.Split(';').ToList();
      all_defines.AddRange(_Symbols.Except(all_defines));
      PlayerSettings.SetScriptingDefineSymbolsForGroup(
          EditorUserBuildSettings.selectedBuildTargetGroup,
          string.Join(";", all_defines.ToArray()));
    }

    /// <summary>
    /// 
    /// </summary>
    public static void AddDebugDefineSymbol() {
      var defines_string =
          PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
      var all_defines = defines_string.Split(';').ToList();
      all_defines.AddRange(_Debug_Symbols.Except(all_defines));

      Debug.LogWarning($"Debug enabled: {true}");

      PlayerSettings.SetScriptingDefineSymbolsForGroup(
          EditorUserBuildSettings.selectedBuildTargetGroup,
          string.Join(";", all_defines.ToArray()));
    }

    /// <summary>
    /// 
    /// </summary>
    public static void RemoveDebugDefineSymbol() {
      var defines_string =
          PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
      var all_defines = defines_string.Split(';').ToList();
      foreach (var b in _Debug_Symbols) {
        var res = all_defines.RemoveAll(c => c == b);
        Debug.LogWarning($"Debug disabled: {res}");
      }

      PlayerSettings.SetScriptingDefineSymbolsForGroup(
          EditorUserBuildSettings.selectedBuildTargetGroup,
          string.Join(";", all_defines.ToArray()));
    }
  }
}
#endif
