using System;
using System.Globalization;
using droid.Runtime.Environments.Prototyping;
using droid.Runtime.GameObjects;
using droid.Runtime.GameObjects.StatusDisplayer.EventRecipients;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.Prototyping.ObjectiveFunctions {
  /// <summary>
  /// </summary>
  [Serializable]
  public abstract class ObjectiveFunction : PrototypingGameObject,
                                            //IHasRegister<Term>,
                                            IObjectiveFunction {




    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public virtual float Evaluate() {
      var signal = 0.0f;
      signal += this.InternalEvaluate();

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"Signal for this step: {signal}");
      }
      #endif

      this.LastSignal = signal;

      return signal;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PrototypingReset() {
      this.LastSignal = 0;
      this.InternalReset();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public sealed override void Setup() {
      this.PreSetup();

      if (this.ParentEnvironment == null) {
        this.ParentEnvironment = NeodroidSceneUtilities
            .RecursiveFirstSelfSiblingParentGetComponent<AbstractSpatialPrototypingEnvironment>(this.transform);
      }

      this.RemotePostSetup();
    }


    /// <summary>
    /// </summary>
    /// <returns></returns>
    public void SignalString(DataPoller recipient) {
      recipient.PollData($"{this.LastSignal.ToString(CultureInfo.InvariantCulture)}");
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public void EpisodeLengthString(DataPoller recipient) {
      recipient.PollData($"");
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

    /// <summary>
    /// </summary>
    [field : Header("References", order = 100)]
    [field : SerializeField]
    public ISpatialPrototypingEnvironment ParentEnvironment { get; set; } = null;

    /// <summary>
    ///
    /// </summary>
    [field : Header("General", order = 101)]
    [field : SerializeField]
    public float LastSignal { get; protected set; } = 0f;



    /// <inheritdoc />
    /// <summary>
    /// </summary>
   [field : SerializeField]
    public Space1 SignalSpace { get; set; }

    #endregion
  }
}
