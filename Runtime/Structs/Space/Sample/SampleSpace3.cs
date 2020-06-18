using System;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using droid.Runtime.Sampling;
using UnityEngine;

namespace droid.Runtime.Structs.Space.Sample {
  /// <inheritdoc />
  ///  <summary>
  ///  </summary>
  [Serializable]
  public struct SampleSpace3 : ISamplable {
    #region Fields

    [Header("Sampling", order = 103)]
    [SerializeField]
    internal Space3 _space;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    internal DistributionSampler _distribution_sampler;

    #endregion

    /// <summary>
    ///
    /// </summary>
    public DistributionSampler DistributionSampler {
      get { return this._distribution_sampler; }
      set { this._distribution_sampler = value; }
    }

    public SampleSpace3(string unused = null) {
      this._space = Space3.ZeroOne;
      this._distribution_sampler = new DistributionSampler();
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <returns></returns>
    public dynamic Sample() {
      float x;
      float y;
      float z;
      switch (this._space.Normalised) {
        case ProjectionEnum.None_:
          x = this._space.Round(this.DistributionSampler.Range(min : this._space.Min.x,
                                                               max : this._space.Max.x,
                                                               granularity : this._space.DecimalGranularity));
          y = this._space.Round(this.DistributionSampler.Range(min : this._space.Min.y,
                                                               max : this._space.Max.y,
                                                               granularity : this._space.DecimalGranularity));
          z = this._space.Round(this.DistributionSampler.Range(min : this._space.Min.z,
                                                               max : this._space.Max.z,
                                                               granularity : this._space.DecimalGranularity));
          break;
        case ProjectionEnum.Zero_one_:
          x = this.DistributionSampler.Range(0, 1);
          y = this.DistributionSampler.Range(0, 1);
          z = this.DistributionSampler.Range(0, 1);
          break;
        case ProjectionEnum.Minus_one_one_:
          x = this.DistributionSampler.Range(-1, 1);
          y = this.DistributionSampler.Range(-1, 1);
          z = this.DistributionSampler.Range(-1, 1);
          break;
        default: throw new ArgumentOutOfRangeException();
      }

      return new Vector3(x : x, y : y, z : z);
    }

    public ISpace Space { get { return this._space; } set { this._space = (Space3)value; } }
  }
}
