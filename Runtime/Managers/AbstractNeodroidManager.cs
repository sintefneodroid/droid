using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using droid.Runtime.Enums;
using droid.Runtime.GameObjects;
using droid.Runtime.GameObjects.StatusDisplayer.EventRecipients;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Structs;
using UnityEngine;
using Object = System.Object;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace droid.Runtime.Managers {
  /// <inheritdoc cref="UnityEngine.MonoBehaviour" />
  /// <summary>
  /// </summary>
  [DisallowMultipleComponent]
  public abstract class AbstractNeodroidManager : MonoBehaviour,
                                                  IManager {
    /// <summary>
    /// </summary>
    public static AbstractNeodroidManager Instance { get; private set; }

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
    // ReSharper disable once EventNeverSubscribedTo.Global
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
          this.Configuration.Port = int.Parse(s : arguments[i + 1]);
        }
      }
    }

    /// <summary>
    /// </summary>
    void CreateMessagingServer() {
      try {
        if (this.Configuration.IpAddress != "" || this.Configuration.Port != 0) {
          this._Message_Server = new MessageServer(ip_address : this.Configuration.IpAddress,
                                                   port : this.Configuration.Port,
                                                   false,
                                                   #if NEODROID_DEBUG
                                                   debug : this.Debugging
                                                   #else
                                                   false
                                                   #endif
                                                  );
        } else {
          this._Message_Server = new MessageServer(
                                                   #if NEODROID_DEBUG
                                                   debug : this.Debugging
                                                   #else
                                                   false
                                                   #endif
                                                  );
        }
      } catch (Exception exception) {
        Debug.Log(message : exception);
        throw;

        //TODO: close application is port is already in use.
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="threaded"></param>
    void StartMessagingServer(bool threaded = false) {
      /*if (threaded) {
        this._Message_Server.ListenForClientToConnect(this.OnConnectCallback, this.OnDebugCallback);
        #if NEODROID_DEBUG

        if (this.Debugging) {
          Debug.Log("Started Messaging Server in a new thread");
        }
        #endif
      } else {*/

      this._Message_Server.ListenForClientToConnect(debug_callback : this.OnDebugCallback);
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
    public void StatusString(DataPoller recipient) { recipient.PollData(data : this.GetStatus()); }

    #region PrivateFields

    /// <summary>
    /// </summary>
    [Header("Development", order = 110)]
    [SerializeField]
    bool _debugging;

    /// <summary>
    /// </summary>
    Object _send_lock = new Object();

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
    bool _awaiting_reply;

    WaitForEndOfFrame _wait_for_end_of_frame = new WaitForEndOfFrame();
    WaitForFixedUpdate _wait_for_fixed_update = new WaitForFixedUpdate();
    List<Reaction> _sample_reactions = new List<Reaction>();

    [SerializeField] SimulationRenderCamera _simulation_render_camera;
    [SerializeField] bool _manual_render = false;

    public Boolean ManualRender {
      get { return this._manual_render; }
      set {
        if (this._simulation_render_camera) {
          if (value) {
            this._simulation_render_camera.DisableCamera();
          } else {
            this._simulation_render_camera.EnableCamera();
          }
        }

        this._manual_render = value;
      }
    }

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
        Time.timeScale = Math.Min(val1 : value, 99);
        this.LastSimulationTime = Math.Min(val1 : value, 99);

        if (this.Configuration.UpdateFixedTimeScale) {
          Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
      }
    }

    /// <summary>
    /// </summary>
    [field : SerializeField]
    public bool HasStepped { get; private set; }

    /// <summary>
    /// </summary>
    [field : SerializeField]
    public bool TestActuators { get; set; }

    #if NEODROID_DEBUG
    /// <summary>
    /// </summary>
    public bool Debugging {
      get { return this._debugging; }
      set {
        if (this._Message_Server != null) {
          this._Message_Server.Debugging = value;
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

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public ISimulatorConfiguration SimulatorConfiguration { get { return this._configuration; } }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public abstract void Setup();

    /// <summary>
    /// </summary>

    public bool ShouldResume {
      get { return this._shouldResume; }
      set {
        if (value != this._shouldResume) {
          if (value) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log("Should Resume Now");
            }
            #endif
          } else {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log("Should Not Resume");
            }
            #endif
          }
        } else {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log(message : $"ShouldResume did not change: {value}");
          }
          #endif
        }

        this._shouldResume = value;
      }
    }

    /// <summary>
    ///
    /// </summary>
    [field : SerializeField]
    public float LastSimulationTime { get; private set; }

    /// <summary>
    /// </summary>
    [field : SerializeField]
    public int SkipFrameI { get; set; }

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
    [SerializeField]
    protected MessageServer _Message_Server;

    /// <summary>
    /// </summary>
    protected Reaction[] _Current_Reactions = { };

    [SerializeField] Boolean _shouldResume;

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
          Debug.Log(message : "Using " + Instance);
        }
        #endif
      } else {
        Debug.LogWarning(message : "WARNING! There are multiple SimulationManagers in the scene! Only using "
                                   + Instance);
      }

      this.Setup();

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
    }

    /// <summary>
    /// </summary>
    protected void Start() {
      this.FetchCommandLineArguments();

      if (this.Configuration == null) {
        this.Configuration = ScriptableObject.CreateInstance<SimulatorConfiguration>();
      }

      this.ApplyConfigurationToUnity(configuration : this.Configuration);

      if (this.Configuration.SimulationType == SimulationTypeEnum.Physics_dependent_) {
        this.EarlyFixedUpdateEvent += this.OnPreTick;
        this.FixedUpdateEvent += this.OnTick;
        this.LateFixedUpdateEvent += this.OnPostTick;
        this.StartCoroutine(routine : this.LateFixedUpdateEventGenerator());
      } else {
        this.EarlyUpdateEvent += this.OnPreTick;
        this.UpdateEvent += this.OnTick;
        switch (this.Configuration.FrameFinishes) {
          case FrameFinishesEnum.Late_update_:
            this.LateUpdateEvent += this.OnPostTick;
            break;
          case FrameFinishesEnum.On_post_render_:
            this.OnPostRenderEvent += this.OnPostTick;
            break;
          case FrameFinishesEnum.On_render_image_:
            this.OnRenderImageEvent += this.OnPostTick;
            break;
          case FrameFinishesEnum.End_of_frame_:
            this.StartCoroutine(routine : this.EndOfFrameEventGenerator());
            this.OnEndOfFrameEvent += this.OnPostTick;
            break;
          default: throw new ArgumentOutOfRangeException();
        }
      }

      this.CreateMessagingServer();
      if (this.Configuration.SimulationType == SimulationTypeEnum.Physics_dependent_) {
        this.StartMessagingServer(); // Remember to manually bind receive to an event in a derivation
      } else {
        this.StartMessagingServer(true);
      }
    }

    /// <summary>
    /// </summary>
    public void ApplyConfigurationToUnity(ISimulatorConfiguration configuration) {
      this.ManualRender = configuration.ManualRender;

      if (configuration.ApplyQualitySettings) {
        QualitySettings.SetQualityLevel(index : configuration.QualityLevel, true);
        QualitySettings.vSyncCount = configuration.VSyncCount;
      }

      this.SimulationTimeScale = configuration.TimeScale;
      Application.targetFrameRate = configuration.TargetFrameRate;

      if (this._configuration.OptimiseWindowForSpeed) {
        Screen.SetResolution(2, 2, false);
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
      PlayerSettings.colorSpace = configuration.UnityColorSpace;
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
      this.OnRenderImageEvent?.Invoke(); //Will need Camera component!
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
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    protected IEnumerator EndOfFrameEventGenerator() {
      while (true) {
        yield return this._wait_for_end_of_frame;
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("EndOfFrameEvent");
        }
        #endif
        this.OnEndOfFrameEvent?.Invoke();
      }
      #pragma warning disable 162 // ReSharper disable once HeuristicUnreachableCode
      yield return null;
      #pragma warning restore 162 // ReSharper disable once IteratorNeverReturns
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

      if (this.Configuration.StepExecutionPhase == ExecutionPhaseEnum.Before_tick_) {
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

      if (this.Configuration.StepExecutionPhase == ExecutionPhaseEnum.On_tick_) {
        this.ExecuteStep();
      }

      if (this.TestActuators) {
        this.DelegateReactions(reactions : this.SampleRandomReactions());
        this.GatherSnapshots();
      }

      foreach (var environment in this._Environments.Values) {
        environment.Tick();
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

      if (this.Configuration.StepExecutionPhase == ExecutionPhaseEnum.After_tick_) {
        this.ExecuteStep();
      }

      foreach (var environment in this._Environments.Values) {
        environment.PostStep();
      }
    }

    /// <summary>
    /// </summary>
    void ExecuteStep() {
      lock (this._send_lock) {
        if (!this.HasStepped) {
          this.HasStepped = true;

          this.DelegateReactions(reactions : this.CurrentReactions);

          if (this.AwaitingReply) {
            var states = this.GatherSnapshots();
            this.PostReact(states : states);
          }

          this.ShouldResume = false;
        }
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="states"></param>
    protected void PostReact(EnvironmentSnapshot[] states) {
      lock (this._send_lock) {
        if (this.SkipFrameI >= this.Configuration.FrameSkips) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log("Not skipping frame, replying...");
          }
          #endif

          this.Reply(states : states);

          this.AwaitingReply = false;
          this.SkipFrameI = 0;
        } else {
          this.SkipFrameI += 1;
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log(message : $"Skipping frame, {this.SkipFrameI}/{this.Configuration.FrameSkips}");
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
      this._sample_reactions.Clear();
      foreach (var environment in this._Environments.Values) {
        this._sample_reactions.Add(item : environment.SampleReaction());
      }

      return this._sample_reactions.ToArray();
    }

    //TODO: Maybe add EnvironmentState[][] states for aggregation of states in unity side buffer, when using skips?
    /// <summary>
    /// </summary>
    /// <param name="states"></param>
    void Reply(EnvironmentSnapshot[] states) {
      lock (this._send_lock) {
        var configuration_message =
            new SimulatorConfigurationMessage(simulator_configuration : this.Configuration);
        var describe = false;
        var should_render = false;
        if (this.CurrentReactions != null) {
          for (var index = 0; index < this.CurrentReactions.Length; index++) {
            var reaction = this.CurrentReactions[index];
            if (reaction.Parameters.Describe) {
              describe = true;
            }

            if (reaction.Parameters.Render) {
              should_render = true;
            }
          }
        }

        if (this.ManualRender && should_render) {
          this._simulation_render_camera.Render();
        }

        this._Message_Server.SendStates(environment_states : states,
                                        simulator_configuration_message : configuration_message,
                                        do_serialise_unobservables :
                                        describe || this.Configuration.SerialiseUnobservables,
                                        serialise_individual_observables :
                                        describe || this.Configuration.SerialiseIndividualObservables,
                                        do_serialise_observables : describe
                                                                   || this._configuration
                                                                          .SerialiseAggregatedFloatArray);

        this.CurrentReactions = new Reaction[] { };
      }
    }

    #endregion

    #region PublicMethods

    /// <summary>
    /// </summary>
    /// <param name="reactions"></param>
    /// <returns></returns>
    public void DelegateReactions(Reaction[] reactions) {
      /*
      for (var index = 0; index < reactions.Length; index++) {
        var reaction = reactions[index];
        if (this._Environments.ContainsKey(key : reaction.RecipientEnvironment)) {
          if (reaction.Parameters.ReactionType == ReactionTypeEnum.Reset_) {
            this._Environments[key : reaction.RecipientEnvironment].Reset();
          }

          if (reaction.Parameters.Configure) {
            this._Environments[key : reaction.RecipientEnvironment].Configure(reaction);
          }
        }
      }
      */

      this.SetStepping(reactions : reactions);
      for (var index = 0; index < reactions.Length; index++) {
        var reaction = reactions[index];
        if (this._Environments.ContainsKey(key : reaction.RecipientEnvironment)) {
          this._Environments[key : reaction.RecipientEnvironment].Step(reaction : reaction);
        } else if (reaction.RecipientEnvironment == "all") {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log("Applying to all environments");
          }
          #endif

          foreach (var environment in this._Environments.Values) {
            environment.Step(reaction : reaction);
          }
        } else {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.LogError(message :
                           $"Could not find an environment with the identifier: {reaction.RecipientEnvironment}");
          }
          #endif
          foreach (var environment in this._Environments.Values) {
            environment.Step(reaction : new Reaction(reaction_parameters :
                                                     new ReactionParameters(reaction_type : ReactionTypeEnum
                                                                                .Observe_)));
          }
        }
      }
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public EnvironmentSnapshot[] GatherSnapshots() {
      var environments = this._Environments.Values;
      var states = new EnvironmentSnapshot[environments.Count];
      var i = 0;
      foreach (var environment in environments) {
        states[i++] = environment.Snapshot();
      }

      return states;
    }

    void SetStepping(Reaction[] reactions) {
      var any = false;
      if (reactions.Length == 0) {
        #if NEODROID_DEBUG
        if (this._debugging) {
          Debug.LogWarning(message : $"Received no reaction!");
        }
        #endif
      }

      for (var index = 0; index < reactions.Length; index++) {
        var reaction = reactions[index];
        #if NEODROID_DEBUG
        if (this._debugging) {
          Debug.LogWarning(message :
                           $"Reaction StepResetObserveEnu Parameter: {reaction.Parameters.ReactionType}");
        }
        #endif
        if (reaction.Parameters.ReactionType == ReactionTypeEnum.Step_) {
          any = true;
          break;
        }
      }

      if (any) {
        this.ShouldResume = true;
      } else {
        this.ShouldResume = false;
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    public void SetTesting(bool v) { this.TestActuators = v; }

    /// <summary>
    /// </summary>
    public void ResetAllEnvironments() {
      var reactions = new List<Reaction>();
      foreach (var environment in this._Environments) {
        reactions.Add(item : new Reaction(parameters :
                                          new ReactionParameters(reaction_type : ReactionTypeEnum.Reset_,
                                                                 false,
                                                                 true),
                                          motions : null,
                                          configurations : null,
                                          unobservables : null,
                                          displayables : null,
                                          "",
                                          recipient_environment : environment.Value.Identifier));
      }

      this.DelegateReactions(reactions : reactions.ToArray());
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public string GetStatus() {
      if (this._Message_Server != null) {
        return this._Message_Server._Listening_For_Clients ? "Connected" : "Not Connected";
      }

      return "No server";
    }

    #endregion

    #region Registration

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="environment"></param>
    public void Register(IEnvironment environment) {
      this.Register(environment : environment, identifier : environment.Identifier);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="environment"></param>
    /// <param name="identifier"></param>
    public void Register(IEnvironment environment, string identifier) {
      if (!this._Environments.ContainsKey(key : identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(message :
                    $"Manager {this.name} already has an environment with the identifier: {identifier}");
        }
        #endif

        this._Environments.Add(key : identifier, value : environment);
      } else {
        Debug.LogWarning(message : $"WARNING! Please check for duplicates, SimulationManager {this.name} "
                                   + $"already has environment {identifier} registered");
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="environment"></param>
    /// <param name="identifier"></param>
    public void UnRegister(IEnvironment environment, string identifier) {
      if (this._Environments.ContainsKey(key : identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(message : $"SimulationManager {this.name} unregistered Environment {identifier}");
        }
        #endif

        this._Environments.Remove(key : identifier);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="neodroid_environment"></param>
    public void UnRegister(IEnvironment neodroid_environment) {
      this.UnRegister(environment : neodroid_environment, identifier : neodroid_environment.Identifier);
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
          if (reactions.Length > 0) {
            Debug.Log(message :
                      $"Received: {reactions.Select(r => r.ToString()).Aggregate((current, next) => $"{current}, {next}")}");
          } else {
            Debug.Log(message : $"Received empty reaction sequence");
          }
        }
        #endif

        this.SetReactions(reactions : reactions);

        this.OnReceiveEvent?.Invoke();
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="reactions"></param>
    protected void SetReactions(Reaction[] reactions) {
      lock (this._send_lock) {
        if (reactions != null) {
          if (this.AwaitingReply || !this.HasStepped) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.LogError(message :
                             $"Got new reaction while not having stepped({!this.HasStepped}) or replied({this.AwaitingReply})");
            }
            #endif
          }

          this.CurrentReactions = reactions;

          var phase = this.Configuration.StepExecutionPhase;
          for (var index = 0; index < this.CurrentReactions.Length; index++) {
            var current_reaction = this.CurrentReactions[index];
            phase = current_reaction.Parameters.PhaseEnum;
          }

          this.Configuration.StepExecutionPhase = phase;
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
        Debug.Log(message : "DebugCallback: " + error);
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

      this._Message_Server.StartReceiving(cmd_callback : this.OnReceiveCallback,
                                          disconnect_callback : this.OnDisconnectCallback,
                                          debug_callback : this.OnDebugCallback);
    }

    #endregion

    #region Deconstruction

    /// <summary>
    /// </summary>
    void OnApplicationQuit() { this._Message_Server.CleanUp(); }

    /// <summary>
    /// </summary>
    void OnDestroy() { this._Message_Server.Destroy(); }

    #endregion

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <returns></returns>
    public override String ToString() {
      var c = this.SimulatorConfiguration.ToString();
      var e = this._Environments.FirstOrDefault().Value.ToString();

      return $"{c}, {e}";
    }
  }
}
