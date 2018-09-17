using UnityEngine;

namespace Neodroid.Utilities.Interfaces {
  /// <summary>
  /// 
  /// </summary>
  public interface IHasQuaternionTransform {
    /// <summary>
    /// 
    /// </summary>
    Vector3 Position { get; }

    /// <summary>
    /// 
    /// </summary>
    Quaternion Rotation { get; }
  }
}
