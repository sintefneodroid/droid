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
      var data_path = Path.Combine(folders);
      data_path = Path.Combine(Application.persistentDataPath, data_path);

      if (!Directory.Exists(data_path)) {
        Directory.CreateDirectory(data_path);
      }

      if (file_name != null) {
        data_path = Path.Combine(data_path, file_name);
      }

      return data_path;
    }
  }
}
