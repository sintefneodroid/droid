using System.IO;
using UnityEngine;

namespace droid.Runtime.Utilities {
  /// <summary>
  /// </summary>
  public static partial class NeodroidUtilities {
    /// <summary>
    ///
    /// </summary>
    /// <param name="folders"></param>
    /// <param name="file_name"></param>
    /// <returns></returns>
    public static string GetPersistentDataPath(string[] folders, string file_name = null) {
      var data_path = Path.Combine(paths : folders);
      data_path = Path.Combine(path1 : Application.persistentDataPath, path2 : data_path);

      if (!Directory.Exists(path : data_path)) {
        Directory.CreateDirectory(path : data_path);
      }

      if (file_name != null) {
        data_path = Path.Combine(path1 : data_path, path2 : file_name);
      }

      return data_path;
    }
  }
}
