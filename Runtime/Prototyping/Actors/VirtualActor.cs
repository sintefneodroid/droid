using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Prototyping.Actors {
  public class VirtualActor : IActor {
    public VirtualActor(Dictionary<string, IActuator> actuators) { this.Actuators = actuators; }
    public String Identifier { get; }
    public void Register(IActuator obj) { throw new NotImplementedException(); }
    public void Register(IActuator obj, String identifier) { throw new NotImplementedException(); }
    public void UnRegister(IActuator obj) { throw new NotImplementedException(); }
    public void UnRegister(IActuator t, String obj) { throw new NotImplementedException(); }
    public Dictionary<String, IActuator> Actuators { get; }
    public Transform Transform { get; }
    public void ApplyMotion(IMotion motion) { throw new NotImplementedException(); }
    public void EnvironmentReset() { throw new NotImplementedException(); }
  }
}
