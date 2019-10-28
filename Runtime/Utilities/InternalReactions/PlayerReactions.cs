using System;
using System.Collections.Generic;
using System.Linq;
using droid.Runtime.Interfaces;
using droid.Runtime.Managers;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.ScriptableObjects;
using UnityEngine;

namespace droid.Runtime.Utilities.InternalReactions {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public class PlayerReactions : ScriptedReactions {
    /// <summary>
    /// </summary>
    [SerializeField]
    bool _auto_reset = true;

    /// <summary>
    /// </summary>
    [SerializeField]
    PlayerMotions _player_motions = null;

    [SerializeField] Boolean terminated;

    /// <summary>
    /// </summary>
    void Start() {
      this._Manager = FindObjectOfType<AbstractNeodroidManager>();
      if (Application.isPlaying) {
        var reset_reaction = new ReactionParameters(StepResetObserve.Reset_);
        this._Manager.SendToEnvironments(new[] {new Reaction(reset_reaction, "all")});
      }
    }

    void Update() {
      if (Application.isPlaying) {
        if (this._player_motions != null) {
          var motions = new List<IMotion>();
          if (this._player_motions._Motions != null) {
            foreach (var player_motion in this._player_motions._Motions) {
              if (Input.GetKey(player_motion._Key)) {
                #if NEODROID_DEBUG
                if (this.Debugging) {
                  Debug.Log($"{player_motion._Actor} {player_motion._Actuator} {player_motion._Strength}");
                }
                #endif

                if (player_motion._Actuator == "Reset") {
                  this.terminated = true;
                  break;
                }

                var motion = new ActuatorMotion(player_motion._Actor,
                                                player_motion._Actuator,
                                                player_motion._Strength);
                motions.Add(motion);
              }
            }
          }

          if (this.terminated && this._auto_reset) {
            var reset_reaction_parameters = new ReactionParameters(StepResetObserve.Reset_);
            this._Manager.SendToEnvironments(new[] {new Reaction(reset_reaction_parameters, "all")});
            this.terminated = this._Manager.GatherSnapshots().Any(e => e.Terminated);
          } else if (motions.Count > 0) {
            var parameters = new ReactionParameters(StepResetObserve.Step_, true, episode_count : true);
            var reaction = new Reaction(parameters,
                                        motions.ToArray(),
                                        null,
                                        null,
                                        null,
                                        "",
                                        reaction_source : "PlayerReactions");
            this._Manager.SendToEnvironments(new[] {reaction});
            this.terminated = this._Manager.GatherSnapshots().Any(e => e.Terminated);
            motions.Clear();
          }
        }
      } else {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("No PlayerMotions ScriptableObject assigned");
        }
        #endif
      }
    }
  }
}
