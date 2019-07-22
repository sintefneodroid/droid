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
    Vector2 _Min_Values;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    Vector2 _Max_Values;

    [Range(0, 15)] [SerializeField] int _decimal_granularity;
    [SerializeField] bool normalised;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    DistributionSampler _distribution_sampler;

    public DistributionSampler DistributionSampler {
      get { return this._distribution_sampler; }
      set { this._distribution_sampler = value; }
    }

    public Space2(int decimal_granularity = 2) : this() {
      this._Min_Values = Vector2.one * -100f; //Vector2.negativeInfinity;
      this._Max_Values = Vector2.one * 100f; //Vector2.positiveInfinity;
      this._decimal_granularity = decimal_granularity;
      this._distribution_sampler = new DistributionSampler();
    }

    /// <summary>
    ///
    /// </summary>
    public Vector2 Span { get { return this._Max_Values - this._Min_Values; } }

    public Space1 Xspace {
      get {
        return new Space1(this.DecimalGranularity) {
                                                       MinValue = this._Min_Values.x,
                                                       MaxValue = this._Max_Values.x
                                                   };
      }
    }

    public Space1 Yspace {
      get {
        return new Space1(this.DecimalGranularity) {
                                                       MinValue = this._Min_Values.y,
                                                       MaxValue = this._Max_Values.y
                                                   };
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public dynamic Sample() {
      var x = this.DistributionSampler.Range(this._Min_Values.x, this._Max_Values.x);
      var y = this.DistributionSampler.Range(this._Min_Values.y, this._Max_Values.y);

      return new Vector3(x, y);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Vector2 ClipNormaliseRound(Vector2 v) {
      if (v.x > this._Max_Values.x) {
        v.x = this._Max_Values.x;
      } else if (v.x < this._Min_Values.x) {
        v.x = this._Min_Values.x;
      }

      if (this.Span.x > 0) {
        v.x = this.Round((v.x - this._Min_Values.x) / this.Span.x);
      } else {
        v.x = 0;
      }

      if (v.y > this._Max_Values.y) {
        v.y = this._Max_Values.y;
      } else if (v.y < this._Min_Values.y) {
        v.y = this._Min_Values.y;
      }

      if (this.Span.y > 0) {
        v.y = this.Round((v.y - this._Min_Values.y) / this.Span.y);
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
      get { return new Space2(1) {_Min_Values = Vector2.zero, MaxValues = Vector2.one}; }
    }

    public static Space2 TwentyEighty {
      get { return new Space2(1) {_Min_Values = Vector2.one * 0.2f, MaxValues = Vector2.one * 0.8f}; }
    }

    public bool Normalised { get { return this.normalised; } set { this.normalised = value; } }

    /// <summary>
    ///
    /// </summary>
    public Vector2 MinValues { get { return this._Min_Values; } set { this._Min_Values = value; } }

    /// <summary>
    ///
    /// </summary>
    public Vector2 MaxValues { get { return this._Max_Values; } set { this._Max_Values = value; } }


    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Vector2 Denormalise01(Vector2 v) { return v * this.Span + this._Min_Values; }



    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Vector2 Normalise01(Vector2 v) { return (v - this._Min_Values) / this.Span; }

  }
}
