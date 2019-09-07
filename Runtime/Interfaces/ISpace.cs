using System;
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

    dynamic ClipRoundDenormaliseClip(dynamic configuration_configurable_value);

    dynamic ClipNormaliseRound(dynamic configuration_configurable_value);
  }
}
