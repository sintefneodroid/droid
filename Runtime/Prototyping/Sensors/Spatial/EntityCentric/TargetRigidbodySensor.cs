﻿using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Prototyping.Actuators;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.EntityCentric {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  public class TargetRigidbodySensor : Sensor,
                                       IHasDouble {
    [SerializeField] TargetRigidbodyActuator _actuator = null;

    [SerializeField] Space2 _observation_space2_d = Space2.ZeroOne;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Vector2 ObservationValue {
      get { return new Vector2(x : this._actuator.MovementSpeed, y : this._actuator.RotationSpeed); }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Space2 DoubleSpace { get { return this._observation_space2_d; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      base.PreSetup();
      if (!this._actuator) {
        this._actuator = this.GetComponent<TargetRigidbodyActuator>();
      }
    }

    public override IEnumerable<Single> FloatEnumerable {
      get {
        yield return this.ObservationValue.x;
        yield return this.ObservationValue.y;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() { }
  }
}
