using Neodroid.Runtime.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Runtime.Interfaces {
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

    /// <summary>
    /// 
    /// </summary>
    Space3 PositionSpace { get; }

    /// <summary>
    /// 
    /// </summary>
    Space3 DirectionSpace { get; }

    /// <summary>
    /// 
    /// </summary>
    Space3 RotationSpace { get; }
  }
}