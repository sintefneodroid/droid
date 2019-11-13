using droid.Runtime.Environments.Prototyping;
using droid.Runtime.GameObjects;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities;

namespace droid.Runtime.Prototyping.EnvironmentListener {
  /// <inheritdoc cref="PrototypingGameObject" />
  /// <summary>
  /// </summary>
  public abstract class EnvironmentListener : PrototypingGameObject,
                                       IUnobservable {
    /// <summary>
    /// </summary>
    public AbstractSpatialPrototypingEnvironment _Parent_Environment;


    /// <summary>
    ///
    /// </summary>
    public virtual void PreStep() { }

    /// <summary>
    ///
    /// </summary>
    public virtual void Step() { }

    /// <summary>
    ///
    /// </summary>
    public virtual void PostStep() { }

    public virtual void PreTick() {  }
    public virtual void PostTick() {  }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this._Parent_Environment =
          NeodroidRegistrationUtilities.RegisterComponent(this._Parent_Environment, this);

      if (this._Parent_Environment != null) {
        this._Parent_Environment.PreTickEvent += this.PreTick;
        this._Parent_Environment.PreStepEvent += this.PreStep;
        this._Parent_Environment.StepEvent += this.Step;
        this._Parent_Environment.PostStepEvent += this.PostStep;
        this._Parent_Environment.PostTickEvent += this.PostTick;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() { this._Parent_Environment?.UnRegister(this); }
  }
}
