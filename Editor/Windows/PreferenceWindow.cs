#if UNITY_EDITOR
using System.Linq;
using Neodroid.Runtime;
using UnityEditor;
using UnityEngine;

namespace Neodroid.Editor.Windows {



  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class PreferenceWindow : MonoBehaviour {

    static bool _preferences_loaded;

    /// <summary>
    /// </summary>
    static bool _EnableNeodroidDebug;
    static bool _UseGithubExtension;

    /// <summary>
    /// </summary>
    [PreferenceItem("Neodroid")]
    public static void PreferencesGui() {
      EditorGUILayout.HelpBox($"Version {NeodroidInfo._Version}", MessageType.Info);

      if (!_preferences_loaded) {
        _EnableNeodroidDebug = EditorPrefs.GetBool(NeodroidInfo._debug_pref_key, false);
        _UseGithubExtension = EditorPrefs.GetBool(NeodroidInfo._debug_pref_key, false);

        #if !NEODROID_PACKAGE
          NeodroidInfo._ImportLocation = EditorPrefs.GetString(NeodroidInfo._import_location_pref_key, "Assets/Neodroid/");
        #endif

        _preferences_loaded = true;
      }

      _EnableNeodroidDebug = EditorGUILayout.Toggle(NeodroidInfo._debug_pref_key, _EnableNeodroidDebug);
      _UseGithubExtension = EditorGUILayout.Toggle(NeodroidInfo._github_extension_pref_key, _UseGithubExtension);

      EditorGUILayout.HelpBox("Enter import path of Neodroid!", MessageType.Info);

      #if !NEODROID_PACKAGE
        NeodroidInfo._ImportLocation = EditorGUILayout.TextField(NeodroidInfo._ImportLocation);
      #endif

      if (GUI.changed) {
        if (_EnableNeodroidDebug) {
          DefineSymbolsFunctionality.AddDebugDefineSymbol();
        } else {
          DefineSymbolsFunctionality.RemoveDebugDefineSymbols();
        }

        if (_UseGithubExtension){
          DefineSymbolsFunctionality.AddGithubDefineSymbol();
        } else {
          DefineSymbolsFunctionality.RemoveGithubDefineSymbol();
        }

        #if !NEODROID_PACKAGE
          EditorPrefs.SetString(NeodroidInfo._import_location_pref_key, NeodroidInfo._ImportLocation);
          Debug.Log($"Set Neodroid import location to: {NeodroidInfo._ImportLocation}");
        #endif

        EditorPrefs.SetBool(NeodroidInfo._debug_pref_key, _EnableNeodroidDebug);
        EditorPrefs.SetBool(NeodroidInfo._github_extension_pref_key, _UseGithubExtension);
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
  ///   Adds the given define symbols to PlayerSettings define symbols.
  ///   Just add your own define symbols to the Symbols property at the below.
  /// </summary>
  [InitializeOnLoad]
  public class DefineSymbolsController : UnityEditor.Editor {
    /// <summary>
    ///   Add define symbols as soon as Unity gets done compiling.
    /// </summary>
    static DefineSymbolsController(){
      DefineSymbolsFunctionality.AddDefineSymbols();
    }
  }

  public static class DefineSymbolsFunctionality {
    /// <summary>
    ///   Symbols that will be added to the editor
    /// </summary>
    public static readonly string[] _Symbols = {"NEODROID", "NEODROID_EXISTS"};

    /// <summary>
    ///   Debug symbols that will be added to the editor
    /// </summary>
    public static readonly string[] _Debug_Symbols = {"NEODROID_DEBUG"};

    public static readonly string[] _Github_Symbols = {"USE_GITHUB_EXTENSION"};

    /// <summary>
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
    /// </summary>
    public static void AddDebugDefineSymbol() {
      AddDefineSymbols(_Debug_Symbols);

      Debug.LogWarning($"Neodroid Debugging enabled");
    }

    public static void RemoveDebugDefineSymbols(){
      RemoveDefineSymbols(_Debug_Symbols);

      Debug.LogWarning($"Neodroid Debugging disabled");
    }

    public static void AddGithubDefineSymbol() {
      AddDefineSymbols(_Github_Symbols);

      Debug.LogWarning($"Github Extension enabled");
    }

    public static void RemoveGithubDefineSymbol(){
      RemoveDefineSymbols(_Github_Symbols);

      Debug.LogWarning($"Github Extension disabled");
    }

    public static void AddDefineSymbols(string[] symbols){
      var defines_string =
        PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
      var all_defines = defines_string.Split(';').ToList();
      all_defines.AddRange(symbols.Except(all_defines));

      PlayerSettings.SetScriptingDefineSymbolsForGroup(
        EditorUserBuildSettings.selectedBuildTargetGroup,
        string.Join(";", all_defines.ToArray()));
    }

    /// <summary>
    /// </summary>
    public static void RemoveDefineSymbols(string[] symbols) {
      var defines_string =
          PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
      var all_defines = defines_string.Split(';').ToList();
      foreach (var b in symbols) {
        var res = all_defines.RemoveAll(c => c == b);
        Debug.LogWarning($"Removed define symbols {symbols.Aggregate((aa,bb)=>aa+","+bb)} : number of entries removed {res}");
      }

      PlayerSettings.SetScriptingDefineSymbolsForGroup(
          EditorUserBuildSettings.selectedBuildTargetGroup,
          string.Join(";", all_defines.ToArray()));
    }
  }
}
#endif
