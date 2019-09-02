using UnityEngine.Profiling;

namespace droid.Runtime.Interfaces {
  /// <summary>
  /// 
  /// </summary>
  public interface ISpace {
    /// <summary>
    /// 
    /// </summary>
    int DecimalGranularity { get; }

    bool Normalised { get; }

    dynamic Max{ get; }

    dynamic Min{ get; }

  }
}
