using System;
using System.Linq;
using droid.Runtime.GameObjects.Plotting;
using droid.Runtime.Structs;
using droid.Runtime.Utilities.Drawing;
using UnityEngine;

namespace droid.Runtime.Prototyping.Displayers.Cells {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  [AddComponentMenu(menuName : DisplayerComponentMenuPath._ComponentMenuPath
                               + "VectorField"
                               + DisplayerComponentMenuPath._Postfix)]
  public class HexCellDisplayer : QuadCellDisplayer {
    [SerializeField] bool _plot_random_series = false;
    [SerializeField] bool _retain_last_plot = true;

    public bool PlotRandomSeries {
      get { return this._plot_random_series; }
      set { this._plot_random_series = value; }
    }

    /// <summary>
    /// </summary>
    public override void Setup() { }

    public override void Display(double value) { }

    public override void Display(float[] values) { }

    public override void Display(string values) { }

    public override void Display(Vector3 value) { throw new NotImplementedException(); }
    public override void Display(Vector3[] value) { this.ScatterPlot(points : value); }

    public override void Display(Points.ValuePoint points) { this.PlotSeries(points : new[] {points}); }

    public override void Display(Points.ValuePoint[] points) { }

    public override void Display(Points.StringPoint point) { throw new NotImplementedException(); }
    public override void Display(Points.StringPoint[] points) { throw new NotImplementedException(); }

    public override void Display(float values) { }

    void Update() {
      if (this._retain_last_plot) {
        if (this._Values != null) {
          PlotSeries(points : this._Values);
        }
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="points"></param>
    public new void ScatterPlot(Vector3[] points) { }

    /*public override void PlotSeries(float[] points) {

    }*/

    #if UNITY_EDITOR
    void OnDrawGizmos() {
      if (this.enabled) {
        if (this._Values == null || this._Values.Length == 0) {
          if (this._plot_random_series) {
            var vs = PlotFunctions.SampleRandomSeries(9);
            this._Values = vs.Select(v => v._Val).ToArray();
            this.PlotSeries(points : vs);
          }
        }
      }
    }
    #endif

    /// <summary>
    /// </summary>
    /// <param name="points"></param>
    public new void PlotSeries(Points.ValuePoint[] points) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Plotting value points");
      }
      #endif

      this._Values = points;

      for (var index = 0; index < points.Length; index++) {
        var point = points[index];
//point._Size
        switch ((int)point._Val) {
          case 0:
            NeodroidDrawingUtilities.ForDebug(pos : point._Pos,
                                              direction : Vector3.forward,
                                              color : Color.cyan);
            break;
          case 1:
            NeodroidDrawingUtilities.ForDebug(pos : point._Pos, direction : Vector3.back, color : Color.cyan);
            break;
          case 2:
            NeodroidDrawingUtilities.ForDebug(pos : point._Pos, direction : Vector3.up, color : Color.cyan);
            break;
          case 3:
            NeodroidDrawingUtilities.ForDebug(pos : point._Pos, direction : Vector3.down, color : Color.cyan);
            break;
          case 4:
            NeodroidDrawingUtilities.ForDebug(pos : point._Pos, direction : Vector3.left, color : Color.cyan);
            break;
          case 5:
            NeodroidDrawingUtilities.ForDebug(pos : point._Pos,
                                              direction : Vector3.right,
                                              color : Color.cyan);
            break;
        }
      }
    }
  }
}
