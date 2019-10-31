using UnityEngine;
#if UNITY_EDITOR
using droid.Runtime.Utilities;
using UnityEditor;

namespace droid.Editor.Utilities.NoteUtilities {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [CustomEditor(typeof(Note))]
  public class NoteEditor : UnityEditor.Editor {
    NoteType _note_type = NoteType.Box_info_;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void OnInspectorGUI() {
      var note = (Note)this.target;

      if (note._Editing) {
        //DrawDefaultInspector();// Unity function
        note._Text = EditorGUILayout.TextArea(note._Text);

        EditorGUILayout.Separator();

        this._note_type = (NoteType)EditorGUILayout.EnumPopup(this._note_type);

        if (GUILayout.Button("Done")) {
          note.EditToggle();
        }
      } else {
        switch (this._note_type) {
          case NoteType.Text_area_:
            EditorGUILayout.TextArea(note._Text);
            break;
          case NoteType.Text_field_:
            EditorGUILayout.TextField(note._Text);
            break;
          case NoteType.Label_:
            EditorGUILayout.LabelField(note._Text);
            break;
          case NoteType.Box_text_:
            EditorGUILayout.HelpBox(note._Text, MessageType.None);
            break;
          case NoteType.Box_info_:
            EditorGUILayout.HelpBox(note._Text, MessageType.Info);
            break;
          case NoteType.Box_warning_:
            EditorGUILayout.HelpBox(note._Text, MessageType.Warning);
            break;
          case NoteType.Box_error_:
            EditorGUILayout.HelpBox(note._Text, MessageType.Error);
            break;
          case NoteType.Delayed_text_field_:
            break;
          default:
            EditorGUILayout.HelpBox(note._Text, MessageType.Info);
            break;
        }

        EditorGUILayout.Separator();

        if (GUILayout.Button("Edit")) {
          note.EditToggle();
        }
      }
    }
  }
}
#endif
