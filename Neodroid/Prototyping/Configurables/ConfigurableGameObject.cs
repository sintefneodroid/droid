using Neodroid.Environments;
using Neodroid.Utilities;
using Neodroid.Utilities.Interfaces;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Prototyping.Configurables {
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
    protected override void Setup() {
      this.InnerStart();
      this.UpdateCurrentConfiguration();
    }

    protected virtual void InnerStart() { }

    /// <summary>
    ///
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment = Utilities.Unsorted.NeodroidUtilities.MaybeRegisterComponent(this.ParentEnvironment, this);
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
    public override void ApplyConfiguration(Utilities.Messaging.Messages.Configuration configuration) { }

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
    public abstract Utilities.Messaging.Messages.Configuration SampleConfiguration(
        System.Random random_generator);
  }
}
