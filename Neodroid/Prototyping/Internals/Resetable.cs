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
  public abstract class Resetable : PrototypingGameObject,
                                    IResetable {
    /// <summary>
    ///
    /// </summary>
    public PrototypingEnvironment _Parent_Environment;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract void EnvironmentReset();

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    protected override void RegisterComponent() {
      this._Parent_Environment = NeodroidUtilities.MaybeRegisterComponent(this._Parent_Environment, this);
    }

    /// <summary>
    /// 
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this._Parent_Environment) {
        this._Parent_Environment.UnRegister(this);
      }
    }

    /// <summary>
    ///
    /// </summary>
    public abstract override String PrototypingTypeName { get; }
  }
}
