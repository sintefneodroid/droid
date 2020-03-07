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
      if (this.Terminated) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("SampleReaction resetting environment");
        }
        #endif

        var reset_reaction = new ReactionParameters(reaction_type : ReactionTypeEnum.Reset_, false, true);
        return new Reaction(reaction_parameters : reset_reaction, recipient_environment : this.Identifier);
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log(message : $"Sampling a reaction for environment {this.Identifier}");
      }
      #endif

      var sample_motions = new List<IMotion>();

      foreach (var actuator in this.Actuators) {
        var actuator_value = actuator.Value;
        if (actuator_value != null) {
          sample_motions.Add(item : new ActuatorMotion(actor_name : actuator.Key,
                                                       actuator_name : actuator.Key,
                                                       strength : actuator_value.Sample()));
        }
      }

      var rp = new ReactionParameters(reaction_type : ReactionTypeEnum.Step_, true, episode_count : true);
      return new Reaction(parameters : rp,
                          motions : sample_motions.ToArray(),
                          null,
                          null,
                          null,
                          "",
                          recipient_environment : this.Identifier);
    }

    #endregion

    #region PublicMethods

    #region Getters

    /// <summary>
    /// </summary>
    public SortedDictionary<string, IActuator> Actuators { get; } = new SortedDictionary<string, IActuator>();

    #endregion

    #region Registration

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <param name="obj"></param>
    public void Register(IActuator obj) { this.Register(obj : obj, identifier : obj.Identifier); }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <param name="obj"></param>
    ///  <param name="identifier"></param>
    public void Register(IActuator obj, String identifier) {
      if (!this.Actuators.ContainsKey(key : identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(message : $"Environment {this.name} has registered actuator {identifier}");
        }
        #endif

        this.Actuators.Add(key : identifier, value : obj);
      } else {
        Debug.LogWarning(message :
                         $"WARNING! Please check for duplicates, Environment {this.name} already has actuator {identifier} registered");
      }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <param name="obj"></param>
    public void UnRegister(IActuator obj) { this.UnRegister(t : obj, obj : obj.Identifier); }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <param name="t"></param>
    ///  <param name="obj"></param>
    public void UnRegister(IActuator t, String obj) {
      if (this.Actuators.ContainsKey(key : obj)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(message : $"Environment {this.name} unregistered actuator {obj}");
        }
        #endif
        this.Actuators.Remove(key : obj);
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
      var signal = 0f;

      if (this.ObjectiveFunction != null) {
        signal = this.ObjectiveFunction.Evaluate();
      }

      if (float.IsInfinity(f : signal)) {
        signal = 0f;
      }

      EnvironmentDescription description = null;
      if (this.SimulationManager.SimulatorConfiguration.SerialiseIndividualObservables
          || this.ProvideFullDescription) {
        var virtual_actors =
            new SortedDictionary<String, IActor> {{"All", new VirtualActor(actuators : this.Actuators)}};

        description = new EnvironmentDescription(objective_function_function : this.ObjectiveFunction,
                                                 actors : virtual_actors,
                                                 configurables : this.Configurables,
                                                 sensors : this.Sensors,
                                                 displayers : this.Displayers);
      }

      var obs = new float[0];
      if (this.SimulationManager.SimulatorConfiguration.SerialiseAggregatedFloatArray
          || this.ProvideFullDescription) {
        this._Observables.Clear();
        foreach (var item in this.Sensors) {
          if (item.Value != null) {
            if (item.Value.FloatEnumerable != null) {
              this._Observables.AddRange(collection : item.Value.FloatEnumerable);
            } else {
              #if NEODROID_DEBUG
              if (this.Debugging) {
                Debug.Log(message : $"Sensor with key {item.Key} has a null FloatEnumerable value");
              }
              #endif
            }
          } else {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log(message : $"Sensor with key {item.Key} has a null value");
            }
            #endif
          }
        }

        obs = this._Observables.ToArray();
      }

      var time = Time.realtimeSinceStartup - this.LastResetTime;

      var state = new EnvironmentSnapshot(environment_name : this.Identifier,
                                          frame_number : this.StepI,
                                          time : time,
                                          signal : signal,
                                          terminated : this.Terminated,
                                          observables : ref obs,
                                          termination_reason : this.LastTerminationReason,
                                          description : description);

      if (this.SimulationManager.SimulatorConfiguration.SerialiseUnobservables
          || this.ProvideFullDescription) {
        state.Unobservables =
            new Unobservables(rigidbodies : ref this._Tracked_Rigid_Bodies, transforms : ref this._Poses);
      }

      //ProvideFullDescription = false;

      return state;
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override void RemotePostSetup() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("PostSetup");
      }
      #endif

      foreach (var actuator in this.Actuators.Values) {
        actuator?.RemotePostSetup();
      }

      foreach (var sensor in this.Sensors.Values) {
        sensor?.RemotePostSetup();
      }

      foreach (var configurable in this.Configurables.Values) {
        configurable?.RemotePostSetup();
      }

      foreach (var displayer in this.Displayers.Values) {
        displayer?.RemotePostSetup();
      }

      this.ObjectiveFunction?.RemotePostSetup();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="recipient"></param>
    public override void ObservationsString(DataPoller recipient) {
      recipient.PollData(data : string.Join("\n\n",
                                            values :
                                            this.Sensors.Values.Select(e =>
                                                                           $"{e.Identifier}:\n{e.ToString()}")));
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="reaction"></param>
    protected override void SendToActors(Reaction reaction) {
      if (reaction.Motions != null && reaction.Motions.Length > 0) {
        for (var index = 0; index < reaction.Motions.Length; index++) {
          var motion = reaction.Motions[index];
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log(message : "Applying " + motion + " To " + this.name + " actuator");
          }
          #endif
          var motion_actuator_name = motion.ActuatorName;
          if (this.Actuators.ContainsKey(key : motion_actuator_name)
              && this.Actuators[key : motion_actuator_name] != null) {
            this.Actuators[key : motion_actuator_name].ApplyMotion(motion : motion);
          } else {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log(message : "Could find not actuator with the specified name: " + motion_actuator_name);
            }
            #endif
          }
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void InnerResetRegisteredObjects() {
      foreach (var actuator in this.Actuators.Values) {
        actuator?.PrototypingReset();
      }
    }

    #endregion

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <returns></returns>
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
