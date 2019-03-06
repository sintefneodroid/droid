using UnityEngine;

namespace droid.Runtime.Prototyping.Evaluation.Terms {
  [AddComponentMenu(
      TermComponentMenuPath._ComponentMenuPath + "CollisionPunishment" + TermComponentMenuPath._Postfix)]
  public class CollisionsPunishmentTerm : Term {
    [SerializeField] Collider _a= null;

    [SerializeField] Collider _b=null;

    public override string PrototypingTypeName { get { return "CollisionPunishment"; } }

    public override float Evaluate() {
      if (this._a.bounds.Intersects(this._b.bounds)) {
        return -1;
      }

      return 0;
    }
  }
}
