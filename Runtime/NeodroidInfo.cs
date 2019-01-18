using System.Linq;
using UnityEditor;

namespace Neodroid.Runtime {
  /// <summary>
  /// </summary>
  public static class NeodroidInfo {
    /// <summary>
    /// </summary>
    public const string _Version = "0.1.1";

    public const string _debug_pref_key = "EnableNeodroidDebug";
    public const string _github_extension_pref_key = "NeodroidGithubExtension";
    public const string _imported_asset_pref_key = "NeodroidImportedAsset";
    
    public static string ImportLocation
    {
      get { return _ImportLocation;}
      set { _ImportLocation = value.TrimEnd('/')+"/"; } 
    }

    #if NEODROID_IMPORTED_ASSET
      public const string _import_location_pref_key = "NeodroidImportLocation";
      static string _ImportLocation = EditorPrefs.GetString(_import_location_pref_key, "Assets/droid");
#else
      static string _ImportLocation = "Packages/com.neodroid.droid/";
    #endif
  }
}
