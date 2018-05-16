using UnityEngine;

namespace Neodroid.Utilities.Interfaces {
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
    Structs.Space3 TripleSpace { get; }
  }
}
