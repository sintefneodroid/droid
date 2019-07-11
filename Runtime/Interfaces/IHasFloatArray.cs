using droid.Runtime.Structs.Space;
using droid.Runtime.Utilities.Structs;

namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IHasFloatArray {
    /// <summary>
    /// </summary>
    float[] ObservationArray { get; }

    /// <summary>
    ///
    /// </summary>
    Space1[] ObservationSpace { get; }
  }
}
