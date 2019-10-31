using System;
using System.Globalization;
using droid.Runtime.Structs;
using droid.Runtime.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace droid.Runtime.Prototyping.Displayers.Canvas {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  [RequireComponent(typeof(Text))]
  [AddComponentMenu(DisplayerComponentMenuPath._ComponentMenuPath
                    + "Canvas/CanvasText"
                    + DisplayerComponentMenuPath._Postfix)]
  public class CanvasTextDisplayer : Displayer {
    Text _text_component;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() { this._text_component = this.GetComponent<Text>(); }

    //public override void Display(Object o) { throw new NotImplementedException(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(float value) {
      #if NEODROID_DEBUG
      DebugPrinting.DisplayPrint(value, this.Identifier, this.Debugging);
      #endif

      this.SetText(value.ToString(CultureInfo.InvariantCulture));
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Double value) {
      #if NEODROID_DEBUG
      DebugPrinting.DisplayPrint(value, this.Identifier, this.Debugging);
      #endif

      this.SetText(value.ToString(CultureInfo.InvariantCulture));
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(float[] values) {
      #if NEODROID_DEBUG
      DebugPrinting.DisplayPrint(values[0], this.Identifier, this.Debugging);
      #endif

      this.SetText(values[0].ToString());
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(String value) {
      #if NEODROID_DEBUG
      DebugPrinting.DisplayPrint(value, this.Identifier, this.Debugging);
      #endif

      this.SetText(value);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Vector3 value) {
      #if NEODROID_DEBUG
      DebugPrinting.DisplayPrint(value, this.Identifier, this.Debugging);
      #endif

      this.SetText(value.ToString());
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Vector3[] value) {
      #if NEODROID_DEBUG
      DebugPrinting.DisplayPrint(value, this.Identifier, this.Debugging);
      #endif

      this.SetText(value.ToString());
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Points.ValuePoint points) {
      #if NEODROID_DEBUG
      DebugPrinting.DisplayPrint(points, this.Identifier, this.Debugging);
      #endif

      this.SetText(points.ToString());
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Points.ValuePoint[] points) {
      #if NEODROID_DEBUG
      DebugPrinting.DisplayPrint(points, this.Identifier, this.Debugging);
      #endif

      this.SetText(points.ToString());
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Points.StringPoint point) {
      #if NEODROID_DEBUG
      DebugPrinting.DisplayPrint(point, this.Identifier, this.Debugging);
      #endif

      this.SetText(point.ToString());
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Points.StringPoint[] points) {
      #if NEODROID_DEBUG
      DebugPrinting.DisplayPrint(points, this.Identifier, this.Debugging);
      #endif

      this.SetText(points.ToString());
    }

    public override void PlotSeries(Points.ValuePoint[] points) {
      #if NEODROID_DEBUG
      DebugPrinting.DisplayPrint(points, this.Identifier, this.Debugging);
      #endif

      this.SetText(points.ToString());
    }

    /// <summary>
    /// </summary>
    /// <param name="text"></param>
    public void SetText(string text) {
      if (this._text_component) {
        this._text_component.text = text;
      }
    }
  }
}
