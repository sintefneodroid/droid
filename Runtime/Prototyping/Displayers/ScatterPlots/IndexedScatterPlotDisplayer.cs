using System;
using System.Collections.Generic;
using droid.Runtime.Structs;
using UnityEngine;

namespace droid.Runtime.Prototyping.Displayers.ScatterPlots {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  [AddComponentMenu(DisplayerComponentMenuPath._ComponentMenuPath
                    + "IndexedScatterPlot"
                    + DisplayerComponentMenuPath._Postfix)]
  public class IndexedScatterPlotDisplayer : Displayer {
    [SerializeField] GameObject[] _designs = null;
    [SerializeField] List<GameObject> _instances = null;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() { }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    public override void Display(Double value) { }

    public override void Display(float[] values) { }

    public override void Display(String values) { }

    public override void Display(Vector3 value) { throw new NotImplementedException(); }
    public override void Display(Vector3[] value) { this.ScatterPlot(points : value); }

    public override void Display(Points.ValuePoint points) { this.PlotSeries(new[] {points}); }

    public override void Display(Points.ValuePoint[] points) { }

    public override void Display(Points.StringPoint point) { throw new NotImplementedException(); }
    public override void Display(Points.StringPoint[] points) { throw new NotImplementedException(); }

    //public override void Display(Object o) { throw new NotImplementedException(); }
    public override void Display(float values) { }

    void Update() {
      if (this._RetainLastPlot) {
        if (this._Values != null) {
          PlotSeries(points : this._Values);
        }
      }
    }

    void SpawnDesign(GameObject design, Vector3 position, Quaternion rotation) {
      //var go = Instantiate(design, position, rotation,this.transform);
      var go = Instantiate(original : design,
                           position : position,
                           rotation : design.transform.rotation,
                           parent : this.transform);
      this._instances.Add(item : go);
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <param name="immediately"></param>
    protected override void Clean() {
      if (Application.isPlaying) {
        for (var index = 0; index < this._instances.Count; index++) {
          var game_object = this._instances[index : index];
          Destroy(obj : game_object);
        }
      } else {
        for (var index = 0; index < this._instances.Count; index++) {
          var game_object = this._instances[index : index];
          DestroyImmediate(obj : game_object);
        }
      }

      this._instances.Clear();

      base.Clean();
    }

    /// <summary>
    /// </summary>
    /// <param name="points"></param>
    public void ScatterPlot(Vector3[] points) { }

    /*public override void PlotSeries(float[] points) {

    }*/

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
      this.Clean();

      for (var index = 0; index < points.Length; index++) {
        var point = points[index];
        var game_objects = this._designs;
        if (game_objects != null && point._Val >= game_objects.Length) {
          continue;
        }

        this.SpawnDesign(this._designs[(int)point._Val], position : point._Pos, rotation : Quaternion.identity);
      }
    }
  }
}
