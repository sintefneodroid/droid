using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IHasDouble {
    /// <summary>
    /// </summary>
    Vector2 ObservationValue { get; }

    /// <summary>
    /// </summary>
    Space2 DoubleSpace { get; }
  }
}
