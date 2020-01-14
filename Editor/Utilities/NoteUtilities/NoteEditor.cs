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
        note._Text = EditorGUILayout.TextArea(text : note._Text);

        EditorGUILayout.Separator();

        this._note_type = (NoteType)EditorGUILayout.EnumPopup(selected : this._note_type);

        if (GUILayout.Button("Done")) {
          note.EditToggle();
        }
      } else {
        switch (this._note_type) {
          case NoteType.Text_area_:
            EditorGUILayout.TextArea(text : note._Text);
            break;
          case NoteType.Text_field_:
            EditorGUILayout.TextField(text : note._Text);
            break;
          case NoteType.Label_:
            EditorGUILayout.LabelField(label : note._Text);
            break;
          case NoteType.Box_text_:
            EditorGUILayout.HelpBox(message : note._Text, type : MessageType.None);
            break;
          case NoteType.Box_info_:
            EditorGUILayout.HelpBox(message : note._Text, type : MessageType.Info);
            break;
          case NoteType.Box_warning_:
            EditorGUILayout.HelpBox(message : note._Text, type : MessageType.Warning);
            break;
          case NoteType.Box_error_:
            EditorGUILayout.HelpBox(message : note._Text, type : MessageType.Error);
            break;
          case NoteType.Delayed_text_field_:
            break;
          default:
            EditorGUILayout.HelpBox(message : note._Text, type : MessageType.Info);
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
