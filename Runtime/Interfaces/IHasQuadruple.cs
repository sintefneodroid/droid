using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IHasQuadruple {
    /// <summary>
    /// </summary>
    Quaternion ObservationValue { get; }

    /// <summary>
    /// </summary>
    Space4 QuadSpace { get; }
  }
}
