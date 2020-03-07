using System;
using UnityEngine;

namespace droid.Runtime.Structs.Space {
//Adapted from http://blog.nobel-joergensen.com/2010/10/22/spherical-coordinates-in-unity/
//http://en.wikipedia.org/wiki/Spherical_coordinate_system
  /// <summary>
  ///
  /// </summary>
  [Serializable]
  public struct SphericalSpace {
    // Determine what happen when a limit is reached, repeat or clamp.
    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    bool _LoopPolar;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    bool _LoopElevation;

    [SerializeField] float _radius;
    [SerializeField] float _polar;
    [SerializeField] float _elevation;
    [SerializeField] float _min_radius;
    [SerializeField] float _max_radius;
    [SerializeField] float _min_polar;
    [SerializeField] float _max_polar;
    [SerializeField] float _min_elevation;
    [SerializeField] float _max_elevation;

    public SphericalSpace(float radius,
                          float polar,
                          float elevation,
                          float min_radius = 1f,
                          float max_radius = 20f,
                          float min_polar = 0f,
                          float max_polar = Mathf.PI * 2f,
                          float min_elevation = 0f,
                          float max_elevation = Mathf.PI / 3f,
                          bool loop_polar = true,
                          bool loop_elevation = false) {
      this._min_radius = min_radius;
      this._max_radius = max_radius;
      this._min_polar = min_polar;
      this._max_polar = max_polar;
      this._min_elevation = min_elevation;
      this._max_elevation = max_elevation;
      this._LoopPolar = loop_polar;
      this._LoopElevation = loop_elevation;

      this._radius = Mathf.Clamp(value : radius, min : this._min_radius, max : this._max_radius);

      this._polar = LoopOrClamp(v : polar,
                                min : this._min_polar,
                                max : this._max_polar,
                                loop : loop_polar);

      this._elevation = LoopOrClamp(v : elevation,
                                    min : this._min_elevation,
                                    max : this._max_elevation,
                                    loop : loop_elevation);
    }

    static float LoopOrClamp(float v, float min, float max, bool loop) {
      return loop ? Mathf.Repeat(t : v, length : max - min) : Mathf.Clamp(value : v, min : min, max : max);
    }

    /// <summary>
    ///
    /// </summary>
    public Single Elevation {
      get { return this._elevation; }
      set {
        this._elevation = LoopOrClamp(v : value,
                                      min : this._min_elevation,
                                      max : this._max_elevation,
                                      loop : this._LoopElevation);
      }
    }

    /// <summary>
    ///
    /// </summary>
    public Single Polar {
      get { return this._polar; }
      set {
        this._polar = LoopOrClamp(v : value,
                                  min : this._min_polar,
                                  max : this._max_polar,
                                  loop : this._LoopPolar);
        ;
      }
    }

    /// <summary>
    ///
    /// </summary>
    public Single Radius {
      get { return this._radius; }
      set { this._radius = Mathf.Clamp(value : value, min : this._min_radius, max : this._max_radius); }
    }

    /// <summary>
    /// Return a 2d vector of the polar and elevation coordinate
    /// </summary>
    public Vector2 ToVector2 { get { return new Vector2(x : this._polar, y : this._elevation); } }

    public Vector3 ToVector3 {
      get { return new Vector3(x : this._polar, y : this._elevation, z : this._radius); }
    }

    /// <summary>
    ///
    /// </summary>
    public Vector3 ToCartesian() {
      var a = this._radius * Mathf.Cos(f : this._elevation);
      return new Vector3(x : a * Mathf.Cos(f : this._polar),
                         y : this._radius * Mathf.Sin(f : this._elevation),
                         z : a * Mathf.Sin(f : this._polar));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="cartesian_coordinate"></param>
    /// <returns></returns>
    public SphericalSpace UpdateFromCartesian(Vector3 cartesian_coordinate) {
      if (Math.Abs(value : cartesian_coordinate.x) < float.Epsilon) {
        cartesian_coordinate.x = Mathf.Epsilon;
      }

      this._radius = cartesian_coordinate.magnitude;

      this._polar = Mathf.Atan(f : cartesian_coordinate.z / cartesian_coordinate.x);

      if (cartesian_coordinate.x < 0f) {
        this._polar += Mathf.PI;
      }

      this._elevation = Mathf.Asin(f : cartesian_coordinate.y / this._radius);

      return this;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="cartesian_coordinate"></param>
    /// <param name="min_radius"></param>
    /// <param name="max_radius"></param>
    /// <param name="min_polar"></param>
    /// <param name="max_polar"></param>
    /// <param name="min_elevation"></param>
    /// <param name="max_elevation"></param>
    /// <returns></returns>
    public static SphericalSpace FromCartesian(Vector3 cartesian_coordinate,
                                               float min_radius = 1f,
                                               float max_radius = 20f,
                                               float min_polar = 0f,
                                               float max_polar = Mathf.PI * 2f,
                                               float min_elevation = 0f,
                                               float max_elevation = Mathf.PI / 3f) {
      var spherical = new SphericalSpace {
                                             _min_radius = min_radius,
                                             _max_radius = max_radius,
                                             _min_polar = min_polar,
                                             _max_polar = max_polar,
                                             _min_elevation = min_elevation,
                                             _max_elevation = max_elevation
                                         };

      if (Math.Abs(value : cartesian_coordinate.x) < float.Epsilon) {
        cartesian_coordinate.x = Mathf.Epsilon;
      }

      spherical._radius = cartesian_coordinate.magnitude;

      spherical._polar = Mathf.Atan(f : cartesian_coordinate.z / cartesian_coordinate.x);

      if (cartesian_coordinate.x < 0f) {
        spherical._polar += Mathf.PI;
      }

      spherical._elevation = Mathf.Asin(f : cartesian_coordinate.y / spherical._radius);

      return spherical;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="T"></param>
    /// <param name="min_radius"></param>
    /// <param name="max_radius"></param>
    /// <param name="min_polar"></param>
    /// <param name="max_polar"></param>
    /// <param name="min_elevation"></param>
    /// <param name="max_elevation"></param>
    /// <returns></returns>
    public static SphericalSpace FromTransform(Transform T,
                                               float min_radius = 1f,
                                               float max_radius = 20f,
                                               float min_polar = 0f,
                                               float max_polar = Mathf.PI * 2f,
                                               float min_elevation = 0f,
                                               float max_elevation = Mathf.PI / 3f) {
      return FromCartesian(cartesian_coordinate : T.position,
                           min_radius : min_radius,
                           max_radius : max_radius,
                           min_polar : min_polar,
                           max_polar : max_polar,
                           min_elevation : min_elevation,
                           max_elevation : max_elevation);
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      return $"Radius:{this.Radius}, Polar:{this.Polar}, Elevation:{this.Elevation}.";
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="scroll_speed"></param>
    /// <returns></returns>
    public SphericalSpace TranslateRadius(Single scroll_speed) {
      this.Radius += scroll_speed;
      return this;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="polar_delta"></param>
    /// <param name="elevation_delta"></param>
    /// <returns></returns>
    public SphericalSpace Rotate(Single polar_delta, Single elevation_delta) {
      this.Polar += polar_delta;
      this.Elevation += elevation_delta;
      return this;
    }
  }
}
