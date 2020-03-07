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
  [CustomPropertyDrawer(type : typeof(SearchableEnumAttribute))]
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
      // If this is not used on an enum, show an error
      if (property.type != "Enum") {
        GUIStyle error_style = "CN EntryErrorIconSmall";
        var r = new Rect(source : position) {width = error_style.fixedWidth};
        position.xMin = r.xMax;
        GUI.Label(position : r, "", style : error_style);
        GUI.Label(position : position, text : _type_error);
        return;
      }

      // By manually creating the control ID, we can keep the ID for the
      // label and button the same. This lets them be selected together
      // with the keyboard in the inspector, much like a normal popup.
      if (this._id_hash == 0) {
        this._id_hash = "SearchableEnumDrawer".GetHashCode();
      }

      var id = GUIUtility.GetControlID(hint : this._id_hash, focusType : FocusType.Keyboard, rect : position);

      label = EditorGUI.BeginProperty(totalPosition : position, label : label, property : property);
      position = EditorGUI.PrefixLabel(totalPosition : position, id : id, label : label);

      var button_text = new GUIContent(text : property.enumDisplayNames[property.enumValueIndex]);
      if (DropdownButton(id : id, position : position, content : button_text)) {
        void OnSelect(Int32 i) {
          property.enumValueIndex = i;
          property.serializedObject.ApplyModifiedProperties();
        }

        SearchablePopup.Show(activator_rect : position,
                             options : property.enumDisplayNames,
                             current : property.enumValueIndex,
                             on_selection_made : OnSelect);
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
          if (position.Contains(point : current.mousePosition) && current.button == 0) {
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
          EditorStyles.popup.Draw(position : position,
                                  content : content,
                                  controlID : id,
                                  false);
          break;
      }

      return false;
    }
  }
}
#endif
