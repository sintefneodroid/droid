using System;
using droid.Runtime.Prototyping.Displayers;
using droid.Runtime.Prototyping.Displayers.Canvas;
using droid.Runtime.Prototyping.Unobservables;
using UnityEngine;

namespace droid.Samples.MultiArmedBandit.Displayers {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(DisplayerComponentMenuPath._ComponentMenuPath
                    + "TextBarPlot"
                    + DisplayerComponentMenuPath._Postfix)]
  public class TextBarPlotDisplayer : Unobservable {
    [SerializeField] CanvasBarDisplayer[] _canvas_bars = { };
    [SerializeField] CanvasTextDisplayer[] _canvas_text = { };

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override String PrototypingTypeName { get { return "TextBarPlot"; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PrototypingReset() {
      var canvas_bar_displayers = this._canvas_bars;
      if (canvas_bar_displayers != null) {
        foreach (var bar in canvas_bar_displayers) {
          bar.Display(0.5);
        }
      }
    }

    void Update() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        //this.Display(this._values);
      }
      #endif
    }

    /// <summary>
    /// </summary>
    /// <param name="values"></param>
    public void Display(float[] values) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"Displaying {values} at {this.Identifier}");
      }
      #endif

      var canvas_bar_displayers = this._canvas_bars;
      var canvas_text_displayers = this._canvas_text;

      if (canvas_bar_displayers != null) {
        for (var i = 0; i < canvas_bar_displayers.Length; i++) {
          if (i < values.Length) {
            var bar = canvas_bar_displayers[i];
            bar?.Display(values[i]);

            if (canvas_text_displayers != null) {
              var text = canvas_text_displayers[i];
              text?.Display(values[i]);
            }
          }
        }
      }
    }
  }
}
