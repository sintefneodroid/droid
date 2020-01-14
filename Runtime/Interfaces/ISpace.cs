using System;
using droid.Runtime.Enums;
using UnityEngine;

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
    dynamic Max { get; }

    /// <summary>
    ///
    /// </summary>
    dynamic Min { get; }

    /// <summary>
    ///
    /// </summary>
    NormalisationEnum Normalised { get; }

    dynamic Mean { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="configuration_configurable_value"></param>
    /// <returns></returns>
    dynamic Reproject(dynamic configuration_configurable_value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="configuration_configurable_value"></param>
    /// <returns></returns>
    dynamic Project(dynamic configuration_configurable_value);
  }
}
