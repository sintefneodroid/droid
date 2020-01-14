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
      EditorGUI.BeginProperty(totalPosition : position, label : label, property : property);

      var first_line = position;
      first_line.height = EditorGUIUtility.singleLineHeight;
      EditorGUI.PropertyField(position : first_line, property : property);

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
        if (this.GetTemplateValueProp(main_prop : property).hasVisibleChildren) {
          // if the value has children, indent to make room for fold arrow
          second_line_value.x += 15;
          second_line_value.width -= 15;
        }

        var second_line_button = second_line_value;
        second_line_button.x += second_line_value.width;
        second_line_button.width = button_width;

        var k_height = EditorGUI.GetPropertyHeight(this.GetTemplateKeyProp(main_prop : property));
        var v_height = EditorGUI.GetPropertyHeight(this.GetTemplateValueProp(main_prop : property));
        var extra_height = Mathf.Max(a : k_height, b : v_height);

        second_line_key.height = k_height;
        second_line_value.height = v_height;

        EditorGUI.PropertyField(position : second_line_key, this.GetTemplateKeyProp(main_prop : property), true);
        EditorGUI.PropertyField(position : second_line_value, this.GetTemplateValueProp(main_prop : property), true);

        var keys_prop = this.GetKeysProp(main_prop : property);
        var values_prop = this.GetValuesProp(main_prop : property);

        var num_lines = keys_prop.arraySize;

        if (GUI.Button(position : second_line_button, "Assign")) {
          var assignment = false;
          for (var i = 0; i < num_lines; i++)
              // Try to replace existing value
          {
            if (SerializedPropertyExtension.EqualBasics(this.GetIndexedItemProp(array_prop : keys_prop, index : i),
                                                        this.GetTemplateKeyProp(main_prop : property))) {
              SerializedPropertyExtension.CopyBasics(this.GetTemplateValueProp(main_prop : property),
                                                     this.GetIndexedItemProp(array_prop : values_prop, index : i));
              assignment = true;
              break;
            }
          }

          if (!assignment) {
            // Create a new value
            keys_prop.arraySize += 1;
            values_prop.arraySize += 1;
            SerializedPropertyExtension.CopyBasics(this.GetTemplateKeyProp(main_prop : property),
                                                   this.GetIndexedItemProp(array_prop : keys_prop, index : num_lines));
            SerializedPropertyExtension.CopyBasics(this.GetTemplateValueProp(main_prop : property),
                                                   this.GetIndexedItemProp(array_prop : values_prop, index : num_lines));
          }
        }

        for (var i = 0; i < num_lines; i++) {
          second_line_key.y += extra_height;
          second_line_value.y += extra_height;
          second_line_button.y += extra_height;

          k_height = EditorGUI.GetPropertyHeight(this.GetIndexedItemProp(array_prop : keys_prop, index : i));
          v_height = EditorGUI.GetPropertyHeight(this.GetIndexedItemProp(array_prop : values_prop, index : i));
          extra_height = Mathf.Max(a : k_height, b : v_height);

          second_line_key.height = k_height;
          second_line_value.height = v_height;

          EditorGUI.PropertyField(position : second_line_key, this.GetIndexedItemProp(array_prop : keys_prop, index : i), true);
          EditorGUI.PropertyField(position : second_line_value, this.GetIndexedItemProp(array_prop : values_prop, index : i), true);

          if (GUI.Button(position : second_line_button, "Remove")) {
            keys_prop.DeleteArrayElementAtIndex(index : i);
            values_prop.DeleteArrayElementAtIndex(index : i);
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

      var k_height = EditorGUI.GetPropertyHeight(this.GetTemplateKeyProp(main_prop : property));
      var v_height = EditorGUI.GetPropertyHeight(this.GetTemplateValueProp(main_prop : property));
      total += Mathf.Max(a : k_height, b : v_height);

      var keys_prop = this.GetKeysProp(main_prop : property);
      var values_prop = this.GetValuesProp(main_prop : property);
      var num_lines = keys_prop.arraySize;
      for (var i = 0; i < num_lines; i++) {
        k_height = EditorGUI.GetPropertyHeight(this.GetIndexedItemProp(array_prop : keys_prop, index : i));
        v_height = EditorGUI.GetPropertyHeight(this.GetIndexedItemProp(array_prop : values_prop, index : i));
        total += Mathf.Max(a : k_height, b : v_height);
      }

      return total;
    }

    SerializedProperty GetTemplateKeyProp(SerializedProperty main_prop) {
      return this.GetTemplateProp(source : this._template_key_prop, main_prop : main_prop);
    }

    SerializedProperty GetTemplateValueProp(SerializedProperty main_prop) {
      return this.GetTemplateProp(source : this._template_value_prop, main_prop : main_prop);
    }

    SerializedProperty GetTemplateProp(Dictionary<int, SerializedProperty> source,
                                       SerializedProperty main_prop) {
      if (!source.TryGetValue(main_prop.GetObjectCode(), out var p)) {
        var template_object = this.GetTemplate();
        var template_serialized_object = new SerializedObject(obj : template_object);
        var k_prop = template_serialized_object.FindProperty("key");
        var v_prop = template_serialized_object.FindProperty("value");
        this._template_key_prop[main_prop.GetObjectCode()] = k_prop;
        this._template_value_prop[main_prop.GetObjectCode()] = v_prop;
        p = source == this._template_key_prop ? k_prop : v_prop;
      }

      return p;
    }

    SerializedProperty GetKeysProp(SerializedProperty main_prop) {
      return this.GetCachedProp(main_prop : main_prop, "keys", source : this._keys_props);
    }

    SerializedProperty GetValuesProp(SerializedProperty main_prop) {
      return this.GetCachedProp(main_prop : main_prop, "values", source : this._values_props);
    }

    SerializedProperty GetCachedProp(SerializedProperty main_prop,
                                     string relative_property_name,
                                     Dictionary<int, SerializedProperty> source) {
      var object_code = main_prop.GetObjectCode();
      if (!source.TryGetValue(key : object_code, out var p)) {
        source[key : object_code] = p = main_prop.FindPropertyRelative(relativePropertyPath : relative_property_name);
      }

      return p;
    }

    SerializedProperty GetIndexedItemProp(SerializedProperty array_prop, int index) {
      if (!this._indexed_property_dicts.TryGetValue(array_prop.GetObjectCode(), out var d)) {
        this._indexed_property_dicts[array_prop.GetObjectCode()] =
            d = new Dictionary<int, SerializedProperty>();
      }

      if (!d.TryGetValue(key : index, out var result)) {
        d[key : index] = result = array_prop.FindPropertyRelative($"Array.data[{index}]");
      }

      return result;
    }
  }
}
#endif
