using System;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using droid.Runtime.Sampling;
using UnityEngine;

namespace droid.Runtime.Structs.Space.Sample {
  /// <inheritdoc cref="ISpace" />
  ///  <summary>
  ///  </summary>
  [Serializable]
  public struct SampleSpace1 : ISamplable {
    #region Fields

    [Header("Sampling", order = 103)]
    [SerializeField]
    internal Space1 _space;

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

    public SampleSpace1(string unused = null) {
      this._space = Space1.ZeroOne;
      this._distribution_sampler = new DistributionSampler();
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <returns></returns>
    public dynamic Sample() {
      switch (this._space.Normalised) {
        case ProjectionEnum.None_:
          return this._space.Round(v : this.DistributionSampler.Range(min : this._space.Min,
                                                                      max : this._space.Max,
                                                                      granularity : this
                                                                                    ._space
                                                                                    .DecimalGranularity));
        case ProjectionEnum.Zero_one_:
          return this.DistributionSampler.Range(0, 1);
        case ProjectionEnum.Minus_one_one_:
          return this.DistributionSampler.Range(-1, 1);
        default: throw new ArgumentOutOfRangeException();
      }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public ISpace Space { get { return this._space; } set { this._space = (Space1)value; } }
  }
}
