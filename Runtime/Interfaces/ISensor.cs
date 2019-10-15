using System.Collections.Generic;

namespace droid.Runtime.Interfaces {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public interface ISensor : IRegisterable {
    /// <summary>
    /// </summary>
    IEnumerable<float> FloatEnumerable { get; }

    /// <summary>
    /// </summary>
    void UpdateObservation();


  }
}
