using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Prototyping.Actors {
  public class VirtualActor : IActor {
    public VirtualActor(SortedDictionary<string, IActuator> actuators) { this.Actuators = actuators; }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public string Identifier { get; }

    /// <summary>
    ///
    /// </summary>
    public SortedDictionary<string, IActuator> Actuators { get; }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public Transform CachedTransform { get; }

    public void Tick() { throw new NotImplementedException(); }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <param name="obj"></param>
    ///  <exception cref="T:System.NotImplementedException"></exception>
    public void Register(IActuator obj) { throw new NotImplementedException(); }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <param name="obj"></param>
    ///  <param name="identifier"></param>
    ///  <exception cref="T:System.NotImplementedException"></exception>
    public void Register(IActuator obj, string identifier) { throw new NotImplementedException(); }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <param name="obj"></param>
    ///  <exception cref="T:System.NotImplementedException"></exception>
    public void UnRegister(IActuator obj) { throw new NotImplementedException(); }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <param name="t"></param>
    ///  <param name="obj"></param>
    ///  <exception cref="T:System.NotImplementedException"></exception>
    public void UnRegister(IActuator t, string obj) { throw new NotImplementedException(); }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <param name="motion"></param>
    ///  <exception cref="T:System.NotImplementedException"></exception>
    public void ApplyMotion(IMotion motion) { throw new NotImplementedException(); }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <exception cref="T:System.NotImplementedException"></exception>
    public void PrototypingReset() { throw new NotImplementedException(); }

    public void PreSetup() { throw new NotImplementedException(); }
    public void Setup() { throw new NotImplementedException(); }

    public void RemotePostSetup() { throw new NotImplementedException(); }
  }
}
