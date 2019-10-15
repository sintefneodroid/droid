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
  public abstract class EpisodicObjective : ObjectiveFunction,
                                            //IHasRegister<Term>,
                                            IEpisodicObjectiveFunction {

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
    protected float _Episode_Return;



    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override float Evaluate() {
      var signal = 0.0f;
      signal += this.InternalEvaluate();

      if (this.EpisodeLength > 0 && this._environment.StepI >= this.EpisodeLength) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Maximum episode length reached, Length {this._environment.StepI}");
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
    public new void PrototypingReset() {
      this._last_signal = 0;
      this._Episode_Return = 0;
      this.InternalReset();
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public new void SignalString(DataPoller recipient) {
      recipient.PollData($"{this.LastSignal.ToString(CultureInfo.InvariantCulture)}, {this._Episode_Return}");
    }

    /// <summary>
    /// </summary>
    [SerializeField]
    int _episode_length = 1000;


    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public int EpisodeLength { get { return this._episode_length; } set { this._episode_length = value; } }

    /// <summary>
    /// </summary>
    public Single EpisodeReturn { get { return this._Episode_Return; } }
  }
}
