using System;
using JetBrains.Annotations;
using Neodroid.Runtime.Prototyping.Displayers;
using Neodroid.Runtime.Prototyping.Displayers.Canvas;
using Neodroid.Runtime.Prototyping.Internals;
using UnityEngine;

namespace Neodroid.Samples.MultiArmedBandit {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      DisplayerComponentMenuPath._ComponentMenuPath + "TextBarPlot" + DisplayerComponentMenuPath._Postfix)]
  public class TextBarPlotDisplayer : Resetable {
    [CanBeNull] [SerializeField] CanvasBarDisplayer[] _canvas_bars;
    [CanBeNull] [SerializeField] CanvasTextDisplayer[] _canvas_text;
    [CanBeNull] [SerializeField] float[] _Values;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override String PrototypingTypeName { get { return "TextBarPlot"; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void EnvironmentReset() {
      foreach (var bar in this._canvas_bars) {
        bar.Display(0.5);
      }
    }

    void Update() {
      if (this.Debugging) {
        this.Display(this._Values);
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="values"></param>
    public void Display(float[] values) {
      for (var i = 0; i < this._canvas_bars.Length; i++) {
        if (i < values.Length) {
          var bar = this._canvas_bars[i];
          bar?.Display(values[i]);

          var text = this._canvas_text[i];
          text?.Display(values[i]);
        }
      }
    }
  }
}
