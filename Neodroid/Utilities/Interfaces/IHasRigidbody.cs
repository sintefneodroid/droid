using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Utilities.Interfaces {
  /// <summary>
  ///
  /// </summary>
  public interface IHasRigidbody {
    /// <summary>
    ///
    /// </summary>
    Vector3 Velocity { get; }

    /// <summary>
    ///
    /// </summary>
    Vector3 AngularVelocity { get; }

    Space3 VelocitySpace { get; }
    Space3 AngularSpace { get; }
  }
}
