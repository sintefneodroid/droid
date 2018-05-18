using System;
using System.Collections;
using System.Collections.Generic;
using droid.Neodroid.Environments;
using droid.Neodroid.Utilities.Interfaces;
using droid.Neodroid.Utilities.Messaging;
using droid.Neodroid.Utilities.Messaging.Messages;
using droid.Neodroid.Utilities.ScriptableObjects;
using UnityEditor;
using UnityEngine;
using Object = System.Object;
using SimulatorConfiguration = droid.Neodroid.Utilities.ScriptableObjects.SimulatorConfiguration;

#if UNITY_EDITOR

#endif

namespace droid.Neodroid.Managers {
  /// <inheritdoc cref="UnityEngine.MonoBehaviour" />
  /// <summary>
  /// </summary>
  [AddComponentMenu("Neodroid/Managers/VanillaManager")]
  public abstract class NeodroidManager : MonoBehaviour,
                                 IHasRegister<NeodroidEnvironment> {
    #region PrivateFields

    /// <summary>
    ///
    /// </summary>
    [Header("Development", order = 110)]
    [SerializeField]
    bool _debugging;

    /// <summary>
    ///
    /// </summary>
    Object _send_lock = new Object();

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    bool _testing_motors;

    #if UNITY_EDITOR
    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    int _script_execution_order = -9999;
#endif

    /// <summary>
    ///
    /// </summary>
    [Header("Simulation", order = 80)]
    [SerializeField]
    SimulatorConfiguration _configuration;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    int _skip_frame_i;

    /// <summary>
    ///
    /// </summary>
    bool _syncing_environments;

    /// <summary>
    ///
    /// </summary>
    [Header("Connection", order = 90)]
    [SerializeField]
    string _ip_address = "localhost";

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    int _port = 6969;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    bool _awaiting_reply;

    #endregion

    /// <summary>
    ///
    /// </summary>
    public static NeodroidManager Instance { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public SimulatorConfiguration Configuration {
      get {
        if (this._configuration == null) {
          this._configuration = ScriptableObject.CreateInstance<SimulatorConfiguration>();
        }

        return this._configuration;
      }
      set { this._configuration = value; }
    }

    /// <summary>
    /// Can be subscribed to for pre fixed update events (Will be called before any FixedUpdate on any script)
    /// </summary>
    public event Action EarlyFixedUpdateEvent;

    /// <summary>
    ///
    /// </summary>
    public event Action FixedUpdateEvent;

    /// <summary>
    ///
    /// </summary>
    public event Action LateFixedUpdateEvent;

    /// <summary>
    /// Can be subscribed to for pre update events (Will be called before any Update on any script)
    /// </summary>
    public event Action EarlyUpdateEvent;

    /// <summary>
    ///
    /// </summary>
    public event Action UpdateEvent;

    /// <summary>
    ///
    /// </summary>
    public event Action LateUpdateEvent;

    /// <summary>
    ///
    /// </summary>
    public event Action OnPostRenderEvent;

    /// <summary>
    ///
    /// </summary>
    public event Action OnRenderImageEvent;

    /// <summary>
    ///
    /// </summary>
    public event Action OnEndOfFrameEvent;

    /// <summary>
    ///
    /// </summary>
    public event Action OnReceiveEvent;

    /// <summary>
    ///
    /// </summary>
    void FetchCommmandLineArguments() {
      var arguments = Environment.GetCommandLineArgs();

      for (var i = 0; i < arguments.Length; i++) {
        if (arguments[i] == "-ip") {
          this.IpAddress = arguments[i + 1];
        }

        if (arguments[i] == "-port") {
          this.Port = int.Parse(arguments[i + 1]);
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    void CreateMessagingServer() {
      try {
        if (this.IpAddress != "" || this.Port != 0) {
          this._Message_Server = new MessageServer(
              this.IpAddress,
              this.Port,
              false,
              this.Debugging);
        } else {
          this._Message_Server = new MessageServer(this.Debugging);
        }
      } catch (Exception exception) {
        Debug.Log(exception);
        throw;

        //TODO: close application is port is already in use.
      }
    }

    /// <summary>
    ///
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

      this._Message_Server.ListenForClientToConnect(this.OnDebugCallback);
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Started Messaging Server");
      }
      #endif

      if (threaded) {
        this.OnListeningCallback();
      }

      //}
    }

    #region Getter And Setters

    /// <summary>
    ///
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
    ///
    /// </summary>
    public float SimulationTime {
      get { return Time.timeScale; }
      set {
        Time.timeScale = value;
        if (this.Configuration.UpdateFixedTimeScale) {
          Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }
      }
    }

    public bool HasStepped { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool TestMotors { get { return this._testing_motors; } set { this._testing_motors = value; } }

    /// <summary>
    ///
    /// </summary>
    public string IpAddress { get { return this._ip_address; } set { this._ip_address = value; } }

    /// <summary>
    ///
    /// </summary>
    public int Port { get { return this._port; } set { this._port = value; } }

    /// <summary>
    ///
    /// </summary>
    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    /// <summary>
    ///
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
    public bool IsSyncingEnvironments {
      get { return this._syncing_environments; }
      set { this._syncing_environments = value; }
    }

    #endregion

    #region PrivateMembers

    /// <summary>
    ///
    /// </summary>
    protected Dictionary<string, NeodroidEnvironment> _Environments =
        new Dictionary<string, NeodroidEnvironment>();

    /// <summary>
    ///
    /// </summary>
    public void Clear() { this._Environments = new Dictionary<string, NeodroidEnvironment>(); }

    /// <summary>
    ///
    /// </summary>
    protected MessageServer _Message_Server;

    /// <summary>
    ///
    /// </summary>
    protected Reaction[] _Current_Reactions =
        new Reaction[] { };

    #endregion

    #region UnityCallbacks

    /// <summary>
    ///
    /// </summary>
    protected void Awake() {
      if (Instance == null) {
        Instance = this;
      } else if (Instance == this) {
        #if NEODROID_DEBUG
        if(this.Debugging){
        Debug.Log("Using " + Instance);
}
        #endif
      }else {
        Debug.LogWarning("WARNING! There are multiple SimulationManagers in the scene! Only using " + Instance);
      }

      #if UNITY_EDITOR
      var manager_script = MonoScript.FromMonoBehaviour(this);
      if (MonoImporter.GetExecutionOrder(manager_script) != this._script_execution_order) {
        MonoImporter.SetExecutionOrder(
            manager_script,
            this._script_execution_order); // Ensures that PreStep is called first, before all other scripts.
        Debug.LogWarning(
            "Execution Order changed, you will need to restart to make everything function correctly!");
        EditorApplication.isPlaying = false;
        //TODO: UnityEngine.Experimental.LowLevel.PlayerLoop.SetPlayerLoop(new UnityEngine.Experimental.LowLevel.PlayerLoopSystem());
      }
      #endif
    }

    /// <summary>
    ///
    /// </summary>
    protected void Start() {
      this.FetchCommmandLineArguments();

      if (this.Configuration == null) {
        this.Configuration = ScriptableObject.CreateInstance<SimulatorConfiguration>();
      }

      this.ApplyConfigurationToUnity(this.Configuration);

      if (this.Configuration.SimulationType == SimulationType.Physics_dependent_) {
        this.EarlyFixedUpdateEvent += this.PreStep;
        this.FixedUpdateEvent += this.Step;
        this.FixedUpdateEvent += this.Tick;
        this.LateFixedUpdateEvent += this.PostStep;
        this.StartCoroutine(this.LateFixedUpdateEventGenerator());
      } else {
        this.EarlyUpdateEvent += this.PreStep;
        this.UpdateEvent += this.Step;
        this.UpdateEvent += this.Tick;
        switch (this.Configuration.FrameFinishes) {
          case FrameFinishes.Late_update_:
            this.LateUpdateEvent += this.PostStep;
            break;
          case FrameFinishes.On_post_render_:
            this.OnPostRenderEvent += this.PostStep;
            break;
          case FrameFinishes.On_render_image_:
            this.OnRenderImageEvent += this.PostStep;
            break;
          case FrameFinishes.End_of_frame_:
            this.StartCoroutine(this.EndOfFrameEventGenerator());
            this.OnEndOfFrameEvent += this.PostStep;
            break;
          default: throw new ArgumentOutOfRangeException();
        }
      }

      this.CreateMessagingServer();
      if (this.Configuration.SimulationType == SimulationType.Physics_dependent_) {
        this.StartMessagingServer(); // Remember to manually bind receive to an event in a derivation
      } else {
        this.StartMessagingServer(threaded : true);
      }
    }

    /// <summary>
    ///
    /// </summary>
    public void ApplyConfigurationToUnity(SimulatorConfiguration configuration) {
      QualitySettings.SetQualityLevel(configuration.QualityLevel, true);
      this.SimulationTime = configuration.TimeScale;
      Application.targetFrameRate = configuration.TargetFrameRate;
      QualitySettings.vSyncCount = 0;

      #if !UNITY_EDITOR
      Screen.SetResolution(
          width : configuration.Width,
          height : configuration.Height,
          fullscreen : configuration.FullScreen);
      #endif
    }

    /// <summary>
    ///
    /// </summary>
    void OnPostRender() { this.OnPostRenderEvent?.Invoke(); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="src"></param>
    /// <param name="dest"></param>
    void OnRenderImage(RenderTexture src, RenderTexture dest) {
      this.OnRenderImageEvent?.Invoke(); //TODO: May not work
    }

    /// <summary>
    ///
    /// </summary>
    protected void FixedUpdate() {
      this.EarlyFixedUpdateEvent?.Invoke();
      this.FixedUpdateEvent?.Invoke();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    IEnumerator LateFixedUpdateEventGenerator() {
      while (true) {
        yield return new WaitForFixedUpdate();
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
    ///
    /// </summary>
    /// <returns></returns>
    protected IEnumerator EndOfFrameEventGenerator() {
      while (true) {
        yield return new WaitForEndOfFrame();
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("LateFixedUpdate");
        }
        #endif
        this.OnEndOfFrameEvent?.Invoke();
      }

      // ReSharper disable once IteratorNeverReturns
    }

    /// <summary>
    ///
    /// </summary>
    protected void Update() {
      this.EarlyUpdateEvent?.Invoke();
      this.UpdateEvent?.Invoke();
    }

    /// <summary>
    ///
    /// </summary>
    protected void LateUpdate() { this.LateUpdateEvent?.Invoke(); }

    #endregion

    #region PrivateMethods

    /// <summary>
    ///
    /// </summary>
    protected void PreStep() {
      if (this.Configuration.StepExecutionPhase == ExecutionPhase.Before_) {
        this.ExecuteStep();
      }
    }

    /// <summary>
    ///
    /// </summary>
    protected void Step() {
      if (this.Configuration.StepExecutionPhase == ExecutionPhase.Middle_) {
        this.ExecuteStep();
      }
    }

    /// <summary>
    ///
    /// </summary>
    protected void Tick() {
      if (this.TestMotors) {
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
    ///
    /// </summary>
    void ExecuteStep() {
      if (!this._syncing_environments) {
        this.React(this.CurrentReactions);
        this.ClearCurrentReactions();
      }

      if (this.AwaitingReply) {
        var states = this.CollectStates();
        this.PostReact(states);
      }

      foreach (var environment in this._Environments.Values) {
        environment.PostStep();
      }
    }

    /// <summary>
    ///
    /// </summary>
    protected void PostStep() {
      if (this.Configuration.StepExecutionPhase == ExecutionPhase.After_) {
        this.ExecuteStep();
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="states"></param>
    protected void PostReact(EnvironmentState[] states) {
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
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    protected Reaction[] SampleRandomReactions() {
      var reactions = new List<Reaction>();
      foreach (var environment in this._Environments.Values) {
        reactions.Add(environment.SampleReaction());
      }

      return reactions.ToArray();
    }

    //TODO: EnvironmentState[][] states for aggregation of states
    /// <summary>
    ///
    /// </summary>
    /// <param name="states"></param>
    void Reply(EnvironmentState[] states) {
      lock (this._send_lock) {
        this._Message_Server.SendStates(states);
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("Replying");
        }

        #endif
      }
    }

    /// <summary>
    ///
    /// </summary>
    void ClearCurrentReactions() { this.CurrentReactions = new Reaction[] { }; }

    #endregion

    #region PublicMethods

    /// <summary>
    ///
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public EnvironmentState[] ReactAndCollectStates(
        Reaction reaction) {
      var states = new EnvironmentState[this._Environments.Values.Count];
      var i = 0;
      foreach (var environment in this._Environments.Values) {
        if (reaction.RecipientEnvironment != "all") {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log($"Applying reaction to {reaction.RecipientEnvironment} environment");
          }
          #endif
          if (this._Environments.ContainsKey(reaction.RecipientEnvironment)) {
            states[i++] = this._Environments[reaction.RecipientEnvironment].ReactAndCollectState(reaction);
          }
          #if NEODROID_DEBUG
          else {

          if (this.Debugging) {
            Debug.Log($"Could not find environment: {reaction.RecipientEnvironment}");
          }

          }
          #endif
        } else {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log("Applying reaction to all environments");
          }
          #endif
          states[i++] = environment.ReactAndCollectState(reaction);
        }
      }

      return states;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public void React(Reaction reaction) {
      if (this._Environments.ContainsKey(reaction.RecipientEnvironment)) {
        this._Environments[reaction.RecipientEnvironment].React(reaction);
      } else {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Could not find an environment with the identifier: {reaction.RecipientEnvironment}");
        }
        #endif

        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Applying to all environments");
        }
        #endif

        foreach (var environment in this._Environments.Values) {
          environment.React(reaction);
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="reactions"></param>
    /// <returns></returns>
    public void React(Reaction[] reactions) {
      foreach (var reaction in reactions) {
        if (this._Environments.ContainsKey(reaction.RecipientEnvironment)) {
          this._Environments[reaction.RecipientEnvironment].React(reaction);
        } else {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log($"Could not find an environment with the identifier: {reaction.RecipientEnvironment}");
          }
          #endif

          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log($"Applying to all environments");
          }
          #endif

          foreach (var environment in this._Environments.Values) {
            environment.React(reaction);
          }
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public EnvironmentState[] CollectStates() {
      var environments = this._Environments.Values;
      var states = new EnvironmentState[environments.Count];
      var i = 0;
      foreach (var environment in environments) {
        states[i++] = environment.CollectState();
      }

      return states;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="reactions"></param>
    /// <returns></returns>
    public EnvironmentState[] ReactAndCollectStates(
        Reaction[] reactions) {
      var states =
          new EnvironmentState[reactions.Length * this._Environments.Count];
      var i = 0;
      foreach (var reaction in reactions) {
        if (this._Environments.ContainsKey(reaction.RecipientEnvironment)) {
          states[i++] = this._Environments[reaction.RecipientEnvironment].ReactAndCollectState(reaction);
        } else {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log($"Could not find an environment with the identifier: {reaction.RecipientEnvironment}");
          }
          #endif

          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log($"Applying to all environments");
          }
          #endif

          foreach (var environment in this._Environments.Values) {
            states[i++] = environment.ReactAndCollectState(reaction);
          }
        }
      }

      return states;
    }

    /// <summary>
    ///
    /// </summary>
    public void ResetAllEnvironments() {
      this.React(
          new Reaction(
              new ReactionParameters(true, false, true, episode_count : true),
              null,
              null,
              null,
              null,
              ""));
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public string GetStatus() {
      return this._Message_Server._Listening_For_Clients ? "Connected" : "Not Connected";
    }

    #endregion

    #region Registration

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="environment"></param>
    public void Register(NeodroidEnvironment environment) {
      this.Register(environment, environment.Identifier);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="environment"></param>
    /// <param name="identifier"></param>
    public void Register(NeodroidEnvironment environment, string identifier) {
      if (!this._Environments.ContainsKey(identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Manager {this.name} already has an environment with the identifier: {identifier}");
        }
        #endif

        this._Environments.Add(identifier, environment);
      } else {
        Debug.LogWarning(
            $"WARNING! Please check for duplicates, SimulationManager {this.name} already has envi"
            + $"ronment {identifier} registered");
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="identifier"></param>
    public void UnRegisterEnvironment(string identifier) {
      if (this._Environments.ContainsKey(identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"SimulationManager {this.name} unregistered enviroment {identifier}");
        }
        #endif

        this._Environments.Remove(identifier);
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="neodroid_environment"></param>
    public void UnRegister(NeodroidEnvironment neodroid_environment) {
      this.UnRegisterEnvironment(neodroid_environment.Identifier);
    }

    #endregion

    #region MessageServerCallbacks

    ///  <summary>
    ///
    ///  </summary>
    /// <param name="reactions"></param>
    void OnReceiveCallback(Reaction[] reactions) {
      lock (this._send_lock) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("Received: " + reactions);
        }
        #endif

        this.SetReactionsFromExternalSource(reactions);

        this.OnReceiveEvent?.Invoke();
      }
    }

    ///  <summary>
    ///
    ///  </summary>
    /// <param name="reactions"></param>
    protected void SetReactionsFromExternalSource(Reaction[] reactions) {
      lock (this._send_lock) {
        if (reactions != null) {
          this.CurrentReactions = reactions;
          foreach (var current_reaction in this.CurrentReactions) {
            current_reaction.Parameters.IsExternal = true;
          }
          this.Configuration.StepExecutionPhase = this.CurrentReactions[0].Parameters.Phase;
          this.AwaitingReply = true;
          this.HasStepped = false;
        } else {
          Debug.LogWarning("Reaction was null");
        }
      }
    }



    /// <summary>
    ///
    /// </summary>
    void OnDisconnectCallback() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Client disconnected.");
      }
      #endif
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="error"></param>
    void OnDebugCallback(string error) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        //Debug.Log("DebugCallback: " + error);
      }
      #endif
    }

    /// <summary>
    ///
    /// </summary>
    void OnListeningCallback() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Client connected");
      }
      #endif

      this._Message_Server.StartReceiving(
          this.OnReceiveCallback,
          this.OnDisconnectCallback,
          this.OnDebugCallback);
    }

    #endregion

    #region Deconstruction

    /// <summary>
    ///
    /// </summary>
    void OnApplicationQuit() { this._Message_Server.CleanUp(); }

    /// <summary>
    ///
    /// </summary>
    void OnDestroy() { this._Message_Server.Destroy(); }

    #endregion
  }
}
