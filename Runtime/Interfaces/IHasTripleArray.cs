using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IHasTripleArray {
    /// <summary>
    /// </summary>
    Vector3[] ObservationArray { get; }

    Space1[] ObservationSpace { get; }
  }
}
