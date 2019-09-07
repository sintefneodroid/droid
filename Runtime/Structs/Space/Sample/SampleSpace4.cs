using System;
using droid.Runtime.Interfaces;
using droid.Runtime.Sampling;
using UnityEngine;

namespace droid.Runtime.Structs.Space.Sample {
  /// <summary>
  /// </summary>
  [Serializable]
  public struct SampleSpace4 : ISamplable {
    #region Fields

    [Header("Sampling", order = 103)]
    [SerializeField] internal Space4 _space;

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

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public dynamic Sample() {
      Single x;
      Single y;
      Single z;
      Single w;
      if (!this._space.normalised) {
        x = this.DistributionSampler.Range(this._space._min_.x, this._space._max_.x);
        y = this.DistributionSampler.Range(this._space._min_.y, this._space._max_.y);
        z = this.DistributionSampler.Range(this._space._min_.z, this._space._max_.z);
        w = this.DistributionSampler.Range(this._space._min_.w, this._space._max_.w);
      } else {
        x = this.DistributionSampler.Range(0, 1);
        y = this.DistributionSampler.Range(0, 1);
        z = this.DistributionSampler.Range(0, 1);
        w = this.DistributionSampler.Range(0, 1);
      }

      return new Vector4(x, y, z, w);
    }

    public ISpace Space { get { return this._space; } set { this._space = (Space4)value; } }
  }
}
