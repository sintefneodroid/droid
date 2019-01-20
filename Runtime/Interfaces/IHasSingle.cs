using droid.Runtime.Utilities.Structs;

namespace droid.Runtime.Interfaces {
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
