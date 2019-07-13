using System;
using droid.Runtime.Interfaces;
using droid.Runtime.Sampling;
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
    [SerializeField]
    int _Decimal_Granularity;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    float _Min_Value;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    float _Max_Value;

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

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public float Clip(float v) { return Mathf.Clamp(v, this._Min_Value, this._Max_Value); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public float ClipNormaliseRound(float v) { return this.Round(this.Normalise01(this.Clip(v))); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public float Normalise01(float v) {

      if (v > this._Max_Value || v < this._Min_Value) {
        throw new ArgumentException();
      }

      if (this.Span <= 0) {
        return 0;
      }



      return (v - this._Min_Value) / this.Span;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Vector2 ToVector2() { return new Vector2(this._Min_Value, this._Max_Value); }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Vector3 ToVector3() {
      return new Vector3(this._Min_Value, this._Max_Value, this._Decimal_Granularity);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public float Denormalise01(float v) {

      if (v > 1 || v < 0) {
        throw new ArgumentException();
      }

      if (this.Span <= 0) {
        return 0;
      }


      return v * this.Span + this._Min_Value;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public float ClipDenormaliseRoundClip(float v) {
      return this.Clip(this.Round(this.Denormalise01(Mathf.Clamp(v, -1, 1))));
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static string Vector3Description() { return "Space (min, max, granularity)"; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public float Round(float v) { return (float)Math.Round(v, this._Decimal_Granularity); }

    /// <summary>
    ///
    /// </summary>
    public static Space1 TwentyEighty { get { return new Space1(1) {_Min_Value = 0.2f, _Max_Value = 0.8f}; } }

    /// <summary>
    ///
    /// </summary>
    public static Space1 ZeroOne { get { return new Space1(1) {_Min_Value = 0, _Max_Value = 1}; } }

    /// <summary>
    ///
    /// </summary>
    public int DecimalGranularity {
      get { return this._Decimal_Granularity; }
      set { this._Decimal_Granularity = value; }
    }

    /// <summary>
    ///
    /// </summary>
    public Boolean Normalised { get { return this.normalised; } set { this.normalised = value; } }

    /// <summary>
    ///
    /// </summary>
    public static Space1 MinusOneOne { get { return new Space1(1) {_Min_Value = -1, _Max_Value = 1}; } }

    /// <summary>
    ///
    /// </summary>
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
