using System.Collections.Generic;
using droid.Runtime.Interfaces;

namespace droid.Runtime.Messaging.Messages {
  /// <summary>
  /// </summary>
  public class EnvironmentDescription {
    public EnvironmentDescription(int max_steps,
                                  SortedDictionary<string, IActor> actors,
                                  SortedDictionary<string, IConfigurable> configurables,
                                  SortedDictionary<string, ISensor> sensors,
                                  float solved_threshold) {
      this.Configurables = configurables;
      this.Actors = actors;
      this.MaxSteps = max_steps;
      this.Sensors = sensors;

      this.SolvedThreshold = solved_threshold;
    }

    /// <summary>
    /// </summary>
    public SortedDictionary<string, IActor> Actors { get; }

    /// <summary>
    /// </summary>
    public SortedDictionary<string, IConfigurable> Configurables { get; }

    /// <summary>
    /// </summary>
    public SortedDictionary<string,ISensor> Sensors { get; }

    /// <summary>
    /// </summary>
    public int MaxSteps { get; }

    /// <summary>
    /// </summary>
    public float SolvedThreshold { get; }
  }
}
