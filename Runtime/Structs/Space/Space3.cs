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
    [SerializeField]
    DistributionSampler _distribution_sampler;

    /// <summary>
    ///
    /// </summary>
    public DistributionSampler DistributionSampler {
      get { return this._distribution_sampler; }
      set { this._distribution_sampler = value; }
    }

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

    [SerializeField] Vector3 _Min_Values;
    [SerializeField] Vector3 _Max_Values;

    public Space3(DistributionSampler ds, int decimal_granularity = 1) : this() {
      this._decimal_granularity = decimal_granularity;
      this._Min_Values = Vector3.one * -100f;
      this._Max_Values = Vector3.one * 100f; //Vector3.positiveInfinity;
      this._distribution_sampler = new DistributionSampler();
    }

    /// <summary>
    ///
    /// </summary>
    public Vector3 Span { get { return this._Max_Values - this._Min_Values; } }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public dynamic Sample() {
      var x = this.DistributionSampler.Range(this._Min_Values.x, this._Max_Values.x);
      var y = this.DistributionSampler.Range(this._Min_Values.y, this._Max_Values.y);
      var z = this.DistributionSampler.Range(this._Min_Values.z, this._Max_Values.z);

      return new Vector3(x, y, z);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Vector3 ClipNormaliseRound(Vector3 v) {
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

      if (v.z > this._Max_Values.z) {
        v.z = this._Max_Values.z;
      } else if (v.z < this._Min_Values.z) {
        v.z = this._Min_Values.z;
      }

      if (this.Span.z > 0) {
        v.z = this.Round((v.z - this._Min_Values.z) / this.Span.z);
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
                                                       MinValue = this._Min_Values.x,
                                                       MaxValue = this._Max_Values.x
                                                   };
      }
    }

    /// <summary>
    ///
    /// </summary>
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
    public Space1 Zspace {
      get {
        return new Space1(this.DecimalGranularity) {
                                                       MinValue = this._Min_Values.z,
                                                       MaxValue = this._Max_Values.z
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
      b._Min_Values += c;
      b._Max_Values += c;
      return b;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static Space3 operator-(Space3 b, Vector3 c) {
      b._Min_Values -= c;
      b._Max_Values -= c;
      return b;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static Space3 operator-(Vector3 c, Space3 b) {
      b._Min_Values -= c;
      b._Max_Values -= c;
      return b;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="c"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Space3 operator+(Vector3 c, Space3 b) {
      b._Min_Values += c;
      b._Max_Values += c;
      return b;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static Space3 ZeroOne {
      get {
        return new Space3(new DistributionSampler()) {_Min_Values = Vector3.zero, MaxValues = Vector3.one};
      }
    }

    /// <summary>
    ///
    /// </summary>
    public static Space3 TwentyEighty {
      get {
        return new Space3(new DistributionSampler()) {
                                                         MinValues = Vector3.one * 0.2f,
                                                         _Max_Values = Vector3.one * 0.8f
                                                     };
      }
    }

    /// <summary>
    ///
    /// </summary>
    public static Space3 MinusOneOne {
      get {
        return new Space3(new DistributionSampler()) {_Min_Values = -Vector3.one, MaxValues = Vector3.one};
      }
    }

    /// <summary>
    ///
    /// </summary>
    public Vector3 MinValues { get { return this._Min_Values; } set { this._Min_Values = value; } }

    /// <summary>
    ///
    /// </summary>
    public Vector3 MaxValues { get { return this._Max_Values; } set { this._Max_Values = value; } }
    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Vector3 Denormalise01(Vector3 v) {  return v.Multiply(this.Span) + this._Min_Values; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Vector3 Normalise01(Vector3 v)  { return (v - this._Min_Values).Divide(this.Span); }
  }
}
