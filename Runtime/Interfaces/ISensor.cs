using System;
using System.Collections.Generic;

namespace droid.Runtime.Interfaces {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public interface ISensor : IRegisterable {
    /// <summary>
    /// </summary>
    IEnumerable<Single> FloatEnumerable { get; }

    /// <summary>
    /// </summary>
    void UpdateObservation();
  }
}
