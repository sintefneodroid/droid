﻿using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Sampling;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Transform {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath + "Scale" + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [Serializable]
  public class ScaleSensor : Sensor,
                             IHasTriple {
    [Header("Observation", order = 103)]
    [SerializeField]
    Vector3 _position;

    [SerializeField] Space3 _scale_space = new Space3(10);

    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "Scale"; } }

    /// <summary>
    /// </summary>
    public Vector3 ObservationValue {
      get { return this._position; }
      set {
        this._position = this._scale_space.NormalisedBool
                             ? this._scale_space.ClipNormaliseRound(value)
                             : value;
      }
    }

    /// <summary>
    /// </summary>
    public Space3 TripleSpace { get { return this._scale_space; } }

    /// <summary>
    /// </summary>
    protected override void PreSetup() { }

    /// <summary>
    ///
    /// </summary>
    public override IEnumerable<float> FloatEnumerable {
      get { return new[] {this.ObservationValue.x, this.ObservationValue.y, this.ObservationValue.z}; }
    }

    /// <summary>
    /// </summary>
    public override void UpdateObservation() { this.ObservationValue = this.transform.localScale; }
  }
}
