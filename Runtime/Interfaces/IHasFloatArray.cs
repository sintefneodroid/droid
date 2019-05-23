using droid.Runtime.Utilities.Structs;

namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IHasFloatArray {
    /// <summary>
    /// </summary>
    float[] ObservationArray { get; }

    Space1[] ObservationSpace { get; }
  }
}
