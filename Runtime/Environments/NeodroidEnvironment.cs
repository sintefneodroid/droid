using System;
using System.Globalization;
using droid.Runtime.GameObjects;
using droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.droid.Neodroid.Utilities.Unsorted;
using droid.Runtime.Interfaces;
using droid.Runtime.Managers;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Utilities;
using UnityEditor;
using UnityEngine;
using NeodroidUtilities = droid.Runtime.Utilities.Extensions.NeodroidUtilities;

namespace droid.Runtime.Environments {
  /// <inheritdoc cref="PrototypingGameObject" />
  /// <summary>
  /// </summary>
  public abstract class NeodroidEnvironment : PrototypingGameObject,
                                              IEnvironment {
    #region Fields

    /// <summary>
    ///
    /// </summary>
    [SerializeField] protected Reaction _LastReaction;

    /// <summary>
    /// </summary>
    [SerializeField]
    protected float _LastResetTime;

    /// <summary>
    /// </summary>
    [SerializeField]
    protected string _LastTerminationReason = "None";

    /// <summary>
    /// </summary>
    [SerializeField]
    protected bool _Resetting;

    /// <summary>
    /// </summary>
    [Header("Environment", order = 100)]
    [SerializeField]
    protected IManager _Simulation_Manager;

    /// <summary>
    /// </summary>
    [SerializeField]
    protected bool _Terminable = true;

    /// <summary>
    /// </summary>
    [SerializeField]
    protected bool _Terminated;

    [SerializeField] protected int _current_frame_number;

    #endregion

    #if UNITY_EDITOR
    const int _script_execution_order = -20;
    #endif

    /// <summary>
    /// </summary>
    protected bool _Configure;

    /// <summary>
    /// </summary>
    protected bool _ReplyWithDescriptionThisStep;


    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract override String PrototypingTypeName { get; }

    /// <summary>
    /// </summary>
    public bool Terminated { get { return this._Terminated; } set { this._Terminated = value; } }

    /// <summary>
    ///
    /// </summary>
    public Reaction LastReaction { get { return this._LastReaction; } set { this._LastReaction = value; } }

    /// <summary>
    /// </summary>
    public bool IsResetting { get { return this._Resetting; } }

    /// <summary>
    /// </summary>
    public String LastTerminationReason {
      get { return this._LastTerminationReason; }
      set { this._LastTerminationReason = value; }
    }

    /// <summary>
    /// </summary>
    public abstract void PostStep();

    /// <summary>
    ///
    /// </summary>
    protected abstract void EnvironmentReset();

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public abstract Reaction SampleReaction();

    /// <summary>
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public EnvironmentState ReactAndCollectState(Reaction reaction) {
      this.React(reaction);
      return this.CollectState();
    }

    /// <summary>
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public abstract void React(Reaction reaction);

    /// <summary>
    /// </summary>
    public abstract void Tick();

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public abstract EnvironmentState CollectState();

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public void IdentifierString(DataPoller recipient) { recipient.PollData(this.Identifier); }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public void TerminatedBoolean(DataPoller recipient) {
      if (this._Terminated) {
        recipient.PollData(true);
      }

      recipient.PollData(false);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Setup() {
      this.PreSetup();
      if (this._Simulation_Manager == null) {
        this._Simulation_Manager = FindObjectOfType<AbstractNeodroidManager>();
      }

      #if UNITY_EDITOR
      if (!Application.isPlaying) {
        var manager_script = MonoScript.FromMonoBehaviour(this);
        if (MonoImporter.GetExecutionOrder(manager_script) != _script_execution_order) {
          MonoImporter.SetExecutionOrder(manager_script,
                                         _script_execution_order); // Ensures that PreStep is called first, before all other scripts.
          Debug.LogWarning("Execution Order changed, you will need to press play again to make everything function correctly!");
          EditorApplication.isPlaying = false;
          //TODO: UnityEngine.Experimental.LowLevel.PlayerLoop.SetPlayerLoop(new UnityEngine.Experimental.LowLevel.PlayerLoopSystem());
        }
      }
      #endif
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      if (this._Simulation_Manager != null) {
        this._Simulation_Manager =
            NeodroidRegistrationUtilities.RegisterComponent((NeodroidManager)this._Simulation_Manager, this);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() { this._Simulation_Manager?.UnRegister(this); }

    /// <summary>
    /// </summary>
    protected virtual void PreSetup() { }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public abstract void ObservationsString(DataPoller recipient);

    #region Public Methods

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public void FrameString(DataPoller recipient) { recipient.PollData($"{this.step_i}"); }

    #endregion

    #region Properties

    /// <summary>
    /// </summary>
    public int step_i {
      get { return this._current_frame_number; }
      set { this._current_frame_number = value; }
    }

    #endregion
  }
}
