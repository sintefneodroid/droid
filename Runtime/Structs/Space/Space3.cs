using System;
using droid.Runtime.Interfaces;
using droid.Runtime.Sampling;
using droid.Runtime.Utilities.Extensions;
using UnityEngine;

namespace droid.Runtime.Structs.Space {
  /// <summary>
  ///
  /// </summary>
  [Serializable]
  public struct Space3 : ISpace {

    /// <summary>
    ///
    /// </summary>
    public Int32 DecimalGranularity {
      get { return this._decimal_granularity; }
      set { this._decimal_granularity = value; }
    }

    [Range(0, 15)] [SerializeField] int _decimal_granularity;
    [SerializeField] bool normalised;
    /// <summary>
    ///
    /// </summary>
    public Boolean Normalised { get { return this.normalised; } set { this.normalised = value; } }

    [SerializeField] internal Vector3 _min_;
    [SerializeField] internal Vector3 _max_;

    public Space3(int decimal_granularity = 1) : this() {
      this._decimal_granularity = decimal_granularity;
      this._min_ = Vector3.one * -100f;
      this._max_ = Vector3.one * 100f; //Vector3.positiveInfinity;
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
    public Vector3 ClipNormaliseRound(Vector3 v) {
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
    /// <returns></returns>
    public static Space3 ZeroOne {
      get {
        return new Space3 {_min_ = Vector3.zero, Max = Vector3.one};
      }
    }

    /// <summary>
    ///
    /// </summary>
    public static Space3 TwentyEighty {
      get {
        return new Space3 {
                                                         Min = Vector3.one * 0.2f,
                                                         _max_ = Vector3.one * 0.8f
                                                     };
      }
    }

    /// <summary>
    ///
    /// </summary>
    public static Space3 MinusOneOne {
      get {
        return new Space3 {_min_ = -Vector3.one, Max = Vector3.one};
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
    public Vector3 Denormalise01(Vector3 v) {  return v.Multiply(this.Span) + this._min_; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Vector3 Normalise01(Vector3 v)  { return (v - this._min_).Divide(this.Span); }
  }
}
