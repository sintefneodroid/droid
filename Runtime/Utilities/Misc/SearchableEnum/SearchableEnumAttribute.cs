using System;
using UnityEngine;

namespace Neodroid.Runtime.Utilities.Misc.SearchableEnum {
  /// <inheritdoc />
  /// <summary>
  /// Put this attribute on a public (or SerialzeField) enum in a
  /// MonoBehaviour or ScriptableObject to get an improved enum selector
  /// popup. The enum list is scrollable and can be filtered by typing.
  /// </summary>
  [AttributeUsage(AttributeTargets.Field)]
  public class SearchableEnumAttribute : PropertyAttribute { }
}
