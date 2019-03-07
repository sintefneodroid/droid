using droid.Runtime.Environments;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Utilities.GameObjects;
using droid.Runtime.Utilities.Misc;
using droid.Runtime.Utilities.Structs;
using UnityEngine;
using Object = System.Object;


namespace droid.Runtime.Prototyping.Configurables {
  /// <inheritdoc cref="PrototypingGameObject" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "Vanilla"
                    + ConfigurableComponentMenuPath._Postfix)]
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
    public virtual void UpdateCurrentConfiguration() { }

    public abstract void ApplyConfiguration(IConfigurableConfiguration configuration);

    /// <summary>
    /// </summary>
    public void EnvironmentReset() { }

    public virtual void PostEnvironmentSetup() { }

    /// <summary>
    /// </summary>

    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public virtual IConfigurableConfiguration SampleConfiguration() {
      return new Configuration(this.Identifier, Space1.ZeroOne.Sample());
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
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent((PrototypingEnvironment)this.ParentEnvironment, this);
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void Update() {
      if (this.SampleRandom && Application.isPlaying) {
        this.ApplyConfiguration(this.SampleConfiguration());
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
    IPrototypingEnvironment _environment = null;

    /// <summary>
    /// </summary>
    [Header("Configurable", order = 30)]
    [SerializeField]
    bool _relative_to_existing_value = false;

    [SerializeField] bool SampleRandom = false;
    [SerializeField] UnityEngine.Random random_generator;

    #endregion
  }
}
