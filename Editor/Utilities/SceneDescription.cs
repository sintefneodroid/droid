
using System.IO;
using droid.Runtime.Managers;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using System.Linq;

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
      if (NeodroidEditorInfo.GenerateSceneDescriptions) {
        var preview_path = GetDescriptionPath(SceneManager.GetActiveScene().name);
        #if NEODROID_DEBUG
        Debug.Log($"Saving scene preview at {preview_path}");
        #endif
        MakeDescription(preview_path);
      }
    }

    public static void MakeDescription(string name) {
      var serializer = new JsonSerializer {NullValueHandling = NullValueHandling.Ignore};
      var simulationManager = FindObjectOfType<PausableManager>();

      using (var sw = new StreamWriter(name)) {
        using (JsonWriter writer = new JsonTextWriter(sw)) {
          serializer.Serialize(writer, simulationManager.ToString());
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void OnInspectorGUI() {
      if (NeodroidEditorInfo.GenerateScenePreviews) {
        var scene_names = this.targets.Select(t => ((SceneAsset)t).name).OrderBy(n => n).ToArray();

        for (var i = 0; i < scene_names.Length; i++) {
          this.DrawDescriptionPreview(i, scene_names[i]);
        }
      }
    }

    void DrawDescriptionPreview(int index, string scene_name) {
      var preview_path = GetDescriptionPath(scene_name);
      var preview = LoadDescription(preview_path);

      if (preview == null) {
        EditorGUILayout.HelpBox(preview, MessageType.Info);
      } else {
        EditorGUILayout
            .HelpBox($"There is no image preview for scene {scene_name} at {preview_path}. Please play the scene on editor and image preview will be captured automatically or create the missing path: {NeodroidEditorInfo.ScenePreviewsLocation}.",
                     MessageType.Info);
      }
    }

    static string GetDescriptionPath(string scene_name) {
      //return $"{NeodroidEditorInfo.ScenePreviewsLocation}{scene_name}.png";
      return $"{Application.dataPath}/{NeodroidEditorInfo.SceneDescriptionLocation}{scene_name}.md";
    }

    /// <summary>
    /// </summary>
    /// <param name="file_path"></param>
    /// <returns></returns>
    public static string LoadDescription(string file_path) {
      var description = "";

      using (var sr = new StreamReader(file_path)) {
        description = sr.ReadToEnd();
      }

      return description;
    }
  }
}
#endif
