using System.Collections.Generic;
using droid.Runtime.Interfaces;

namespace droid.Runtime.Messaging.Messages {
  /// <summary>
  /// </summary>
  public class EnvironmentDescription {
    public EnvironmentDescription(
        int max_steps,
        Dictionary<string, IActor> actors,
        Dictionary<string, IConfigurable> configurables,
        float solved_threshold) {
      this.Configurables = configurables;
      this.Actors = actors;
      this.MaxSteps = max_steps;

      this.SolvedThreshold = solved_threshold;
    }

    /// <summary>
    /// </summary>
    public Dictionary<string, IActor> Actors { get; }

    /// <summary>
    /// </summary>
    public Dictionary<string, IConfigurable> Configurables { get; }

    /// <summary>
    /// </summary>
    public int MaxSteps { get; }

    /// <summary>
    /// </summary>
    public float SolvedThreshold { get; }
  }

  public interface IActors { }
}
