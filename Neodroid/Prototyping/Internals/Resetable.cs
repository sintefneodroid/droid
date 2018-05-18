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
  public abstract class Resetable : PrototypingGameObject {
    /// <summary>
    ///
    /// </summary>
    public PrototypingEnvironment _Parent_Environment;

    /// <summary>
    ///
    /// </summary>
    public abstract void Reset();

    /// <summary>
    ///
    /// </summary>
    protected override void RegisterComponent() {
      this._Parent_Environment = NeodroidUtilities.MaybeRegisterComponent(this._Parent_Environment, this);
    }

    protected override void UnRegisterComponent() {
      if (this._Parent_Environment) {
        this._Parent_Environment.UnRegister(this);
      }
    }

    /// <summary>
    ///
    /// </summary>
    public abstract override String PrototypingType { get; }
  }
}
