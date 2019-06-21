using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Prototyping.Actors;
using droid.Runtime.Prototyping.Evaluation;
using droid.Runtime.Utilities.Enums;
using droid.Runtime.Utilities.GameObjects.BoundingBoxes;
using droid.Runtime.Utilities.GameObjects.StatusDisplayer.EventRecipients.droid.Neodroid.Utilities.Unsorted;
using droid.Runtime.Utilities.Misc;
using droid.Runtime.Utilities.Misc.Extensions;
using UnityEngine;
using Object = System.Object;

namespace droid.Runtime.Environments {
  /// <inheritdoc cref="NeodroidEnvironment" />
  /// <summary>
  ///   Environment to be used with the prototyping components.
  /// </summary>
  [AddComponentMenu("Neodroid/Environments/PrototypingEnvironment")]
  public class PrototypingEnvironment : AbstractPrototypingEnvironment,
                                        IPrototypingEnvironment {
    #region NeodroidCallbacks

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Clear() {
      this.Displayers.Clear();
      this.Configurables.Clear();
      this.Actuators.Clear();
      this.Sensors.Clear();
      this.Listeners.Clear();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override Reaction SampleReaction() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"Sampling a reaction for environment {this.Identifier}");
      }
      #endif

      this._Sample_Motions.Clear();

      foreach (var actuator in this.Actuators) {
        var actor_value = actuator.Value;

        var actuator_value = actuator.Value;
        if (actuator_value != null) {
          this._Sample_Motions.Add(new ActuatorMotion(actuator.Key, actuator.Key, actuator_value.Sample()));
        }
      }

      if (this._Terminated) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("SampleReaction resetting environment");
        }
        #endif

        var reset_reaction =
            new ReactionParameters(false, false, true, episode_count : true) {IsExternal = false};
        return new Reaction(reset_reaction, this.Identifier);
      }

      var rp = new ReactionParameters(true, true, episode_count : true) {IsExternal = false};
      return new Reaction(rp, this._Sample_Motions.ToArray(), null, null, null, "", this.Identifier);
    }

    #endregion

    #region PublicMethods

    #region Getters

    /// <summary>
    /// </summary>
    public SortedDictionary<string, IActuator> Actuators { get; } = new SortedDictionary<string, IActuator>();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "PrototypingEnvironment"; } }

    #endregion

    #region Registration

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    public void Register(IActuator obj) { this.Register(obj, obj.Identifier); }

    public void Register(IActuator obj, String identifier) {
      if (!this.Actuators.ContainsKey(identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} has registered actuator {identifier}");
        }
        #endif

        this.Actuators.Add(identifier, obj);
      } else {
        Debug.LogWarning($"WARNING! Please check for duplicates, Environment {this.name} already has actuator {identifier} registered");
      }
    }

    public void UnRegister(IActuator obj) { this.UnRegister(obj, obj.Identifier); }

    public void UnRegister(IActuator t, String obj) {
      if (this.Actuators.ContainsKey(obj)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} unregistered observer {obj}");
        }
        #endif
        this.Actuators.Remove(obj);
      }
    }

    #endregion

    #endregion

    #region PrivateMethods

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override EnvironmentState CollectState() {
      lock (this._Reaction_Lock) {

        var signal = 0f;

        if (this._objective_function != null) {
          signal = this._objective_function.Evaluate();
        }

        EnvironmentDescription description = null;
        if (this._ReplyWithDescriptionThisStep) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log("Describing Environment");
          }
          #endif
          var threshold = 0f;
          if (this._objective_function != null) {
            threshold = this._objective_function.SolvedThreshold;
          }

          var episode_length = 0;
          if (this._objective_function) {
            episode_length = this._objective_function.EpisodeLength;
          }

          var virtual_actors =
              new SortedDictionary<String, IActor> {{"All", new VirtualActor(this.Actuators)}};

          description = new EnvironmentDescription(episode_length,
                                                   virtual_actors,
                                                   this.Configurables,
                                                   this.Sensors,
                                                   threshold);
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

        var time = Time.time - this._Lastest_Reset_Time;

        var state = new EnvironmentState(this.Identifier,
                                         this.CurrentFrameNumber,
                                         time,
                                         signal,
                                         this._Terminated,
                                         ref obs,
                                         this.LastTerminationReason,
                                         description);

        if (this._Simulation_Manager.SimulatorConfiguration.AlwaysSerialiseUnobservables
            || this._ReplyWithDescriptionThisStep) {
          state.Unobservables = new Unobservables(ref this._tracked_rigid_bodies, ref this._Poses);
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
    /// </summary>
    /// <param name="reaction"></param>
    void SendToActuators(Reaction reaction) {
      if (reaction.Motions != null && reaction.Motions.Length > 0) {
        foreach (var motion in reaction.Motions) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log("Applying " + motion + " To " + this.name + " actuator");
          }
          #endif
          var motion_actuator_name = motion.ActuatorName;
          if (this.Actuators.ContainsKey(motion_actuator_name)
              && this.Actuators[motion_actuator_name] != null) {
            this.Actuators[motion_actuator_name].ApplyMotion(motion);
          } else {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log("Could find not actuator with the specified name: " + motion_actuator_name);
            }
            #endif
          }
        }
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="reaction"></param>
    protected override void InnerStep(Reaction reaction) { this.SendToActuators(reaction); }

    /// <summary>
    /// </summary>
    protected override void InnerResetRegisteredObjects() {
      foreach (var actuator in this.Actuators.Values) {
        actuator?.EnvironmentReset();
      }
    }

    #endregion
  }
}
