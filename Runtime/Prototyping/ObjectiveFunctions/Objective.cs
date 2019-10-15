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
  /// <summary>
  /// </summary>
  [Serializable]
  public abstract class ObjectiveFunction : PrototypingGameObject,
                                            //IHasRegister<Term>,
                                            IObjectiveFunction {


    /// <summary>
    /// </summary>
    public IPrototypingEnvironment ParentEnvironment {
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

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"Signal for this step: {signal}");
      }
      #endif

      this._last_signal = signal;

      return signal;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PrototypingReset() {
      this._last_signal = 0;
      this.InternalReset();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public sealed override void Setup() {
      this.PreSetup();

      if (this.ParentEnvironment == null) {
        this.ParentEnvironment = NeodroidSceneUtilities
            .RecursiveFirstSelfSiblingParentGetComponent<PrototypingEnvironment>(this.transform);
      }

      this.RemotePostSetup();
    }


    /// <summary>
    /// </summary>
    /// <returns></returns>
    public void SignalString(DataPoller recipient) {
      recipient.PollData($"{this._last_signal.ToString(CultureInfo.InvariantCulture)}");
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
    ///
    /// </summary>
    [Header("References", order = 100)]
    [SerializeField]
    protected IPrototypingEnvironment _environment = null;

    /// <summary>
    ///
    /// </summary>
    [Header("General", order = 101)]
    [SerializeField]
    protected float _last_signal = 0f;

    /// <summary>
    ///
    /// </summary>
    public float LastSignal { get { return this._last_signal; } }

    [SerializeField] Space1 _signal_space;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Space1 SignalSpace { get { return this._signal_space; } set { this._signal_space = value; } }

    #endregion
  }
}
