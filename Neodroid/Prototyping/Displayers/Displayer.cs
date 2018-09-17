using Neodroid.Environments;
using Neodroid.Utilities.GameObjects;
using Neodroid.Utilities.Structs;
using Neodroid.Utilities.Unsorted;
using UnityEngine;

namespace Neodroid.Prototyping.Displayers {
  /// <inheritdoc />
  ///  <summary>
  ///  </summary>
  public abstract class Displayer : PrototypingGameObject {
    /// <summary>
    ///
    /// </summary>
    PrototypingEnvironment _environment;

    /// <summary>
    ///
    /// </summary>
    public PrototypingEnvironment ParentEnvironment {
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
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterComponent(this.ParentEnvironment, this);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment) {
        this.ParentEnvironment.UnRegister(this);
      }
    }

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
  }
}
