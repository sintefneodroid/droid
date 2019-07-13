using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IHasTriple {
    /// <summary>
    /// </summary>
    Vector3 ObservationValue { get; }

    /// <summary>
    /// </summary>
    Space3 TripleSpace { get; }
  }
}
