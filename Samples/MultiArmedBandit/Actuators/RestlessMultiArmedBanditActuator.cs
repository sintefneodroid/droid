using System;
using System.Collections;
using droid.Runtime.Prototyping.Actuators;
using UnityEngine;
using Random = UnityEngine.Random;

namespace droid.Samples.MultiArmedBandit.Actuators {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ActuatorComponentMenuPath._ComponentMenuPath
                    + "RestlessMultiArmedBandit"
                    + ActuatorComponentMenuPath._Postfix)]
  public class RestlessMultiArmedBanditActuator : MultiArmedBanditActuator {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() {
      var mvs = this.MotionSpace;
      mvs.Min = 0;
      mvs.Max = this._Indicators.Length - 1;
      this.MotionSpace = mvs;

      this.ReAssignValues();
      this.StartCoroutine(this.ExecuteAfterTime(2));
    }

    protected void ReAssignValues() {
      for (var index = 0; index < this._Win_Likelihoods.Length; index++) {
        this._Win_Likelihoods[index] = Random.Range(0.1f, 0.9f);
        this._Win_Amounts[index] = Random.Range(0.1f, 0.9f);
      }

      if (this._Win_Likelihoods == null || this._Win_Likelihoods.Length == 0) {
        this._Win_Likelihoods = new Single[this._Indicators.Length];
        for (var index = 0; index < this._Indicators.Length; index++) {
          this._Win_Likelihoods[index] = 0.5f;
        }
      }

      if (this._Win_Amounts == null || this._Win_Amounts.Length == 0) {
        this._Win_Amounts = new Single[this._Indicators.Length];
        for (var index = 0; index < this._Indicators.Length; index++) {
          this._Win_Amounts[index] = 10f;
        }
      }
    }

    IEnumerator ExecuteAfterTime(float time) {
      while (true) {
        yield return new WaitForSeconds(time);

        this.ReAssignValues();
      }
    }
  }
}
