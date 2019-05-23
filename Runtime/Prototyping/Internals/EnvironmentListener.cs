using System;
using droid.Runtime.Environments;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities.GameObjects;
using droid.Runtime.Utilities.Misc;

namespace droid.Runtime.Prototyping.Internals {
  /// <inheritdoc cref="PrototypingGameObject" />
  /// <summary>
  /// </summary>
  public abstract class EnvironmentListener : PrototypingGameObject,
                                    IEnvironmentListener {
    /// <summary>
    /// </summary>
    public IActorisedPrototypingEnvironment _Parent_Environment;

    /// <summary>
    /// </summary>
    public abstract override String PrototypingTypeName { get; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract void EnvironmentReset();

    public virtual void PreStep() {  }
    public virtual void Step() { }
    public virtual void PostStep() { }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this._Parent_Environment =
          NeodroidUtilities.RegisterComponent((ActorisedPrototypingEnvironment)this._Parent_Environment, this);

      if (this._Parent_Environment != null) {
        this._Parent_Environment.PreStepEvent += this.PreStep;
        this._Parent_Environment.StepEvent += this.Step;
        this._Parent_Environment.PostStepEvent += this.PostStep;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() { this._Parent_Environment?.UnRegister(this); }
  }
}
