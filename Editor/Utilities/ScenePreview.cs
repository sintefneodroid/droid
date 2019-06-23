using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
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
  public class ScenePreview : UnityEditor.Editor {
    const float _editor_margin = 50;
    const float _preview_margin = 5;

    /// <summary>
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    public static void CaptureScreenShot() {
      if (NeodroidSettings.Current.NeodroidGeneratePreviewsProp) {
        var preview_path = GetPreviewPath(SceneManager.GetActiveScene().name);
        Debug.Log($"Saving scene preview at {preview_path}");
        TakeScreenshot(preview_path);
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="name"></param>
    public static void TakeScreenshot(string name) {
      // Take the screenshot
      ScreenCapture.CaptureScreenshot(name); // TODO: VERY broken, unitys fault

/*
      //Wait for 4 frames
      for (int i = 0; i < 5; i++)
      {
        yield return null;
      }

      // Read the data from the file
      byte[] data = File.ReadAllBytes(Application.persistentDataPath + "/" + name);

      // Create the texture
      Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height);

      // Load the image
      screenshotTexture.LoadImage(data);

      // Create a sprite
      Sprite screenshotSprite = Sprite.Create(screenshotTexture, new Rect(0, 0, Screen.width, Screen.height), new Vector2(0.5f, 0.5f));

      // Set the sprite to the screenshotPreview
      screenshotPreview.GetComponent<Image>().sprite = screenshotSprite;

      OR

          Texture2D screenImage = new Texture2D(Screen.width, Screen.height);
    //Get Image from screen
    screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
    screenImage.Apply();
    //Convert to png
    byte[] imageBytes = screenImage.EncodeToPNG();

    //Save image to file
    System.IO.File.WriteAllBytes(path, imageBytes);

*/
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
        var preview_height = (Screen.height - _editor_margin * 2 - _preview_margin * previews_count)
                             / previews_count;

        for (var i = 0; i < scene_names.Length; i++) {
          this.DrawPreview(i, scene_names[i], preview_width, preview_height);
        }
      }
    }

    void DrawPreview(int index, string scene_name, float width, float height) {
      var preview_path = GetPreviewPath(scene_name);
      //var ob = Resources.Load(scene_name);
      //var preview = ob as RenderTexture;
      var preview = LoadPng(preview_path);

      if (preview == null) {
        EditorGUILayout
            .HelpBox($"There is no image preview for scene {scene_name} at {preview_path}. Please play the scene on editor and image preview will be captured automatically or create the missing path: {NeodroidSettings.Current.NeodroidPreviewsLocationProp}.",
                     MessageType.Info);
      } else {
        GUI.DrawTexture(new Rect(index, _editor_margin + index * (height + _preview_margin), width, height),
                        preview,
                        ScaleMode.ScaleToFit);
      }
    }

    static string GetPreviewPath(string scene_name) {
      //return $"{NeodroidEditorInfo.ScenePreviewsLocation}{scene_name}.png";
      return $"{Application.dataPath}/{NeodroidSettings.Current.NeodroidPreviewsLocationProp}{scene_name}.png";
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
