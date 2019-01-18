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
    static bool _ImportedAsset;
    static string _ImportLocation;
    static bool _GenerateScenePreviews;
    static string _ScenePreviewsLocation;

    /// <summary>
    /// </summary>
    [PreferenceItem("Neodroid")]
    public static void PreferencesGui() {


      if (!_preferences_loaded) {
        _EnableNeodroidDebug = EditorPrefs.GetBool(NeodroidInfo._debug_pref_key, false);
        _UseGithubExtension = EditorPrefs.GetBool(NeodroidInfo._github_extension_pref_key, false);
        _ImportedAsset = EditorPrefs.GetBool(NeodroidInfo._imported_asset_pref_key, false);
        _GenerateScenePreviews = EditorPrefs.GetBool(NeodroidInfo._generate_previews_pref_key, false);
        if(_GenerateScenePreviews){_ScenePreviewsLocation = EditorPrefs.GetString(NeodroidInfo
        ._generate_previews_loc_pref_key, NeodroidInfo.ScenePreviewsLocation);}
          
        
        #if NEODROID_IMPORTED_ASSET
          _ImportLocation = EditorPrefs.GetString(NeodroidInfo._import_location_pref_key, NeodroidInfo.ImportLocation);
        #endif

        _preferences_loaded = true;
      }
      
      EditorGUILayout.HelpBox($"Version {NeodroidInfo._Version}", MessageType.Info);
      
      _ImportedAsset = EditorGUILayout.Toggle(NeodroidInfo._imported_asset_pref_key, _ImportedAsset);
      
      #if NEODROID_IMPORTED_ASSET
        EditorGUILayout.HelpBox("Enter import path of Neodroid", MessageType.Info);
        _ImportLocation = EditorGUILayout.TextField(_ImportLocation);
      #endif
             
      EditorGUILayout.HelpBox("Functionality", MessageType.Info);

      _EnableNeodroidDebug = EditorGUILayout.Toggle(NeodroidInfo._debug_pref_key, _EnableNeodroidDebug);
      _UseGithubExtension = EditorGUILayout.Toggle(NeodroidInfo._github_extension_pref_key, _UseGithubExtension);
      _GenerateScenePreviews = EditorGUILayout.Toggle(NeodroidInfo._generate_previews_pref_key, _GenerateScenePreviews);
      if (_GenerateScenePreviews){
        EditorGUILayout.HelpBox("Enter path for scene preview storage", MessageType.Info);
        _ScenePreviewsLocation = EditorGUILayout.TextField(_ScenePreviewsLocation);
      }



      if (GUI.changed) {
        if (_EnableNeodroidDebug) {
          DefineSymbolsFunctionality.AddDebugDefineSymbol();
        } else {
          DefineSymbolsFunctionality.RemoveDebugDefineSymbols();
        }

        if (_UseGithubExtension){
          DefineSymbolsFunctionality.AddGithubDefineSymbols();
        } else {
          DefineSymbolsFunctionality.RemoveGithubDefineSymbols();
        }
        
        if (_ImportedAsset){
          DefineSymbolsFunctionality.AddImportedAssetDefineSymbols();
        } else {
          DefineSymbolsFunctionality.RemoveImportedAssetDefineSymbols();
        }

        #if NEODROID_IMPORTED_ASSET
          if (          NeodroidInfo.ImportLocation != _ImportLocation){
            NeodroidInfo.ImportLocation = _ImportLocation;
            Debug.Log($"Set Neodroid import location to: {NeodroidInfo.ImportLocation}");
          }
         
          EditorPrefs.SetString(NeodroidInfo._import_location_pref_key, _ImportLocation);
        #endif

        EditorPrefs.SetBool(NeodroidInfo._debug_pref_key, _EnableNeodroidDebug);
        EditorPrefs.SetBool(NeodroidInfo._github_extension_pref_key, _UseGithubExtension);
        EditorPrefs.SetBool(NeodroidInfo._imported_asset_pref_key, _ImportedAsset);
        
        EditorPrefs.SetBool(NeodroidInfo._generate_previews_pref_key, _GenerateScenePreviews);
        if (_GenerateScenePreviews){
          EditorPrefs.SetString(NeodroidInfo._generate_previews_loc_pref_key, _ScenePreviewsLocation);
        }

        _preferences_loaded = false;
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

    public static readonly string[] _Github_Symbols = {"NEODROID_USE_GITHUB_EXTENSION"};
    
    public static readonly string[] _ImportedAsset_Symbols = {"NEODROID_IMPORTED_ASSET"};

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

    public static void AddGithubDefineSymbols() {
      AddDefineSymbols(_Github_Symbols);

      Debug.LogWarning($"Github Extension enabled");
    }

    public static void RemoveGithubDefineSymbols(){
      RemoveDefineSymbols(_Github_Symbols);

      Debug.LogWarning($"Github Extension disabled");
    }
    
    public static void AddImportedAssetDefineSymbols() {
      AddDefineSymbols(_ImportedAsset_Symbols);

      Debug.LogWarning($"Neodroid is assumed to be an imported asset");
    }

    public static void RemoveImportedAssetDefineSymbols(){
      RemoveDefineSymbols(_ImportedAsset_Symbols);

      Debug.LogWarning($"Neodroid is assumed to be an installed package");
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
