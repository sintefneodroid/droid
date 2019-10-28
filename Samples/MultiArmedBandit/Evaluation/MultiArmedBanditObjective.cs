using System;
using System.Linq;
using droid.Runtime.Prototyping.ObjectiveFunctions;
using droid.Samples.MultiArmedBandit.Actuators;
using droid.Samples.MultiArmedBandit.Displayers;
using UnityEngine;

namespace droid.Samples.MultiArmedBandit.Evaluation {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(EvaluationComponentMenuPath._ComponentMenuPath
                    + "MultiArmedBandit"
                    + EvaluationComponentMenuPath._Postfix)]
  public class MultiArmedBanditObjective : EpisodicObjective {
    [SerializeField] MultiArmedBanditActuator _arms;
    [SerializeField] float[] _normalised_values;

    [SerializeField] TextBarPlotDisplayer _text_bar_plot_displayer = null;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void RemotePostSetup() {
      if (this._arms == null) {
        this._arms = FindObjectOfType<MultiArmedBanditActuator>();
      }

      this.ComputeNormalisedValues();
    }

    void ComputeNormalisedValues() {
      /*var sum = this._arms.WinAmounts.Sum();
this._normalised_values = new Single[this._arms.WinAmounts.Length];
for (var i = 0; i < this._arms.WinAmounts.Length; i++) {
  this._normalised_values[i] = this._arms.WinAmounts[i] / sum;
}*/

      var values = this._arms.WinAmounts.Zip(this._arms.WinLikelihoods, (f, f1) => f * f1).ToArray();
      var values_sum = values.Sum();

      this._normalised_values = new Single[values.Length];
      for (var i = 0; i < values.Length; i++) {
        this._normalised_values[i] = values[i] / values_sum;
      }

      this._text_bar_plot_displayer.Display(this._normalised_values);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void InternalReset() { this.ComputeNormalisedValues(); }

    void Update() { this.ComputeNormalisedValues(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    /// <exception cref="T:System.NotImplementedException"></exception>
    public override Single InternalEvaluate() {
      if (this._arms.Won) {
        return this._arms.WinAmounts[this._arms.LastIndex];
      }

      return 0;
    }
  }
}
