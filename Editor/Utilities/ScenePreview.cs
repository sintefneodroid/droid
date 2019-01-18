#if UNITY_EDITOR
using System.Linq;
using Neodroid.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

namespace Neodroid.Editor.Utilities {
  /// <inheritdoc />
  /// <summary>
  ///   Scene preview.
  ///   https://diegogiacomelli.com.br/unity3d-scenepreview-inspector/
  /// </summary>
  [CustomEditor(typeof(SceneAsset))]
  [CanEditMultipleObjects]
  public class ScenePreview : UnityEditor.Editor {
    const float _editor_margin = 50;
    const float _preview_margin = 5;

    /// <summary>
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    public void CaptureScreenShot() {
      if (NeodroidEditorInfo.GenerateScenePreviews){
        var preview_path = GetPreviewPath(SceneManager.GetActiveScene().name);
        //Debug.LogFormat("Saving scene preview at {0}", preview_path);
        ScreenCapture.CaptureScreenshot(preview_path);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void OnInspectorGUI()
    {
      if (NeodroidEditorInfo.GenerateScenePreviews){
        //AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        var scene_names = this.targets.Select(t => ((SceneAsset) t).name).OrderBy(n => n).ToArray();

        var previews_count = scene_names.Length;
        var preview_width = Screen.width;
        var preview_height = (Screen.height - _editor_margin * 2 - _preview_margin * previews_count)
                             / previews_count;

        for (var i = 0; i < scene_names.Length; i++){
          DrawPreview(i, scene_names[i], preview_width, preview_height);
        }
      }
    }

    void DrawPreview(int index, string scene_name, float width, float height) {
      var preview_path = GetPreviewPath(scene_name);
      //var ob = Resources.Load(scene_name);
      //var preview = ob as RenderTexture;
      var preview = LoadPng(preview_path);

      if (preview == null) {
        EditorGUILayout.HelpBox(
            $"There is no image preview for scene {scene_name} at {preview_path}. Please play the scene on editor and image preview will be captured automatically or create the missing path: {NeodroidEditorInfo.ScenePreviewsLocation}.",
            MessageType.Info);
      } else {
        GUI.DrawTexture(
            new Rect(index, _editor_margin + index * (height + _preview_margin), width, height),
            preview,
            ScaleMode.ScaleToFit);
      }
    }

    string GetPreviewPath(string scene_name) {
      return $"{Application.dataPath}/{NeodroidEditorInfo.ScenePreviewsLocation}{scene_name}.png";
    }

    /// <summary>
    /// </summary>
    /// <param name="file_path"></param>
    /// <returns></returns>
    public static Texture2D LoadPng(string file_path) {
      Texture2D tex = null;

      if (File.Exists(file_path)) {
        var file_data = File.ReadAllBytes(file_path);
        tex = new Texture2D(2, 2);
        tex.LoadImage(file_data); //..this will auto-resize the texture dimensions.
      }

      return tex;
    }
  }
}
#endif