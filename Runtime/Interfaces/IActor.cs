using System.Collections.Generic;
using UnityEngine;

namespace droid.Runtime.Interfaces {
  public interface IActor : IRegisterable {
    Dictionary<string, IActuator> Actuators { get; }
    Transform Transform { get; }
    void ApplyMotion(IMotion motion);
    void EnvironmentReset();
    void UnRegister(IActuator Actuator);
    void Register(IActuator Actuator);
  }
}
