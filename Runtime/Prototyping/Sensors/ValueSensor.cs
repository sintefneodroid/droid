using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "Value"
                    + SensorComponentMenuPath._Postfix)]
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
        this._observation_value = this.SingleSpace.IsNormalised
                                      ? this._observation_value_space.ClipNormaliseRound(value)
                                      : value;
      }
    }

    protected override void PreSetup() { }

    public override IEnumerable<float> FloatEnumerable { get { return new[] {this.ObservationValue}; } }
    public override void UpdateObservation() { ; }
  }
}
