using droid.Neodroid.Environments;
using droid.Neodroid.Utilities.GameObjects;
using droid.Neodroid.Utilities.Structs;
using droid.Neodroid.Utilities.Unsorted;
using UnityEngine;

namespace droid.Neodroid.Prototyping.Displayers {
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
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterComponent(this.ParentEnvironment, this);
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
    public abstract void Display(Points.ValuePoint point);
    public abstract void Display(Points.ValuePoint[] points);
    public abstract void Display(Points.StringPoint point);
    public abstract void Display(Points.StringPoint[] points);
  }
}
