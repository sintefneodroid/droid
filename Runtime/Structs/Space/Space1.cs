using System;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities.Sampling;
using UnityEngine;

namespace droid.Runtime.Structs.Space {
  /// <inheritdoc cref="ISpace" />
  ///  <summary>
  ///  </summary>
  [Serializable]
  public struct Space1 : ISpace {
    /// <summary>
    ///
    /// </summary>
    ///
    [Range(0, 15)]
    [SerializeField]int _Decimal_Granularity;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]float _Min_Value;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]float _Max_Value;

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

    public Space1(int decimal_granularity = 2) : this() {
      this._Decimal_Granularity = decimal_granularity;
      this._Min_Value = -1f; //float.NegativeInfinity;
      this._Max_Value = 1f; //float.PositiveInfinity;

      this._distribution_sampler = new DistributionSampler();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public dynamic Sample() {
      var x = this.DistributionSampler.Range(this._Min_Value, this._Max_Value);

      return x;
    }

    /// <summary>
    ///
    /// </summary>
    public float Span { get { return this._Max_Value - this._Min_Value; } }

    public float ClipNormaliseRound(float v) {
      if (v > this._Max_Value) {
        v = this._Max_Value;
      } else if (v < this._Min_Value) {
        v = this._Min_Value;
      }

      return this.Round((v - this._Min_Value) / this.Span);
    }

    public Vector2 ToVector2() { return new Vector2(this._Min_Value, this._Max_Value); }

    public Vector3 ToVector3() {
      return new Vector3(this._Min_Value, this._Max_Value, this._Decimal_Granularity);
    }

    public string Vector3Description() { return "Space (min, max, granularity)"; }

    public float Round(float v) { return (float)Math.Round(v, this._Decimal_Granularity); }

    public static Space1 TwentyEighty { get { return new Space1(1) {_Min_Value = 0.2f, _Max_Value = 0.8f}; } }

    public static Space1 ZeroOne { get { return new Space1(1) {_Min_Value = 0, _Max_Value = 1}; } }
    public int DecimalGranularity {
      get { return this._Decimal_Granularity; }
      set { this._Decimal_Granularity = value; }
    }

    /// <summary>
    ///
    /// </summary>
    public Boolean Normalised { get { return this.normalised; } set { this.normalised = value; } }

    public static Space1 MinusOneOne { get { return new Space1(1) {_Min_Value = -1, _Max_Value = 1}; } }
    public static Space1 DiscreteZeroOne { get { return new Space1(0) {_Min_Value = 0, _Max_Value = 1}; } }

    /// <summary>
    ///
    /// </summary>
    public Single MinValue { get { return this._Min_Value; } set { this._Min_Value = value; } }

    /// <summary>
    ///
    /// </summary>
    public Single MaxValue { get { return this._Max_Value; } set { this._Max_Value = value; } }

    /// <summary>
    ///
    /// </summary>
    /// <param name="vector3_field"></param>
    public void FromVector3(Vector3 vector3_field) {
      this._Decimal_Granularity = (int)vector3_field.z;
      this._Max_Value = vector3_field.y;
      this._Min_Value = vector3_field.x;
    }
  }
}
