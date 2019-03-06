using droid.Runtime.Environments;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Utilities.GameObjects;
using droid.Runtime.Utilities.Misc;
using UnityEngine;
using Random = System.Random;

namespace droid.Runtime.Prototyping.Configurables {
  /// <inheritdoc cref="PrototypingGameObject" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      ConfigurableComponentMenuPath._ComponentMenuPath + "Vanilla" + ConfigurableComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  public abstract class Configurable : PrototypingGameObject,
                                       IConfigurable {
    /// <summary>
    /// </summary>
    public bool RelativeToExistingValue { get { return this._relative_to_existing_value; } }

    /*
    /// <summary>
    ///
    /// </summary>
    public ValueSpace ConfigurableValueSpace {
      get { return this._configurable_value_space; }
      set { this._configurable_value_space = value; }
    }*/

    /// <summary>
    /// </summary>
    public IPrototypingEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "Configurable"; } }

    /// <summary>
    /// </summary>
    public virtual void UpdateCurrentConfiguration() { }

    public abstract void ApplyConfiguration(IConfigurableConfiguration configuration);

    /// <summary>
    /// </summary>
    public void EnvironmentReset() { }

    public virtual void PostEnvironmentSetup(){}

    /// <summary>
    /// </summary>
    /// <param name="random_generator"></param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public virtual IConfigurableConfiguration SampleConfiguration(Random random_generator) {
      return new Configuration(this.Identifier, random_generator.Next());
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected sealed override void Setup() {
      this.PreSetup();
      this.UpdateCurrentConfiguration();
    }

    /// <summary>
    /// </summary>
    protected virtual void PreSetup() { }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          this);
    }

    protected virtual void Update() {
      if (this.SampleRandom && Application.isPlaying) {
        var random_generator = new Random();
        this.ApplyConfiguration(this.SampleConfiguration(random_generator));
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() { this.ParentEnvironment?.UnRegister(this); }

    #region Fields

    /// <summary>
    /// </summary>
    [Header("References", order = 20)]
    [SerializeField]
    IPrototypingEnvironment _environment=null;

    /// <summary>
    /// </summary>
    [Header("Configurable", order = 30)]
    [SerializeField]
    bool _relative_to_existing_value = false;

    [SerializeField] bool SampleRandom=false;

    #endregion
  }
}
