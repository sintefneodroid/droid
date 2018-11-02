using System.Collections.Generic;

namespace Neodroid.Runtime.Interfaces {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public interface IObserver : IRegisterable {
    /// <summary>
    /// </summary>
    IEnumerable<float> FloatEnumerable { get; }

    /// <summary>
    /// </summary>
    void UpdateObservation();

    /// <summary>
    /// </summary>
    void EnvironmentReset();
  }
}