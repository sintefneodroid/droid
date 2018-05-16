using Neodroid.Environments;
using Neodroid.Utilities;
using Neodroid.Utilities.GameObjects;
using Neodroid.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Prototyping.Displayers {
  /// <summary>
  ///
  /// </summary>
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

    public override string PrototypingType { get { return "Displayer"; } }

    protected override void RegisterComponent() {
      this.ParentEnvironment = Utilities.Unsorted.NeodroidUtilities.MaybeRegisterComponent(this.ParentEnvironment, this);
    }

    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment) {
        this.ParentEnvironment.UnRegister(this);
      }
    }

    public abstract void Display(float value);
    public abstract void Display(double value);
    public abstract void Display(float[] values);
    public abstract void Display(string value);
    public abstract void Display(Vector3 value);
    public abstract void Display(Vector3[] value);
    public abstract void Display(Utilities.Structs.Points.ValuePoint point);
    public abstract void Display(Utilities.Structs.Points.ValuePoint[] points);
    public abstract void Display(Utilities.Structs.Points.StringPoint point);
    public abstract void Display(Utilities.Structs.Points.StringPoint[] points);
  }
}
