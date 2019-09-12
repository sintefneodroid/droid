using System;
using UnityEngine;

namespace droid.Runtime.Utilities.Extensions {
  /// <summary>
  ///
  /// </summary>
  public static class RayDirectionHelper {
    const int _num_view_directions = 300;
    static readonly Single _golden_ratio = (1 + Mathf.Sqrt(5)) / 2;
    static readonly Single _angle_increment = Mathf.PI * 2 * _golden_ratio;

    /// <summary>
    ///
    /// </summary>
    public static readonly Vector3[] _Directions;

    static RayDirectionHelper() {
      _Directions = new Vector3[_num_view_directions];

      for (var i = 0; i < _num_view_directions; i++) {
        var t = (float)i / _num_view_directions;
        var inclination = Mathf.Acos(1 - 2 * t);
        var azimuth = _angle_increment * i;

        var x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
        var y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
        var z = Mathf.Cos(inclination);
        _Directions[i] = new Vector3(x, y, z);
      }
    }

    /*
   *     var rayDirections = RayDirectionHelper._Directions;

    foreach (var t in rayDirections) {
      var dir = this._cached_transform.TransformDirection(t);
      var ray = new Ray(this.position, dir);
      if (!Physics.SphereCast(ray,
                              this._settings.boundsRadius,
                              this._settings.collisionAvoidDst,
                              this._settings.obstacleMask)) {
        return dir;
      }
    }
   */
  }
}
