using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Prototyping.Actors {
  public class VirtualActor : IActor {
    public VirtualActor(SortedDictionary<string, IActuator> actuators) { this.Actuators = actuators; }
    /// <summary>
    ///
    /// </summary>
    public String Identifier { get; }


    /// <summary>
    ///
    /// </summary>
    public SortedDictionary<String, IActuator> Actuators { get; }
    /// <summary>
    ///
    /// </summary>
    public Transform Transform { get; }

    public void Tick() { throw new NotImplementedException(); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Register(IActuator obj) { throw new NotImplementedException(); }
    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="identifier"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Register(IActuator obj, String identifier) { throw new NotImplementedException(); }
    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void UnRegister(IActuator obj) { throw new NotImplementedException(); }
    /// <summary>
    ///
    /// </summary>
    /// <param name="t"></param>
    /// <param name="obj"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void UnRegister(IActuator t, String obj) { throw new NotImplementedException(); }


    /// <summary>
    ///
    /// </summary>
    /// <param name="motion"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void ApplyMotion(IMotion motion) { throw new NotImplementedException(); }
    /// <summary>
    ///
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void PrototypingReset() { throw new NotImplementedException(); }

    public void PreSetup() { throw new NotImplementedException(); }
    public void Setup() { throw new NotImplementedException(); }

    public void RemotePostSetup() { throw new NotImplementedException();  }
  }
}
