using droid.Runtime.Utilities.Structs;

namespace droid.Runtime.Interfaces {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public interface IObjective : IRegisterable {
    /// <summary>
    /// Specify an signal value for which the objective is considered solved
    /// </summary>
    float SolvedThreshold { get; set; }

    /// <summary>
    /// Compute signal
    /// </summary>
    /// <returns>floating point signal value</returns>
    float Evaluate();

    /// <summary>
    /// Reset for function for resetting stateful evaluation functions
    /// </summary>
    void EnvironmentReset();

    /// <summary>
    /// The length of an episode
    /// </summary>
    int EpisodeLength { get; set; }

    /// <summary>
    /// The range for which the signal might fall
    /// </summary>
    Space1 SignalSpace { get; set; }
  }
}
