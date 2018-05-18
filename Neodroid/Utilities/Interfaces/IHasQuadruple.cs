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

    Structs.Space4 QuadSpace { get; }
  }
}
