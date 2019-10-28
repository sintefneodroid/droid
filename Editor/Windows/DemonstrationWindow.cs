#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace droid.Editor.Windows {
  /// <summary>
  ///
  /// </summary>
  public class DemonstrationWindow : EditorWindow {
    int _captured_frame;

    string _file_name = "Demonstration/frame";
    Texture _icon;
    float _last_frame_time;
    string _record_button = "Record";
    bool _recording;

    string _status = "Idle";

    /// <summary>
    ///
    /// </summary>
    [MenuItem(EditorWindowMenuPath._WindowMenuPath + "DemonstrationWindow")]
    [MenuItem(EditorWindowMenuPath._ToolMenuPath + "DemonstrationWindow")]
    public static void ShowWindow() {
      GetWindow(typeof(DemonstrationWindow)); //Show existing window instance. If one doesn't exist, make one.
    }

    void OnEnable() {
      this._icon =
          (Texture2D)AssetDatabase.LoadAssetAtPath(NeodroidSettings.Current.NeodroidImportLocationProp
                                                   + "Gizmos/Icons/bullet_red.png",
                                                   typeof(Texture2D));
      this.titleContent = new GUIContent("Neo:Rec", this._icon, "Window for recording demonstrations");
    }

    /// <summary>
    ///
    /// </summary>
    public void OnInspectorUpdate() { this.Repaint(); }

    void OnGUI() {
      this._file_name = EditorGUILayout.TextField("File Name:", this._file_name);

      if (GUILayout.Button(this._record_button)) {
        if (this._recording) {
          //recording
          this._status = "Idle...";
          this._record_button = "Record";
          this._recording = false;
        } else {
          // idle
          this._captured_frame = 0;
          this._record_button = "Stop";
          this._recording = true;
        }
      }

      EditorGUILayout.LabelField("Status: ", this._status);
    }

    void Update() {
      if (this._recording) {
        if (EditorApplication.isPlaying && !EditorApplication.isPaused) {
          this.RecordImages();
          this.Repaint();
        } else {
          this._status = "Waiting for Editor to Play";
        }
      }
    }

    void RecordImages() {
      if (this._last_frame_time < Time.time + 1 / 24f) {
        // 24fps
        this._status = "Captured frame" + this._captured_frame;
        ScreenCapture.CaptureScreenshot(this._file_name + this._captured_frame + ".png");
        this._captured_frame++;
        this._last_frame_time = Time.time;
      }
    }
  }
}
#endif
