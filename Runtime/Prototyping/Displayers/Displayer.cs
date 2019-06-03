using System;
using System.Linq;
using droid.Runtime.Environments;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities.GameObjects;
using droid.Runtime.Utilities.GameObjects.Plotting;
using droid.Runtime.Utilities.Misc;
using droid.Runtime.Utilities.Structs;
using UnityEditor;
using UnityEngine;

namespace droid.Runtime.Prototyping.Displayers {
  /// <inheritdoc cref="PrototypingGameObject" />
  /// <summary>
  /// </summary>
  public abstract class Displayer : PrototypingGameObject,
                                    IDisplayer {
    /// <summary>
    /// </summary>
    IActorisedPrototypingEnvironment _environment = null;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    protected bool _RetainLastPlot = true;

    /// <summary>
    ///
    /// </summary>
    protected dynamic _Values = null;

    [SerializeField] bool clean_all_children = true;
    [SerializeField] bool clean_before_every_plot = true;

    #if UNITY_EDITOR
    /// <summary>
    /// </summary>
    [Header("OnGizmo")]
    [SerializeField]
    bool _PlotRandomSeries = false;

    [SerializeField] bool always_random_sample_new = true;
    #endif

    /// <summary>
    /// </summary>
    public IActorisedPrototypingEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "Displayer"; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent((ActorisedPrototypingEnvironment)this.ParentEnvironment, this);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() { this.ParentEnvironment?.UnRegister(this); }

    /// <summary>
    /// </summary>
    void Update() {
      if (this._RetainLastPlot) {
        if (this.clean_before_every_plot) {
          this.Clean();
        }

        if (this._Values != null) {
          PlotSeries(this._Values);
        }
      }
    }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected() {
      if (this.enabled && Selection.activeGameObject == this.gameObject) {
        if (!this._PlotRandomSeries && !this._RetainLastPlot) {
          this.Clean();
        }

        if (this._Values == null || this._Values.Length == 0 || this.always_random_sample_new) {
          if (this._PlotRandomSeries) {
            this.Clean();
            var vs = PlotFunctions.SampleRandomSeries(9);
            this._Values = vs.Select(v => v._Val).ToArray();
            this.PlotSeries(vs);
          }
        }
      } else {
        this.Clean();
      }
    }

    #endif

    /// <summary>
    ///
    /// </summary>
    protected virtual void Clean() {
      if (this.clean_all_children) {
        foreach (Transform child in this.transform) {
          if (Application.isPlaying) {
            Destroy(child.gameObject);
          } else {
            DestroyImmediate(child.gameObject);
          }
        }
      }

      if (this._RetainLastPlot) {
        this._Values = null;
      }
    }

    void OnDestroy() { this.Clean(); }

    void OnDisable() { this.Clean(); }

    void OnEnable() { this.Clean(); }

    //void OnValidate() { this.Clean(); }

    public abstract void Display(Single value);
    public abstract void Display(Double value);
    public abstract void Display(Single[] values);
    public abstract void Display(String value);
    public abstract void Display(Vector3 value);
    public abstract void Display(Vector3[] value);
    public abstract void Display(Points.ValuePoint point);
    public abstract void Display(Points.ValuePoint[] points);
    public abstract void Display(Points.StringPoint point);
    public abstract void Display(Points.StringPoint[] points);
    public abstract void PlotSeries(Points.ValuePoint[] points);
  }
}
