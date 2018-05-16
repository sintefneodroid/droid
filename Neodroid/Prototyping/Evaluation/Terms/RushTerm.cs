using System;
using Neodroid.Environments;
using UnityEngine;

namespace Neodroid.Prototyping.Evaluation.Terms {
  [AddComponentMenu(TermComponentMenuPath._ComponentMenuPath + "Rush" + TermComponentMenuPath._Postfix)]
  public class RushTerm : Term {
    [SerializeField] float _penalty_size = 0.01f;
    [SerializeField] PrototypingEnvironment _env;

    /// <summary>
    ///
    /// </summary>
    protected override void Setup() {
      if (!this._env) {
        this._env = FindObjectOfType<PrototypingEnvironment>();
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override float Evaluate() {
      if (this._env) {
        return -(1f / this._env.EpisodeLength);
      }

      return -this._penalty_size;
    }

    /// <summary>
    ///
    /// </summary>
    public override string PrototypingType { get { return "Rush"; } }
  }
}
