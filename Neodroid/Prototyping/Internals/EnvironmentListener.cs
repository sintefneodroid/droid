using System;
using droid.Neodroid.Environments;
using droid.Neodroid.Utilities.GameObjects;
using droid.Neodroid.Utilities.Unsorted;
using UnityEngine;

namespace droid.Neodroid.Prototyping.Internals {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public abstract class EnvironmentListener : PrototypingGameObject {
    /// <summary>
    ///
    /// </summary>
    public PrototypingEnvironment _Parent_Environment;

    /// <summary>
    /// 
    /// </summary>
    protected abstract void PreStep();

    /// <summary>
    /// 
    /// </summary>
    protected abstract void Step();

    /// <summary>
    /// 
    /// </summary>
    protected abstract void PostStep();

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    protected override void RegisterComponent() {
      this._Parent_Environment =
          NeodroidUtilities.MaybeRegisterComponent(this._Parent_Environment, this);

      if (this._Parent_Environment) {
        this._Parent_Environment.PreStepEvent += this.PreStep;
        this._Parent_Environment.StepEvent += this.Step;
        this._Parent_Environment.PostStepEvent += this.PostStep;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this._Parent_Environment) {
        this._Parent_Environment.UnRegister(this);
      }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public abstract override String PrototypingType { get; }
  }
}
