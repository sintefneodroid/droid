using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using droid.Runtime.Enums;
using droid.Runtime.GameObjects.StatusDisplayer.EventRecipients;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Experimental;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Structs;
using UnityEngine;
using Object = System.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace droid.Runtime.Managers.Experimental {
  /// <inheritdoc cref="UnityEngine.MonoBehaviour" />
  /// <summary>
  /// </summary>
  [DisallowMultipleComponent]
  [AddComponentMenu("Neodroid/Managers/SlaveNeodroidManager")]
  public class SlaveNeodroidManager : MonoBehaviour,
                                      IManager {
    /// <summary>
    /// </summary>
    public static SlaveNeodroidManager Instance { get; private set; }

    /// <summary>
    /// </summary>
    public ISimulatorConfiguration Configuration {
      get {
        if (this._configuration == null) {
          this._configuration = ScriptableObject.CreateInstance<SimulatorConfiguration>();
        }

        return this._configuration;
      }
      set { this._configuration = (SimulatorConfiguration)value; }
    }

    /// <summary>
    ///   Can be subscribed to for pre fixed update events (Will be called before any FixedUpdate on any script)
    /// </summary>
    public event Action EarlyFixedUpdateEvent;

    /// <summary>
    /// </summary>
    public event Action FixedUpdateEvent;

    /// <summary>
    /// </summary>
    public event Action LateFixedUpdateEvent;

    /// <summary>
    ///   Can be subscribed to for pre update events (Will be called before any Update on any script)
    /// </summary>
    public event Action EarlyUpdateEvent;

    /// <summary>
    /// </summary>
    public event Action UpdateEvent;

    /// <summary>
    /// </summary>
    public event Action LateUpdateEvent;

    /// <summary>
    /// </summary>
    public event Action OnPostRenderEvent;

    /// <summary>
    /// </summary>
    public event Action OnRenderImageEvent;

    /// <summary>
    /// </summary>
    public event Action OnEndOfFrameEvent;

    /// <summary>
    /// </summary>
    public event Action OnReceiveEvent;

    /// <summary>
    /// </summary>
    void FetchCommandLineArguments() {
      var arguments = Environment.GetCommandLineArgs();

      for (var i = 0; i < arguments.Length; i++) {
        if (arguments[i] == "-ip") {
          this.Configuration.IpAddress = arguments[i + 1];
        }

        if (arguments[i] == "-port") {
          this.Configuration.Port = int.Parse(arguments[i + 1]);
        }
      }
    }

    /// <summary>
    /// </summary>
    void CreateMessagingClient() {
      try {
        if (this.Configuration.IpAddress != "" || this.Configuration.Port != 0) {
          this._Message_Client = new MessageClient(this.Configuration.IpAddress,
                                                   this.Configuration.Port,
                                                   false,
                                                   #if NEODROID_DEBUG
                                                   this.Debugging
                                                   #else
                                                   false
                                                   #endif
                                                  );
        } else {
          this._Message_Client = new MessageClient(
                                                   #if NEODROID_DEBUG
                                                   this.Debugging
                                                   #else
                                                   false
                                                   #endif
                                                  );
        }
      } catch (Exception exception) {
        Debug.Log(exception);
        throw;

        //TODO: close application is port is already in use.
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="threaded"></param>
    void StartMessagingServer(bool threaded = false) {
      this._Message_Client.ListenForClientToConnect(this.OnDebugCallback);
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log(" Messaging Server is listening for clients");
      }
      #endif

      if (threaded) {
        this.OnListeningCallback();
      }

      //}
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="recipient"></param>
    public void StatusString(DataPoller recipient) { recipient.PollData(this.GetStatus()); }

    #region PrivateFields

    /// <summary>
    /// </summary>
    [Header("Development", order = 110)]
    [SerializeField]
    bool _debugging;

    /// <summary>
    /// </summary>
    Object _send_lock = new Object();

    /// <summary>
    /// </summary>
    [SerializeField]
    bool _testing_Actuators;

    #if UNITY_EDITOR
    /// <summary>
    /// </summary>
    const int _script_execution_order = -1000;
    #endif

    /// <summary>
    /// </summary>
    [Header("Simulation", order = 80)]
    [SerializeField]
    SimulatorConfiguration _configuration;

    /// <summary>
    /// </summary>
    [SerializeField]
    int _skip_frame_i;

    /// <summary>
    /// </summary>
    [SerializeField]
    bool _syncing_environments;

    /// <summary>
    /// </summary>
    [SerializeField]
    bool _awaiting_reply;

    [SerializeField] bool _step;

    WaitForEndOfFrame _wait_for_end_of_frame = new WaitForEndOfFrame();
    WaitForFixedUpdate _wait_for_fixed_update = new WaitForFixedUpdate();

    #endregion

    #region Getter And Setters

    /// <summary>
    /// </summary>
    public Reaction[] CurrentReactions {
      get {
        lock (this._send_lock) {
          return this._Current_Reactions;
        }
      }
      set {
        lock (this._send_lock) {
          this._Current_Reactions = value;
        }
      }
    }

    /// <summary>
    /// </summary>
    public float SimulationTimeScale {
      get { return Time.timeScale; }
      set {
        #if UNITY_EDITOR
        Time.timeScale = Math.Min(value, 99);
        this._last_simulation_time = Math.Min(value, 99);
        #else
        Time.timeScale = value;
        this._last_simulation_time = value;
        #endif

        if (this.Configuration.UpdateFixedTimeScale) {
          Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }
      }
    }

    [SerializeField] float _last_simulation_time;

    /// <summary>
    /// </summary>
    public bool HasStepped { get { return this._has_stepped; } set { this._has_stepped = value; } }

    /// <summary>
    /// </summary>
    public bool TestActuators {
      get { return this._testing_Actuators; }
      set { this._testing_Actuators = value; }
    }

    #if NEODROID_DEBUG
    /// <summary>
    /// </summary>
    public bool Debugging {
      get { return this._debugging; }
      set {
        if (this._Message_Client != null) {
          this._Message_Client.Debugging = value;
        }

        this._debugging = value;
      }
    }
    #endif

    /// <summary>
    /// </summary>
    public bool AwaitingReply {
      get {
        lock (this._send_lock) {
          return this._awaiting_reply;
        }
      }
      set {
        lock (this._send_lock) {
          this._awaiting_reply = value;
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    public ISimulatorConfiguration SimulatorConfiguration { get { return this._configuration; } }

    /// <summary>
    /// </summary>
    public bool IsSyncingEnvironments {
      get { return this._syncing_environments; }
      set { this._syncing_environments = value; }
    }

    /// <summary>
    /// </summary>
    public bool Stepping { get { return this._step; } }

    #endregion

    #region PrivateMembers

    /// <summary>
    /// </summary>
    protected Dictionary<string, IEnvironment> _Environments = new Dictionary<string, IEnvironment>();

    /// <summary>
    /// </summary>
    public void Clear() { this._Environments.Clear(); }

    /// <summary>
    /// </summary>
    protected MessageClient _Message_Client;

    /// <summary>
    /// </summary>
    protected Reaction[] _Current_Reactions = { };

    [SerializeField] bool _has_stepped;

    #endregion

    #region UnityCallbacks

    /// <summary>
    /// </summary>
    protected void Awake() {
      if (Instance == null) {
        Instance = this;
      } else if (Instance == this) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("Using " + Instance);
        }
        #endif
      } else {
        Debug.LogWarning("WARNING! There are multiple SimulationManagers in the scene! Only using "
                         + Instance);
      }

      this.Setup();

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

    public virtual void Setup() { }

    /// <summary>
    /// </summary>
    protected void Start() {
      this.FetchCommandLineArguments();

      if (this.Configuration == null) {
        this.Configuration = ScriptableObject.CreateInstance<SimulatorConfiguration>();
      }

      this.ApplyConfigurationToUnity(this.Configuration);

      if (this.Configuration.SimulationType == SimulationType.Physics_dependent_) {
        this.EarlyFixedUpdateEvent += this.OnPreTick;
        this.FixedUpdateEvent += this.OnTick;
        this.FixedUpdateEvent += this.Tick;
        this.LateFixedUpdateEvent += this.OnPostTick;
        this.StartCoroutine(this.LateFixedUpdateEventGenerator());
      } else {
        this.EarlyUpdateEvent += this.OnPreTick;
        this.UpdateEvent += this.OnTick;
        this.UpdateEvent += this.Tick;
        switch (this.Configuration.FrameFinishes) {
          case FrameFinishes.Late_update_:
            this.LateUpdateEvent += this.OnPostTick;
            break;
          case FrameFinishes.On_post_render_:
            this.OnPostRenderEvent += this.OnPostTick;
            break;
          case FrameFinishes.On_render_image_:
            this.OnRenderImageEvent += this.OnPostTick;
            break;
          case FrameFinishes.End_of_frame_:
            this.StartCoroutine(this.EndOfFrameEventGenerator());
            this.OnEndOfFrameEvent += this.OnPostTick;
            break;
          default: throw new ArgumentOutOfRangeException();
        }
      }

      this.CreateMessagingClient();
      if (this.Configuration.SimulationType == SimulationType.Physics_dependent_) {
        this.StartMessagingServer(); // Remember to manually bind receive to an event in a derivation
      } else {
        this.StartMessagingServer(true);
      }
    }

    /// <summary>
    /// </summary>
    public void ApplyConfigurationToUnity(ISimulatorConfiguration configuration) {
      if (configuration.ApplyQualitySettings) {
        QualitySettings.SetQualityLevel(configuration.QualityLevel, true);
        QualitySettings.vSyncCount = configuration.VSyncCount;
      }

      this.SimulationTimeScale = configuration.TimeScale;
      Application.targetFrameRate = configuration.TargetFrameRate;

      if (this._configuration.OptimiseWindowForSpeed) {
        Screen.SetResolution(1, 1, false);
      }
      #if !UNITY_EDITOR
      else if( configuration.ApplyResolutionSettings ){
      Screen.SetResolution(
          width : configuration.Width,
          height : configuration.Height,
          fullscreen : configuration.FullScreen);
        }
      #else

      PlayerSettings.resizableWindow = configuration.ResizableWindow;
      PlayerSettings.colorSpace = configuration.ColorSpace;
      PlayerSettings.displayResolutionDialog = ResolutionDialogSetting.Disabled;
      //PlayerSettings.use32BitDisplayBuffer
      #endif
    }

    /// <summary>
    /// </summary>
    void OnPostRender() { this.OnPostRenderEvent?.Invoke(); }

    /// <summary>
    /// </summary>
    /// <param name="src"></param>
    /// <param name="dest"></param>
    void OnRenderImage(RenderTexture src, RenderTexture dest) {
      this.OnRenderImageEvent?.Invoke(); //TODO: May not work
    }

    /// <summary>
    /// </summary>
    protected void FixedUpdate() {
      this.EarlyFixedUpdateEvent?.Invoke();
      this.FixedUpdateEvent?.Invoke();
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    IEnumerator LateFixedUpdateEventGenerator() {
      while (true) {
        yield return this._wait_for_fixed_update;
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("LateFixedUpdate");
        }
        #endif
        this.LateFixedUpdateEvent?.Invoke();
      }

      // ReSharper disable once IteratorNeverReturns
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    protected IEnumerator EndOfFrameEventGenerator() {
      while (true) {
        yield return this._wait_for_end_of_frame;
        //yield return new WaitForEndOfFrame();
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("EndOfFrameEvent");
        }
        #endif
        this.OnEndOfFrameEvent?.Invoke();
      }
      #pragma warning disable 162
      // ReSharper disable once HeuristicUnreachableCode
      yield return null;
      #pragma warning restore 162
      // ReSharper disable once IteratorNeverReturns
    }

    /// <summary>
    /// </summary>
    protected void Update() {
      this.EarlyUpdateEvent?.Invoke();
      this.UpdateEvent?.Invoke();
    }

    /// <summary>
    /// </summary>
    protected void LateUpdate() { this.LateUpdateEvent?.Invoke(); }

    #endregion

    #region PrivateMethods

    /// <summary>
    /// </summary>
    protected void OnPreTick() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("OnPreTick");
      }
      #endif

      if (this.Configuration.StepExecutionPhase == ExecutionPhase.Before_) {
        this.ExecuteStep();
      }
    }

    /// <summary>
    /// </summary>
    protected void OnTick() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("OnTick");
      }
      #endif
      if (this.Configuration.StepExecutionPhase == ExecutionPhase.Middle_) {
        this.ExecuteStep();
      }
    }

    /// <summary>
    /// </summary>
    protected void OnPostTick() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("OnPostTick");
      }
      #endif

      foreach (var environment in this._Environments.Values) {
        environment.PostStep();
      }

      if (this.Configuration.StepExecutionPhase == ExecutionPhase.After_) {
        this.ExecuteStep();
      }

      this.CurrentReactions = new Reaction[] { };
    }

    /// <summary>
    /// </summary>
    void ExecuteStep() {
      if (!this.HasStepped) {
        if (!this._syncing_environments) {
          this.React(this.CurrentReactions);
        }

        if (this.AwaitingReply) {
          var states = this.CollectStates();
          this.PostReact(states);
        }

        this.HasStepped = true;
      }
    }

    /// <summary>
    /// </summary>
    protected void Tick() {
      if (this.TestActuators) {
        this.React(this.SampleRandomReactions());
        this.CollectStates();
      }

      foreach (var environment in this._Environments.Values) {
        environment.Tick();
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Tick");
      }
      #endif
    }

    /// <summary>
    /// </summary>
    /// <param name="states"></param>
    protected void PostReact(EnvironmentSnapshot[] states) {
      lock (this._send_lock) {
        foreach (var env in this._Environments.Values) {
          if (env.IsResetting) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log($"Environment {env} is resetting");
            }
            #endif

            this._syncing_environments = true;
            return;
          }
        }

        this._syncing_environments = false;
        if (this._skip_frame_i >= this.Configuration.FrameSkips) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log("Not skipping frame, replying...");
          }
          #endif

          this.Reply(states);
          this.AwaitingReply = false;
          this._skip_frame_i = 0;
        } else {
          this._skip_frame_i += 1;
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log($"Skipping frame, {this._skip_frame_i}/{this.Configuration.FrameSkips}");
          }
          #endif
          if (this.Configuration.ReplayReactionInSkips) { }
        }
      }
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    protected Reaction[] SampleRandomReactions() {
      var sample_reactions = new List<Reaction>();
      foreach (var environment in this._Environments.Values) {
        sample_reactions.Add(environment.SampleReaction());
      }

      return sample_reactions.ToArray();
    }

    //TODO: Maybe add EnvironmentState[][] states for aggregation of states in unity side buffer, when using skips?
    /// <summary>
    /// </summary>
    /// <param name="states"></param>
    void Reply(EnvironmentSnapshot[] states) {
      lock (this._send_lock) {
        var configuration_message = new SimulatorConfigurationMessage(this.Configuration);
        var describe = false;
        if (this.CurrentReactions != null) {
          foreach (var reaction in this.CurrentReactions) {
            if (reaction.Parameters.Describe) {
              describe = true;
            }
          }
        }

        this._Message_Client.SendStates(states,
                                        simulator_configuration_message : configuration_message,
                                        do_serialise_unobservables :
                                        describe || this.Configuration.AlwaysSerialiseUnobservables,
                                        serialise_individual_observables :
                                        describe || this.Configuration.AlwaysSerialiseIndividualObservables,
                                        do_serialise_observables : describe
                                                                   || this._configuration
                                                                          .AlwaysSerialiseAggregatedFloatArray);
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("Replying");
        }

        #endif
      }
    }

    #endregion

    #region PublicMethods

    /// <summary>
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public void React(Reaction reaction) { this.React(new[] {reaction}); }

    /// <summary>
    /// </summary>
    /// <param name="reactions"></param>
    /// <returns></returns>
    public void React(Reaction[] reactions) {
      this.SetStepping(reactions);
      foreach (var reaction in reactions) {
        if (this._Environments.ContainsKey(reaction.RecipientEnvironment)) {
          this._Environments[reaction.RecipientEnvironment].React(reaction);
        } else if (reaction.RecipientEnvironment == "all") {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log("Applying to all environments");
          }
          #endif

          foreach (var environment in this._Environments.Values) {
            environment.React(reaction);
          }
        } else {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log($"Could not find an environment with the identifier: {reaction.RecipientEnvironment}");
          }
          #endif
        }
      }
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public EnvironmentSnapshot[] CollectStates() {
      var environments = this._Environments.Values;
      var states = new EnvironmentSnapshot[environments.Count];
      var i = 0;
      foreach (var environment in environments) {
        states[i++] = environment.Snapshot();
      }

      return states;
    }

    void SetStepping(bool step) {
      if (step) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("Stepping from Reaction");
        }
        #endif

        this._step = true;
      } else {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("Not stepping from Reaction");
        }
        #endif
        if (this.HasStepped) {
          this._step = false;
        }
      }
    }

    void SetStepping(Reaction[] reactions) {
      if (reactions.Any(reaction => reaction.Parameters.StepResetObserveEnu == StepResetObserve.Step_)) {
        this.SetStepping(true);
      }
    }

    public void SetTesting(bool arg0) { this.TestActuators = arg0; }

    /// <summary>
    /// </summary>
    public void ResetAllEnvironments() {
      this.React(new Reaction(new ReactionParameters(StepResetObserve.Reset_, false, true),
                              null,
                              null,
                              null,
                              null,
                              ""));
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public string GetStatus() {
      if (this._Message_Client != null) {
        return this._Message_Client._Listening_For_Clients ? "Connected" : "Not Connected";
      }

      return "No server";
    }

    #endregion

    #region Registration

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="environment"></param>
    public void Register(IEnvironment environment) { this.Register(environment, environment.Identifier); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="environment"></param>
    /// <param name="identifier"></param>
    public void Register(IEnvironment environment, string identifier) {
      if (!this._Environments.ContainsKey(identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Manager {this.name} already has an environment with the identifier: {identifier}");
        }
        #endif

        this._Environments.Add(identifier, environment);
      } else {
        Debug.LogWarning($"WARNING! Please check for duplicates, SimulationManager {this.name} "
                         + $"already has environment {identifier} registered");
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="environment"></param>
    /// <param name="identifier"></param>
    public void UnRegister(IEnvironment environment, string identifier) {
      if (this._Environments.ContainsKey(identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"SimulationManager {this.name} unregistered Environment {identifier}");
        }
        #endif

        this._Environments.Remove(identifier);
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="neodroid_environment"></param>
    public void UnRegister(IEnvironment neodroid_environment) {
      this.UnRegister(neodroid_environment, neodroid_environment.Identifier);
    }

    #endregion

    #region MessageServerCallbacks

    /// <summary>
    /// </summary>
    /// <param name="reactions"></param>
    void OnReceiveCallback(Reaction[] reactions) {
      lock (this._send_lock) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Received: {reactions.Select(r => r.ToString()).Aggregate((current, next) => $"{current}, {next}")}");
        }
        #endif

        this.SetReactionsFromExternalSource(reactions);

        this.OnReceiveEvent?.Invoke();
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="reactions"></param>
    protected void SetReactionsFromExternalSource(Reaction[] reactions) {
      lock (this._send_lock) {
        if (reactions != null) {
          if (this.AwaitingReply || !this.HasStepped) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log($"Got new reaction while not having stepped({!this.HasStepped}) or replied({this.AwaitingReply})");
            }
            #endif
          }

          this.CurrentReactions = reactions;

          this.Configuration.StepExecutionPhase = this.CurrentReactions[0].Parameters.Phase;
          this.AwaitingReply = true;
          this.HasStepped = false;
        } else {
          Debug.LogWarning("Reaction was null");
        }
      }
    }

    /// <summary>
    /// </summary>
    void OnDisconnectCallback() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Client disconnected.");
      }
      #endif
    }

    /// <summary>
    /// </summary>
    /// <param name="error"></param>
    void OnDebugCallback(string error) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("DebugCallback: " + error);
      }
      #endif
    }

    /// <summary>
    /// </summary>
    void OnListeningCallback() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Client connected");
      }
      #endif

      this._Message_Client.StartReceiving(this.OnReceiveCallback,
                                          this.OnDisconnectCallback,
                                          this.OnDebugCallback);
    }

    #endregion

    #region Deconstruction

    /// <summary>
    /// </summary>
    void OnApplicationQuit() { this._Message_Client.CleanUp(); }

    /// <summary>
    /// </summary>
    void OnDestroy() { this._Message_Client.Destroy(); }

    #endregion

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override String ToString() {
      var c = this.SimulatorConfiguration.ToString();
      var e = this._Environments.FirstOrDefault().Value.ToString();

      return $"{c}, {e}";
    }
  }
}
