using Neodroid.Runtime.Utilities.Structs;

namespace Neodroid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IHasSingle {
    /// <summary>
    /// </summary>
    float ObservationValue { get; }

    /// <summary>
    /// </summary>
    ValueSpace SingleSpace { get; }
  }
}