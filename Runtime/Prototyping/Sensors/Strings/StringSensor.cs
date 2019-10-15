using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Strings {
  /// <summary>
  ///
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath + "String" + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  public class StringSensor : Sensor,
                              IHasString {
    [Header("Observation", order = 103)]
    [SerializeField]
    string _observation_value;

    /// <summary>
    /// 
    /// </summary>
    public String ObservationValue {
      get { return this._observation_value; }
      set { this._observation_value = value; }
    }

    /// <summary>
    ///
    /// </summary>
    public override IEnumerable<Single> FloatEnumerable { get { return new float[] { }; } }

    /// <summary>
    ///
    /// </summary>
    public override void UpdateObservation() {
      this._observation_value = this.ParentEnvironment.StepI.ToString();
    }
  }
}
