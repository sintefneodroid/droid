using System;
using System.Globalization;
using droid.Runtime.GameObjects.StatusDisplayer.EventRecipients;
using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Prototyping.ObjectiveFunctions {
  /// <inheritdoc cref="ObjectiveFunction" />
  /// <summary>
  /// </summary>
  [Serializable]
  public abstract class EpisodicObjective : ObjectiveFunction,
                                            //IHasRegister<Term>,
                                            IEpisodicObjectiveFunction {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override float Evaluate() {
      var signal = 0.0f;
      signal += this.InternalEvaluate();

      if (this.EpisodeLength > 0 && this.ParentEnvironment.StepI >= this.EpisodeLength) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Maximum episode length reached, Length {this.ParentEnvironment.StepI}");
        }
        #endif

        signal = this.FailedSignal;

        this.ParentEnvironment.Terminate("Maximum episode length reached");
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"Signal for this step: {signal}");
      }
      #endif

      this.LastSignal = signal;

      this.EpisodeReturn += signal;

      return signal;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public new void PrototypingReset() {
      this.LastSignal = 0;
      this.EpisodeReturn = 0;
      this.InternalReset();
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public new void SignalString(DataPoller recipient) {
      recipient.PollData($"{this.LastSignal.ToString(CultureInfo.InvariantCulture)}, {this.EpisodeReturn}");
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public new void EpisodeLengthString(DataPoller recipient) {
      recipient.PollData($"{this.EpisodeLength.ToString(CultureInfo.InvariantCulture)}");
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [field : SerializeField]
    public int EpisodeLength { get; set; } = 1000;

    /// <summary>
    /// </summary>
    [field : SerializeField]
    public Single EpisodeReturn { get; protected set; } = 0;

    /// <summary>
    /// </summary>
    [field : SerializeField]
    protected Single SolvedSignal { get; set; } = 1.0f;

    /// <summary>
    /// </summary>
    [field : SerializeField]
    protected Single FailedSignal { get; set; } = -1.0f;

    /// <summary>
    /// </summary>
    [field : SerializeField]
    protected Single DefaultSignal { get; set; } = -0.001f;
  }
}
