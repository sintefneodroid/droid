using System;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities.Extensions;
using UnityEngine;

namespace droid.Runtime.Structs.Space {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [Serializable]
  public struct Space4 : ISpace {
    #region Fields
    public NormalisationEnum Normalised { get { return this.normalised; }      set { this.normalised = value; } }
    /// <summary>
    ///
    /// </summary>
    [Header("Space", order = 103)]
    [SerializeField]
     Vector4 _min_;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
     Vector4 _max_;

    /// <summary>
    ///
    /// </summary>
    [Range(0, 15)]
    [SerializeField]
    int _decimal_granularity;

    [SerializeField]  NormalisationEnum normalised;

    #endregion



    /// <summary>
    ///
    /// </summary>
    public Vector4 Span { get { return this._max_ - this._min_; } }

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


    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public int DecimalGranularity {
      get { return this._decimal_granularity; }
      set { this._decimal_granularity = value; }
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
    public Space1 Wspace {
      get { return new Space1 {Min = this._min_.w, Max = this._max_.w,DecimalGranularity = this.DecimalGranularity}; }
    }

    public Vector4 Clip(Vector4 v, Vector4 min, Vector4 max) {
      return new Vector4(Mathf.Clamp(value : v.x, min : min.x, max : max.x),
                         Mathf.Clamp(value : v.y, min : min.y, max : max.y),
                         Mathf.Clamp(value : v.z, min : min.z, max : max.z),
                         Mathf.Clamp(value : v.w, min : min.w, max : max.w));
    }

    public Vector4 Clip(Vector4 v) { return this.Clip(v : v, min : this._min_, max : this._max_); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    Vector4 ClipRound(Vector4 v) { return this.Clip(this.Round(v : v)); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Vector4 Round(Vector4 v) {
      v.x = this.Round(v : v.x);
      v.y = this.Round(v : v.y);
      v.w = this.Round(v : v.z);
      v.z = this.Round(v : v.w);
      return v;
    }

     dynamic ClipRoundDenormalise01Clip(dynamic configuration_configurable_value) {
      return this.Clip(this.Round(this.Denormalise01(Clip(v : configuration_configurable_value,
                                                          min : Vector4.zero,
                                                          max : Vector4.one))));
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
        v = this._min_;
      }

      if (this.Span.x > 0) {
        v.x = this.Round((v.x - this._min_.x) / this.Span.x);
      } else {
        v.x = 0;
      }

      if (v.y > this._max_.y) {
        v.y = this._max_.y;
      } else if (v.y < this._min_.y) {
        v = this._min_;
      }

      if (this.Span.y > 0) {
        v.y = this.Round((v.y - this._min_.y) / this.Span.y);
      } else {
        v.y = 0;
      }

      if (v.z > this._max_.z) {
        v.z = this._max_.z;
      } else if (v.z < this._min_.z) {
        v = this._min_;
      }

      if (this.Span.z > 0) {
        v.z = this.Round((v.z - this._min_.z) / this.Span.z);
      } else {
        v.z = 0;
      }

      if (v.w > this._max_.w) {
        v.w = this._max_.w;
      } else if (v.w < this._min_.w) {
        v = this._min_;
      }

      if (this.Span.w > 0) {
        v.w = this.Round((v.w - this._min_.w) / this.Span.w);
      } else {
        v.w = 0;
      }

      return v;
    }


    public dynamic Mean { get { return (this.Max + this.Min) * 0.5f; } }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public float Round(float v) { return (float)Math.Round(value : v, digits : this.DecimalGranularity); }

    /// <summary>
    ///
    /// </summary>
    public static Space4 ZeroOne { get { return new Space4 {_min_ = Vector4.zero, Max = Vector4.one,
                                                               DecimalGranularity = 4,
                                                               Normalised = NormalisationEnum.Zero_one_
                                                           }; } }

    /// <summary>
    ///
    /// </summary>
    public static Space4 TwentyEighty {
      get { return new Space4 {_min_ = Vector4.one * 0.2f, Max = Vector4.one * 0.8f,
                                  DecimalGranularity = 4,
                                  Normalised = NormalisationEnum.Zero_one_
                              }; }
    }

    /// <summary>
    ///
    /// </summary>
    public static Space4 MinusOneOne {
      get { return new Space4 {_min_ = -Vector4.one, Max = Vector4.one,
                                  DecimalGranularity = 4,
                                  Normalised = NormalisationEnum.Zero_one_
                              }; }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public dynamic Max { get { return this._max_; } set { this._max_ = value; } }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public dynamic Min { get { return this._min_; } set { this._min_ = value; } }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
     Vector4 Denormalise01(Vector4 v) { return v.Multiply(b : this.Span) + this._min_; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
     Vector4 Normalise01(Vector4 v) { return (v - this._min_).Divide(b : this.Span); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Space4 operator*(Space4 a, float b) {
      a.Max *= b;
      a.Min *= b;
      return a;
    }
  }
}
