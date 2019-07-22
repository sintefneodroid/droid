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

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    DistributionSampler _distribution_sampler;

    [SerializeField] bool normalised;

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
    public bool Normalised { get { return this.normalised; } set { this.normalised = value; } }

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    Vector4 _Min_Values;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    Vector4 _Max_Values;

    /// <summary>
    ///
    /// </summary>
    [Range(0, 15)]
    [SerializeField]
    int _decimal_granularity;

    public Space4(int decimal_granularity = 2) : this() {
      this._decimal_granularity = decimal_granularity;
      this._Min_Values = Vector4.negativeInfinity;
      this._Max_Values = Vector4.positiveInfinity;
      this._distribution_sampler = new DistributionSampler();
    }

    /// <summary>
    ///
    /// </summary>
    public Vector4 Span { get { return this._Max_Values - this._Min_Values; } }

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
    public Space1 Wspace {
      get {
        return new Space1(this.DecimalGranularity) {
                                                       MinValue = this._Min_Values.w,
                                                       MaxValue = this._Max_Values.w
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
      var z = this.DistributionSampler.Range(this._Min_Values.z, this._Max_Values.z);
      var w = this.DistributionSampler.Range(this._Min_Values.w, this._Max_Values.w);

      return new Vector4(x, y, z, w);
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Quaternion RandomQuaternion() {
      var vector = this.Sample();
      return new Quaternion(vector.x, vector.y, vector.z, vector.w);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Vector4 ClipNormaliseRound(Vector4 v) {
      if (v.x > this._Max_Values.x) {
        v.x = this._Max_Values.x;
      } else if (v.x < this._Min_Values.x) {
        v = this._Min_Values;
      }

      if (this.Span.x > 0) {
        v.x = this.Round((v.x - this._Min_Values.x) / this.Span.x);
      } else {
        v.x = 0;
      }

      if (v.y > this._Max_Values.y) {
        v.y = this._Max_Values.y;
      } else if (v.y < this._Min_Values.y) {
        v = this._Min_Values;
      }

      if (this.Span.y > 0) {
        v.y = this.Round((v.y - this._Min_Values.y) / this.Span.y);
      } else {
        v.y = 0;
      }

      if (v.z > this._Max_Values.z) {
        v.z = this._Max_Values.z;
      } else if (v.z < this._Min_Values.z) {
        v = this._Min_Values;
      }

      if (this.Span.z > 0) {
        v.z = this.Round((v.z - this._Min_Values.z) / this.Span.z);
      } else {
        v.z = 0;
      }

      if (v.w > this._Max_Values.w) {
        v.w = this._Max_Values.w;
      } else if (v.w < this._Min_Values.w) {
        v = this._Min_Values;
      }

      if (this.Span.w > 0) {
        v.w = this.Round((v.w - this._Min_Values.w) / this.Span.w);
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
      get { return new Space4(1) {_Min_Values = Vector4.zero, MaxValues = Vector4.one}; }
    }

    /// <summary>
    ///
    /// </summary>
    public static Space4 TwentyEighty {
      get { return new Space4(1) {_Min_Values = Vector4.one * 0.2f, MaxValues = Vector4.one * 0.8f}; }
    }

    /// <summary>
    ///
    /// </summary>
    public static Space4 MinusOneOne {
      get { return new Space4(2) {_Min_Values = -Vector4.one, MaxValues = Vector4.one}; }
    }

    /// <summary>
    ///
    /// </summary>
    public Vector4 MaxValues { get { return this._Max_Values; } set { this._Max_Values = value; } }

    /// <summary>
    ///
    /// </summary>
    public Vector4 MinValues { get { return this._Min_Values; } set { this._Min_Values = value; } }

    public Vector4 Denormalise01(Vector4 v) {  return v.Multiply(this.Span) + this._Min_Values; }

    public Vector4 Normalise01(Vector4 v)  { return (v - this._Min_Values).Divide(this.Span); }
  }
}
