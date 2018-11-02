using Neodroid.Runtime.Interfaces;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Evaluation.Terms {
  [AddComponentMenu(TermComponentMenuPath._ComponentMenuPath + "Rush" + TermComponentMenuPath._Postfix)]
  public class RushTerm : Term {
    [SerializeField] IPrototypingEnvironment _env;
    [SerializeField] float _penalty_size = 0.01f;

    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "Rush"; } }

    /// <summary>
    /// </summary>
    protected override void Setup() {
      if (this._env == null) {
        //this._env = FindObjectOfType<PrototypingEnvironment>();
      }
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override float Evaluate() {
      if (this._env != null) {
        return -(1f / this._env.EpisodeLength);
      }

      return -this._penalty_size;
    }
  }
}