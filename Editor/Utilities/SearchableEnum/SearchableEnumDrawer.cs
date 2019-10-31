using System;
using droid.Runtime.Utilities;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace droid.Editor.Utilities.SearchableEnum {
  /// <inheritdoc />
  /// <summary>
  ///   Draws the custom enum selector popup for enum fileds using the
  ///   SearchableEnumAttribute.
  /// </summary>
  [CustomPropertyDrawer(typeof(SearchableEnumAttribute))]
  public class SearchableEnumDrawer : PropertyDrawer {
    const string _type_error = "SearchableEnum can only be used on enum fields.";

    /// <summary>
    ///   Cache of the hash to use to resolve the ID for the drawer.
    /// </summary>
    int _id_hash;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="property"></param>
    /// <param name="label"></param>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      // If this is not used on an eunum, show an error
      if (property.type != "Enum") {
        GUIStyle error_style = "CN EntryErrorIconSmall";
        var r = new Rect(position) {width = error_style.fixedWidth};
        position.xMin = r.xMax;
        GUI.Label(r, "", error_style);
        GUI.Label(position, _type_error);
        return;
      }

      // By manually creating the control ID, we can keep the ID for the
      // label and button the same. This lets them be selected together
      // with the keyboard in the inspector, much like a normal popup.
      if (this._id_hash == 0) {
        this._id_hash = "SearchableEnumDrawer".GetHashCode();
      }

      var id = GUIUtility.GetControlID(this._id_hash, FocusType.Keyboard, position);

      label = EditorGUI.BeginProperty(position, label, property);
      position = EditorGUI.PrefixLabel(position, id, label);

      var button_text = new GUIContent(property.enumDisplayNames[property.enumValueIndex]);
      if (DropdownButton(id, position, button_text)) {
        Action<int> on_select = i => {
                                  property.enumValueIndex = i;
                                  property.serializedObject.ApplyModifiedProperties();
                                };

        SearchablePopup.Show(position,
                             property.enumDisplayNames,
                             property.enumValueIndex,
                             on_select);
      }

      EditorGUI.EndProperty();
    }

    /// <summary>
    ///   A custom button drawer that allows for a controlID so that we can
    ///   sync the button ID and the label ID to allow for keyboard
    ///   navigation like the built-in enum drawers.
    /// </summary>
    static bool DropdownButton(int id, Rect position, GUIContent content) {
      var current = Event.current;
      switch (current.type) {
        case EventType.MouseDown:
          if (position.Contains(current.mousePosition) && current.button == 0) {
            Event.current.Use();
            return true;
          }

          break;
        case EventType.KeyDown:
          if (GUIUtility.keyboardControl == id && current.character == '\n') {
            Event.current.Use();
            return true;
          }

          break;
        case EventType.Repaint:
          EditorStyles.popup.Draw(position,
                                  content,
                                  id,
                                  false);
          break;
      }

      return false;
    }
  }
}
#endif
