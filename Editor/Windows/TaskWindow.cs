using droid.Runtime.Prototyping.ObjectiveFunctions.Tasks;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace droid.Editor.Windows {
  public class TaskWindow : EditorWindow {
    Texture _icon;
    Vector2 _scroll_position;

    TaskSequence _task_sequence;

    [MenuItem(EditorWindowMenuPath._WindowMenuPath + "TaskWindow")]
    [MenuItem(EditorWindowMenuPath._ToolMenuPath + "TaskWindow")]
    public static void ShowWindow() {
      GetWindow(typeof(TaskWindow)); //Show existing window instance. If one doesn't exist, make one.
    }

    void OnEnable() {
      this._icon =
          (Texture2D)AssetDatabase.LoadAssetAtPath(NeodroidSettings.Current.NeodroidImportLocationProp
                                                   + "Gizmos/Icons/script.png",
                                                   typeof(Texture2D));
      this.titleContent = new GUIContent("Neo:Task", this._icon, "Window for task descriptions");
      if (!this._task_sequence) {
        this._task_sequence = FindObjectOfType<TaskSequence>();
      }
    }

    /// <summary>
    ///
    /// </summary>
    public void OnInspectorUpdate() { this.Repaint(); }

    void OnGUI() {
      GUILayout.Label("Task list", EditorStyles.boldLabel);
      this._task_sequence = FindObjectOfType<TaskSequence>();
      if (this._task_sequence != null) {
        this._scroll_position = EditorGUILayout.BeginScrollView(this._scroll_position);
        EditorGUILayout.BeginVertical("Box");

        var seq = this._task_sequence.GetSequence();
        if (seq != null) {
          foreach (var g in seq) {
            if (g != null) {
              if (this._task_sequence.CurrentGoalCell != null
                  && this._task_sequence.CurrentGoalCell.name == g.name) {
                GUILayout.Label(g.name, EditorStyles.whiteLabel);
              } else {
                GUILayout.Label(g.name);
              }
            }
          }
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
      }
    }
  }
}
#endif
