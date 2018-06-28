using System;
using System.Globalization;
using droid.Neodroid.Managers;
using droid.Neodroid.Utilities.EventRecipients.droid.Neodroid.Utilities.Unsorted;
using droid.Neodroid.Utilities.GameObjects;
using droid.Neodroid.Utilities.Messaging.Messages;
using droid.Neodroid.Utilities.Unsorted;
using UnityEditor;
using UnityEngine;

namespace droid.Neodroid.Environments {
  /// <inheritdoc />
  ///  <summary>
  ///  </summary>
  public abstract class NeodroidEnvironment : PrototypingGameObject {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract override String PrototypingTypeName { get; }

    /// <summary>
    ///
    /// </summary>
    [Header("Environment", order = 100)]
    [SerializeField]
    protected NeodroidManager _Simulation_Manager;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    int _episode_length = 1000;

    /// <summary>
    ///
    /// </summary>
    [SerializeField] protected float _Lastest_Reset_Time;

    /// <summary>
    ///
    /// </summary>
    public bool Terminated { get { return this._Terminated; } }

    /// <summary>
    ///
    /// </summary>
[SerializeField]    protected float _Energy_Spent;

    /// <summary>
    ///
    /// </summary>
    [SerializeField] protected bool _Terminated;

    #if UNITY_EDITOR
    const int _script_execution_order = -20;
    #endif

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public void IdentifierString(DataPoller recipient) { recipient.PollData(this.Identifier); }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public void EnergyString(DataPoller recipient) {
      recipient.PollData(this._Energy_Spent.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public void FrameString(DataPoller recipient) {
      recipient.PollData($"{this.CurrentFrameNumber}/{this.EpisodeLength}");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public void TerminatedBoolean(DataPoller recipient) {
      if (this._Terminated) {
        recipient.PollData(true);
      }

      recipient.PollData(false);
    }

    /// <summary>
    ///
    /// </summary>
    [SerializeField] protected bool _Terminable = true;

    /// <summary>
    ///
    /// </summary>
    [SerializeField] protected string _LastTermination_Reason = "None";

    /// <summary>
    ///
    /// </summary>
    protected bool _Configure;

    /// <summary>
    ///
    /// </summary>
    protected bool _ReplyWithDescriptionThisStep;

    /// <summary>
    ///
    /// </summary>
    public int CurrentFrameNumber {
      get { return this._current_frame_number; }
      protected set { this._current_frame_number = value; }
    }

    /// <summary>
    ///
    /// </summary>
    public int EpisodeLength { get { return this._episode_length; } set { this._episode_length = value; } }

    /// <summary>
    ///
    /// </summary>
    [SerializeField] protected bool _Resetting = false;

    [SerializeField] int _current_frame_number;

    /// <summary>
    ///
    /// </summary>
    public bool IsResetting { get { return this._Resetting; } }

    /// <summary>
    ///
    /// </summary>
    public String LastTerminationReason { get { return this._LastTermination_Reason; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Setup() {
      this.PreSetup();
      if (!this._Simulation_Manager) {
        this._Simulation_Manager = FindObjectOfType<NeodroidManager>();
      }

      #if UNITY_EDITOR
      if (!Application.isPlaying) {
        var manager_script = MonoScript.FromMonoBehaviour(this);
        if (MonoImporter.GetExecutionOrder(manager_script) != _script_execution_order) {
          MonoImporter.SetExecutionOrder(
              manager_script,
              _script_execution_order); // Ensures that PreStep is called first, before all other scripts.
          Debug.LogWarning(
              "Execution Order changed, you will need to press play again to make everything function correctly!");
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
      if (this._Simulation_Manager) {
        this._Simulation_Manager = NeodroidUtilities.MaybeRegisterComponent(this._Simulation_Manager, this);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this._Simulation_Manager) {
        this._Simulation_Manager.UnRegister(this);
      }
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void PreSetup() { }

    /// <summary>
    ///
    /// </summary>
    public abstract void PostStep();

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public abstract Reaction SampleReaction();

    /// <summary>
    ///
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public abstract EnvironmentState ReactAndCollectState(Reaction reaction);

    /// <summary>
    ///
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public abstract void React(Reaction reaction);

    /// <summary>
    ///
    /// </summary>
    public abstract void Tick();

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public abstract EnvironmentState CollectState();
  }
}
