using System;
using UnityEngine;

namespace droid.Runtime.Utilities {
  /// <inheritdoc />
  /// <summary>
  ///   Put this attribute on a public (or SerialzeField) enum in a
  ///   MonoBehaviour or ScriptableObject to get an improved enum selector
  ///   popup. The enum list is scrollable and can be filtered by typing.
  /// </summary>
  [AttributeUsage(AttributeTargets.Field)]
  public class SearchableEnumAttribute : PropertyAttribute { }
}
