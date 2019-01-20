using System;
using System.Collections.Generic;
using System.Linq;
using droid.Runtime.Utilities.Plotting;
using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Prototyping.Displayers.ScatterPlots {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  [AddComponentMenu(
      DisplayerComponentMenuPath._ComponentMenuPath
      + "IndexedScatterPlot"
      + DisplayerComponentMenuPath._Postfix)]
  public class IndexedScatterPlotDisplayer : Displayer {
    [SerializeField] GameObject[] _designs;
    [SerializeField] List<GameObject> _instances;


    /// <summary>
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

    //public override void Display(Object o) { throw new NotImplementedException(); }
    public override void Display(float values) { }

    void Update() {
      if (this._RetainLastPlot) {
        if (this._Values != null) {
          PlotSeries(this._Values);
        }
      }
    }

    void SpawnDesign(GameObject design, Vector3 position, Quaternion rotation = default) {
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
    /// </summary>
    /// <param name="points"></param>
    public void ScatterPlot(Vector3[] points) { }

    /*public override void PlotSeries(float[] points) {

    }*/

    #if UNITY_EDITOR
    void OnDrawGizmos() {
      if (this.enabled) {
        if (this._Values == null || this._Values.Length == 0) {
          if (this._PlotRandomSeries) {
            var vs = PlotFunctions.SampleRandomSeries(9);
            this._Values = vs.Select(v => v._Val).ToArray();
            this.PlotSeries(vs);
          }
        }
      }
    }
    #endif

    /// <summary>
    /// </summary>
    /// <param name="points"></param>
    public override void PlotSeries(Points.ValuePoint[] points) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Plotting value points");
      }
      #endif

      this._Values = points;
      this.DestroyInstances(true);

      foreach (var point in points) {
        var game_objects = this._designs;
        if (game_objects != null && point._Val >= game_objects.Length) {
          continue;
        }

        this.SpawnDesign(this._designs[(int)point._Val], point._Pos);
      }
    }

    void OnDestroy() { this.DestroyInstances(true); }

    void OnDisable() { this.DestroyInstances(true); }

    void OnEnable() { this.DestroyInstances(true); }

    void OnValidate() { this.DestroyInstances(); }
  }
}
