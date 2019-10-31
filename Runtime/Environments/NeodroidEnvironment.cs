using System;
using droid.Runtime.GameObjects;
using droid.Runtime.GameObjects.StatusDisplayer.EventRecipients;
using droid.Runtime.Interfaces;
using droid.Runtime.Managers;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Utilities;
using UnityEditor;
using UnityEngine;

namespace droid.Runtime.Environments {
  /// <inheritdoc cref="PrototypingGameObject" />
  /// <summary>
  /// </summary>
  public abstract class NeodroidEnvironment : PrototypingGameObject,
                                              IEnvironment {
    #region Fields

    #endregion

    #if UNITY_EDITOR
    const int _script_execution_order = -20;
    #endif

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract override String PrototypingTypeName { get; }


    /// <summary>
    /// </summary>
    public abstract void PostStep();

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public abstract Reaction SampleReaction();

    /// <summary>
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public abstract void React(Reaction reaction);


    /// <summary>
    /// </summary>
    /// <returns></returns>
    public abstract EnvironmentSnapshot Snapshot();

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public void IdentifierString(DataPoller recipient) { recipient.PollData(this.Identifier); }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public void TerminatedBoolean(DataPoller recipient) {
      if (this.Terminated) {
        recipient.PollData(true);
      }

      recipient.PollData(false);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() {
      this.PreSetup();

      if (this.SimulationManager == null) {
        this.SimulationManager = FindObjectOfType<AbstractNeodroidManager>();
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
      if (this.SimulationManager != null) {
        this.SimulationManager =
            NeodroidRegistrationUtilities.RegisterComponent((NeodroidManager)this.SimulationManager, this);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() { this.SimulationManager?.UnRegister(this); }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public abstract void ObservationsString(DataPoller recipient);

    #region Public Methods

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public void FrameString(DataPoller recipient) { recipient.PollData($"{this.StepI}"); }

    #endregion

    #region Properties

    /// <summary>
    /// </summary>
    [field : SerializeField]
    public int StepI { get; protected internal set; }

    /// <summary>
    /// </summary>
    [field : SerializeField]
    protected Single LastResetTime { get; set; }

    /// <summary>
    /// </summary>
    [field : SerializeField]
    protected Boolean Terminable { get; set; } = true;

    /// <summary>
    /// </summary>
    [field : SerializeField]
    public bool Terminated { get; set; } = false;

    /// <summary>
    ///
    /// </summary>
    [field : SerializeField]
    public Reaction LastReaction { get; set; }

    /// <summary>
    /// </summary>
    [field : SerializeField]
    public bool IsResetting { get; set; }

    /// <summary>
    /// </summary>
    [field : SerializeField]
    public String LastTerminationReason { get; set; } = "None";

    /// <summary>
    /// </summary>
    [field : SerializeField]
    protected Boolean Configure { get; set; }

    /// <summary>
    /// </summary>
    [field : SerializeField]
    protected Boolean ReplyWithDescriptionThisStep { get; set; }

    /// <summary>
    /// </summary>
    [field : Header("Environment", order = 100)]
    [field : SerializeField]
    protected IManager SimulationManager { get; set; }

    #endregion
  }
}
