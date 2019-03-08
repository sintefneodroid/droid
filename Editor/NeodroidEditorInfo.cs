using UnityEditor;
using UnityEngine;

namespace droid.Editor {
  /// <summary>
  /// </summary>
  public static class NeodroidEditorInfo {
    /// <summary>
    /// </summary>
    public const string _Version = "0.1.1";

    public const string _Debug_Pref_Key = "EnableNeodroidDebug";
    public const string _Github_Extension_Pref_Key = "NeodroidGithubExtension";
    public const string _Imported_Asset_Pref_Key = "NeodroidImportedAsset";
    public const string _Generate_Previews_Pref_Key = "NeodroidGeneratePreviews";
    public const string _Generate_Previews_Loc_Pref_Key = "NeodroidPreviewsLocation";
    public const string _Generate_Descriptions_Pref_Key = "NeodroidGenerateDescriptions";
    public const string _Generate_Descriptions_Loc_Pref_Key = "NeodroidDescriptionLocation";

    static string _scene_previews_location =
      EditorPrefs.GetString(_Generate_Previews_Loc_Pref_Key, "ScenePreviews/");

    static string _scene_description_location =
      EditorPrefs.GetString(_Generate_Descriptions_Loc_Pref_Key, "SceneDescriptions/");

    public static string ImportLocation {
      get { return _import_location; }
      set {
        var new_path = value.TrimEnd('/') + "/";
        Debug.Log($"Setting Neodroid import location to: {new_path}");
        _import_location = new_path;
      }
    }

    public static bool GenerateScenePreviews {
      get { return EditorPrefs.GetBool(_Generate_Previews_Pref_Key, false); }
    }

    public static bool GenerateSceneDescriptions {
      get { return EditorPrefs.GetBool(_Generate_Descriptions_Pref_Key, false); }
    }

    public static string ScenePreviewsLocation {
      get { return _scene_previews_location; }
      set {
        var new_path = value.TrimEnd('/') + "/";
        Debug.Log($"Setting Neodroid ScenePreview location to: {new_path}");
        _scene_previews_location = new_path;
      }
    }

    public static string SceneDescriptionLocation {
      get { return _scene_description_location; }
      set {
        var new_path = value.TrimEnd('/') + "/";
        Debug.Log($"Setting Neodroid SceneDescription location to: {new_path}");
        _scene_description_location = new_path;
      }
    }

    #if NEODROID_IMPORTED_ASSET
    public const string _Import_Location_Pref_Key = "NeodroidImportLocation";
    static string _import_location = EditorPrefs.GetString(_Import_Location_Pref_Key, "Assets/droid/");
    #else
        static string _import_location = "Packages/com.neodroid.droid/";
    #endif
  }
}
