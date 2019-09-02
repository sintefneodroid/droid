using System;
using droid.Runtime.Interfaces;
using droid.Runtime.Sampling;
using UnityEngine;

namespace droid.Runtime.Structs.Space.Sample {
  /// <inheritdoc cref="ISpace" />
  ///  <summary>
  ///  </summary>
  [Serializable]
  public struct SampleSpace1 : ISamplable {
    [SerializeField]
    internal
    Space1 _space1;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    internal DistributionSampler _distribution_sampler;

    /// <summary>
    ///
    /// </summary>
    public DistributionSampler DistributionSampler {
      get { return this._distribution_sampler; }
      set { this._distribution_sampler = value; }
    }

    public SampleSpace1(string unused = null){
      this._space1 = Space1.ZeroOne;
      this._distribution_sampler = new DistributionSampler();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public dynamic Sample() {
      if (!this._space1.normalised) {
        var x = this.DistributionSampler.Range(this._space1._min_, this._space1._max_);
        return x;
      } else
      {
        var x = this.DistributionSampler.Range(0, 1);
        return x;
      }
    }

    public ISpace Space {
      get { return this._space1; }
      set { this._space1 = (Space1) value; }
    }
  }
}
