using System;
using droid.Runtime.Enums;
using droid.Runtime.Environments;
using droid.Runtime.GameObjects;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Utilities;
using UnityEngine;
using NeodroidUtilities = droid.Runtime.Utilities.Extensions.NeodroidUtilities;
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

    /// <summary>
    ///
    /// </summary>
    public abstract ISpace ConfigurableValueSpace { get; }

    /// <summary>
    /// </summary>
    public AbstractPrototypingEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    /// <summary>
    ///
    /// </summary>
    public RandomSamplingMode RandomSamplingMode {
      get { return this.random_sampling_mode; }
      set { this.random_sampling_mode = value; }
    }

    /// <summary>
    /// </summary>
    public virtual void UpdateCurrentConfiguration() { }

    /// <summary>
    ///
    /// </summary>
    /// <param name="configuration"></param>
    public abstract void ApplyConfiguration(IConfigurableConfiguration configuration);

    /// <summary>
    /// </summary>
    public void EnvironmentReset() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"OnReset");
      }

      #endif

      if (this.random_sampling_mode == RandomSamplingMode.On_reset_ && Application.isPlaying) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Random reconfiguring {this} OnReset");
        }

        #endif
        foreach (var v in this.SampleConfigurations()) {
          this.ApplyConfiguration(v);
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    public virtual void PostEnvironmentSetup() { this.UpdateCurrentConfiguration(); }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public virtual Configuration[] SampleConfigurations() {
      return new[] {new Configuration(this.Identifier, this.ConfigurableValueSpace.Sample())};
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected sealed override void Setup() { this.PreSetup(); }

    /// <summary>
    /// </summary>
    protected virtual void PreSetup() { }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment = NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment, this);
    }

    /// <summary>
    ///
    /// </summary>
    public virtual void Tick() {
      if (this.RandomSamplingMode == RandomSamplingMode.On_tick_ && Application.isPlaying) {
        foreach (var v in this.SampleConfigurations()) {
          this.ApplyConfiguration(v);
        }
      }
    }

    void Update() {
      if (this.RandomSamplingMode == RandomSamplingMode.On_update_ && Application.isPlaying) {
        this.Randomise();
      }
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void Randomise() {
      foreach (var v in this.SampleConfigurations()) {
        this.ApplyConfiguration(v);
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
    AbstractPrototypingEnvironment _environment = null;

    /// <summary>
    /// </summary>
    [Header("Configurable", order = 30)]
    [SerializeField]
    bool _relative_to_existing_value = false;

    [SerializeField] RandomSamplingMode random_sampling_mode = RandomSamplingMode.On_reset_;

    #endregion
  }
}
