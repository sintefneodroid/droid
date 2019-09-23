using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Transform {
  public class SphericalCoordinateSensor : Sensor,
                                           IHasDouble {
    [SerializeField]
    Space2 _spherical_space = new Space2(4) {
                                                Min = Vector2.zero,
                                                Max = new Vector2(Mathf.PI * 2f, Mathf.PI * 2f)
                                            };

    [SerializeField] SphericalSpace sc;

    void Reset() {
      this.sc = SphericalSpace.FromCartesian(this.transform.position,
                                             3f,
                                             10f,
                                             0f,
                                             Mathf.PI * 2f,
                                             0f,
                                             Mathf.PI * 2f);
    }

    public override IEnumerable<Single> FloatEnumerable {
      get { return new[] {ObservationValue.x, ObservationValue.y}; }
    }

    public override void UpdateObservation() { this.sc.UpdateFromCartesian(this.transform.position); }
    public Vector2 ObservationValue { get { return this.sc.ToVector2; } }
    public Space2 DoubleSpace { get { return this._spherical_space; } }
  }
}
