#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Utilities.Unsorted {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [System.Serializable]
  public class Note : MonoBehaviour {
    /// <summary>
    ///
    /// </summary>
    [TextArea]
    [Tooltip("A component for holding notes or comments")]
    [SerializeField]
    public string _Text;

    /// <summary>
    ///
    /// </summary>
    [System.NonSerialized]
    public bool _Editing;

    /// <summary>
    ///
    /// </summary>
    public void EditToggle() { this._Editing = !this._Editing; }

    void Start() {
      this.enabled = false;
    }
  }

  /// <summary>
  ///
  /// </summary>
  enum NoteType {
    Text_area_,
    Text_field_,
    Delayed_text_field_,
    Label_,
    Box_text_,
    Box_info_,
    Box_warning_,
    Box_error_
  }

  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEditor.CustomEditor(typeof(Note))]
  public class NoteEditor : UnityEditor.Editor {
    NoteType _note_type = NoteType.Box_info_;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void OnInspectorGUI() {
      var note = (Note)this.target;

      if (note._Editing) {
        //DrawDefaultInspector();// Unity function
        note._Text = UnityEditor.EditorGUILayout.TextArea(note._Text);

        UnityEditor.EditorGUILayout.Separator();

        this._note_type = (NoteType)UnityEditor.EditorGUILayout.EnumPopup(this._note_type);

        if (GUILayout.Button("Done")) {
          note.EditToggle();
        }

      } else {

        switch (this._note_type) {
          case NoteType.Text_area_:
            UnityEditor.EditorGUILayout.TextArea(note._Text);
            break;
          case NoteType.Text_field_:
            UnityEditor.EditorGUILayout.TextField(note._Text);
            break;
          case NoteType.Label_:
            UnityEditor.EditorGUILayout.LabelField(note._Text);
            break;
          case NoteType.Box_text_:
            UnityEditor.EditorGUILayout.HelpBox(
                note._Text,
                UnityEditor.MessageType.None);
            break;
          case NoteType.Box_info_:
            UnityEditor.EditorGUILayout.HelpBox(note._Text, UnityEditor.MessageType.Info);
            break;
          case NoteType.Box_warning_:
            UnityEditor.EditorGUILayout.HelpBox(
                note._Text,
                UnityEditor.MessageType.Warning);
            break;
          case NoteType.Box_error_:
            UnityEditor.EditorGUILayout.HelpBox(
                note._Text,
                UnityEditor.MessageType.Error);
            break;
          case NoteType.Delayed_text_field_:
            break;
          default:
            UnityEditor.EditorGUILayout.HelpBox(note._Text, UnityEditor.MessageType.Info);
            break;
        }

        UnityEditor.EditorGUILayout.Separator();

        if (GUILayout.Button("Edit")) {
          note.EditToggle();
        }
      }
    }
  }
}
#endif

