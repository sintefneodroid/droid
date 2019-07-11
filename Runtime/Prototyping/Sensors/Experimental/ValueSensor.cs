using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Experimental {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath + "Value" + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  public class ValueSensor : Sensor,
                             IHasSingle {
    [Header("Observation", order = 103)]
    [SerializeField]
    float _observation_value;

    [SerializeField] Space1 _observation_value_space;

    /// <summary>
    ///
    /// </summary>
    public Space1 SingleSpace { get { return this._observation_value_space; } }

    /// <summary>
    ///
    /// </summary>
    public float ObservationValue {
      get { return this._observation_value; }
      set {
        this._observation_value = this.SingleSpace.Normalised
                                      ? this._observation_value_space.ClipNormaliseRound(value)
                                      : value;
      }
    }

    /// <summary>
    ///
    /// </summary>
    protected override void PreSetup() { }

    /// <summary>
    ///
    /// </summary>
    public override IEnumerable<float> FloatEnumerable { get { return new[] {this.ObservationValue}; } }
    /// <summary>
    ///
    /// </summary>
    public override void UpdateObservation() { ; }
  }
}
