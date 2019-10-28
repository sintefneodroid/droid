using UnityEngine;
#if UNITY_EDITOR
using droid.Runtime.GameObjects.NeodroidCamera;
using UnityEditor;

namespace droid.Editor.Windows {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class CameraSynchronisationWindow : EditorWindow {
    SynchroniseCameraProperties[] _cameras;

    Texture _icon;
    Vector2 _scroll_position;
    bool[] _show_camera_properties;

    /// <summary>
    /// </summary>
    [MenuItem(EditorWindowMenuPath._WindowMenuPath + "CameraSynchronisationWindow")]
    [MenuItem(EditorWindowMenuPath._ToolMenuPath + "CameraSynchronisationWindow")]
    public static void ShowWindow() {
      GetWindow(typeof(CameraSynchronisationWindow)); //Show existing window instance. If one doesn't exist, make one.
    }

    /// <summary>
    /// </summary>
    void OnEnable() {
      this._cameras = FindObjectsOfType<SynchroniseCameraProperties>();
      this.Setup();
      this._icon =
          (Texture2D)AssetDatabase.LoadAssetAtPath(NeodroidSettings.Current.NeodroidImportLocationProp
                                                   + "Gizmos/Icons/arrow_refresh.png",
                                                   typeof(Texture2D));
      this.titleContent =
          new GUIContent("Neo:Sync", this._icon, "Window for controlling synchronisation of cameras");
    }

    /// <summary>
    /// </summary>
    void Setup() {
      this._show_camera_properties = new bool[this._cameras.Length];
      for (var i = 0; i < this._cameras.Length; i++) {
        this._show_camera_properties[i] = false;
      }
    }

    /// <summary>
    /// </summary>
    void OnGUI() {
      this._cameras = FindObjectsOfType<SynchroniseCameraProperties>();
      if (this._cameras.Length > 0) {
        var serialised_object = new SerializedObject(this);
        this._scroll_position = EditorGUILayout.BeginScrollView(this._scroll_position);
        if (this._show_camera_properties != null) {
          for (var i = 0; i < this._show_camera_properties.Length; i++) {
            this._show_camera_properties[i] =
                EditorGUILayout.Foldout(this._show_camera_properties[i], this._cameras[i].name);
            if (this._show_camera_properties[i]) {
              EditorGUILayout.BeginVertical("Box");
              /*
              this._cameras[i].SyncOrthographicSize =
                  EditorGUILayout.Toggle("Synchronise Orthographic Size",
                                         this._cameras[i].SyncOrthographicSize);
              this._cameras[i].SyncNearClipPlane =
                  EditorGUILayout.Toggle("Synchronise Near Clip Plane", this._cameras[i].SyncNearClipPlane);
              this._cameras[i].SyncFarClipPlane =
                  EditorGUILayout.Toggle("Synchronise Far Clip Plane", this._cameras[i].SyncFarClipPlane);
              this._cameras[i].SyncCullingMask =
                  EditorGUILayout.Toggle("Synchronise Culling Mask", this._cameras[i].SyncCullingMask);
              */
              EditorGUILayout.EndVertical();
            }
          }
        }

        EditorGUILayout.EndScrollView();
        serialised_object.ApplyModifiedProperties(); // Remember to apply modified properties
      }

      /*if (GUI.changed) {
      EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
      // Unity not tracking changes to properties of gameobject made through this window automatically and
      // are not saved unless other changes are made from a working inpector window
      }*/
    }

    /// <summary>
    /// </summary>
    public void OnInspectorUpdate() { this.Repaint(); }
  }
}

#endif
