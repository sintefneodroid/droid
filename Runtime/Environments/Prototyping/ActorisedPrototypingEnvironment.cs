using System.Collections.Generic;
using System.Linq;
using droid.Runtime.GameObjects.StatusDisplayer.EventRecipients;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using UnityEngine;

namespace droid.Runtime.Environments.Prototyping {
  /// <inheritdoc cref="NeodroidEnvironment" />
  /// <summary>
  ///   Environment to be used with the prototyping components.
  /// </summary>
  [AddComponentMenu("Neodroid/Environments/ActorisedPrototypingEnvironment")]
  public class ActorisedPrototypingEnvironment : AbstractSpatialPrototypingEnvironment,
                                                 IActorisedPrototypingEnvironment {
    #region NeodroidCallbacks

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Clear() {
      this.Displayers.Clear();
      this.Configurables.Clear();
      this.Actors.Clear();
      this.Sensors.Clear();
      this.Listeners.Clear();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override Reaction SampleReaction() {
      if (this.Terminated) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("SampleReaction resetting environment");
        }
        #endif

        var reset_reaction = new ReactionParameters(StepResetObserve.Reset_, false, true);
        return new Reaction(reset_reaction, this.Identifier);
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"Sampling a reaction for environment {this.Identifier}");
      }
      #endif

      var sample_motions = new List<IMotion>();

      foreach (var actor in this.Actors) {
        var actor_value = actor.Value;
        if (actor_value?.Actuators != null) {
          foreach (var actuator in actor_value.Actuators) {
            var actuator_value = actuator.Value;
            if (actuator_value != null) {
              sample_motions.Add(new ActuatorMotion(actor.Key, actuator.Key, actuator_value.Sample()));
            }
          }
        }
      }

      var rp = new ReactionParameters(StepResetObserve.Step_, true, episode_count : true);
      return new Reaction(rp,
                          sample_motions.ToArray(),
                          null,
                          null,
                          null,
                          "",
                          this.Identifier);
    }

    #endregion

    #region PublicMethods

    #region Getters

    /// <summary>
    /// </summary>
    public SortedDictionary<string, IActor> Actors { get; } = new SortedDictionary<string, IActor>();

    #endregion

    #region Registration

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="actor"></param>
    public void Register(IActor actor) { this.Register(actor, actor.Identifier); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="identifier"></param>
    public void Register(IActor actor, string identifier) {
      if (!this.Actors.ContainsKey(identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} has registered actor {identifier}");
        }
        #endif

        this.Actors.Add(identifier, actor);
      } else {
        Debug.LogWarning($"WARNING! Please check for duplicates, Environment {this.name} already has actor {identifier} registered");
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="actor"></param>
    public void UnRegister(IActor actor) { this.UnRegister(actor, actor.Identifier); }

    public void UnRegister(IActor t, string obj) {
      if (this.Actors.ContainsKey(obj)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} unregistered actor {obj}");
        }
        #endif
        this.Actors.Remove(obj);
      }
    }

    #endregion

    #endregion

    #region PrivateMethods

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override EnvironmentSnapshot Snapshot() {
      lock (this._Reaction_Lock) {
        var signal = 0f;

        if (this.ObjectiveFunction != null) {
          signal = this.ObjectiveFunction.Evaluate();
        }

        EnvironmentDescription description = null;
        if (this.ReplyWithDescriptionThisStep
            || this.SimulationManager.SimulatorConfiguration.AlwaysSerialiseIndividualObservables) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log("Describing Environment");
          }
          #endif

          description = new EnvironmentDescription(this.ObjectiveFunction,
                                                   this.Actors,
                                                   this.Configurables,
                                                   this.Sensors,
                                                   this.Displayers);
        }

        this._Observables.Clear();

        foreach (var item in this.Sensors) {
          if (item.Value != null) {
            if (item.Value.FloatEnumerable != null) {
              this._Observables.AddRange(item.Value.FloatEnumerable);
            } else {
              #if NEODROID_DEBUG
              if (this.Debugging) {
                Debug.Log($"Sensor with key {item.Key} has a null FloatEnumerable value");
              }
              #endif
            }
          } else {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log($"Sensor with key {item.Key} has a null value");
            }
            #endif
          }
        }

        var obs = this._Observables.ToArray();

        var time = Time.realtimeSinceStartup - this.LastResetTime;

        var state = new EnvironmentSnapshot(this.Identifier,
                                         this.StepI,
                                         time,
                                         signal,
                                         this.Terminated,
                                         ref obs,
                                         this.LastTerminationReason,
                                         description);

        if (this.SimulationManager.SimulatorConfiguration.AlwaysSerialiseUnobservables
            || this.ReplyWithDescriptionThisStep) {
          state.Unobservables = new Unobservables(ref this._Tracked_Rigid_Bodies, ref this._Poses);
        }

        return state;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="recipient"></param>
    public override void ObservationsString(DataPoller recipient) {
      recipient.PollData(string.Join("\n\n",
                                     this.Sensors.Values.Select(e => $"{e.Identifier}:\n{e.ToString()}")));
    }

    /// <summary>
    ///
    /// </summary>
    public override void RemotePostSetup() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("PostSetup");
      }
      #endif

      foreach (var configurable in this.Configurables.Values) {
        configurable?.RemotePostSetup();
      }

      foreach (var actor in this.Actors.Values) {
        actor?.RemotePostSetup();
      }

      foreach (var sensor in this.Sensors.Values) {
        sensor?.RemotePostSetup();
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="reaction"></param>
    void SendToDisplayers(Reaction reaction) {
      if (reaction.Displayables != null && reaction.Displayables.Length > 0) {
        foreach (var displayable in reaction.Displayables) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log("Applying " + displayable + " To " + this.name + "'s displayers");
          }
          #endif
          var displayable_name = displayable.DisplayableName;
          if (this.Displayers.ContainsKey(displayable_name) && this.Displayers[displayable_name] != null) {
            var v = displayable.DisplayableValue;
            this.Displayers[displayable_name].Display(v);
          } else {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log("Could find not displayer with the specified name: " + displayable_name);
            }
            #endif
          }
        }
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="reaction"></param>
    protected override void SendToActors(Reaction reaction) {
      if (reaction.Motions != null && reaction.Motions.Length > 0) {
        foreach (var motion in reaction.Motions) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log("Applying " + motion + " To " + this.name + "'s actors");
          }
          #endif
          var motion_actor_name = motion.ActorName;
          if (this.Actors.ContainsKey(motion_actor_name) && this.Actors[motion_actor_name] != null) {
            this.Actors[motion_actor_name].ApplyMotion(motion);
          } else {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log("Could find not actor with the specified name: " + motion_actor_name);
            }
            #endif
          }
        }
      }
    }

    /// <summary>
    /// </summary>
    protected override void InnerResetRegisteredObjects() {
      foreach (var actor in this.Actors.Values) {
        actor?.PrototypingReset();
      }
    }

    #endregion
  }
}
