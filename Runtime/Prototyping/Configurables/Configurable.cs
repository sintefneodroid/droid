using droid.Runtime.Enums;
using droid.Runtime.Environments.Prototyping;
using droid.Runtime.GameObjects;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables {
  /// <inheritdoc cref="PrototypingGameObject" />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public abstract class Configurable : PrototypingGameObject,
                                       IConfigurable {
    /// <summary>
    ///
    /// </summary>
    public abstract ISamplable ConfigurableValueSpace { get; }

    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "Configurable"; } }

    /// <summary>
    /// </summary>
    public AbstractSpatialPrototypingEnvironment ParentEnvironment {
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
    public override void PrototypingReset() {
      if (this.random_sampling_mode == RandomSamplingMode.On_reset_ && Application.isPlaying) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Random reconfiguring {this} Reset");
        }
        #endif
        this.Randomise();
      }
    }

    /// <summary>
    ///
    /// </summary>
    public override void RemotePostSetup() { this.UpdateCurrentConfiguration(); }

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
    protected override void RegisterComponent() {
      this.ParentEnvironment = NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment, this);
    }

    /// <summary>
    ///
    /// </summary>
    public override void Tick() {
      if (this.RandomSamplingMode == RandomSamplingMode.On_tick_ && Application.isPlaying) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Random reconfiguring {this} Tick");
        }
        #endif
        this.Randomise();
      }
    }

    void Update() {
      if (this.RandomSamplingMode == RandomSamplingMode.On_update_ && Application.isPlaying) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Random reconfiguring {this} Update");
        }
        #endif
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
    AbstractSpatialPrototypingEnvironment _environment = null;

    [SerializeField] RandomSamplingMode random_sampling_mode = RandomSamplingMode.Disabled_;

    #endregion
  }
}
