using System;
using Neodroid.Utilities;
using Neodroid.Utilities.GameObjects;
using Neodroid.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Prototyping.Evaluation.Terms {
  /// <summary>
  /// 
  /// </summary>
  [Serializable]
  public abstract class Term : PrototypingGameObject {
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public abstract float Evaluate();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract override String PrototypingType { get; }

    /// <summary>
    /// 
    /// </summary>
    ObjectiveFunction _objective_function;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this._objective_function = Utilities.Unsorted.NeodroidUtilities.MaybeRegisterComponent(this._objective_function, this);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this._objective_function) {
        this._objective_function.UnRegister(this);
      }
    }
  }
}
