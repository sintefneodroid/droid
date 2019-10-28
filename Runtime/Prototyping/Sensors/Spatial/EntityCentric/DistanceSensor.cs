using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.EntityCentric {
  /// <summary>
  ///
  /// </summary>
  public class DistanceSensor : Sensor,
                                IHasSingle {
    [SerializeField] UnityEngine.Transform t1 = null;
    [SerializeField] UnityEngine.Transform t2 = null;
    [SerializeField] float _observationValue = 0;
    [SerializeField] Space1 _single_space = Space1.MinusOneOne * 4;

    /// <summary>
    ///
    /// </summary>
    public override IEnumerable<float> FloatEnumerable { get { return new[] {this.ObservationValue}; } }

    /// <summary>
    ///
    /// </summary>
    public override void UpdateObservation() {
      this.ObservationValue = Vector3.Distance(this.t1.position, this.t2.position);
    }

    /// <summary>
    ///
    /// </summary>
    public float ObservationValue {
      get { return this._observationValue; }
      private set { this._observationValue = this._single_space.Project(value); }
    }

    /// <summary>
    ///
    /// </summary>
    public Space1 SingleSpace { get { return this._single_space; } }
  }
}
