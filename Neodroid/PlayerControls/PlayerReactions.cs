using System.Collections.Generic;
using droid.Neodroid.Managers;
using droid.Neodroid.Utilities.Messaging.Messages;
using droid.Neodroid.Utilities.ScriptableObjects;
using UnityEngine;

namespace droid.Neodroid.PlayerControls {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
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
    }

    /// <summary>
    ///
    /// </summary>
    void Start() {
      this._manager = FindObjectOfType<NeodroidManager>();
      var reset_reaction =
          new ReactionParameters(false, false, true, episode_count : true);
      this._states =
          this._manager.ReactAndCollectStates(
              new Reaction(reset_reaction, "all"));
    }

    /// <summary>
    ///
    /// </summary>
    void Update() {
      if (this._states == null) {
        var reset_reaction =
            new ReactionParameters(false, false, true, episode_count : true);
        this._states =
            this._manager.ReactAndCollectStates(
                new Reaction(reset_reaction, "all"));
      }

      if (this._player_motions != null) {
        var motions = new List<MotorMotion>();
        if(this._player_motions._Motions!=null){
          foreach (var player_motion in this._player_motions._Motions) {
            if (Input.GetKey(player_motion._Key)) {
              if (this._debugging) {
                Debug.Log($"{player_motion._Actor} {player_motion._Motor} {player_motion._Strength}");
              }

              var motion = new MotorMotion(
                  player_motion._Actor,
                  player_motion._Motor,
                  player_motion._Strength);
              motions.Add(motion);
            }
          }
        }

        var step = motions.Count > 0;
        if (step) {
          foreach (var state in this._states) {
            if (this._auto_reset && state.Terminated) {
              var reset_reaction =
                  new ReactionParameters(
                      false,
                      false,
                      true,
                      episode_count : true) {IsExternal = false};
              this._manager.ReactAndCollectStates(
                  new Reaction(reset_reaction, state.EnvironmentName));
            }
          }

          var parameters =
              new ReactionParameters(true, true, episode_count : true) {
                  IsExternal = false
              };
          var reaction = new Reaction(
              parameters,
              motions.ToArray(),
              null,
              null,
              null,
              "");
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
