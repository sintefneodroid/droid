using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IHasQuaternionTransform {
    /// <summary>
    /// </summary>
    Vector3 Position { get; }

    /// <summary>
    /// </summary>
    Quaternion Rotation { get; }

    /// <summary>
    ///
    /// </summary>
    Space1 PositionSpace { get; }

    /// <summary>
    ///
    /// </summary>
    Space1 RotationSpace { get; }
  }
}
