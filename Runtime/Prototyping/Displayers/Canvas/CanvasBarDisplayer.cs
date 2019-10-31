using System;
using droid.Runtime.Structs;
using droid.Runtime.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace droid.Runtime.Prototyping.Displayers.Canvas {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  [RequireComponent(typeof(Image))]
  [AddComponentMenu(DisplayerComponentMenuPath._ComponentMenuPath
                    + "Canvas/CanvasBar"
                    + DisplayerComponentMenuPath._Postfix)]
  public class CanvasBarDisplayer : Displayer {
    Image _image;
    [SerializeField] [Range(0.0f, 1.0f)] float _value;

    /// <summary>
    /// </summary>
    public float Value {
      get { return this._value; }
      set {
        this._value = value;
        this.SetFillAmount(value);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() { this._image = this.GetComponent<Image>(); }

    /// <summary>
    /// </summary>
    /// <param name="amount"></param>
    public void SetFillAmount(float amount) {
      if (this._image) {
        this._image.fillAmount = amount;
      }
    }

    //public override void Display(Object o) { throw new NotImplementedException(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(float value) {
      #if NEODROID_DEBUG
      DebugPrinting.DisplayPrint(value, this.Identifier, this.Debugging);
      #endif

      this.SetFillAmount(value);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Double value) {
      #if NEODROID_DEBUG
      DebugPrinting.DisplayPrint(value, this.Identifier, this.Debugging);
      #endif

      this.SetFillAmount((float)value);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(float[] values) {
      #if NEODROID_DEBUG
      DebugPrinting.DisplayPrint(values[0], this.Identifier, this.Debugging);
      #endif

      this.SetFillAmount(values[0]);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(String value) { throw new NotImplementedException(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Vector3 value) { throw new NotImplementedException(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Vector3[] value) { throw new NotImplementedException(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Points.ValuePoint points) { throw new NotImplementedException(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Points.ValuePoint[] points) { throw new NotImplementedException(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Points.StringPoint point) { throw new NotImplementedException(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Points.StringPoint[] points) { throw new NotImplementedException(); }

    public override void PlotSeries(Points.ValuePoint[] points) { throw new NotImplementedException(); }
  }
}
