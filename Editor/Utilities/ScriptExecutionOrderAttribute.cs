using System;

namespace Common.Editors {
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