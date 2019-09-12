using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace droid.Runtime.ScriptableObjects.SerialisableDictionary {
  public abstract class SerializableKeyValueTemplate<TK, TV> : ScriptableObject {
    public TK _Key;
    public TV _Value;
  }

  public abstract class SerializableDictionaryDrawer<TK, TV> : PropertyDrawer {
    Dictionary<int, Dictionary<int, SerializedProperty>> _indexed_property_dicts =
        new Dictionary<int, Dictionary<int, SerializedProperty>>();

    Dictionary<int, SerializedProperty> _keys_props = new Dictionary<int, SerializedProperty>();

    Dictionary<int, SerializedProperty> _template_key_prop = new Dictionary<int, SerializedProperty>();

    Dictionary<int, SerializedProperty> _template_value_prop = new Dictionary<int, SerializedProperty>();

    Dictionary<int, SerializedProperty> _values_props = new Dictionary<int, SerializedProperty>();

    protected abstract SerializableKeyValueTemplate<TK, TV> GetTemplate();

    protected T GetGenericTemplate<T>() where T : SerializableKeyValueTemplate<TK, TV> {
      return ScriptableObject.CreateInstance<T>();
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      EditorGUI.BeginProperty(position, label, property);

      var first_line = position;
      first_line.height = EditorGUIUtility.singleLineHeight;
      EditorGUI.PropertyField(first_line, property);

      if (property.isExpanded) {
        var second_line = first_line;
        second_line.y += EditorGUIUtility.singleLineHeight;

        EditorGUIUtility.labelWidth = 50f;

        second_line.x += 15f; // indentation
        second_line.width -= 15f;

        var second_line_key = second_line;

        var button_width = 60f;
        second_line_key.width -= button_width; // assign button
        second_line_key.width /= 2f;

        var second_line_value = second_line_key;
        second_line_value.x += second_line_value.width;
        if (this.GetTemplateValueProp(property).hasVisibleChildren) {
          // if the value has children, indent to make room for fold arrow
          second_line_value.x += 15;
          second_line_value.width -= 15;
        }

        var second_line_button = second_line_value;
        second_line_button.x += second_line_value.width;
        second_line_button.width = button_width;

        var k_height = EditorGUI.GetPropertyHeight(this.GetTemplateKeyProp(property));
        var v_height = EditorGUI.GetPropertyHeight(this.GetTemplateValueProp(property));
        var extra_height = Mathf.Max(k_height, v_height);

        second_line_key.height = k_height;
        second_line_value.height = v_height;

        EditorGUI.PropertyField(second_line_key, this.GetTemplateKeyProp(property), true);
        EditorGUI.PropertyField(second_line_value, this.GetTemplateValueProp(property), true);

        var keys_prop = this.GetKeysProp(property);
        var values_prop = this.GetValuesProp(property);

        var num_lines = keys_prop.arraySize;

        if (GUI.Button(second_line_button, "Assign")) {
          var assignment = false;
          for (var i = 0; i < num_lines; i++)
              // Try to replace existing value
          {
            if (SerializedPropertyExtension.EqualBasics(this.GetIndexedItemProp(keys_prop, i),
                                                        this.GetTemplateKeyProp(property))) {
              SerializedPropertyExtension.CopyBasics(this.GetTemplateValueProp(property),
                                                     this.GetIndexedItemProp(values_prop, i));
              assignment = true;
              break;
            }
          }

          if (!assignment) {
            // Create a new value
            keys_prop.arraySize += 1;
            values_prop.arraySize += 1;
            SerializedPropertyExtension.CopyBasics(this.GetTemplateKeyProp(property),
                                                   this.GetIndexedItemProp(keys_prop, num_lines));
            SerializedPropertyExtension.CopyBasics(this.GetTemplateValueProp(property),
                                                   this.GetIndexedItemProp(values_prop, num_lines));
          }
        }

        for (var i = 0; i < num_lines; i++) {
          second_line_key.y += extra_height;
          second_line_value.y += extra_height;
          second_line_button.y += extra_height;

          k_height = EditorGUI.GetPropertyHeight(this.GetIndexedItemProp(keys_prop, i));
          v_height = EditorGUI.GetPropertyHeight(this.GetIndexedItemProp(values_prop, i));
          extra_height = Mathf.Max(k_height, v_height);

          second_line_key.height = k_height;
          second_line_value.height = v_height;

          EditorGUI.PropertyField(second_line_key, this.GetIndexedItemProp(keys_prop, i), true);
          EditorGUI.PropertyField(second_line_value, this.GetIndexedItemProp(values_prop, i), true);

          if (GUI.Button(second_line_button, "Remove")) {
            keys_prop.DeleteArrayElementAtIndex(i);
            values_prop.DeleteArrayElementAtIndex(i);
          }
        }
      }

      EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
      if (!property.isExpanded) {
        return EditorGUIUtility.singleLineHeight;
      }

      var total = EditorGUIUtility.singleLineHeight;

      var k_height = EditorGUI.GetPropertyHeight(this.GetTemplateKeyProp(property));
      var v_height = EditorGUI.GetPropertyHeight(this.GetTemplateValueProp(property));
      total += Mathf.Max(k_height, v_height);

      var keys_prop = this.GetKeysProp(property);
      var values_prop = this.GetValuesProp(property);
      var num_lines = keys_prop.arraySize;
      for (var i = 0; i < num_lines; i++) {
        k_height = EditorGUI.GetPropertyHeight(this.GetIndexedItemProp(keys_prop, i));
        v_height = EditorGUI.GetPropertyHeight(this.GetIndexedItemProp(values_prop, i));
        total += Mathf.Max(k_height, v_height);
      }

      return total;
    }

    SerializedProperty GetTemplateKeyProp(SerializedProperty main_prop) {
      return this.GetTemplateProp(this._template_key_prop, main_prop);
    }

    SerializedProperty GetTemplateValueProp(SerializedProperty main_prop) {
      return this.GetTemplateProp(this._template_value_prop, main_prop);
    }

    SerializedProperty GetTemplateProp(Dictionary<int, SerializedProperty> source,
                                       SerializedProperty main_prop) {
      if (!source.TryGetValue(main_prop.GetObjectCode(), out var p)) {
        var template_object = this.GetTemplate();
        var template_serialized_object = new SerializedObject(template_object);
        var k_prop = template_serialized_object.FindProperty("key");
        var v_prop = template_serialized_object.FindProperty("value");
        this._template_key_prop[main_prop.GetObjectCode()] = k_prop;
        this._template_value_prop[main_prop.GetObjectCode()] = v_prop;
        p = source == this._template_key_prop ? k_prop : v_prop;
      }

      return p;
    }

    SerializedProperty GetKeysProp(SerializedProperty main_prop) {
      return this.GetCachedProp(main_prop, "keys", this._keys_props);
    }

    SerializedProperty GetValuesProp(SerializedProperty main_prop) {
      return this.GetCachedProp(main_prop, "values", this._values_props);
    }

    SerializedProperty GetCachedProp(SerializedProperty main_prop,
                                     string relative_property_name,
                                     Dictionary<int, SerializedProperty> source) {
      var object_code = main_prop.GetObjectCode();
      if (!source.TryGetValue(object_code, out var p)) {
        source[object_code] = p = main_prop.FindPropertyRelative(relative_property_name);
      }

      return p;
    }

    SerializedProperty GetIndexedItemProp(SerializedProperty array_prop, int index) {
      if (!this._indexed_property_dicts.TryGetValue(array_prop.GetObjectCode(), out var d)) {
        this._indexed_property_dicts[array_prop.GetObjectCode()] =
            d = new Dictionary<int, SerializedProperty>();
      }

      if (!d.TryGetValue(index, out var result)) {
        d[index] = result = array_prop.FindPropertyRelative($"Array.data[{index}]");
      }

      return result;
    }
  }
}
#endif
