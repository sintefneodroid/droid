using System;
using System.Collections.Generic;
using Neodroid.Prototyping.Observers;
using UnityEngine;

namespace Neodroid.Utilities.Messaging.Messages {
  [Serializable]
  public class EnvironmentState {
    public EnvironmentState(
        string environment_name,
        float total_energy_spent_since_reset,
        Dictionary<string, Observer> observations,
        int frame_number,
        float time,
        float signal,
        bool terminated,
        float[] observables,
        Rigidbody[] bodies,
        Transform[] poses,
        string termination_reason = "",
        EnvironmentDescription description = null,
        string debug_message = "") {
      this.Observables = observables;
      this.DebugMessage = debug_message;
      this.TerminationReason = termination_reason;
      this.EnvironmentName = environment_name;
      this.TotalEnergySpentSinceReset = total_energy_spent_since_reset;
      this.Observations = observations;
      this.Signal = signal;
      this.FrameNumber = frame_number;
      this.Time = time;
      this.Terminated = terminated;
      this.Description = description;
      this.Unobservables = new Unobservables(bodies, poses);
    }

    public EnvironmentState(
        string environment_name,
        int frame_number,
        float time,
        float signal,
        bool terminated,
        float[] observables,
        Rigidbody[] bodies,
        Transform[] poses,
        string termination_reason = "",
        EnvironmentDescription description = null,
        string debug_message = "") {
      this.Observables = observables;
      this.DebugMessage = debug_message;
      this.TerminationReason = termination_reason;
      this.EnvironmentName = environment_name;
      this.Signal = signal;
      this.FrameNumber = frame_number;
      this.Time = time;
      this.Terminated = terminated;
      this.Description = description;
      this.Unobservables = new Unobservables(bodies, poses);
    }

    public float[] Observables { get; }

    public String TerminationReason { get; }

    public string EnvironmentName { get; }

    public float TotalEnergySpentSinceReset { get; }

    /// <summary>
    ///
    /// </summary>
    public int FrameNumber { get; }

    /// <summary>
    ///
    /// </summary>
    public Single Time { get; }

    /// <summary>
    ///
    /// </summary>
    public bool Terminated { get; }

    /// <summary>
    ///
    /// </summary>
    public string DebugMessage { get; }

    /// <summary>
    ///
    /// </summary>
    public Dictionary<string, Observer> Observations { get; }

    /// <summary>
    ///
    /// </summary>
    public EnvironmentDescription Description { get; }

    /// <summary>
    ///
    /// </summary>
    public float Signal { get; }

    /// <summary>
    ///
    /// </summary>
    public Unobservables Unobservables { get; }
  }
}
