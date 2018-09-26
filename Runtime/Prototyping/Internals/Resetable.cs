using System;
using Neodroid.Runtime.Environments;
using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Utilities.GameObjects;
using Neodroid.Runtime.Utilities.Misc.Drawing;
using Neodroid.Runtime.Utilities.Misc.Grasping;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Internals {
  /// <inheritdoc cref="PrototypingGameObject" />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public abstract class Resetable : PrototypingGameObject,
                                    IResetable {
    /// <summary>
    ///
    /// </summary>
    public IPrototypingEnvironment _Parent_Environment;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract void EnvironmentReset();

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    protected override void RegisterComponent() {
      this._Parent_Environment = NeodroidUtilities.MaybeRegisterComponent(
          (PrototypingEnvironment)this._Parent_Environment,
          this);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this._Parent_Environment != null) {
        this._Parent_Environment.UnRegister(this);
      }
    }

    /// <summary>
    ///
    /// </summary>
    public abstract override String PrototypingTypeName { get; }
  }
}