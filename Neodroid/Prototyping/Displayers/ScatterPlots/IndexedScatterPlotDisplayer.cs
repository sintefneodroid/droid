using System;
using System.Collections.Generic;
using System.Linq;
using Neodroid.Utilities.Plotting;
using Neodroid.Utilities.Structs;
using Neodroid.Utilities.Unsorted;
using UnityEngine;

namespace Neodroid.Prototyping.Displayers.ScatterPlots {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode,
   AddComponentMenu(
       DisplayerComponentMenuPath._ComponentMenuPath
       + "IndexedScatterPlot"
       + DisplayerComponentMenuPath._Postfix)]
  public class IndexedScatterPlotDisplayer : Displayer {
    [SerializeField] float[] _values;
    [SerializeField] bool _retain_last_plot = true;
    [SerializeField] bool _plot_random_series;
    dynamic _vals;

    [SerializeField] GameObject[] _designs;
    [SerializeField] List<GameObject> _instances;

    /// <summary>
    /// 
    /// </summary>
    protected override void Setup() { }

    public override void Display(Double value) { }

    public override void Display(float[] values) { }

    public override void Display(String values) { }

    public override void Display(Vector3 value) { throw new NotImplementedException(); }
    public override void Display(Vector3[] value) { this.ScatterPlot(value); }

    public override void Display(Points.ValuePoint points) { this.PlotSeries(new[] {points}); }

    public override void Display(Points.ValuePoint[] points) { }

    public override void Display(Points.StringPoint point) { throw new NotImplementedException(); }
    public override void Display(Points.StringPoint[] points) { throw new NotImplementedException(); }

    public override void Display(float values) { }

    void Update() {
      if (this._retain_last_plot) {
        if (this._vals != null) {
          PlotSeries(this._vals);
        }
      }
    }

    void SpawnDesign(GameObject design, Vector3 position, Quaternion rotation = default(Quaternion)) {
      //var go = Instantiate(design, position, rotation,this.transform);
      var go = Instantiate(design, position, design.transform.rotation, this.transform);
      this._instances.Add(go);
    }

    void DestroyInstances(bool immediately = false) {
      if (!immediately) {
        this._instances.ForEach(Destroy);
      } else {
        this._instances.ForEach(DestroyImmediate);
      }

      this._instances.Clear();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="points"></param>
    public void ScatterPlot(Vector3[] points) { }

    /*public void PlotSeries(float[] points) {

    }*/

    #if UNITY_EDITOR
    void OnDrawGizmos() {
      if (this.enabled) {
        if (this._values == null || this._values.Length == 0) {
          if (this._plot_random_series) {
            var vs = PlotFunctions.SampleRandomSeries(9);
            this._values = vs.Select(v => v._Val).ToArray();
            this.PlotSeries(vs);
          }
        }
      }
    }
    #endif

    /// <summary>
    ///
    /// </summary>
    /// <param name="points"></param>
    public void PlotSeries(Points.ValuePoint[] points) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Plotting value points");
      }
      #endif

      this._vals = points;
      this.DestroyInstances(true);

      foreach (var point in points) {
        var game_objects = this._designs;
        if (game_objects != null && point._Val >= game_objects.Length) {
          continue;
        }

        this.SpawnDesign(this._designs[(int)(point._Val)], point._Pos);
      }
    }

    void OnDestroy() { this.DestroyInstances(true); }

    void OnDisable() { this.DestroyInstances(true); }

    void OnEnable() { this.DestroyInstances(true); }

    void OnValidate() { this.DestroyInstances(); }
  }
}
