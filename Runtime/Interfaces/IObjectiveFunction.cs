using droid.Runtime.Structs.Space;
using droid.Runtime.Utilities.Structs;

namespace droid.Runtime.Interfaces {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public interface IObjectiveFunction : IRegisterable {

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
