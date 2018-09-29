using System;
using Neodroid.Runtime.Environments;
using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Utilities.GameObjects;
using Neodroid.Runtime.Utilities.Misc;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Internals {
  /// <inheritdoc cref="PrototypingGameObject" />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public abstract class EnvironmentListener : PrototypingGameObject,
                                              IEnvironmentListener {
    /// <summary>
    /// </summary>
    public IPrototypingEnvironment _Parent_Environment;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
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
    /// </summary>
    public virtual void EnvironmentReset() { }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this._Parent_Environment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this._Parent_Environment,
          this);

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
