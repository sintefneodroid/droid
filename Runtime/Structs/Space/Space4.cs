using System;
using droid.Runtime.Interfaces;
using droid.Runtime.Sampling;
using droid.Runtime.Utilities.Extensions;
using UnityEngine;

namespace droid.Runtime.Structs.Space {
  /// <summary>
  /// </summary>
  [Serializable]
  public struct Space4 : ISpace {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public int DecimalGranularity {
      get { return this._decimal_granularity; }
      set { this._decimal_granularity = value; }
    }


    [SerializeField] bool normalised;


    /// <summary>
    ///
    /// </summary>
    public bool Normalised { get { return this.normalised; } set { this.normalised = value; } }

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    internal Vector4 _min_;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    internal Vector4 _max_;

    /// <summary>
    ///
    /// </summary>
    [Range(0, 15)]
    [SerializeField]
    int _decimal_granularity;

    public Space4(int decimal_granularity = 2) : this() {
      this._decimal_granularity = decimal_granularity;
      this._min_ = Vector4.negativeInfinity;
      this._max_ = Vector4.positiveInfinity;
    }

    /// <summary>
    ///
    /// </summary>
    public Vector4 Span { get { return this._max_ - this._min_; } }

    /// <summary>
    ///
    /// </summary>
    public Space1 Xspace {
      get {
        return new Space1(this.DecimalGranularity) {
                                                       Min = this._min_.x,
                                                       Max = this._max_.x
                                                   };
      }
    }

    /// <summary>
    ///
    /// </summary>
    public Space1 Yspace {
      get {
        return new Space1(this.DecimalGranularity) {
                                                       Min = this._min_.y,
                                                       Max = this._max_.y
                                                   };
      }
    }

    /// <summary>
    ///
    /// </summary>
    public Space1 Zspace {
      get {
        return new Space1(this.DecimalGranularity) {
                                                       Min = this._min_.z,
                                                       Max = this._max_.z
                                                   };
      }
    }

    /// <summary>
    ///
    /// </summary>
    public Space1 Wspace {
      get {
        return new Space1(this.DecimalGranularity) {
                                                       Min = this._min_.w,
                                                       Max = this._max_.w
                                                   };
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Vector4 ClipNormaliseRound(Vector4 v) {
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

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public float Round(float v) { return (float)Math.Round(v, this.DecimalGranularity); }

    /// <summary>
    ///
    /// </summary>
    public static Space4 ZeroOne {
      get { return new Space4(1) {_min_ = Vector4.zero, Max = Vector4.one}; }
    }

    /// <summary>
    ///
    /// </summary>
    public static Space4 TwentyEighty {
      get { return new Space4(1) {_min_ = Vector4.one * 0.2f, Max = Vector4.one * 0.8f}; }
    }

    /// <summary>
    ///
    /// </summary>
    public static Space4 MinusOneOne {
      get { return new Space4(2) {_min_ = -Vector4.one, Max = Vector4.one}; }
    }

    /// <summary>
    ///
    /// </summary>
    public dynamic Max { get { return this._max_; } set { this._max_ = value; } }

    /// <summary>
    ///
    /// </summary>
    public dynamic Min { get { return this._min_; } set { this._min_ = value; } }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Vector4 Denormalise01(Vector4 v) {  return v.Multiply(this.Span) + this._min_; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Vector4 Normalise01(Vector4 v)  { return (v - this._min_).Divide(this.Span); }
  }
}
