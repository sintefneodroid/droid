using System;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using droid.Runtime.Sampling;
using UnityEngine;

namespace droid.Runtime.Structs.Space {
  /// <inheritdoc cref="ISpace" />
  ///  <summary>
  ///  </summary>
  [Serializable]
  public struct Space1 : ISpace {
    #region Fields

    /// <summary>
    ///
    /// </summary>
    [Header("Space", order = 103)]
    [SerializeField]
    internal float _min_;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    internal float _max_;

    /// <summary>
    ///
    /// </summary>
    ///
    [Range(0, 15)]
    [SerializeField]
    int _decimal_granularity;

    [SerializeField] Normalisation normalised;

    #endregion

    public Space1(int decimal_granularity = 2) : this() {
      this._decimal_granularity = decimal_granularity;
      this._min_ = -1f; //float.NegativeInfinity;
      this._max_ = 1f; //float.PositiveInfinity;
    }

    /// <summary>
    ///
    /// </summary>
    public float Span { get { return this._max_ - this._min_; } }

    public float Clip(float v, float min, float max) { return Mathf.Clamp(v, min, max); }
    public float Clip(float v) { return this.Clip(v, this._min_, this._max_); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public dynamic ClipNormaliseRound(dynamic v) { return this.Round(this.Normalise01(this.Clip(v))); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public float Normalise01(float v) {
      if (v > this._max_ || v < this._min_) {
        throw new ArgumentException();
      }

      if (this.Span <= 0) {
        return 0;
      }

      return (v - this._min_) / this.Span;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Vector2 ToVector2() { return new Vector2(this._min_, this._max_); }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Vector3 ToVector3() { return new Vector3(this._min_, this._max_, this._decimal_granularity); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public float Denormalise01(float v) {
      if (v > 1 || v < 0) {
        throw new ArgumentException();
      }

      if (this.Span <= 0) {
        return 0;
      }

      return v * this.Span + this._min_;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public dynamic ClipRoundDenormaliseClip(dynamic v) {
      return this.Clip(this.Round(this.Denormalise01(Clip(v, 0, 1))));
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static string Vector3Description() { return "Space (min, max, granularity)"; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public float Round(float v) { return (float)Math.Round(v, this._decimal_granularity); }

    /// <summary>
    ///
    /// </summary>
    public static Space1 TwentyEighty { get { return new Space1(1) {_min_ = 0.2f, _max_ = 0.8f}; } }

    /// <summary>
    ///
    /// </summary>
    public static Space1 ZeroOne { get { return new Space1(1) {_min_ = 0, _max_ = 1}; } }

    /// <summary>
    ///
    /// </summary>
    public int DecimalGranularity {
      get { return this._decimal_granularity; }
      set { this._decimal_granularity = value; }
    }

    /// <summary>
    ///
    /// </summary>
    public Boolean NormalisedBool {
      get { return this.normalised == Normalisation.Zero_one_; }
      set { this.normalised = value ? Normalisation.Zero_one_ : Normalisation.None_; }
    }

    /// <summary>
    ///
    /// </summary>
    public static Space1 DiscreteMinusOneOne { get { return new Space1(0) {_min_ = -1, _max_ = 1}; } }

    /// <summary>
    ///
    /// </summary>
    public static Space1 DiscreteZeroOne { get { return new Space1(0) {_min_ = 0, _max_ = 1}; } }

    /// <summary>
    ///
    /// </summary>
    public dynamic Min { get { return this._min_; } set { this._min_ = value; } }

    /// <summary>
    ///
    /// </summary>
    public dynamic Max { get { return this._max_; } set { this._max_ = value; } }

    public Normalisation Normalised { get { return this.normalised; } }

    /// <summary>
    ///
    /// </summary>
    /// <param name="vector3_field"></param>
    public void FromVector3(Vector3 vector3_field) {
      this._decimal_granularity = (int)vector3_field.z;
      this._max_ = vector3_field.y;
      this._min_ = vector3_field.x;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Space1 operator*(Space1 a, float b) {
      a.Max *= b;
      a.Min *= b;
      return a;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="bounds_extents"></param>
    /// <param name="normalised"></param>
    /// <param name="decimal_granularity"></param>
    /// <returns></returns>
    public static Space1 FromCenterExtents(float bounds_extents,
                                           bool normalised = true,
                                           int decimal_granularity = 4) {
      return new Space1 {
                            _min_ = -bounds_extents,
                            Max = bounds_extents,
                            NormalisedBool = normalised,
                            DecimalGranularity = decimal_granularity
                        };
    }
  }
}
