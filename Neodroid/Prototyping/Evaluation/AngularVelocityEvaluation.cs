using UnityEngine;

namespace Neodroid.Prototyping.Evaluation {
  [AddComponentMenu(
      EvaluationComponentMenuPath._ComponentMenuPath
      + "AngularVelocity"
      + EvaluationComponentMenuPath._Postfix)]
  public class AngularVelocityEvaluation : ObjectiveFunction {
    [SerializeField] bool _penalty;
    [SerializeField] Rigidbody _rigidbody;

    public override float InternalEvaluate() {
      if (this._penalty) {
        if (this._rigidbody) {
          return -this._rigidbody.angularVelocity.magnitude;
        }
      }

      if (this._rigidbody) {
        return 1 / (this._rigidbody.angularVelocity.magnitude + 1);
      }

      return 0;
    }

    protected override void Setup() {
      if (this._rigidbody == null) {
        this._rigidbody = FindObjectOfType<Rigidbody>();
      }
    }
  }
}
