using System;
using Neodroid.Environments;
using Neodroid.Utilities.GameObjects;
using Neodroid.Utilities.Interfaces;
using Neodroid.Utilities.Unsorted;
using UnityEngine;

namespace Neodroid.Prototyping.Internals {
  /// <inheritdoc cref="PrototypingGameObject" />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public abstract class EnvironmentListener : PrototypingGameObject,
                                              IEnvironmentListener {
    /// <summary>
    ///
    /// </summary>
    public PrototypingEnvironment _Parent_Environment;

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    protected override void RegisterComponent() {
      this._Parent_Environment = NeodroidUtilities.MaybeRegisterComponent(this._Parent_Environment, this);

      if (this._Parent_Environment) {
        this._Parent_Environment.PreStepEvent += this.PreStep;
        this._Parent_Environment.StepEvent += this.Step;
        this._Parent_Environment.PostStepEvent += this.PostStep;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this._Parent_Environment) {
        this._Parent_Environment.UnRegister(this);
      }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public abstract override String PrototypingTypeName { get; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public virtual void PreStep() { }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public virtual void Step() { }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public virtual void PostStep() { }

    /// <summary>
    /// 
    /// </summary>
    public virtual void EnvironmentReset() { }
  }
}
