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
  public class PlayerReactions : ScriptedReactions {


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
    EnvironmentState[] _states;

    List<MotorMotion> _motions = new List<MotorMotion>();



    /// <summary>
    ///
    /// </summary>
    void Start() {
      this._Manager = FindObjectOfType<NeodroidManager>();
      if (Application.isPlaying) {
        var reset_reaction = new ReactionParameters(reset: true);
        this._states = this._Manager.ReactAndCollectStates(new Reaction(reset_reaction, "all"));
      }
    }

    void Update() { 
      if (Application.isPlaying) {
        if (this._states == null) {
          var reset_reaction_parameters = new ReactionParameters(reset: true);
          this._states = this._Manager.ReactAndCollectStates(new Reaction(reset_reaction_parameters, "all"));
        }

        var reset = false;
        if (this._player_motions != null) {
          this._motions.Clear();
          if (this._player_motions._Motions != null) {
            foreach (var player_motion in this._player_motions._Motions) {
              if (Input.GetKey(player_motion._Key)) {
                if (this.Debugging) {
                  Debug.Log($"{player_motion._Actor} {player_motion._Motor} {player_motion._Strength}");
                }

                if (player_motion._Motor=="Reset") {
                  reset = true;
                  break;
                } else {
                  var motion = new MotorMotion(
                      player_motion._Actor,
                      player_motion._Motor,
                      player_motion._Strength);
                  this._motions.Add(motion);
                }
              }
            }
          }

          if (reset) {
            var reset_reaction_parameters = new ReactionParameters(reset: true);
            this._states = this._Manager.ReactAndCollectStates(new Reaction(reset_reaction_parameters, "all"));
          } else {
            var step = this._motions.Count > 0;
            if (step) {
              foreach (var state in this._states) {
                if (this._auto_reset && state.Terminated) {
                  var reset_reaction =
                      new ReactionParameters(reset: true) {IsExternal = false};
                  this._Manager.ReactAndCollectStates(new Reaction(reset_reaction, state.EnvironmentName));
                }
              }

              var parameters = new ReactionParameters(terminable:true, step: true, episode_count : true) {IsExternal = false};
              var reaction = new Reaction(parameters, this._motions.ToArray(), null, null, null, "");
              this._states = this._Manager.ReactAndCollectStates(reaction);
            }

          }
        } else {
          if (this.Debugging) {
            Debug.Log("No playermotions scriptable object assigned");
          }
        }
      }
    }
  }
}
