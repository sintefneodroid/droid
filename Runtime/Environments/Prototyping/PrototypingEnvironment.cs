using System;
using System.Collections.Generic;
using System.Linq;
using droid.Runtime.GameObjects.StatusDisplayer.EventRecipients;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Prototyping.Actors;
using UnityEngine;

namespace droid.Runtime.Environments.Prototyping {
  /// <inheritdoc cref="NeodroidEnvironment" />
  /// <summary>
  ///   Environment to be used with the prototyping components.
  /// </summary>
  [AddComponentMenu("Neodroid/Environments/PrototypingEnvironment")]
  public class PrototypingEnvironment : AbstractSpatialPrototypingEnvironment,
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

      foreach (var actuator in this.Actuators) {
        var actuator_value = actuator.Value;
        if (actuator_value != null) {
          sample_motions.Add(new ActuatorMotion(actuator.Key, actuator.Key, actuator_value.Sample()));
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

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="identifier"></param>
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

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    public void UnRegister(IActuator obj) { this.UnRegister(obj, obj.Identifier); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="t"></param>
    /// <param name="obj"></param>
    public void UnRegister(IActuator t, String obj) {
      if (this.Actuators.ContainsKey(obj)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Environment {this.name} unregistered actuator {obj}");
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

          var virtual_actors =
              new SortedDictionary<String, IActor> {{"All", new VirtualActor(this.Actuators)}};

          description = new EnvironmentDescription(this.ObjectiveFunction,
                                                   virtual_actors,
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

      foreach (var actuator in this.Actuators.Values) {
        actuator?.RemotePostSetup();
      }

      foreach (var sensor in this.Sensors.Values) {
        sensor?.RemotePostSetup();
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

    protected override void SendToActors(Reaction reaction) {
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
    protected override void InnerResetRegisteredObjects() {
      foreach (var actuator in this.Actuators.Values) {
        actuator?.PrototypingReset();
      }
    }

    #endregion

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override String ToString() {
      var e = " - ";

      e += this.Identifier;
      e += ", Sensors: ";
      e += this.Sensors.Count;
      e += ", Actuators: ";
      e += this.Actuators.Count;
      e += ", Objective: ";
      e += this.ObjectiveFunction != null ? this.ObjectiveFunction.Identifier : "None";

      return e;
    }
  }
}
