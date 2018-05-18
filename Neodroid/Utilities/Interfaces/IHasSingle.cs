using droid.Neodroid.Utilities.Structs;

namespace droid.Neodroid.Utilities.Interfaces {
  /// <summary>
  ///
  /// </summary>
  public interface IHasSingle {
    /// <summary>
    ///
    /// </summary>
    float ObservationValue { get; }

    /// <summary>
    ///
    /// </summary>
    ValueSpace SingleSpace { get; }
  }
}
