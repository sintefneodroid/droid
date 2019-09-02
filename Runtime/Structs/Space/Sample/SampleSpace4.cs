using System;
using droid.Runtime.Interfaces;
using droid.Runtime.Sampling;
using UnityEngine;

namespace droid.Runtime.Structs.Space.Sample {
  /// <summary>
  /// </summary>
  [Serializable]
  public struct SampleSpace4 :  ISamplable {


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



    [SerializeField]
    internal  Space4 _space;

    public SampleSpace4(string unused = null){
      this._space = Space4.ZeroOne;
      this._distribution_sampler = new DistributionSampler();
    }


    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public dynamic Sample() {
      var x = this.DistributionSampler.Range(this._space._min_.x, this._space._max_.x);
      var y = this.DistributionSampler.Range(this._space._min_.y, this._space._max_.y);
      var z = this.DistributionSampler.Range(this._space._min_.z, this._space._max_.z);
      var w = this.DistributionSampler.Range(this._space._min_.w, this._space._max_.w);

      return new Vector4(x, y, z, w);
    }

    public ISpace Space {
      get { return this._space; }
      set { this._space = (Space4) value; }
    }



  }
}
