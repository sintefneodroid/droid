using System;
using droid.Runtime.Structs;
using UnityEngine;
using UnityEngine.UI;

namespace droid.Runtime.Prototyping.Displayers.Canvas {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  [AddComponentMenu(DisplayerComponentMenuPath._ComponentMenuPath
                    + "Canvas/CanvasBar"
                    + DisplayerComponentMenuPath._Postfix)]
  public class DistributionDisplayer : Displayer {
    [SerializeField] Image[] _images;
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
    public override void Setup() {
      if (this._images == null || this._images.Length == 0) {
        this._images = this.GetComponentsInChildren<Image>();
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="amount"></param>
    public void SetFillAmount(float amount) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"Setting amount to {amount}");
      }
      #endif

      if (this._images[0]) {
        this._images[0].fillAmount = amount;
      }
    }

    //public override void Display(Object o) { throw new NotImplementedException(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(float value) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Applying " + value + " To " + this.name);
      }
      #endif

      this.Value = value;

      this.SetFillAmount(value);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Double value) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Applying " + value + " To " + this.name);
      }
      #endif

      this.Value = (float)value;

      this.SetFillAmount((float)value);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(float[] values) { throw new NotImplementedException(); }

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
