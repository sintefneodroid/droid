using System;
using System.Globalization;
using droid.Runtime.Environments.Prototyping;
using droid.Runtime.GameObjects;
using droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.droid.Neodroid.Utilities.Unsorted;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.Prototyping.ObjectiveFunctions {
  /// <inheritdoc cref="ObjectiveFunction" />
  /// <summary>
  /// </summary>
  [Serializable]
  public abstract class ObjectiveFunction : PrototypingGameObject,
                                            //IHasRegister<Term>,
                                            IObjectiveFunction {
    /// <summary>
    /// </summary>
    [SerializeField]
    protected float _solved_reward = 1.0f;

    /// <summary>
    /// </summary>
    [SerializeField]
    protected float _failed_reward = -1.0f;

    /// <summary>
    /// </summary>
    [SerializeField]
    protected float _default_reward = -0.001f;

    /// <summary>
    /// </summary>
    [SerializeField]
    public float _Episode_Return;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override String PrototypingTypeName { get { return ""; } }

    /// <summary>
    /// </summary>
    public ISpatialPrototypingEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public virtual float Evaluate() {
      var signal = 0.0f;
      signal += this.InternalEvaluate();

      if (this.EpisodeLength > 0 && this._environment.step_i >= this.EpisodeLength) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Maximum episode length reached, Length {this._environment.step_i}");
        }
        #endif

        signal = this._failed_reward;

        this._environment.Terminate("Maximum episode length reached");
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"Signal for this step: {signal}");
      }
      #endif

      this._last_signal = signal;

      this._Episode_Return += signal;

      return signal;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public void EnvironmentReset() {
      this._last_signal = 0;
      this._Episode_Return = 0;
      this.InternalReset();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected sealed override void Setup() {
      if (this.ParentEnvironment == null) {
        this.ParentEnvironment = NeodroidSceneUtilities
            .RecursiveFirstSelfSiblingParentGetComponent<BaseSpatialPrototypingEnvironment>(this.transform);
      }

      this.PostSetup();
    }

    /// <summary>
    /// </summary>
    protected virtual void PostSetup() { }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public void SignalString(DataPoller recipient) {
      recipient.PollData($"{this._last_signal.ToString(CultureInfo.InvariantCulture)}, {this._Episode_Return}");
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() { }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() { }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public abstract float InternalEvaluate();

    /// <summary>
    /// </summary>
    public abstract void InternalReset();

    #region Fields

    [Header("References", order = 100)]
    [SerializeField]
    protected ISpatialPrototypingEnvironment _environment = null;

    [Header("General", order = 101)]
    [SerializeField]
    protected float _last_signal = 0f;

    /// <summary>
    ///
    /// </summary>
    public float LastSignal { get { return this._last_signal; } }

    /// <summary>
    /// </summary>
    [SerializeField]
    int _episode_length = 1000;

    [SerializeField] Space1 _signal_space;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public int EpisodeLength { get { return this._episode_length; } set { this._episode_length = value; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Space1 SignalSpace { get { return this._signal_space; } set { this._signal_space = value; } }

    #endregion
  }
}
