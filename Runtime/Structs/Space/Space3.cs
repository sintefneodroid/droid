using System;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities.Extensions;
using UnityEngine;

namespace droid.Runtime.Structs.Space {
  /// <summary>
  ///
  /// </summary>
  [Serializable]
  public struct Space3 : ISpace {
    #region Fields
    public Normalisation Normalised { get { return this.normalised; }      set { this.normalised = value; } }
    [Header("Space", order = 103)]
    [SerializeField]
     Vector3 _min_;

    [SerializeField]  Vector3 _max_;
    [Range(0, 15)] [SerializeField] int _decimal_granularity;
    [SerializeField]  Normalisation normalised;

    #endregion


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
    public Vector3 Span { get { return this._max_ - this._min_; } }


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
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static Vector3 Clip(Vector3 v, Vector3 min, Vector3 max) {
      return new Vector3(Mathf.Clamp(v.x, min.x, max.x),
                         Mathf.Clamp(v.y, min.y, max.y),
                         Mathf.Clamp(v.z, min.z, max.z));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Vector3 Clip(Vector3 v) { return Clip(v, this._min_, this._max_); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
     Vector3 ClipRound(Vector3 v) { return this.Clip(this.Round(v)); }

     dynamic ClipRoundDenormalise01Clip(dynamic configuration_configurable_value) {
      return this.Clip(this.Round(this.Denormalise01(Clip(configuration_configurable_value,
                                                          Vector3.zero,
                                                          Vector3.one))));
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

      if (v.z > this._max_.z) {
        v.z = this._max_.z;
      } else if (v.z < this._min_.z) {
        v.z = this._min_.z;
      }

      if (this.Span.z > 0) {
        v.z = this.Round((v.z - this._min_.z) / this.Span.z);
      } else {
        v.z = 0;
      }

      return v;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public float Round(float v) { return (float)Math.Round(v, this.DecimalGranularity); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Vector3 Round(Vector3 v) {
      v.x = this.Round(v.x);
      v.y = this.Round(v.y);
      v.z = this.Round(v.z);
      return v;
    }


    /// <summary>
    ///
    /// </summary>
    public Space1 Xspace {
      get { return new Space1 {Min = this._min_.x, Max = this._max_.x,DecimalGranularity = this.DecimalGranularity}; }
    }

    /// <summary>
    ///
    /// </summary>
    public Space1 Yspace {
      get { return new Space1 {Min = this._min_.y, Max = this._max_.y,DecimalGranularity = this.DecimalGranularity}; }
    }

    /// <summary>
    ///
    /// </summary>
    public Space1 Zspace {
      get { return new Space1 {Min = this._min_.z, Max = this._max_.z,DecimalGranularity = this.DecimalGranularity}; }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static Space3 operator+(Space3 b, Vector3 c) {
      b._min_ += c;
      b._max_ += c;
      return b;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static Space3 operator-(Space3 b, Vector3 c) {
      b._min_ -= c;
      b._max_ -= c;
      return b;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static Space3 operator-(Vector3 c, Space3 b) {
      b._min_ -= c;
      b._max_ -= c;
      return b;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="c"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Space3 operator+(Vector3 c, Space3 b) {
      b._min_ += c;
      b._max_ += c;
      return b;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Space3 operator*(Space3 a, float b) {
      a.Max *= b;
      a.Min *= b;
      return a;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static Space3 ZeroOne { get { return new Space3 {_min_ = Vector3.zero, Max = Vector3.one,
                                                               DecimalGranularity = 4,
                                                               Normalised = Normalisation.Zero_one_
                                                           }; } }

    /// <summary>
    ///
    /// </summary>
    public static Space3 TwentyEighty {
      get { return new Space3 {Min = Vector3.one * 0.2f, _max_ = Vector3.one * 0.8f,
                                  DecimalGranularity = 4,
                                  Normalised = Normalisation.Zero_one_
                              }; }
    }

    /// <summary>
    ///
    /// </summary>
    public static Space3 MinusOneOne { get { return new Space3 {_min_ = -Vector3.one, Max = Vector3.one,
                                                                   DecimalGranularity = 4,
                                                                   Normalised = Normalisation.Zero_one_
                                                               }; } }



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
    Vector3 Denormalise01(Vector3 v) { return v.Multiply(this.Span) + this._min_; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    Vector3 Normalise01(Vector3 v) { return (v - this._min_).Divide(this.Span); }

    /// <summary>
    /// Return Space3 with the negative and positive extents respectively as min and max for each dimension
    /// </summary>
    /// <param name="bounds_extents"></param>
    /// <returns></returns>
    public static Space3
        FromCenterExtents(Vector3 bounds_extents, Normalisation normalised=Normalisation.Zero_one_, int decimal_granularity = 4) {
      return new Space3 {
                            _min_ = -bounds_extents,
                            Max = bounds_extents,
                            normalised = normalised,
                            _decimal_granularity = decimal_granularity
                        };
    }
  }
}
