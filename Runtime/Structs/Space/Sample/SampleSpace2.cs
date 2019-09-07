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
    #region Fields

    [Header("Sampling", order = 103)]
    [SerializeField]internal Space2 _space;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]internal DistributionSampler _distribution_sampler;

    #endregion

    /// <summary>
    ///
    /// </summary>
    public DistributionSampler DistributionSampler {
      get { return this._distribution_sampler; }
      set { this._distribution_sampler = value; }
    }

    public SampleSpace2(string unused = null) {
      this._space = Space2.ZeroOne;
      this._distribution_sampler = new DistributionSampler();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public dynamic Sample() {
      Single x;
      Single y;
      if (!this._space.normalised) {
        x = this.DistributionSampler.Range(this._space._min_.x, this._space._max_.x);
        y = this.DistributionSampler.Range(this._space._min_.y, this._space._max_.y);
      } else {
        x = this.DistributionSampler.Range(0, 1);
        y = this.DistributionSampler.Range(0, 1);
      }

      return new Vector2(x, y);
    }

    public ISpace Space { get { return this._space; } set { this._space = (Space2)value; } }
  }
}
