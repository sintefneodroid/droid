/*
#if UNITY_EDITOR
using droid.Editor.Utilities;

using UnityEditor;
using UnityEngine;

using System.Linq;
using droid.Runtime;

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

    static bool _generate_scene_descriptions;
    static string _scene_descriptions_location;

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
          _scene_previews_location = EditorPrefs.GetString(NeodroidEditorInfo._Generate_Previews_Loc_Pref_Key,
                                                           NeodroidEditorInfo.ScenePreviewsLocation);
        }

        _generate_scene_descriptions =
            EditorPrefs.GetBool(NeodroidEditorInfo._Generate_Descriptions_Pref_Key, false);
        if (_generate_scene_descriptions) {
          _scene_descriptions_location =
              EditorPrefs.GetString(NeodroidEditorInfo._Generate_Descriptions_Loc_Pref_Key,
                                    NeodroidEditorInfo.SceneDescriptionLocation);
        }

        #if NEODROID_IS_PACKAGE
        _import_location = EditorPrefs.GetString(NeodroidEditorInfo._Import_Location_Pref_Key,
                                                 NeodroidEditorInfo.ImportLocation);
        #endif

        _preferences_loaded = true;
      }

      EditorGUILayout.HelpBox($"Version {NeodroidRuntimeInfo._Version}", MessageType.Info);

      var imported_asset_new =
          EditorGUILayout.Toggle(NeodroidEditorInfo._Imported_Asset_Pref_Key, _imported_asset);

      #if NEODROID_IS_PACKAGE
      EditorGUILayout.HelpBox("Enter import path of Neodroid", MessageType.Info);
      _import_location = EditorGUILayout.TextField(_import_location);
      #endif

      EditorGUILayout.HelpBox("Functionality", MessageType.Info);

      var enable_neodroid_debug_new =
          EditorGUILayout.Toggle(NeodroidEditorInfo._Debug_Pref_Key, _enable_neodroid_debug);
      var use_github_extension_new =
          EditorGUILayout.Toggle(NeodroidEditorInfo._Github_Extension_Pref_Key, _use_github_extension);
      var generate_scene_previews_new =
          EditorGUILayout.Toggle(NeodroidEditorInfo._Generate_Previews_Pref_Key, _generate_scene_previews);
      if (_generate_scene_previews) {
        EditorGUILayout.HelpBox("Enter path for scene preview storage", MessageType.Info);
        _scene_previews_location = EditorGUILayout.TextField(_scene_previews_location);
      }

      var generate_scene_descriptions_new =
          EditorGUILayout.Toggle(NeodroidEditorInfo._Generate_Descriptions_Pref_Key,
                                 _generate_scene_descriptions);
      if (_generate_scene_descriptions) {
        EditorGUILayout.HelpBox("Enter path for scene description storage", MessageType.Info);
        _scene_descriptions_location = EditorGUILayout.TextField(_scene_descriptions_location);
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

        #if NEODROID_IS_PACKAGE
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
            EditorPrefs.SetString(NeodroidEditorInfo._Generate_Previews_Loc_Pref_Key,
                                  _scene_previews_location);
          }
        }

        if (generate_scene_descriptions_new != _generate_scene_descriptions) {
          _generate_scene_descriptions = generate_scene_descriptions_new;
          Debug.Log($"Setting Neodroid Generate SceneDescription: {_generate_scene_descriptions}");
          EditorPrefs.SetBool(NeodroidEditorInfo._Generate_Descriptions_Pref_Key,
                              _generate_scene_descriptions);
        }

        if (_generate_scene_descriptions) {
          if (NeodroidEditorInfo.SceneDescriptionLocation != _scene_descriptions_location) {
            NeodroidEditorInfo.SceneDescriptionLocation = _scene_descriptions_location;
            EditorPrefs.SetString(NeodroidEditorInfo._Generate_Descriptions_Loc_Pref_Key,
                                  _scene_descriptions_location);
          }
        }

        _preferences_loaded = false;
      }
    }

    void OnValidate() { _preferences_loaded = false; }


  }
}
#endif
*/


