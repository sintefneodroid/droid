using System;
using droid.Runtime.Interfaces;
using droid.Runtime.Sampling;
using UnityEngine;

namespace droid.Runtime.Structs.Space.Sample {
  /// <summary>
  ///
  /// </summary>
  [Serializable]
  public struct SampleSpace3 :  ISamplable {
    [SerializeField]
    internal Space3 _space3;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    internal  DistributionSampler _distribution_sampler;

    /// <summary>
    ///
    /// </summary>
    public DistributionSampler DistributionSampler {
      get { return this._distribution_sampler; }
      set { this._distribution_sampler = value; }
    }

    public SampleSpace3(string unused = null){
      this._space3 = Space3.ZeroOne;
      this._distribution_sampler = new DistributionSampler();
    }



    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public dynamic Sample() {
      var x = this.DistributionSampler.Range(this._space3._min_.x, this._space3._max_.x);
      var y = this.DistributionSampler.Range(this._space3._min_.y, this._space3._max_.y);
      var z = this.DistributionSampler.Range(this._space3._min_.z, this._space3._max_.z);

      return new Vector3(x, y, z);
    }

    public ISpace Space {
      get { return this._space3; }
      set { this._space3 = (Space3) value; }
    }
  }
}
