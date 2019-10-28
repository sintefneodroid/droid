using System.Collections.Generic;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace droid.Editor.Utilities.ObjectDropdown {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [CustomPropertyDrawer(typeof(ObjectDropdownAttribute))]
  public class ObjectSelectorDropdown : PropertyDrawer {
    List<Object> _m_list = new List<Object>();

    /// <summary>
    ///
    /// </summary>
    /// <param name="position"></param>
    /// <param name="property"></param>
    /// <param name="label"></param>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      var e = Event.current;
      if (property.propertyType == SerializedPropertyType.ObjectReference) {
        if ((e.type == EventType.DragPerform
             || e.type == EventType.DragExited
             || e.type == EventType.DragUpdated
             || e.type == EventType.Repaint)
            && position.Contains(e.mousePosition)
            && e.shift) {
          if (DragAndDrop.objectReferences != null) {
            this._m_list.Clear();
            foreach (var o in DragAndDrop.objectReferences) {
              this._m_list.Add(o);
              var go = o as GameObject;
              if (go == null && o is Component) {
                go = ((Component)o).gameObject;
                this._m_list.Add(go);
              }

              if (go != null) {
                foreach (var c in go.GetComponents<Component>()) {
                  if (c != o) {
                    this._m_list.Add(c);
                  }
                }
              }
            }

            var field_info = property.GetPropertyReferenceType();
            if (field_info != null) {
              var type = field_info.FieldType;
              for (var i = this._m_list.Count - 1; i >= 0; i--) {
                if (this._m_list[i] == null || !type.IsAssignableFrom(this._m_list[i].GetType())) {
                  this._m_list.RemoveAt(i);
                }
              }
            }

            if (this.attribute is ObjectDropdownFilterAttribute att) {
              var type = att._FilterType;
              for (var i = this._m_list.Count - 1; i >= 0; i--) {
                if (!type.IsAssignableFrom(this._m_list[i].GetType())) {
                  this._m_list.RemoveAt(i);
                }
              }
            }

            if (this._m_list.Count == 0) {
              DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
            } else {
              DragAndDrop.visualMode = DragAndDropVisualMode.Link;
              if (e.type == EventType.DragPerform) {
                var gm = new GenericMenu();
                GenericMenu.MenuFunction2 func = o => {
                                                   property.objectReferenceValue = (Object)o;
                                                   property.serializedObject.ApplyModifiedProperties();
                                                 };
                foreach (var item in this._m_list) {
                  gm.AddItem(new GUIContent(item.name + "(" + item.GetType().Name + ")"),
                             false,
                             func,
                             item);
                }

                gm.ShowAsContext();
                e.Use();
              }
            }

            this._m_list.Clear();
          }
        }

        EditorGUI.ObjectField(position, property, label);
      } else {
        EditorGUI.PropertyField(position, property, label);
      }
    }
  }

  /// <summary>
  ///
  /// </summary>
  public static class SerializedPropertyExt {
    /// <summary>
    ///
    /// </summary>
    /// <param name="a_property"></param>
    /// <returns></returns>
    public static FieldInfo GetPropertyReferenceType(this SerializedProperty a_property) {
      var current_type = a_property.serializedObject.targetObject.GetType();
      FieldInfo fi = null;
      var parts = a_property.propertyPath.Split('.');
      foreach (var field_name in parts) {
        fi = current_type.GetField(field_name,
                                   BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (fi == null) {
          return null;
        }

        current_type = fi.FieldType;
      }

      return fi;
    }
  }
}
#endif
