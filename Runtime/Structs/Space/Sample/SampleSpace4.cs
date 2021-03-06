﻿using System;
using droid.Runtime.Enums;
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

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public dynamic Sample() {
      Single x;
      Single y;
      Single z;
      Single w;
      switch(this._space.Normalised) {
        case Normalisation.None_:
          x = this._space.Round(this.DistributionSampler.Range(this._space.Min.x, this._space.Max.x));
          y = this._space.Round(this.DistributionSampler.Range(this._space.Min.y, this._space.Max.y));
          z = this._space.Round(this.DistributionSampler.Range(this._space.Min.z, this._space.Max.z));
          w = this._space.Round(this.DistributionSampler.Range(this._space.Min.w, this._space.Max.w));
          break;
        case Normalisation.Zero_one_:
          x = this.DistributionSampler.Range(0, 1);
          y = this.DistributionSampler.Range(0, 1);
          z = this.DistributionSampler.Range(0, 1);
          w = this.DistributionSampler.Range(0, 1);
          break;
        case Normalisation.Minus_one_one_:
          x = this.DistributionSampler.Range(-1, 1);
          y = this.DistributionSampler.Range(-1, 1);
          z = this.DistributionSampler.Range(-1, 1);
          w = this.DistributionSampler.Range(-1, 1);
          break;
        default: throw new ArgumentOutOfRangeException();
      }

      return new Vector4(x,
                         y,
                         z,
                         w);
    }

    public ISpace Space { get { return this._space; } set { this._space = (Space4)value; } }
  }
}
