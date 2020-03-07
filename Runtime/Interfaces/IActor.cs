using System.Collections.Generic;
using UnityEngine;

namespace droid.Runtime.Interfaces {
  /// <summary>
  ///
  /// </summary>
  public interface IActor : IRegisterable,
                            IHasRegister<IActuator> {
    /// <summary>
    ///
    /// </summary>
    SortedDictionary<string, IActuator> Actuators { get; }

    /// <summary>
    ///
    /// </summary>
    Transform CachedTransform { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="motion"></param>
    void ApplyMotion(IMotion motion);
  }
}
