using System;
using System.Linq;
using UnityEngine;

namespace Neodroid.Prototyping.Evaluation.Terms {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      TermComponentMenuPath._ComponentMenuPath + "CollidersPunishment" + TermComponentMenuPath._Postfix)]
  public class CollidersPunishmentTerm : Term {
    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    Collider[] _avoid_collders;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    Collider _subject_collider;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override float Evaluate() {
      if (this._avoid_collders.Any(a => a.bounds.Intersects(this._subject_collider.bounds))) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("Intersecting bounds");
        }
        #endif

        return -1;
      }

      return 0;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingType { get { return "CollidersPunishment"; } }
  }
}
