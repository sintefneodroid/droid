using UnityEditor;
using UnityEngine;

namespace droid.Editor
{
    /// <summary>
    /// </summary>
    public static class NeodroidEditorInfo
    {
        /// <summary>
        /// </summary>
        public const string _Version = "0.1.1";

        public const string _debug_pref_key = "EnableNeodroidDebug";
        public const string _github_extension_pref_key = "NeodroidGithubExtension";
        public const string _imported_asset_pref_key = "NeodroidImportedAsset";
        public const string _generate_previews_pref_key = "NeodroidGeneratePreviews";
        public const string _generate_previews_loc_pref_key = "NeodroidPreviewsLocation";

        public static string ImportLocation{
            get { return _import_location; }
            set { var new_path = value.TrimEnd('/') +"/";
                Debug.Log($"Setting Neodroid import location to: {new_path}");
                _import_location = new_path;
            }
        }

        public static bool GenerateScenePreviews {
            get { return EditorPrefs.GetBool(_generate_previews_pref_key, false); }
        }

        static string _scene_previews_location = EditorPrefs.GetString(_generate_previews_loc_pref_key, "ScenePreviews/");

        public static string ScenePreviewsLocation{
            get { return _scene_previews_location; }
            set {
              var new_path = value.TrimEnd('/') + "/";
                Debug.Log($"Setting Neodroid ScenePreview location to: {new_path}");
                _scene_previews_location = new_path;
            }
        }



#if NEODROID_IMPORTED_ASSET
        public const string _import_location_pref_key = "NeodroidImportLocation";
        static string _import_location = EditorPrefs.GetString(_import_location_pref_key, "Assets/droid/");
#else
        static string _import_location = "Packages/com.neodroid.droid/";
#endif
    }
}
