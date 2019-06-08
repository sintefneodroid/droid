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

    bool IsNormalised { get; }

    dynamic Sample();
  }
}
