﻿using UnityEngine;

namespace droid.Runtime.Prototyping.Evaluation.Spatial {
  /// <summary>
  ///
  /// </summary>
  [AddComponentMenu(EvaluationComponentMenuPath._ComponentMenuPath
                    + "AngularVelocity"
                    + EvaluationComponentMenuPath._Postfix)]
  public class AngularVelocityEvaluation : SpatialObjectionFunction {
    [SerializeField] bool _penalty = false;
    [SerializeField] Rigidbody _rigidbody = null;

    /// <summary>
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// </summary>
    public override void InternalReset() { }

    /// <summary>
    ///
    /// </summary>
    protected override void PostSetup() {
      if (this._rigidbody == null) {
        this._rigidbody = FindObjectOfType<Rigidbody>();
      }
    }
  }
}