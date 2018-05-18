using droid.Neodroid.Utilities.Structs;
using UnityEngine;

namespace droid.Neodroid.Utilities.Interfaces {
  /// <summary>
  ///
  /// </summary>
  public interface IHasDouble {
    /// <summary>
    ///
    /// </summary>
    Vector2 ObservationValue { get; }

    /// <summary>
    ///
    /// </summary>
    Space2 ObservationSpace2D { get; }
  }
}
