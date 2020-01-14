using System;
using droid.Runtime.Prototyping.Actors;
using UnityEngine;
using Object = System.Object;

namespace droid.Runtime.Prototyping.ObjectiveFunctions.Spatial {
  /// <inheritdoc />
  ///  <summary>
  ///  </summary>
  [AddComponentMenu(EvaluationComponentMenuPath._ComponentMenuPath
                    + "EuclideanDistance"
                    + EvaluationComponentMenuPath._Postfix)]
  class EuclideanDistanceObjective : SpatialObjective {
    [SerializeField] Transform _g1;
    [SerializeField] Transform _g2;
    [SerializeField] float margin = 0.01f;
    [SerializeField] bool inverse = false;
    public override void InternalReset() { }

    public override float InternalEvaluate() {
      var signal = 0.0f;
      var distance = Vector3.Distance(a : this._g1.position, b : this._g2.position);

      if (distance <= this.margin) {
        this.ParentEnvironment.Terminate("Within margin");
        signal += this.SolvedSignal;
      } else {
        if (this.inverse) {
          signal += distance;
        } else {
          signal -= distance;
        }
      }

      return signal;
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
