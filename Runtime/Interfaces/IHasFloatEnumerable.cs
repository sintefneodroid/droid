using System.Collections.Generic;

namespace Neodroid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IHasFloatEnumerable {
    /// <summary>
    /// </summary>
    IEnumerable<float> FloatEnumerable { get; }
  }
}