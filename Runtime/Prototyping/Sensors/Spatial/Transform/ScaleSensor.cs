using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.Transform {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                               + "Scale"
                               + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [Serializable]
  public class ScaleSensor : Sensor,
                             IHasTriple {
    [Header("Observation", order = 103)]
    [SerializeField]
    Vector3 _scale;

    [SerializeField] Space3 _scale_space = Space3.ZeroOne;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Vector3 ObservationValue { //TODO: IMPLEMENT LOCAL SPACE
      get { return this._scale; }
      set { this._scale = this._scale_space.Project(v : value); }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Space3 TripleSpace { get { return this._scale_space; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() { }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override IEnumerable<float> FloatEnumerable {
      get {
        yield return this.ObservationValue.x;
        yield return this.ObservationValue.y;
        yield return this.ObservationValue.z;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() { this.ObservationValue = this.transform.localScale; }
  }
}
