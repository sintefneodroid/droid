using System;

namespace droid.Editor.Utilities {
  /// <summary>
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  public class ScriptExecutionOrderAttribute : Attribute {
    int _order;

    public ScriptExecutionOrderAttribute(int order) { this._order = order; }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public int GetOrder() { return this._order; }
  }
}
