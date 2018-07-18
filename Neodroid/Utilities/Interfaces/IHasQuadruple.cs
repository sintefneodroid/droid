using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Utilities.Interfaces {
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
