using System;
using System.Collections;
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
    /// <summary>
    ///
    /// </summary>
    protected WaitForFixedUpdate _Wait_For_Fixed_Update = new WaitForFixedUpdate();

    #if UNITY_EDITOR
    const int _script_execution_order = -20;
    #endif

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract override string PrototypingTypeName { get; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract void PostStep();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public abstract Reaction SampleReaction();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public abstract void Step(Reaction reaction);

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public abstract void Reset();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public abstract void Configure(Reaction reaction);

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public abstract EnvironmentSnapshot Snapshot();

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public void IdentifierString(DataPoller recipient) { recipient.PollData(data : this.Identifier); }

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
      base.Setup();

      if (this.SimulationManager == null) {
        this.SimulationManager = FindObjectOfType<AbstractNeodroidManager>();
      }

      #if UNITY_EDITOR
      if (!Application.isPlaying) {
        var manager_script = MonoScript.FromMonoBehaviour(behaviour : this);
        if (MonoImporter.GetExecutionOrder(script : manager_script) != _script_execution_order) {
          MonoImporter.SetExecutionOrder(script : manager_script,
                                         order :
                                         _script_execution_order); // Ensures that PreStep is called first, before all other scripts.
          Debug.LogWarning("Execution Order changed, you will need to press play again to make everything function correctly!");
          EditorApplication.isPlaying = false;
          //TODO: UnityEngine.Experimental.LowLevel.PlayerLoop.SetPlayerLoop(new UnityEngine.Experimental.LowLevel.PlayerLoopSystem());
        }
      }
      #endif

      this.StartCoroutine(routine : this.RemotePostSetupIe());
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    IEnumerator RemotePostSetupIe() {
      yield return this._Wait_For_Fixed_Update;
      this.RemotePostSetup();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      if (this.SimulationManager != null) {
        this.SimulationManager =
            NeodroidRegistrationUtilities.RegisterComponent(r : (NeodroidManager)this.SimulationManager,
                                                            c : this);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() { this.SimulationManager?.UnRegister(obj : this); }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public abstract void ObservationsString(DataPoller recipient);

    #region Public Methods

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public void FrameString(DataPoller recipient) { recipient.PollData(data : $"{this.StepI}"); }

    #endregion

    #region Properties

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [field : SerializeField]
    public int StepI { get; protected internal set; }

    /// <summary>
    /// </summary>
    [field : SerializeField]
    protected float LastResetTime { get; set; }

    /// <summary>
    /// </summary>
    [field : SerializeField]
    protected bool Terminable { get; set; } = true;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [field : SerializeField]
    public bool Terminated { get; set; } = false;

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    [field : SerializeField]
    public Reaction LastReaction { get; set; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [field : SerializeField]
    public bool IsResetting { get; set; }

    /// <summary>
    /// </summary>

    [field : SerializeField]
    public bool ProvideFullDescription { get; set; } = true;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [field : SerializeField]
    public string LastTerminationReason { get; set; } = "None";

    /// <summary>
    /// </summary>
    [field : SerializeField]
    protected bool ShouldConfigure { get; set; }

    /// <summary>
    /// </summary>
    [field : Header("Environment", order = 100)]
    [field : SerializeField]
    protected IManager SimulationManager { get; set; }

    #endregion
  }
}
