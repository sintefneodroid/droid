using droid.Neodroid.Environments;
using droid.Neodroid.Utilities.Messaging.Messages;
using droid.Neodroid.Utilities.Unsorted;
using UnityEngine;
using Random = System.Random;

namespace droid.Neodroid.Prototyping.Configurables {
  /// <summary>
  ///
  /// </summary>
  [AddComponentMenu(
      ConfigurableComponentMenuPath._ComponentMenuPath + "Vanilla" + ConfigurableComponentMenuPath._Postfix)]
  public abstract class ConfigurableGameObject : Configurable {
    /// <summary>
    ///
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
    ///
    /// </summary>
    public PrototypingEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    /// <summary>
    ///
    /// </summary>
    public override string PrototypingType { get { return "Configurable"; } }

    /// <summary>
    ///
    /// </summary>
    public virtual void UpdateCurrentConfiguration() { }

    /// <summary>
    ///
    /// </summary>
    protected sealed override void Setup() {
      this.PreSetup();
      this.UpdateCurrentConfiguration();
    }

    protected virtual void PreSetup() { }

    /// <summary>
    ///
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterComponent(this.ParentEnvironment, this);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment) {
        this.ParentEnvironment.UnRegister(this);
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="configuration"></param>
    public override void ApplyConfiguration(Configuration configuration) { }

    #region Fields

    /// <summary>
    ///
    /// </summary>
    [Header("References", order = 20)]
    [SerializeField]
    PrototypingEnvironment _environment;

    /// <summary>
    ///
    /// </summary>
    [Header("Configurable", order = 30)]
    [SerializeField]
    bool _relative_to_existing_value;

    #endregion

    /// <summary>
    ///
    /// </summary>
    /// <param name="random_generator"></param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public abstract Configuration SampleConfiguration(
        Random random_generator);
  }
}
