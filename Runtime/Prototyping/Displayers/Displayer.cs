using System;
using droid.Runtime.Environments;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities.GameObjects;
using droid.Runtime.Utilities.Misc;
using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Prototyping.Displayers {
  /// <inheritdoc cref="PrototypingGameObject" />
  /// <summary>
  /// </summary>
  public abstract class Displayer : PrototypingGameObject,
                                    IDisplayer {
    /// <summary>
    /// </summary>
    IPrototypingEnvironment _environment;

    #if UNITY_EDITOR
    /// <summary>
    /// </summary>
    protected bool _PlotRandomSeries;
    #endif

    [SerializeField] protected bool _RetainLastPlot = true;

    protected dynamic _Values;

    /// <summary>
    /// </summary>
    public IPrototypingEnvironment ParentEnvironment {
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
          NeodroidUtilities.RegisterComponent((PrototypingEnvironment)this.ParentEnvironment, this);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() { this.ParentEnvironment?.UnRegister(this); }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    void Update() {
      if (this._RetainLastPlot) {
        if (this._Values != null) {
          PlotSeries(this._Values);
        }
      }
    }

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
