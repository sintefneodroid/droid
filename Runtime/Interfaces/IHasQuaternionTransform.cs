using UnityEngine;

namespace Neodroid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IHasQuaternionTransform {
    /// <summary>
    /// </summary>
    Vector3 Position { get; }

    /// <summary>
    /// </summary>
    Quaternion Rotation { get; }
  }
}