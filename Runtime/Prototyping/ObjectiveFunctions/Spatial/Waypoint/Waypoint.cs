using System;
using UnityEngine;

namespace droid.Runtime.Prototyping.ObjectiveFunctions.Spatial.Waypoint {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class Waypoint : MonoBehaviour {
    [SerializeField] float radius = 1.0f;

    public Single Radius { get { return this.radius; } }
  }
}
