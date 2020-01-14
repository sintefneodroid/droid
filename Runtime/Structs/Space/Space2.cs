using System;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Structs.Space {
  /// <inheritdoc />
  ///  <summary>
  ///  </summary>
  [Serializable]
  public struct Space2 : ISpace {
    #region Fields

    public NormalisationEnum Normalised { get { return this.normalised; } set { this.normalised = value; } }

    /// <summary>
    ///
    /// </summary>
    [Header("Space", order = 103)]
    [SerializeField]
    Vector2 _min_;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    Vector2 _max_;

    [Range(0, 15)] [SerializeField] int _decimal_granularity;
    [SerializeField] NormalisationEnum normalised;


    #endregion

    /// <summary>
    ///
    /// </summary>
    public Vector2 Span { get { return this._max_ - this._min_; } }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public int DecimalGranularity {
      get { return this._decimal_granularity; }
      set { this._decimal_granularity = value; }
    }

    /// <summary>
    ///
    /// </summary>
    public Space1 Xspace {
      get {
        return new Space1 {
                              Min = this._min_.x,
                              Max = this._max_.x,
                              DecimalGranularity = this.DecimalGranularity
                          };
      }
    }

    /// <summary>
    ///
    /// </summary>
    public Space1 Yspace {
      get {
        return new Space1 {
                              Min = this._min_.y,
                              Max = this._max_.y,
                              DecimalGranularity = this.DecimalGranularity
                          };
      }
    }

    public Vector2 Clip(Vector2 v, Vector2 min, Vector2 max) {
      return new Vector2(Mathf.Clamp(value : v.x, min : min.x, max : max.x), Mathf.Clamp(value : v.y, min : min.y, max : max.y));
    }

    public Vector2 Clip(Vector2 v) { return this.Clip(v : v, min : this._min_, max : this._max_); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    Vector2 ClipRound(Vector2 v) { return this.Clip(this.Round(v : v)); }

    dynamic ClipRoundDenormalise01Clip(dynamic configuration_configurable_value) {
      return this.Clip(this.Round(this.Denormalise01(Clip(v : configuration_configurable_value,
                                                          min : Vector2.zero,
                                                          max : Vector2.one))));
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <param name="v"></param>
    ///  <returns></returns>
    ///  <exception cref="T:System.ArgumentOutOfRangeException"></exception>
    public dynamic Project(dynamic v) {
      switch (this.Normalised) {
        case NormalisationEnum.None_:
          return ClipRound(v : v);
        case NormalisationEnum.Zero_one_:
          return ClipNormalise01Round(v : v);
        case NormalisationEnum.Minus_one_one_:
          return ClipNormalise01Round(v : v); //return ClipNormaliseMinusOneOneRound(v);
        default: throw new ArgumentOutOfRangeException();
      }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <param name="v"></param>
    ///  <returns></returns>
    ///  <exception cref="T:System.ArgumentOutOfRangeException"></exception>
    public dynamic Reproject(dynamic v) {
      switch (this.Normalised) {
        case NormalisationEnum.None_:
          return ClipRound(v : v);
        case NormalisationEnum.Zero_one_:
          return ClipRoundDenormalise01Clip(configuration_configurable_value : v);
        case NormalisationEnum.Minus_one_one_:
          return ClipRoundDenormalise01Clip(configuration_configurable_value : v); // return ClipRoundDenormaliseMinusOneOneClip(v);
        default: throw new ArgumentOutOfRangeException();
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    dynamic ClipNormalise01Round(dynamic v) {
      if (v.x > this._max_.x) {
        v.x = this._max_.x;
      } else if (v.x < this._min_.x) {
        v.x = this._min_.x;
      }

      if (this.Span.x > 0) {
        v.x = this.Round((v.x - this._min_.x) / this.Span.x);
      } else {
        v.x = 0;
      }

      if (v.y > this._max_.y) {
        v.y = this._max_.y;
      } else if (v.y < this._min_.y) {
        v.y = this._min_.y;
      }

      if (this.Span.y > 0) {
        v.y = this.Round((v.y - this._min_.y) / this.Span.y);
      } else {
        v.y = 0;
      }

      return v;
    }

    /// <summary>
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public float Round(float v) { return (float)Math.Round(value : v, digits : this.DecimalGranularity); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Vector2 Round(Vector2 v) {
      v.x = this.Round(v : v.x);
      v.y = this.Round(v : v.y);
      return v;
    }

    /// <summary>
    ///
    /// </summary>
    public static Space2 ZeroOne {
      get {
        return new Space2 {
                              _min_ = Vector2.zero,
                              Max = Vector2.one,
                              DecimalGranularity = 4,
                              Normalised = NormalisationEnum.Zero_one_
                          };
      }
    }

    /// <summary>
    ///
    /// </summary>
    public static Space2 TwentyEighty {
      get {
        return new Space2 {
                              _min_ = Vector2.one * 0.2f,
                              Max = Vector2.one * 0.8f,
                              DecimalGranularity = 4,
                              Normalised = NormalisationEnum.Zero_one_
                          };
      }
    }

    public static Space2 MinusOneOne {
      get {
        return new Space2 {
                              _min_ = -Vector2.one,
                              Max = Vector2.one,
                              DecimalGranularity = 4,
                              Normalised = NormalisationEnum.Zero_one_
                          };
      }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public dynamic Min { get { return this._min_; } set { this._min_ = value; } }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public dynamic Max { get { return this._max_; } set { this._max_ = value; } }

    public dynamic Mean { get { return (this.Max + this.Min) * 0.5f; } }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    Vector2 Denormalise01(Vector2 v) { return v * this.Span + this._min_; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    Vector2 Normalise01(Vector2 v) { return (v - this._min_) / this.Span; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Space2 operator*(Space2 a, float b) {
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
    public static Space2 FromCenterExtents(Vector2 bounds_extents,
                                           NormalisationEnum normalised = NormalisationEnum.Zero_one_,
                                           int decimal_granularity = 4) {
      return new Space2 {
                            _min_ = -bounds_extents,
                            Max = bounds_extents,
                            normalised = normalised,
                            _decimal_granularity = decimal_granularity
                        };
    }
  }
}
