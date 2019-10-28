using System;
using System.Linq;
using droid.Runtime.Environments.Prototyping;
using droid.Runtime.GameObjects;
using droid.Runtime.GameObjects.Plotting;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs;
using droid.Runtime.Utilities;
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
    AbstractSpatialPrototypingEnvironment _environment = null;

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
    public AbstractSpatialPrototypingEnvironment ParentEnvironment {
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
      this.ParentEnvironment = NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment, this);
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

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    public abstract void Display(Single value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    public abstract void Display(Double value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="values"></param>
    public abstract void Display(Single[] values);

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    public abstract void Display(String value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    public abstract void Display(Vector3 value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    public abstract void Display(Vector3[] value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="point"></param>
    public abstract void Display(Points.ValuePoint point);

    /// <summary>
    ///
    /// </summary>
    /// <param name="points"></param>
    public abstract void Display(Points.ValuePoint[] points);

    /// <summary>
    ///
    /// </summary>
    /// <param name="point"></param>
    public abstract void Display(Points.StringPoint point);

    /// <summary>
    ///
    /// </summary>
    /// <param name="points"></param>
    public abstract void Display(Points.StringPoint[] points);

    /// <summary>
    ///
    /// </summary>
    /// <param name="points"></param>
    public abstract void PlotSeries(Points.ValuePoint[] points);
  }
}
