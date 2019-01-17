using UnityEditor;

namespace Neodroid.Runtime {
  /// <summary>
  /// </summary>
  public static class NeodroidInfo {
    /// <summary>
    /// </summary>
    public const string _Version = "0.1.0";

    public const string _debug_pref_key = "EnableNeodroidDebug";
    public const string _import_location_pref_key = "NeodroidImportLocation";
    public const string _github_extension_pref_key = "NeodroidGithubExtension";

    /// <summary>
    /// </summary>

    #if NEODROID_PACKAGE
      public static string _ImportLocation = "Packages/com.neodroid.droid";
    #else
      public static string _ImportLocation = EditorPrefs.GetString(_import_location_pref_key,"Assets/Neodroid/");
    #endif
  }
}
