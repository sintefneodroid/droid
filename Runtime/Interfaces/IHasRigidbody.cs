using Neodroid.Runtime.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IHasRigidbody {
    /// <summary>
    /// </summary>
    Vector3 Velocity { get; }

    /// <summary>
    /// </summary>
    Vector3 AngularVelocity { get; }

    /// <summary>
    /// </summary>
    Space3 VelocitySpace { get; }

    /// <summary>
    /// </summary>
    Space3 AngularSpace { get; }
  }
}
