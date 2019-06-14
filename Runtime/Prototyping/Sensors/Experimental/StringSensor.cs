using System;
using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Experimental {
  public abstract class StringSensor : Sensor,
                                       IHasString {
    [Header("Observation", order = 103)]
    [SerializeField]
    string _observation_value;

    public String ObservationValue {
      get { return this._observation_value; }
      set { this._observation_value = value; }
    }
  }
}
