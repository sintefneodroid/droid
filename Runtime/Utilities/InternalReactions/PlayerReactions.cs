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
        var reset_reaction = new ReactionParameters(reaction_type : ReactionTypeEnum.Reset_);
        this._Manager.DelegateReactions(reactions : new[] {
                                                              new Reaction(reaction_parameters :
                                                                           reset_reaction,
                                                                           "all")
                                                          });
      }
    }

    void Update() {
      if (Application.isPlaying) {
        if (this._player_motions != null) {
          var motions = new List<IMotion>();
          if (this._player_motions._Motions != null) {
            for (var index = 0; index < this._player_motions._Motions.Length; index++) {
              var player_motion = this._player_motions._Motions[index];
              if (Input.GetKey(key : player_motion._Key)) {
                #if NEODROID_DEBUG
                if (this.Debugging) {
                  Debug.Log(message :
                            $"{player_motion._Actor} {player_motion._Actuator} {player_motion._Strength}");
                }
                #endif

                if (player_motion._Actuator == "Reset") {
                  this.terminated = true;
                  break;
                }

                var motion = new ActuatorMotion(actor_name : player_motion._Actor,
                                                actuator_name : player_motion._Actuator,
                                                strength : player_motion._Strength);
                motions.Add(item : motion);
              }
            }
          }

          if (this.terminated && this._auto_reset) {
            var reset_reaction_parameters = new ReactionParameters(reaction_type : ReactionTypeEnum.Reset_);
            this._Manager.DelegateReactions(reactions : new[] {
                                                                  new Reaction(reaction_parameters :
                                                                               reset_reaction_parameters,
                                                                               "all")
                                                              });
            var any = false;
            var es = this._Manager.GatherSnapshots();
            for (var index = 0; index < es.Length; index++) {
              var e = es[index];
              if (e.Terminated) {
                any = true;
                break;
              }
            }

            this.terminated = any;
          } else if (motions.Count > 0) {
            var parameters =
                new ReactionParameters(reaction_type : ReactionTypeEnum.Step_, true, episode_count : true);
            var reaction = new Reaction(parameters : parameters,
                                        motions : motions.ToArray(),
                                        null,
                                        null,
                                        null,
                                        "",
                                        reaction_source : "PlayerReactions");
            this._Manager.DelegateReactions(reactions : new[] {reaction});
            var any = false;
            var es = this._Manager.GatherSnapshots();
            for (var index = 0; index < es.Length; index++) {
              var e = es[index];
              if (e.Terminated) {
                any = true;
                break;
              }
            }

            this.terminated = any;
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
