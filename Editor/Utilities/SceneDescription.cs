using System.IO;
using droid.Runtime.Managers;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using System.Linq;
using UnityEditor;

namespace droid.Editor.Utilities {
  /// <inheritdoc />
  /// <summary>
  ///   Scene preview.
  ///   https://diegogiacomelli.com.br/unity3d-scenepreview-inspector/
  /// </summary>
  [CustomEditor(typeof(SceneAsset))]
  [CanEditMultipleObjects]
  public class SceneDescription : UnityEditor.Editor {
    /// <summary>
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    public static void CaptureDescription() {
      if (NeodroidSettings.Current.NeodroidGenerateDescriptionsProp) {
        var preview_path = GetDescriptionPath(SceneManager.GetActiveScene().name);
        #if NEODROID_DEBUG
        Debug.Log($"Saving scene preview at {preview_path}");
        #endif
        MakeDescription(preview_path);
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="name"></param>
    public static void MakeDescription(string name) {
      var serializer = new JsonSerializer {NullValueHandling = NullValueHandling.Ignore};
      var simulation_manager = FindObjectOfType<AbstractNeodroidManager>();

      var path = Path.GetDirectoryName(name);
      Directory.CreateDirectory(path);

      using (var sw = new StreamWriter(name)) {
        using (JsonWriter writer = new JsonTextWriter(sw)) {
          serializer.Serialize(writer, simulation_manager.ToString());
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void OnInspectorGUI() {
      if (NeodroidSettings.Current.NeodroidGeneratePreviewsProp) {
        //AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        var scene_names = this.targets.Select(t => ((SceneAsset)t).name).OrderBy(n => n).ToArray();

        var previews_count = scene_names.Length;
        var preview_width = Screen.width;
        var preview_height =
            (Screen.height
             - NeodroidEditorConstants._Editor_Margin * 2
             - NeodroidEditorConstants._Preview_Margin * previews_count)
            / previews_count;

        for (var i = 0; i < scene_names.Length; i++) {
          ScenePreview.DrawPreview(i,
                                   scene_names[i],
                                   preview_width,
                                   preview_height);
        }
      }

      if (NeodroidSettings.Current.NeodroidGeneratePreviewsProp) {
        var scene_names = this.targets.Select(t => ((SceneAsset)t).name).OrderBy(n => n).ToArray();

        for (var i = 0; i < scene_names.Length; i++) {
          PrintDescription(i, scene_names[i]);
        }
      }
    }

    static void PrintDescription(int index, string scene_name) {
      var preview_path = GetDescriptionPath(scene_name);
      var preview = LoadDescription(preview_path);

      if (preview != null) {
        EditorGUILayout.HelpBox(preview, MessageType.Info);
      } else {
        EditorGUILayout
            .HelpBox($"There is no image preview for scene {scene_name} at {preview_path}. Please play the scene on editor and image preview will be captured automatically or create the missing path: {NeodroidSettings.Current.NeodroidPreviewsLocationProp}.",
                     MessageType.Info);
      }
    }

    static string GetDescriptionPath(string scene_name) {
      //return $"{NeodroidEditorInfo.ScenePreviewsLocation}{scene_name}.png";
      return
          $"{Application.dataPath}/{NeodroidSettings.Current.NeodroidDescriptionLocationProp}{scene_name}.md";
    }

    /// <summary>
    /// </summary>
    /// <param name="file_path"></param>
    /// <returns></returns>
    public static string LoadDescription(string file_path) {
      var description = "The is no description available, press play to generate a description";

      if (File.Exists(file_path)) {
        using (var sr = new StreamReader(file_path)) {
          description = sr.ReadToEnd();
        }
      }

      return description;
    }
  }
}
#endif
