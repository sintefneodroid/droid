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
    public AbstractSpatialPrototypingEnvironment ParentEnvironment { get; set; } = null;

    /// <summary>
    ///
    /// </summary>
    [field : SerializeField]
    public RandomSamplingPhase RandomSamplingPhase { get; set; } = RandomSamplingPhase.Disabled_;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public abstract Configuration[] SampleConfigurations();

    /// <summary>
    /// </summary>
    public abstract void UpdateCurrentConfiguration();

    /// <summary>
    ///
    /// </summary>
    /// <param name="configuration"></param>
    public abstract void ApplyConfiguration(IConfigurableConfiguration configuration);

    /// <summary>
    /// </summary>
    public override void PrototypingReset() {
      if (this.RandomSamplingPhase == RandomSamplingPhase.On_reset_ && Application.isPlaying) {
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
      this.ParentEnvironment = NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment, this);
    }

    void PostTick() {
      if (this.RandomSamplingPhase == RandomSamplingPhase.On_post_tick_ && Application.isPlaying) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Random reconfiguring {this} Tick");
        }
        #endif
        this.Randomise();
      }
    }

    void PreTick() {
      if (this.RandomSamplingPhase == RandomSamplingPhase.On_pre_tick_ && Application.isPlaying) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Random reconfiguring {this} Tick");
        }
        #endif
        this.Randomise();
      }
    }

    /// <summary>
    ///
    /// </summary>
    public override void Tick() {
      if (this.RandomSamplingPhase == RandomSamplingPhase.On_tick_ && Application.isPlaying) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Random reconfiguring {this} Tick");
        }
        #endif
        this.Randomise();
      }
    }

    void Update() {
      if (this.RandomSamplingPhase == RandomSamplingPhase.On_update_ && Application.isPlaying) {
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

    #endregion
  }
}
