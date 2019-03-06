#if UNITY_EDITOR
using System.Linq;
using droid.Runtime;
using UnityEditor;
using UnityEngine;

namespace droid.Editor.Windows {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class PreferenceWindow : MonoBehaviour {
    static bool _preferences_loaded;

    /// <summary>
    /// </summary>
    static bool _enable_neodroid_debug;

    static bool _use_github_extension;
    static bool _imported_asset;
    static string _import_location;
    static bool _generate_scene_previews;
    static string _scene_previews_location;

    /// <summary>
    /// </summary>
    [PreferenceItem("Neodroid")]
    public static void PreferencesGui() {
      if (!_preferences_loaded) {
        _enable_neodroid_debug = EditorPrefs.GetBool(NeodroidEditorInfo._Debug_Pref_Key, false);
        _use_github_extension = EditorPrefs.GetBool(NeodroidEditorInfo._Github_Extension_Pref_Key, false);
        _imported_asset = EditorPrefs.GetBool(NeodroidEditorInfo._Imported_Asset_Pref_Key, false);
        _generate_scene_previews = EditorPrefs.GetBool(NeodroidEditorInfo._Generate_Previews_Pref_Key, false);
        if (_generate_scene_previews) {
          _scene_previews_location = EditorPrefs.GetString(
              NeodroidEditorInfo._Generate_Previews_Loc_Pref_Key,
              NeodroidEditorInfo.ScenePreviewsLocation);
        }

        #if NEODROID_IMPORTED_ASSET
        _import_location = EditorPrefs.GetString(
            NeodroidEditorInfo._Import_Location_Pref_Key,
            NeodroidEditorInfo.ImportLocation);
        #endif

        _preferences_loaded = true;
      }

      EditorGUILayout.HelpBox($"Version {NeodroidEditorInfo._Version}", MessageType.Info);

      var imported_asset_new = EditorGUILayout.Toggle(
          NeodroidEditorInfo._Imported_Asset_Pref_Key,
          _imported_asset);

      #if NEODROID_IMPORTED_ASSET
      EditorGUILayout.HelpBox("Enter import path of Neodroid", MessageType.Info);
      _import_location = EditorGUILayout.TextField(_import_location);
      #endif

      EditorGUILayout.HelpBox("Functionality", MessageType.Info);

      var enable_neodroid_debug_new = EditorGUILayout.Toggle(
          NeodroidEditorInfo._Debug_Pref_Key,
          _enable_neodroid_debug);
      var use_github_extension_new = EditorGUILayout.Toggle(
          NeodroidEditorInfo._Github_Extension_Pref_Key,
          _use_github_extension);
      var generate_scene_previews_new = EditorGUILayout.Toggle(
          NeodroidEditorInfo._Generate_Previews_Pref_Key,
          _generate_scene_previews);
      if (_generate_scene_previews) {
        EditorGUILayout.HelpBox("Enter path for scene preview storage", MessageType.Info);
        _scene_previews_location = EditorGUILayout.TextField(_scene_previews_location);
      }

      if (GUI.changed) {
        if (enable_neodroid_debug_new != _enable_neodroid_debug) {
          _enable_neodroid_debug = enable_neodroid_debug_new;
          EditorPrefs.SetBool(NeodroidEditorInfo._Debug_Pref_Key, _enable_neodroid_debug);
          Debug.Log($"Neodroid Debugging {_enable_neodroid_debug}");
          if (_enable_neodroid_debug) {
            DefineSymbolsFunctionality.AddDebugDefineSymbol();
          } else {
            DefineSymbolsFunctionality.RemoveDebugDefineSymbols();
          }
        }

        if (use_github_extension_new != _use_github_extension) {
          _use_github_extension = use_github_extension_new;
          EditorPrefs.SetBool(NeodroidEditorInfo._Github_Extension_Pref_Key, _use_github_extension);
          Debug.Log($"Neodroid GitHub Extension{_use_github_extension}");
          if (_use_github_extension) {
            DefineSymbolsFunctionality.AddGithubDefineSymbols();
          } else {
            DefineSymbolsFunctionality.RemoveGithubDefineSymbols();
          }
        }

        if (imported_asset_new != _imported_asset) {
          _imported_asset = imported_asset_new;
          EditorPrefs.SetBool(NeodroidEditorInfo._Imported_Asset_Pref_Key, _imported_asset);
          Debug.Log($"Neodroid is set as an imported asset {_imported_asset}");
          if (_imported_asset) {
            DefineSymbolsFunctionality.AddImportedAssetDefineSymbols();
          } else {
            DefineSymbolsFunctionality.RemoveImportedAssetDefineSymbols();
          }
        }

        #if NEODROID_IMPORTED_ASSET
        if (NeodroidEditorInfo.ImportLocation != _import_location) {
          NeodroidEditorInfo.ImportLocation = _import_location;
          EditorPrefs.SetString(NeodroidEditorInfo._Import_Location_Pref_Key, _import_location);
        }
        #endif

        if (generate_scene_previews_new != _generate_scene_previews) {
          _generate_scene_previews = generate_scene_previews_new;
          Debug.Log($"Setting Neodroid Generate ScenePreview: {_generate_scene_previews}");
          EditorPrefs.SetBool(NeodroidEditorInfo._Generate_Previews_Pref_Key, _generate_scene_previews);
        }

        if (_generate_scene_previews) {
          if (NeodroidEditorInfo.ScenePreviewsLocation != _scene_previews_location) {
            NeodroidEditorInfo.ScenePreviewsLocation = _scene_previews_location;
            EditorPrefs.SetString(
                NeodroidEditorInfo._Generate_Previews_Loc_Pref_Key,
                _scene_previews_location);

          }
        }

        _preferences_loaded = false;
      }
    }

    void OnValidate() { _preferences_loaded = false; }

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
    static DefineSymbolsController() { DefineSymbolsFunctionality.AddDefineSymbols(); }
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

    public static readonly string[] _IsImportedAsset_Symbols = {"NEODROID_ASSET_IMPORT"};

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

    public static void RemoveDebugDefineSymbols() {
      RemoveDefineSymbols(_Debug_Symbols);

      Debug.LogWarning($"Neodroid Debugging disabled");
    }

    public static void AddGithubDefineSymbols() {
      AddDefineSymbols(_Github_Symbols);

      Debug.LogWarning($"Github Extension enabled");
    }

    public static void RemoveGithubDefineSymbols() {
      RemoveDefineSymbols(_Github_Symbols);

      Debug.LogWarning($"Github Extension disabled");
    }

    public static void AddImportedAssetDefineSymbols() {
      AddDefineSymbols(_ImportedAsset_Symbols);

      Debug.LogWarning($"Neodroid is assumed to be an imported asset");
    }

    public static void RemoveImportedAssetDefineSymbols() {
      RemoveDefineSymbols(_ImportedAsset_Symbols);

      Debug.LogWarning($"Neodroid is assumed to be an installed package");
    }

    public static void AddDefineSymbols(string[] symbols) {
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
        Debug.LogWarning(
            $"Removed define symbols {symbols.Aggregate((aa, bb) => aa + "," + bb)} : number of entries removed {res}");
      }

      PlayerSettings.SetScriptingDefineSymbolsForGroup(
          EditorUserBuildSettings.selectedBuildTargetGroup,
          string.Join(";", all_defines.ToArray()));
    }
  }
}
#endif
