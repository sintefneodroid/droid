using System;
using System.Linq;
using droid.Neodroid.Prototyping.Evaluation;
using droid.Neodroid.Prototyping.Motors;
using UnityEngine;

namespace SceneAssets.Sanity.NoHorison.MultiArmedBandit {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      EvaluationComponentMenuPath._ComponentMenuPath + "PoseDeviance" + EvaluationComponentMenuPath._Postfix)]
  public class MultiArmedBanditEvaluation : ObjectiveFunction {
    //[SerializeField] BanditArmMotor[] _arms;
    //[SerializeField] float[] _values;
    [SerializeField] float[] _normalised_values;
    [SerializeField] MultiArmedBanditMotor _arms;

    [SerializeField] TextBarPlotDisplayer _text_bar_plot_displayer;

    /// <summary>
    /// 
    /// </summary>
    protected override void PostSetup() {
      if (this._arms == null) {
        this._arms = FindObjectOfType<MultiArmedBanditMotor>();
      }

      var sum = this._arms.Values.Sum();
      this._normalised_values = new Single[this._arms.Values.Length];
      for (var i = 0; i < this._arms.Values.Length; i++) {
        this._normalised_values[i] = this._arms.Values[i] / sum;
      }

      this._text_bar_plot_displayer.Display(this._normalised_values);
    }

    /// <summary>
    /// 
    /// </summary>
    public override void InternalReset() { this._text_bar_plot_displayer.Display(this._normalised_values); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    /// <exception cref="T:System.NotImplementedException"></exception>
    public override Single InternalEvaluate() { return this._arms.Values[this._arms.LastIndex]; }
  }
}
