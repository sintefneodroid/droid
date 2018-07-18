using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Utilities.Interfaces {
  /// <summary>
  ///
  /// </summary>
  public interface IHasEulerTransform {
    /// <summary>
    ///
    /// </summary>
    Vector3 Position { get; }

    /// <summary>
    ///
    /// </summary>
    Vector3 Direction { get; }

    /// <summary>
    ///
    /// </summary>
    Vector3 Rotation { get; }

    Space3 PositionSpace { get; }
    Space3 DirectionSpace { get; }
    Space3 RotationSpace { get; }
  }
}
