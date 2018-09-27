using Neodroid.Runtime.Environments;
using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Utilities.GameObjects;
using Neodroid.Runtime.Utilities.Misc.Drawing;
using Neodroid.Runtime.Utilities.Misc.Grasping;
using Neodroid.Runtime.Utilities.Structs;
using UnityEngine;
using NeodroidUtilities = Neodroid.Runtime.Utilities.Misc.NeodroidUtilities;

namespace Neodroid.Runtime.Prototyping.Displayers {
  /// <inheritdoc cref="PrototypingGameObject" />
  ///  <summary>
  ///  </summary>
  public abstract class Displayer : PrototypingGameObject,
                                    IDisplayer {
    /// <summary>
    ///
    /// </summary>
    IPrototypingEnvironment _environment;

    #if UNITY_EDITOR
    /// <summary>
    /// 
    /// </summary>
    protected bool _PlotRandomSeries;
    #endif

    [SerializeField] protected bool _RetainLastPlot = true;

    /// <summary>
    ///
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
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          this);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() { this.ParentEnvironment?.UnRegister(this); }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public abstract void Display(float value);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public abstract void Display(double value);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="values"></param>
    public abstract void Display(float[] values);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public abstract void Display(string value);

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

    public void Display(object o) { throw new System.NotImplementedException(); }

    protected dynamic _values;

    //public abstract void PlotSeries(dynamic points);
    public abstract void PlotSeries(Points.ValuePoint[] points);

    void Update() {
      if (this._RetainLastPlot) {
        if (this._values != null) {
          PlotSeries(this._values);
        }
      }
    }
  }
}
