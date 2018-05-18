using droid.Neodroid.Utilities.Structs;
using UnityEngine;

namespace droid.Neodroid.Utilities.Interfaces {
  /// <summary>
  ///
  /// </summary>
  public interface IHasQuadruple {
    /// <summary>
    ///
    /// </summary>
    Quaternion ObservationValue { get; }

    Space4 QuadSpace { get; }
  }
}
