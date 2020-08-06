using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.Transform {
  public class SphericalCoordinateSensor : Sensor,
                                           IHasDouble {
    [SerializeField]
    Space2 _spherical_space = new Space2 {
                                             Min = Vector2.zero,
                                             Max = new Vector2(x : Mathf.PI * 2f, y : Mathf.PI * 2f),
                                             DecimalGranularity = 4
                                         };

    [SerializeField] SphericalSpace sc;

    public override void PrototypingReset() {
      this.sc = SphericalSpace.FromCartesian(cartesian_coordinate : this.transform.position,
                                             3f,
                                             10f,
                                             0f,
                                             max_polar : Mathf.PI * 2f,
                                             0f,
                                             max_elevation : Mathf.PI * 2f);
    }

    public override IEnumerable<float> FloatEnumerable {
      get {
        yield return this.ObservationValue.x;
        yield return this.ObservationValue.y;
      }
    }

    public override void UpdateObservation() {
      this.sc.UpdateFromCartesian(cartesian_coordinate : this.transform.position);
    } //TODO: IMPLEMENT LOCAL SPACE

    public Vector2 ObservationValue { get { return this.sc.ToVector2; } }
    public Space2 DoubleSpace { get { return this._spherical_space; } }
  }
}
