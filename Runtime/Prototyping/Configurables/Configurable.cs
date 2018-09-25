using Neodroid.Runtime.Environments;
using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Messaging.Messages;
using Neodroid.Runtime.Utilities.GameObjects;
using Neodroid.Runtime.Utilities.Unsorted;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Configurables {
  /// <inheritdoc cref="PrototypingGameObject" />
  ///  <summary>
  ///  </summary>
  [AddComponentMenu(
      ConfigurableComponentMenuPath._ComponentMenuPath + "Vanilla" + ConfigurableComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  public abstract class Configurable : PrototypingGameObject,
                                       IConfigurable {
    /// <summary>
    ///
    /// </summary>
    public bool RelativeToExistingValue {
      get { return this._relative_to_existing_value; }
    }

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
    public IPrototypingEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    /// <summary>
    ///
    /// </summary>
    public override string PrototypingTypeName {
      get { return "Configurable"; }
    }

    /// <summary>
    ///
    /// </summary>
    public virtual void UpdateCurrentConfiguration() { }

    public abstract void ApplyConfiguration(IConfigurableConfiguration configuration);

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    protected sealed override void Setup() {
      this.PreSetup();
      this.UpdateCurrentConfiguration();
    }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void PreSetup() { }

    /// <summary>
    /// 
    /// </summary>
    public void EnvironmentReset() { }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          this);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() { this.ParentEnvironment?.UnRegister(this); }

    #region Fields

    /// <summary>
    ///
    /// </summary>
    [Header("References", order = 20), SerializeField]
    IPrototypingEnvironment _environment;

    /// <summary>
    ///
    /// </summary>
    [Header("Configurable", order = 30), SerializeField]
    bool _relative_to_existing_value;

    #endregion

    /// <summary>
    ///
    /// </summary>
    /// <param name="random_generator"></param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public virtual IConfigurableConfiguration SampleConfiguration(System.Random random_generator) {
      return new Configuration(this.Identifier,random_generator.Next());
    }
  }
}