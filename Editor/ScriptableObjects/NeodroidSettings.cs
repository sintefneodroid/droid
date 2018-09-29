using System;
using Common.Editors;
using UnityEngine;

namespace Neodroid.Editor.ScriptableObjects {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [Serializable]
  [ExecuteInEditMode]
  // ReSharper disable once RequiredBaseTypesIsNotInherited
  public class NeodroidSettings : ScriptableObject {
    static NeodroidSettings _instance;

    /// <summary>
    ///   Returns the release version of the product.
    /// </summary>
    public static string Version { get { return "1.3.0"; } }

    /// <summary>
    ///   Get a singleton instance of the settings class.
    /// </summary>
    public static NeodroidSettings Instance {
      get {
        if (_instance == null) {
          _instance = Resources.Load<NeodroidSettings>("TMP Settings");

          #if UNITY_EDITOR
          // Make sure UPM(Unity Package Manager) packages resources have been added to the user project
          if (_instance == null) {
            // Open Resources Importer
            NeodroidPackageImporterWindow.ShowPackageImporterWindow();
          }
          #endif
        }

        return _instance;
      }
    }

    /// <summary>
    ///   Static Function to load the Settings file.
    /// </summary>
    /// <returns></returns>
    public static NeodroidSettings LoadDefaultSettings() {
      if (_instance == null) {
        // Load settings from Settings file
        var settings = Resources.Load<NeodroidSettings>("Neodroid Settings");
        if (settings != null) {
          _instance = settings;
        }
      }

      return _instance;
    }

    /// <summary>
    ///   Returns the Sprite Asset defined in the Settings file.
    /// </summary>
    /// <returns></returns>
    public static NeodroidSettings GetSettings() {
      if (Instance == null) {
        return null;
      }

      return Instance;
    }
  }
}
