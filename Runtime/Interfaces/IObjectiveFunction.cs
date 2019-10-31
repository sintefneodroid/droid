using droid.Runtime.Structs.Space;

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
    /// The range for which the signal might fall
    /// </summary>
    Space1 SignalSpace { get; set; }
  }
}
