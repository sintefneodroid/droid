using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;

namespace droid.Runtime.Messaging.Messages {
  /// <summary>
  /// </summary>
  public class EnvironmentDescription {
    public EnvironmentDescription(IEpisodicObjectiveFunction objective_function_function,
                                  SortedDictionary<string, IActor> actors,
                                  SortedDictionary<string, IConfigurable> configurables,
                                  SortedDictionary<string, ISensor> sensors,
                                  SortedDictionary<string, IDisplayer> displayers) {
      this.Configurables = configurables;
      this.Actors = actors;
      ;
      this.Sensors = sensors;

      this.Displayers = displayers;

      this.ObjectiveFunction = objective_function_function;
    }

    /// <summary>
    ///
    /// </summary>
    public SortedDictionary<String, IDisplayer> Displayers { get; }

    /// <summary>
    ///
    /// </summary>
    public IEpisodicObjectiveFunction ObjectiveFunction { get; }

    /// <summary>
    /// </summary>
    public SortedDictionary<string, IActor> Actors { get; }

    /// <summary>
    /// </summary>
    public SortedDictionary<string, IConfigurable> Configurables { get; }

    /// <summary>
    /// </summary>
    public SortedDictionary<string, ISensor> Sensors { get; }
  }
}
