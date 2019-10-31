using droid.Runtime.Prototyping.Actors;
using UnityEngine;

namespace droid.Runtime.Prototyping.ObjectiveFunctions.Spatial {
  /// <summary>
  ///
  /// </summary>
  [AddComponentMenu(EvaluationComponentMenuPath._ComponentMenuPath
                    + "EuclideanDistance"
                    + EvaluationComponentMenuPath._Postfix)]
  class EuclideanDistanceObjective : SpatialObjective {
    [SerializeField] Transform _g1;
    [SerializeField] Transform _g2;

    public override void InternalReset() { }

    public override float InternalEvaluate() {
      return -Vector3.Distance(this._g1.position, this._g2.position);
    }

    public override void RemotePostSetup() {
      if (this._g1 == null) {
        this._g1 = FindObjectOfType<Actor>().transform;
      }

      if (this._g2 == null) {
        this._g2 = this.transform;
      }
    }
  }
}
