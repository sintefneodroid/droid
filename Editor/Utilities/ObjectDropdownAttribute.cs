using System;
using UnityEngine;

namespace Common.Editors {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class ObjectDropdownAttribute : PropertyAttribute { }

  public class ObjectDropdownFilterAttribute : PropertyAttribute {
    public Type _FilterType;
    public ObjectDropdownFilterAttribute(Type a_type) { this._FilterType = a_type; }
  }
}