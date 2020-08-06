using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.EntityCentric {
  /// <inheritdoc cref="Sensor" />
  ///  <summary>
  ///  </summary>
  public class DistanceSensor : Sensor,
                                IHasSingle {
    [SerializeField] UnityEngine.Transform t1 = null;
    [SerializeField] UnityEngine.Transform t2 = null;
    [SerializeField] float _observationValue = 0;
    [SerializeField] Space1 _single_space = Space1.MinusOneOne * 4;

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override IEnumerable<float> FloatEnumerable { get { yield return this.ObservationValue; } }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override void UpdateObservation() {
      this.ObservationValue = Vector3.Distance(a : this.t1.position, b : this.t2.position);
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public float ObservationValue {
      get { return this._observationValue; }
      private set { this._observationValue = this._single_space.Project(v : value); }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public Space1 SingleSpace { get { return this._single_space; } }
  }
}
