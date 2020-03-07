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
  [RequireComponent(requiredComponent : typeof(Text))]
  [AddComponentMenu(menuName : DisplayerComponentMenuPath._ComponentMenuPath
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
      DebugPrinting.DisplayPrint(value : value, identifier : this.Identifier, debugging : this.Debugging);
      #endif

      this.SetText(text : value.ToString(provider : CultureInfo.InvariantCulture));
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Double value) {
      #if NEODROID_DEBUG
      DebugPrinting.DisplayPrint(value : value, identifier : this.Identifier, debugging : this.Debugging);
      #endif

      this.SetText(text : value.ToString(provider : CultureInfo.InvariantCulture));
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(float[] values) {
      #if NEODROID_DEBUG
      DebugPrinting.DisplayPrint(value : values[0], identifier : this.Identifier, debugging : this.Debugging);
      #endif

      this.SetText(text : values[0].ToString(provider : CultureInfo.InvariantCulture));
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(String value) {
      #if NEODROID_DEBUG
      DebugPrinting.DisplayPrint(value : value, identifier : this.Identifier, debugging : this.Debugging);
      #endif

      this.SetText(text : value);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Vector3 value) {
      #if NEODROID_DEBUG
      DebugPrinting.DisplayPrint(value : value, identifier : this.Identifier, debugging : this.Debugging);
      #endif

      this.SetText(text : value.ToString());
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Vector3[] value) {
      #if NEODROID_DEBUG
      DebugPrinting.DisplayPrint(value : value, identifier : this.Identifier, debugging : this.Debugging);
      #endif

      this.SetText(text : value.ToString());
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Points.ValuePoint points) {
      #if NEODROID_DEBUG
      DebugPrinting.DisplayPrint(value : points, identifier : this.Identifier, debugging : this.Debugging);
      #endif

      this.SetText(text : points.ToString());
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Points.ValuePoint[] points) {
      #if NEODROID_DEBUG
      DebugPrinting.DisplayPrint(value : points, identifier : this.Identifier, debugging : this.Debugging);
      #endif

      this.SetText(text : points.ToString());
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Points.StringPoint point) {
      #if NEODROID_DEBUG
      DebugPrinting.DisplayPrint(value : point, identifier : this.Identifier, debugging : this.Debugging);
      #endif

      this.SetText(text : point.ToString());
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Points.StringPoint[] points) {
      #if NEODROID_DEBUG
      DebugPrinting.DisplayPrint(value : points, identifier : this.Identifier, debugging : this.Debugging);
      #endif

      this.SetText(text : points.ToString());
    }

    public override void PlotSeries(Points.ValuePoint[] points) {
      #if NEODROID_DEBUG
      DebugPrinting.DisplayPrint(value : points, identifier : this.Identifier, debugging : this.Debugging);
      #endif

      this.SetText(text : points.ToString());
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
