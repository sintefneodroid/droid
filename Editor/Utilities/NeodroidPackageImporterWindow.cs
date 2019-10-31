using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace droid.Editor.Utilities {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class NeodroidPackageImporterWindow : EditorWindow {
    [SerializeField] bool kEssentialResourcesImported;
    [SerializeField] bool kExamplesAndExtrasResourcesImported;
    [SerializeField] bool kIsImportingExamples;

    /// <summary>
    /// </summary>
    public static void ShowPackageImporterWindow() {
      var window = GetWindow<NeodroidPackageImporterWindow>();
      window.titleContent = new GUIContent("Neodroid Importer");
      window.Focus();
    }

    void OnEnable() {
      // Set Editor Window Size
      this.SetEditorWindowSize();

      // Special handling due to scripts imported in a .unitypackage result in resulting in an assembly reload which clears the callbacks.
      if (this.kIsImportingExamples) {
        AssetDatabase.importPackageCompleted += this.ImportCallback;
        this.kIsImportingExamples = false;
      }
    }

    void OnDestroy() {
      this.kEssentialResourcesImported = false;
      this.kExamplesAndExtrasResourcesImported = false;
    }

    void OnGUI() {
      var import_essentials_package = false;
      var import_examples_package = false;

      GUILayout.BeginVertical();
      {
        // Display options to import Essential resources
        GUILayout.BeginVertical(EditorStyles.helpBox);
        {
          GUILayout.Label("Neodroid Essentials", EditorStyles.boldLabel);
          GUILayout.Label("This appears to be the first time you access TextMesh Pro, as such we need to add resources to your project that are essential for using TextMesh Pro. These new resources will be placed at the root of your project in the \"TextMesh Pro\" folder.",
                          new GUIStyle(EditorStyles.label) {wordWrap = true});
          GUILayout.Space(5f);

          GUI.enabled = !this.kEssentialResourcesImported;
          if (GUILayout.Button("Import TMP Essentials")) {
            import_essentials_package = true;
          }

          GUILayout.Space(5f);
          GUI.enabled = true;
        }
        GUILayout.EndVertical();

        // Display options to import Examples & Extras
        GUILayout.BeginVertical(EditorStyles.helpBox);
        {
          GUILayout.Label("TMP Examples & Extras", EditorStyles.boldLabel);
          GUILayout.Label("The Examples & Extras package contains addition resources and examples that will make discovering and learning about TextMesh Pro's powerful features easier. These additional resources will be placed in the same folder as the TMP essential resources.",
                          new GUIStyle(EditorStyles.label) {wordWrap = true});
          GUILayout.Space(5f);

          GUI.enabled = this.kEssentialResourcesImported && !this.kExamplesAndExtrasResourcesImported;
          if (GUILayout.Button("Import TMP Examples & Extras")) {
            import_examples_package = true;
          }

          GUILayout.Space(5f);
          GUI.enabled = true;
        }
        GUILayout.EndVertical();
      }
      GUILayout.EndVertical();
      GUILayout.Space(5f);

      // Import Essential Resources
      if (import_essentials_package) {
        AssetDatabase.importPackageCompleted += this.ImportCallback;

        var package_full_path = GetPackageFullPath();
        AssetDatabase.ImportPackage(package_full_path
                                    + "/Package Resources/TMP Essential Resources.unitypackage",
                                    false);
      }

      // Import Examples & Extras
      if (import_examples_package) {
        // Set flag to get around importing scripts as per of this package which results in an assembly reload which in turn prevents / clears any callbacks.
        this.kIsImportingExamples = true;

        var package_full_path = GetPackageFullPath();
        AssetDatabase.ImportPackage(package_full_path
                                    + "/Package Resources/TMP Examples & Extras.unitypackage",
                                    false);
      }
    }

    void OnInspectorUpdate() { this.Repaint(); }

    /// <summary>
    ///   Limits the minimum size of the editor window.
    /// </summary>
    void SetEditorWindowSize() {
      EditorWindow editor_window = this;

      var window_size = new Vector2(640, 210);
      editor_window.minSize = window_size;
      editor_window.maxSize = window_size;
    }

    /// <summary>
    /// </summary>
    /// <param name="package_name"></param>
    void ImportCallback(string package_name) {
      if (package_name == "TMP Essential Resources") {
        this.kEssentialResourcesImported = true;
        //TMPro_EventManager.ON_RESOURCES_LOADED();

        #if UNITY_2018_3_OR_NEWER
        SettingsService.NotifySettingsProviderChanged();
        #endif
      } else if (package_name == "TMP Examples & Extras") {
        this.kExamplesAndExtrasResourcesImported = true;
        this.kIsImportingExamples = false;
      }

      Debug.Log("[" + package_name + "] have been imported.");

      AssetDatabase.importPackageCompleted -= this.ImportCallback;
    }

    static string GetPackageFullPath() {
      // Check for potential UPM package
      var package_path = Path.GetFullPath("Packages/com.unity.textmeshpro");
      if (Directory.Exists(package_path)) {
        return package_path;
      }

      package_path = Path.GetFullPath("Assets/..");
      if (Directory.Exists(package_path)) {
        // Search default location for development package
        if (Directory.Exists(package_path + "/Assets/Packages/com.unity.TextMeshPro/Editor Resources")) {
          return package_path + "/Assets/Packages/com.unity.TextMeshPro";
        }

        // Search for default location of normal TextMesh Pro AssetStore package
        if (Directory.Exists(package_path + "/Assets/TextMesh Pro/Editor Resources")) {
          return package_path + "/Assets/TextMesh Pro";
        }

        // Search for potential alternative locations in the user project
        var matching_paths =
            Directory.GetDirectories(package_path, "TextMesh Pro", SearchOption.AllDirectories);
        var path = ValidateLocation(matching_paths, package_path);
        if (path != null) {
          return package_path + path;
        }
      }

      return null;
    }

    static string ValidateLocation(string[] paths, string project_path) {
      for (var i = 0; i < paths.Length; i++) {
        // Check if the Editor Resources folder exists.
        if (Directory.Exists(paths[i] + "Editor/Resources")) {
          var folder_path = paths[i].Replace(project_path, "");
          folder_path = folder_path.TrimStart('\\', '/');
          return folder_path;
        }
      }

      return null;
    }
  }
}

#endif
