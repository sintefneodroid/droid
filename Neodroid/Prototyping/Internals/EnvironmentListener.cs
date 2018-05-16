using UnityEngine;

namespace Neodroid.Prototyping.Internals {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public abstract class EnvironmentListener : Utilities.GameObjects.PrototypingGameObject {
    /// <summary>
    ///
    /// </summary>
    public Environments.PrototypingEnvironment _Parent_Environment;

    protected abstract void PreStep();

    protected abstract void Step();

    protected abstract void PostStep();

    /// <summary>
    ///
    /// </summary>
    protected override void RegisterComponent() {
      this._Parent_Environment =
          Utilities.Unsorted.NeodroidUtilities.MaybeRegisterComponent(this._Parent_Environment, this);

      if (this._Parent_Environment) {
        this._Parent_Environment.PreStepEvent += this.PreStep;
        this._Parent_Environment.StepEvent += this.Step;
        this._Parent_Environment.PostStepEvent += this.PostStep;
      }
    }

    protected override void UnRegisterComponent() {
      if (this._Parent_Environment) {
        this._Parent_Environment.UnRegister(this);
      }
    }

    /// <summary>
    ///
    /// </summary>
    public abstract override System.String PrototypingType { get; }
  }
}
