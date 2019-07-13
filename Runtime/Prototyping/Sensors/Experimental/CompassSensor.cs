using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Experimental {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "Compass"
                    + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [Serializable]
  public class CompassSensor : Sensor,
                               IHasDouble {
    /// <summary>
    /// </summary>
    [SerializeField]
    Vector2 _2_d_position = Vector2.zero;

    /// <summary>
    /// </summary>
    [Header("Observation", order = 103)]
    [SerializeField]
    Vector3 _position = Vector3.zero;

    /// <summary>
    /// </summary>
    [SerializeField]
    Space3 _position_space = new Space3 {
                                            DecimalGranularity = 1,
                                            MaxValues = Vector3.one,
                                            MinValues = -Vector3.one
                                        };

    /// <summary>
    /// </summary>
    [Header("Specific", order = 102)]
    [SerializeField]
    UnityEngine.Transform _target = null;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "Compass"; } }

    /// <summary>
    /// </summary>
    public Vector3 Position {
      get { return this._position; }
      set {
        this._position = this._position_space.Normalised
                             ? this._position_space.ClipNormaliseRound(value)
                             : value;
        this._2_d_position = new Vector2(this._position.x, this._position.z);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Space2 DoubleSpace {
      get {
        return new Space2(this._position_space.DecimalGranularity) {
                                                                       MaxValues =
                                                                           new Vector2(this._position_space
                                                                                           .MaxValues.x,
                                                                                       this._position_space
                                                                                           .MaxValues.y),
                                                                       MinValues =
                                                                           new Vector2(this._position_space
                                                                                           .MinValues.x,
                                                                                       this._position_space
                                                                                           .MinValues.y)
                                                                   };
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Vector2 ObservationValue { get { return this._2_d_position; } set { this._2_d_position = value; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() { }

    public override IEnumerable<float> FloatEnumerable {
      get { return new[] {this.Position.x, this.Position.z}; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      this.Position = this.transform.InverseTransformVector(this.transform.position - this._target.position)
                          .normalized;
    }
  }
}
