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

    /// <summary>
    ///
    /// </summary>
    bool NormalisedBool { get; }

    /// <summary>
    ///
    /// </summary>
    dynamic Max { get; }

    /// <summary>
    ///
    /// </summary>
    dynamic Min { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="configuration_configurable_value"></param>
    /// <returns></returns>
    dynamic ClipRoundDenormaliseClip(dynamic configuration_configurable_value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="configuration_configurable_value"></param>
    /// <returns></returns>
    dynamic ClipNormaliseRound(dynamic configuration_configurable_value);
  }
}
