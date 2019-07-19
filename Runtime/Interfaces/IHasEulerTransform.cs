using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IHasEulerTransform {
    /// <summary>
    /// </summary>
    Vector3 Position { get; }

    /// <summary>
    /// </summary>
    Vector3 Direction { get; }

    /// <summary>
    /// </summary>
    Vector3 Rotation { get; }

    /// <summary>
    /// </summary>
    Space3 PositionSpace { get; }

    /// <summary>
    /// </summary>
    Space3 DirectionSpace { get; }

    /// <summary>
    /// </summary>
    Space3 RotationSpace { get; }
  }
}
