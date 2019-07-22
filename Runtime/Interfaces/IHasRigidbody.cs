using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Interfaces {
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
