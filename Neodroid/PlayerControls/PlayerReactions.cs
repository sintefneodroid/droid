using System;
using System.Collections.Generic;
using droid.Neodroid.Managers;
using droid.Neodroid.Utilities.Messaging.Messages;
using droid.Neodroid.Utilities.ScriptableObjects;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

namespace droid.Neodroid.PlayerControls {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public class PlayerReactions : MonoBehaviour {
    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    NeodroidManager _manager;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    PlayerMotions _player_motions;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    bool _auto_reset = true;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    bool _debugging;

    /// <summary>
    ///
    /// </summary>
    EnvironmentState[] _states;
    List<MotorMotion> _motions = new List<MotorMotion>();

    #if UNITY_EDITOR
    [SerializeField] int _script_execution_order = -2000;
    #endif

    /// <summary>
    ///
    /// </summary>
    public static PlayerReactions Instance { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    /// <summary>
    ///
    /// </summary>
    void Awake() {
      if (Instance == null) {
        Instance = this;
      } else {
        Debug.LogWarning("WARNING! Multiple PlayerReactions in the scene! Only using " + Instance);
      }
      
      #if UNITY_EDITOR
      if (!Application.isPlaying) {
        var manager_script = MonoScript.FromMonoBehaviour(this);
        if (MonoImporter.GetExecutionOrder(manager_script) != this._script_execution_order) {
          MonoImporter.SetExecutionOrder(
              manager_script,
              this._script_execution_order); // Ensures that PreStep is called first, before all other scripts.
          Debug.LogWarning(
              "Execution Order changed, you will need to press play again to make everything function correctly!");
          EditorApplication.isPlaying = false;
          //TODO: UnityEngine.Experimental.LowLevel.PlayerLoop.SetPlayerLoop(new UnityEngine.Experimental.LowLevel.PlayerLoopSystem());
        }
      }
      #endif

    }
    

    /// <summary>
    ///
    /// </summary>
    void Start() {
      this._manager = FindObjectOfType<NeodroidManager>();
      if (Application.isPlaying) {
        var reset_reaction = new ReactionParameters(false, false, true, episode_count : false);
        this._states = this._manager.ReactAndCollectStates(new Reaction(reset_reaction, "all"));
      }
    }


    
    
    /// <summary>
    ///
    /// </summary>
    void Update() {
      if (Application.isPlaying) {
        if (this._states == null) {
          var reset_reaction = new ReactionParameters(false, false, true, episode_count : false);
          this._states = this._manager.ReactAndCollectStates(new Reaction(reset_reaction, "all"));
        }

        if (this._player_motions != null) {
          this._motions.Clear();
          if (this._player_motions._Motions != null) {
            foreach (var player_motion in this._player_motions._Motions) {
              if (Input.GetKey(player_motion._Key)) {
                if (this._debugging) {
                  Debug.Log($"{player_motion._Actor} {player_motion._Motor} {player_motion._Strength}");
                }

                var motion = new MotorMotion(
                    player_motion._Actor,
                    player_motion._Motor,
                    player_motion._Strength);
                this._motions.Add(motion);
              }
            }
          }

          var step = this._motions.Count > 0;
          if (step) {
            foreach (var state in this._states) {
              if (this._auto_reset && state.Terminated) {
                var reset_reaction =
                    new ReactionParameters(false, false, true, episode_count : false) {IsExternal = false};
                this._manager.ReactAndCollectStates(new Reaction(reset_reaction, state.EnvironmentName));
              }
            }

            var parameters = new ReactionParameters(true, true, episode_count : true) {IsExternal = false};
            var reaction = new Reaction(parameters, this._motions.ToArray(), null, null, null, "");
            this._states = this._manager.ReactAndCollectStates(reaction);
          }
        } else {
          if (this._debugging) {
            Debug.Log("No playermotions scriptable object assigned");
          }
        }
      }
    }
  }
}
