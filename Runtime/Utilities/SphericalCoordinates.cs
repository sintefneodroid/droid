using System;
using UnityEngine;

namespace droid.Runtime.Utilities {

//http://blog.nobel-joergensen.com/2010/10/22/spherical-coordinates-in-unity/
//http://en.wikipedia.org/wiki/Spherical_coordinate_system
  /// <summary>
  ///
  /// </summary>
  [Serializable]
  public class SphericalCoordinates {
    /// <summary>
    ///
    /// </summary>
    public float Radius {
      get { return this._radius; }
      set { this._radius = Mathf.Clamp(value, this._min_radius, this._max_radius); }
    }

    /// <summary>
    ///
    /// </summary>
    public float Polar {
      get { return this._polar; }
      set {
        this._polar = this._LoopPolar
                          ? Mathf.Repeat(value, this._max_polar - this._min_polar)
                          : Mathf.Clamp(value, this._min_polar, this._max_polar);
      }
    }

    /// <summary>
    ///
    /// </summary>
    public float Elevation {
      get { return this._elevation; }
      set {
        this._elevation = this._LoopElevation
                              ? Mathf.Repeat(value, this._max_elevation - this._min_elevation)
                              : Mathf.Clamp(value, this._min_elevation, this._max_elevation);
      }
    }

    // Determine what happen when a limit is reached, repeat or clamp.
    /// <summary>
    ///
    /// </summary>
    [SerializeField] bool _LoopPolar = true;
    /// <summary>
    ///
    /// </summary>
    [SerializeField] bool _LoopElevation = false;
    [SerializeField]
    float _radius;
    [SerializeField]float _polar;
    [SerializeField]float _elevation;
    [SerializeField]float _min_radius;
    [SerializeField]float _max_radius;
    [SerializeField]float _min_polar;
    [SerializeField]float _max_polar;
    [SerializeField]float _min_elevation;
    [SerializeField]float _max_elevation;

    public SphericalCoordinates(float r,
                                float p,
                                float s,
                                float min_radius = 1f,
                                float max_radius = 20f,
                                float min_polar = 0f,
                                float max_polar = (Mathf.PI * 2f),
                                float min_elevation = 0f,
                                float max_elevation = (Mathf.PI / 3f)) {
      this._min_radius = min_radius;
      this._max_radius = max_radius;
      this._min_polar = min_polar;
      this._max_polar = max_polar;
      this._min_elevation = min_elevation;
      this._max_elevation = max_elevation;

      this.SetRadius(r);
      this.SetRotation(p, s);
    }

    public SphericalCoordinates(Transform T,
                                float min_radius = 1f,
                                float max_radius = 20f,
                                float min_polar = 0f,
                                float max_polar = (Mathf.PI * 2f),
                                float min_elevation = 0f,
                                float max_elevation = (Mathf.PI / 3f)) : this(T.position,
                                                                             min_radius,
                                                                             max_radius,
                                                                             min_polar,
                                                                             max_polar,
                                                                             min_elevation,
                                                                             max_elevation) { }

    public SphericalCoordinates(Vector3 cartesian_coordinate,
                                float min_radius = 1f,
                                float max_radius = 20f,
                                float min_polar = 0f,
                                float max_polar = (Mathf.PI * 2f),
                                float min_elevation = 0f,
                                float max_elevation = (Mathf.PI / 3f)) {
      this._min_radius = min_radius;
      this._max_radius = max_radius;
      this._min_polar = min_polar;
      this._max_polar = max_polar;
      this._min_elevation = min_elevation;
      this._max_elevation = max_elevation;

      this.FromCartesian(cartesian_coordinate);
    }

    /// <summary>
    ///
    /// </summary>
    public Vector3 ToCartesian {
      get {
        var a = this.Radius * Mathf.Cos(this.Elevation);
        return new Vector3(a * Mathf.Cos(this.Polar), this.Radius * Mathf.Sin(this.Elevation), a * Mathf.Sin(this.Polar));
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="cartesian_coordinate"></param>
    /// <returns></returns>
    public SphericalCoordinates FromCartesian(Vector3 cartesian_coordinate) {
      if (Math.Abs(cartesian_coordinate.x) < float.Epsilon) {
        cartesian_coordinate.x = Mathf.Epsilon;
      }

      this.Radius = cartesian_coordinate.magnitude;

      this.Polar = Mathf.Atan(cartesian_coordinate.z / cartesian_coordinate.x);

      if (cartesian_coordinate.x < 0f) {
        this.Polar += Mathf.PI;
      }

      this.Elevation = Mathf.Asin(cartesian_coordinate.y / this.Radius);

      return this;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public SphericalCoordinates RotatePolarAngle(float x) { return this.Rotate(x, 0f); }
    /// <summary>
    ///
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public SphericalCoordinates RotateElevationAngle(float x) { return this.Rotate(0f, x); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="new_polar"></param>
    /// <param name="new_elevation"></param>
    /// <returns></returns>
    public SphericalCoordinates Rotate(float new_polar, float new_elevation) {
      return this.SetRotation(this.Polar + new_polar, this.Elevation + new_elevation);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public SphericalCoordinates SetPolarAngle(float x) { return this.SetRotation(x, this.Elevation); }
    /// <summary>
    ///
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public SphericalCoordinates SetElevationAngle(float x) { return this.SetRotation(this.Polar, x); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="new_polar"></param>
    /// <param name="new_elevation"></param>
    /// <returns></returns>
    public SphericalCoordinates SetRotation(float new_polar, float new_elevation) {
      this.Polar = new_polar;
      this.Elevation = new_elevation;

      return this;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public SphericalCoordinates TranslateRadius(float x) { return this.SetRadius(this.Radius + x); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="rad"></param>
    /// <returns></returns>
    public SphericalCoordinates SetRadius(float rad) {
      this.Radius = rad;
      return this;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      return $"[Radius] {this.Radius}. [Polar] {this.Polar}. [Elevation] {this.Elevation}.";
    }
  }
}

