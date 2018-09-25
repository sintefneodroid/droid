using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Runtime.Interfaces {
  public interface IActor : IRegisterable {
    Dictionary<string, IMotor> Motors { get; }
    Transform Transform { get; }
    void ApplyMotion(IMotorMotion motion);
    void EnvironmentReset();
    void UnRegister(IMotor motor);
    void Register(IMotor motor);
  }
}