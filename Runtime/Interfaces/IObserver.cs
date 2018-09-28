using System;
using System.Collections.Generic;

namespace Neodroid.Runtime.Interfaces {
  /// <summary>
  /// 
  /// </summary>
  public interface IObserver : IRegisterable {
    /// <summary>
    /// 
    /// </summary>
    void UpdateObservation();

    /// <summary>
    /// 
    /// </summary>
    IEnumerable<float> FloatEnumerable { get; }

    /// <summary>
    /// 
    /// </summary>
    void EnvironmentReset();

  }
}
