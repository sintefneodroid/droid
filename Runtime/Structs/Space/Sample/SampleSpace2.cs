using System;
using droid.Runtime.Interfaces;
using droid.Runtime.Sampling;
using UnityEngine;

namespace droid.Runtime.Structs.Space.Sample {
  /// <summary>
  /// 
  /// </summary>
  [Serializable]
  public struct SampleSpace2 : ISamplable {
    [SerializeField]
    internal Space2 _space2;

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

    public SampleSpace2(string unused = null) {
      this._space2 = Space2.ZeroOne;
      this._distribution_sampler = new DistributionSampler();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public dynamic Sample() {
      var x = this.DistributionSampler.Range(this._space2._min_.x, this._space2._max_.x);
      var y = this.DistributionSampler.Range(this._space2._min_.y, this._space2._max_.y);

      return new Vector3(x, y);
    }

    public ISpace Space {
      get { return this._space2; }
      set { this._space2 = (Space2) value; }
    }
  }
}
