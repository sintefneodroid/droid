using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Experimental {
  /// <summary>
  /// 
  /// </summary>
  public class DistanceSensor : Sensor,
                                IHasSingle {
    [SerializeField] UnityEngine.Transform t1 = null;
    [SerializeField] UnityEngine.Transform t2 = null;
    [SerializeField] float _observationValue = 0;

    public override IEnumerable<float> FloatEnumerable { get { return new[] {this.ObservationValue}; } }

    public override void UpdateObservation() {
      this.ObservationValue = Vector3.Distance(this.t1.position, this.t2.position);
    }

    public float ObservationValue {
      get { return this._observationValue; }
      private set { this._observationValue = value; }
    }

    public Space1 SingleSpace { get; }
  }
}
