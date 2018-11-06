using System;
using JetBrains.Annotations;
using Neodroid.Runtime.Prototyping.Displayers;
using Neodroid.Runtime.Prototyping.Displayers.Canvas;
using Neodroid.Runtime.Prototyping.Internals;
using UnityEngine;
using Object = System.Object;

namespace Neodroid.Samples.MultiArmedBandit {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      DisplayerComponentMenuPath._ComponentMenuPath + "TextBarPlot" + DisplayerComponentMenuPath._Postfix)]
  public class TextBarPlotDisplayer : Resetable {
    [CanBeNull] [SerializeField] CanvasBarDisplayer[] _canvas_bars;
    [CanBeNull] [SerializeField] CanvasTextDisplayer[] _canvas_text;
    [SerializeField] Single[] _values;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override String PrototypingTypeName { get { return "TextBarPlot"; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void EnvironmentReset() {
      var canvas_bar_displayers = this._canvas_bars;
      if (canvas_bar_displayers != null) {
        foreach (var bar in canvas_bar_displayers) {
          bar.Display(0.5);
        }
      }
    }

    void Update() {
      if (this.Debugging) {
        //this.Display(this._values);
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="values"></param>
    public void Display(float[] values) {
      #if NEODROID_DEBUG
        if(this.Debugging) {
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
