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
    /// </summary>
    [field : Header("References", order = 20)]
    [field : SerializeField]
    public AbstractPrototypingEnvironment ParentEnvironment { get; set; } = null;

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    [field : SerializeField]
    public RandomSamplingPhaseEnum RandomSamplingPhaseEnum { get; set; } = RandomSamplingPhaseEnum.Disabled_;

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <returns></returns>
    public abstract Configuration[] SampleConfigurations();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract void UpdateCurrentConfiguration();

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <param name="configuration"></param>
    public abstract void ApplyConfiguration(IConfigurableConfiguration configuration);

    /// <inheritdoc />
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PrototypingReset() {
      if (this.RandomSamplingPhaseEnum == RandomSamplingPhaseEnum.On_reset_ && Application.isPlaying) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(message : $"Random reconfiguring {this} Reset");
        }
        #endif
        this.Randomise();
      }
    }

    /// <summary>
    ///
    /// </summary>
    public override void RemotePostSetup() {
      this.UpdateCurrentConfiguration();

      if (this.ParentEnvironment != null) {
        this.ParentEnvironment.PreTickEvent += this.PreTick;
        this.ParentEnvironment.PostTickEvent += this.PostTick;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment, c : this);
    }

    void PostTick() {
      if (this.RandomSamplingPhaseEnum == RandomSamplingPhaseEnum.On_post_tick_ && Application.isPlaying) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(message : $"Random reconfiguring {this} Tick");
        }
        #endif
        this.Randomise();
      }
    }

    void PreTick() {
      if (this.RandomSamplingPhaseEnum == RandomSamplingPhaseEnum.On_pre_tick_ && Application.isPlaying) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(message : $"Random reconfiguring {this} Tick");
        }
        #endif
        this.Randomise();
      }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override void Tick() {
      if (this.RandomSamplingPhaseEnum == RandomSamplingPhaseEnum.On_tick_ && Application.isPlaying) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(message : $"Random reconfiguring {this} Tick");
        }
        #endif
        this.Randomise();
      }
    }

    void Update() {
      if (this.RandomSamplingPhaseEnum == RandomSamplingPhaseEnum.On_update_ && Application.isPlaying) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(message : $"Random reconfiguring {this} Update");
        }
        #endif
        this.Randomise();
      }
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void Randomise() {
      var vs = this.SampleConfigurations();
      for (var index = 0; index < vs.Length; index++) {
        var v = vs[index];
        this.ApplyConfiguration(configuration : v);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() { this.ParentEnvironment?.UnRegister(configurable : this); }

    #region Fields

    #endregion
  }
}
