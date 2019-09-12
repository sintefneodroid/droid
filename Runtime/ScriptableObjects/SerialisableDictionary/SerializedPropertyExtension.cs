using System;
using UnityEditor;

#if UNITY_EDITOR

namespace droid.Runtime.ScriptableObjects.SerialisableDictionary {
  /// <summary>
  /// </summary>
  public static class SerializedPropertyExtension {
    public static int GetObjectCode(this SerializedProperty p) {
      // Unique code per serialized object and property path
      return p.propertyPath.GetHashCode() ^ p.serializedObject.GetHashCode();
    }

    public static bool EqualBasics(SerializedProperty left, SerializedProperty right) {
      if (left.propertyType != right.propertyType) {
        return false;
      }

      if (left.propertyType == SerializedPropertyType.Integer) {
        if (left.type == right.type) {
          if (left.type == "int") {
            return left.intValue == right.intValue;
          }

          return left.longValue == right.longValue;
        }

        return false;
      }

      if (left.propertyType == SerializedPropertyType.String) {
        return left.stringValue == right.stringValue;
      }

      if (left.propertyType == SerializedPropertyType.ObjectReference) {
        return left.objectReferenceValue == right.objectReferenceValue;
      }

      if (left.propertyType == SerializedPropertyType.Enum) {
        return left.enumValueIndex == right.enumValueIndex;
      }

      if (left.propertyType == SerializedPropertyType.Boolean) {
        return left.boolValue == right.boolValue;
      }

      if (left.propertyType == SerializedPropertyType.Float) {
        if (left.type == right.type) {
          if (left.type == "float") {
            return Math.Abs(left.floatValue - right.floatValue) < double.Epsilon;
          }

          return Math.Abs(left.doubleValue - right.doubleValue) < double.Epsilon;
        }

        return false;
      }

      if (left.propertyType == SerializedPropertyType.Color) {
        return left.colorValue == right.colorValue;
      }

      if (left.propertyType == SerializedPropertyType.LayerMask) {
        return left.intValue == right.intValue;
      }

      if (left.propertyType == SerializedPropertyType.Vector2) {
        return left.vector2Value == right.vector2Value;
      }

      if (left.propertyType == SerializedPropertyType.Vector3) {
        return left.vector3Value == right.vector3Value;
      }

      if (left.propertyType == SerializedPropertyType.Vector4) {
        return left.vector4Value == right.vector4Value;
      }

      if (left.propertyType == SerializedPropertyType.Rect) {
        return left.rectValue == right.rectValue;
      }

      if (left.propertyType == SerializedPropertyType.ArraySize) {
        return left.arraySize == right.arraySize;
      }

      if (left.propertyType == SerializedPropertyType.Character) {
        return left.intValue == right.intValue;
      }

      if (left.propertyType == SerializedPropertyType.AnimationCurve) {
        return false;
      }

      if (left.propertyType == SerializedPropertyType.Bounds) {
        return left.boundsValue == right.boundsValue;
      }

      if (left.propertyType == SerializedPropertyType.Gradient) {
        return false;
      }

      if (left.propertyType == SerializedPropertyType.Quaternion) {
        return left.quaternionValue == right.quaternionValue;
      }

      return false;
    }

    public static void CopyBasics(SerializedProperty source, SerializedProperty target) {
      if (source.propertyType != target.propertyType) {
        return;
      }

      if (source.propertyType == SerializedPropertyType.Integer) {
        if (source.type == target.type) {
          if (source.type == "int") {
            target.intValue = source.intValue;
          } else {
            target.longValue = source.longValue;
          }
        }
      } else if (source.propertyType == SerializedPropertyType.String) {
        target.stringValue = source.stringValue;
      } else if (source.propertyType == SerializedPropertyType.ObjectReference) {
        target.objectReferenceValue = source.objectReferenceValue;
      } else if (source.propertyType == SerializedPropertyType.Enum) {
        target.enumValueIndex = source.enumValueIndex;
      } else if (source.propertyType == SerializedPropertyType.Boolean) {
        target.boolValue = source.boolValue;
      } else if (source.propertyType == SerializedPropertyType.Float) {
        if (source.type == target.type) {
          if (source.type == "float") {
            target.floatValue = source.floatValue;
          } else {
            target.doubleValue = source.doubleValue;
          }
        }
      } else if (source.propertyType == SerializedPropertyType.Color) {
        target.colorValue = source.colorValue;
      } else if (source.propertyType == SerializedPropertyType.LayerMask) {
        target.intValue = source.intValue;
      } else if (source.propertyType == SerializedPropertyType.Vector2) {
        target.vector2Value = source.vector2Value;
      } else if (source.propertyType == SerializedPropertyType.Vector3) {
        target.vector3Value = source.vector3Value;
      } else if (source.propertyType == SerializedPropertyType.Vector4) {
        target.vector4Value = source.vector4Value;
      } else if (source.propertyType == SerializedPropertyType.Rect) {
        target.rectValue = source.rectValue;
      } else if (source.propertyType == SerializedPropertyType.ArraySize) {
        target.arraySize = source.arraySize;
      } else if (source.propertyType == SerializedPropertyType.Character) {
        target.intValue = source.intValue;
      } else if (source.propertyType == SerializedPropertyType.AnimationCurve) {
        target.animationCurveValue = source.animationCurveValue;
      } else if (source.propertyType == SerializedPropertyType.Bounds) {
        target.boundsValue = source.boundsValue;
      } else if (source.propertyType == SerializedPropertyType.Gradient) {
        // TODO?
      } else if (source.propertyType == SerializedPropertyType.Quaternion) {
        target.quaternionValue = source.quaternionValue;
      } else {
        if (source.hasChildren && target.hasChildren) {
          var source_iterator = source.Copy();
          var target_iterator = target.Copy();
          while (true) {
            if (source_iterator.propertyType == SerializedPropertyType.Generic) {
              if (!source_iterator.Next(true) || !target_iterator.Next(true)) {
                break;
              }
            } else if (!source_iterator.Next(false) || !target_iterator.Next(false)) {
              break;
            }

            CopyBasics(source_iterator, target_iterator);
          }
        }
      }
    }
  }
}
#endif
