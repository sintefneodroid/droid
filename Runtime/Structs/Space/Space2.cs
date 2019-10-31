using System;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Structs.Space {
  /// <summary>
  ///
  /// </summary>
  [Serializable]
  public struct Space2 : ISpace {
    #region Fields

    public Normalisation Normalised { get { return this.normalised; } set { this.normalised = value; } }

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
    [SerializeField] Normalisation normalised;


    #endregion

    /// <summary>
    ///
    /// </summary>
    public Vector2 Span { get { return this._max_ - this._min_; } }

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
      return new Vector2(Mathf.Clamp(v.x, min.x, max.x), Mathf.Clamp(v.y, min.y, max.y));
    }

    public Vector2 Clip(Vector2 v) { return this.Clip(v, this._min_, this._max_); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    Vector2 ClipRound(Vector2 v) { return this.Clip(this.Round(v)); }

    dynamic ClipRoundDenormalise01Clip(dynamic configuration_configurable_value) {
      return this.Clip(this.Round(this.Denormalise01(Clip(configuration_configurable_value,
                                                          Vector2.zero,
                                                          Vector2.one))));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public dynamic Project(dynamic v) {
      switch (this.Normalised) {
        case Normalisation.None_:
          return ClipRound(v);
        case Normalisation.Zero_one_:
          return ClipNormalise01Round(v);
        case Normalisation.Minus_one_one_:
          return ClipNormalise01Round(v); //return ClipNormaliseMinusOneOneRound(v);
        default: throw new ArgumentOutOfRangeException();
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public dynamic Reproject(dynamic v) {
      switch (this.Normalised) {
        case Normalisation.None_:
          return ClipRound(v);
        case Normalisation.Zero_one_:
          return ClipRoundDenormalise01Clip(v);
        case Normalisation.Minus_one_one_:
          return ClipRoundDenormalise01Clip(v); // return ClipRoundDenormaliseMinusOneOneClip(v);
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
    public float Round(float v) { return (float)Math.Round(v, this.DecimalGranularity); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Vector2 Round(Vector2 v) {
      v.x = this.Round(v.x);
      v.y = this.Round(v.y);
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
                              Normalised = Normalisation.Zero_one_
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
                              Normalised = Normalisation.Zero_one_
                          };
      }
    }

    public static Space2 MinusOneOne {
      get {
        return new Space2 {
                              _min_ = -Vector2.one,
                              Max = Vector2.one,
                              DecimalGranularity = 4,
                              Normalised = Normalisation.Zero_one_
                          };
      }
    }

    /// <summary>
    ///
    /// </summary>
    public dynamic Min { get { return this._min_; } set { this._min_ = value; } }

    /// <summary>
    ///
    /// </summary>
    public dynamic Max { get { return this._max_; } set { this._max_ = value; } }

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
                                           Normalisation normalised = Normalisation.Zero_one_,
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
