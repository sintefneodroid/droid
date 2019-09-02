using System;
using droid.Runtime.Interfaces;
using droid.Runtime.Sampling;
using UnityEngine;

namespace droid.Runtime.Structs.Space {
  /// <summary>
  /// 
  /// </summary>
  [Serializable]
  public struct Space2 : ISpace {
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
    [SerializeField]
    internal Vector2 _min_;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    internal Vector2 _max_;

    [Range(0, 15)] [SerializeField] int _decimal_granularity;
    [SerializeField] bool normalised;


    public Space2(int decimal_granularity = 2) : this() {
      this._min_ = Vector2.one * -100f; //Vector2.negativeInfinity;
      this._max_ = Vector2.one * 100f; //Vector2.positiveInfinity;
      this._decimal_granularity = decimal_granularity;
    }

    /// <summary>
    ///
    /// </summary>
    public Vector2 Span { get { return this._max_ - this._min_; } }

    public Space1 Xspace {
      get {
        return new Space1(this.DecimalGranularity) {
                                                       Min = this._min_.x,
                                                       Max = this._max_.x
                                                   };
      }
    }

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
    /// <param name="v"></param>
    /// <returns></returns>
    public Vector2 ClipNormaliseRound(Vector2 v) {
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
    public static Space2 ZeroOne {
      get { return new Space2(1) {_min_ = Vector2.zero, Max = Vector2.one}; }
    }

    /// <summary>
    ///
    /// </summary>
    public static Space2 TwentyEighty {
      get { return new Space2(1) {_min_ = Vector2.one * 0.2f, Max = Vector2.one * 0.8f}; }
    }

    /// <summary>
    ///
    /// </summary>
    public bool Normalised { get { return this.normalised; } set { this.normalised = value; } }

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
    public Vector2 Denormalise01(Vector2 v) { return v * this.Span + this._min_; }



    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Vector2 Normalise01(Vector2 v) { return (v - this._min_) / this.Span; }

  }
}
