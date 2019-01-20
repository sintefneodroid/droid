using System;
using droid.Runtime.Environments;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities.GameObjects;
using droid.Runtime.Utilities.Misc;
using UnityEngine;

namespace droid.Runtime.Prototyping.Internals {
  /// <inheritdoc cref="PrototypingGameObject" />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public abstract class Resetable : PrototypingGameObject,
                                    IResetable {
    /// <summary>
    /// </summary>
    public IPrototypingEnvironment _Parent_Environment;

    /// <summary>
    /// </summary>
    public abstract override String PrototypingTypeName { get; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract void EnvironmentReset();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this._Parent_Environment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this._Parent_Environment,
          this);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      this._Parent_Environment?.UnRegister(this);
    }
  }
}
