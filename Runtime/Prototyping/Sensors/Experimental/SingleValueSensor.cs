using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Experimental {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath + "Value" + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  public abstract class SingleValueSensor : Sensor,
                             IHasSingle {
    [Header("Observation", order = 103)]
    [SerializeField]
    float _observation_value;

    [SerializeField] protected Space1 _observation_value_space = Space1.ZeroOne;

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
        this._observation_value = this.SingleSpace.Project(value);
      }
    }

    /// <summary>
    ///
    /// </summary>
    public override IEnumerable<float> FloatEnumerable { get { return new[] {this.ObservationValue}; } }


  }
}
