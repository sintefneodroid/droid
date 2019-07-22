using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Occupancy {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "Occupancy3d"
                    + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  public class Occupancy3dSensor : Sensor,
                                   IHasTripleArray {
    [Header("Observation", order = 103)]
    [SerializeField]
    Vector3[] _observation_value;

    [SerializeField] Space1 _observation_value_space = Space1.ZeroOne;

    /// <summary>
    ///
    /// </summary>
    public Space1 SingleSpace { get { return this._observation_value_space; } }

    /// <summary>
    ///
    /// </summary>
    public Vector3[] ObservationArray {
      get { return this._observation_value; }
      set {
        this._observation_value = this.SingleSpace.Normalised
                                      ? value //this._observation_value_space.ClipNormaliseRound(value)
                                      : value;
      }
    }

    protected override void PreSetup() { }

    public override IEnumerable<float> FloatEnumerable {
      get {
        var a = new float[this.ObservationArray.Length * 3];
        for (var i = 0; i < this.ObservationArray.Length * 3; i += 3) {
          a[i] = this.ObservationArray[i].x;
          a[i + 1] = this.ObservationArray[i].y;
          a[i + 2] = this.ObservationArray[i].z;
        }

        return a;
      }
    }

    public override void UpdateObservation() { ; }
    public Space1[] ObservationSpace { get; } = new Space1[1];
  }
}
