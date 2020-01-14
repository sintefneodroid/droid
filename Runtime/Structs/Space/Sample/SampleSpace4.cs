using System;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using droid.Runtime.Sampling;
using UnityEngine;

namespace droid.Runtime.Structs.Space.Sample {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [Serializable]
  public struct SampleSpace4 : ISamplable {
    #region Fields

    [Header("Sampling", order = 103)]
    [SerializeField]
    internal Space4 _space;

    [SerializeField] internal DistributionSampler _distribution_sampler;

    #endregion

    /// <summary>
    ///
    /// </summary>
    public DistributionSampler DistributionSampler {
      get { return this._distribution_sampler; }
      set { this._distribution_sampler = value; }
    }

    public SampleSpace4(string unused = null) {
      this._space = Space4.ZeroOne;
      this._distribution_sampler = new DistributionSampler();
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <returns></returns>
    public dynamic Sample() {
      Single x;
      Single y;
      Single z;
      Single w;
      switch(this._space.Normalised) {
        case NormalisationEnum.None_:
          x = this._space.Round(this.DistributionSampler.Range(min : this._space.Min.x, max : this._space.Max.x,granularity : this._space.DecimalGranularity));
          y = this._space.Round(this.DistributionSampler.Range(min : this._space.Min.y, max : this._space.Max.y,granularity : this._space.DecimalGranularity));
          z = this._space.Round(this.DistributionSampler.Range(min : this._space.Min.z, max : this._space.Max.z,granularity : this._space.DecimalGranularity));
          w = this._space.Round(this.DistributionSampler.Range(min : this._space.Min.w, max : this._space.Max.w,granularity : this._space.DecimalGranularity));
          break;
        case NormalisationEnum.Zero_one_:
          x = this.DistributionSampler.Range(0, 1);
          y = this.DistributionSampler.Range(0, 1);
          z = this.DistributionSampler.Range(0, 1);
          w = this.DistributionSampler.Range(0, 1);
          break;
        case NormalisationEnum.Minus_one_one_:
          x = this.DistributionSampler.Range(-1, 1);
          y = this.DistributionSampler.Range(-1, 1);
          z = this.DistributionSampler.Range(-1, 1);
          w = this.DistributionSampler.Range(-1, 1);
          break;
        default: throw new ArgumentOutOfRangeException();
      }

      return new Vector4(x : x,
                         y : y,
                         z : z,
                         w : w);
    }

    public ISpace Space { get { return this._space; } set { this._space = (Space4)value; } }
  }
}
