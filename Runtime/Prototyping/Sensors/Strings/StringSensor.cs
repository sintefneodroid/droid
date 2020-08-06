using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Strings {
  /// <summary>
  ///
  /// </summary>
  [AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                               + "String"
                               + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  public class StringSensor : Sensor,
                              IHasString {
    [Header("Observation", order = 103)]
    [SerializeField]
    string _observation_value;

    /// <summary>
    ///
    /// </summary>
    public string ObservationValue {
      get { return this._observation_value; }
      set { this._observation_value = value; }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override IEnumerable<float> FloatEnumerable { get { return null; } }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override void UpdateObservation() {
      this._observation_value = this.ParentEnvironment.StepI.ToString();
    }
  }
}
