using Neodroid.Runtime.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Runtime.Interfaces {
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
