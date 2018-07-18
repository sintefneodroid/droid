using System;
using droid.Neodroid.Prototyping.Displayers;
using droid.Neodroid.Prototyping.Displayers.Canvas;
using droid.Neodroid.Prototyping.Internals;
using droid.Neodroid.Utilities.Structs;
using UnityEngine;

namespace SceneAssets.Sanity.NoHorison.MultiArmedBandit {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      DisplayerComponentMenuPath._ComponentMenuPath + "TextBarPlot" + DisplayerComponentMenuPath._Postfix)]
  public class TextBarPlotDisplayer : Resetable {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override String PrototypingTypeName { get { return "TextBarPlot"; } }

    [SerializeField] CanvasBarDisplayer[] _canvas_bars;
    [SerializeField] CanvasTextDisplayer[] _canvas_text;
    [SerializeField] float[] _values;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Reset() {
      foreach (var bar in this._canvas_bars) {
        bar.Display(0.5);
      }
    }

    void Update() {
      if (this.Debugging) {
        this.Display(this._values);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="values"></param>
    public void Display(float[] values) {
      for (var i = 0; i < this._canvas_bars.Length; i++) {
        if (i < values.Length) {
          var bar = this._canvas_bars[i];
          if (bar) {
            bar.Display(values[i]);
          }

          var text = this._canvas_text[i];
          if (text) {
            text.Display(values[i]);
          }
        }
      }
    }
  }
}
