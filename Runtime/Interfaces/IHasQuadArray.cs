using droid.Runtime.Structs.Space;
using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IHasQuadArray {
    /// <summary>
    /// </summary>
    Quaternion[] ObservationArray { get; }

    Space1[] ObservationSpace { get; }
  }
}
