using droid.Neodroid.Utilities.Structs;
using UnityEngine;

namespace droid.Neodroid.Utilities.Interfaces {
  /// <summary>
  ///
  /// </summary>
  public interface IHasTriple {
    /// <summary>
    ///
    /// </summary>
    Vector3 ObservationValue { get; }

    /// <summary>
    ///
    /// </summary>
    Space3 TripleSpace { get; }
  }
}
