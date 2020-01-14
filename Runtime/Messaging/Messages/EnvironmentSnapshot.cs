using System;

namespace droid.Runtime.Messaging.Messages {
  /// <summary>
  ///
  /// </summary>
  [Serializable]
  public class EnvironmentSnapshot {
    public EnvironmentSnapshot(string environment_name,
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
      this.Signal = signal;
      this.FrameNumber = frame_number;
      this.Time = time;
      this.Terminated = terminated;
      this.Description = description;
    }

    /// <summary>
    ///
    /// </summary>
    public float[] Observables { get; }

    /// <summary>
    /// The reason for termination
    /// </summary>
    public String TerminationReason { get; }

    /// <summary>
    ///
    /// </summary>
    public string EnvironmentName { get; }

    /// <summary>
    /// </summary>
    public int FrameNumber { get; }

    /// <summary>
    /// </summary>
    public float Time { get; }

    /// <summary>
    /// </summary>
    public bool Terminated { get; }

    /// <summary>
    /// </summary>
    public string DebugMessage { get; }

    /// <summary>
    /// </summary>
    public EnvironmentDescription Description { get; }

    /// <summary>
    /// </summary>
    public float Signal { get; }

    /// <summary>
    /// </summary>
    public Unobservables Unobservables { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override String ToString() { return $"SnapShot: {this.Description},Term.{Terminated}"; }
  }
}
