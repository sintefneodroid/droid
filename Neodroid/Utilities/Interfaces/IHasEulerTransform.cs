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

    Structs.Space3 PositionSpace { get; }
    Structs.Space3 DirectionSpace { get; }
    Structs.Space3 RotationSpace { get; }
  }
}
