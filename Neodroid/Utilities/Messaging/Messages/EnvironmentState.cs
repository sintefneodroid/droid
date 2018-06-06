using System;
using System.Collections.Generic;
using droid.Neodroid.Prototyping.Observers;
using UnityEngine;

namespace droid.Neodroid.Utilities.Messaging.Messages {
  [Serializable]
  public class EnvironmentState {
    public EnvironmentState(
        string environment_name,
        float total_energy_spent_since_reset,
        int frame_number,
        float time,
        float signal,
        bool terminated,
        ref float[] observables,
        string termination_reason = "",
        EnvironmentDescription description = null,
        string debug_message = "") {
      this.Observables = observables;
      this.DebugMessage = debug_message;
      this.TerminationReason = termination_reason;
      this.EnvironmentName = environment_name;
      this.TotalEnergySpentSinceReset = total_energy_spent_since_reset;
      this.Signal = signal;
      this.FrameNumber = frame_number;
      this.Time = time;
      this.Terminated = terminated;
      this.Description = description;
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
    public float Time { get; }

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
    public Observer[] Observers { get; set; }

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
    public Unobservables Unobservables { get; set; }
  }
}
