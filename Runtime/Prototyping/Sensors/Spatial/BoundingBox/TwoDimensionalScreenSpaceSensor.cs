using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.BoundingBox {
  public class TwoDimensionalScreenSpaceSensor : Sensor,
                                                 IHasDouble {
    [SerializeField] Vector2 _observation_value = Vector2.zero;
    [SerializeField] Space2 _observation_space2_d = Space2.ZeroOne;

    [SerializeField] Camera _reference_camera = null;

    [SerializeField] bool _use_viewport = true; // Already normalised between 0 and 1

    // Update is called once per frame
    public override IEnumerable<float> FloatEnumerable {
      get { return new[] {this._observation_value.x, this.ObservationValue.y}; }
    }

    public override void UpdateObservation() {
      if (this._reference_camera) {
        Vector3 point;
        if (this._use_viewport) {
          point = this._reference_camera.WorldToViewportPoint(this.transform.position);
        } else {
          point = this._reference_camera.WorldToScreenPoint(this.transform.position);
        }

        this._observation_value = point;
      }
    }

    /// <summary>
    ///
    /// </summary>
    public Vector2 ObservationValue { get { return this._observation_value; } }

    /// <summary>
    ///
    /// </summary>
    public Space2 DoubleSpace { get { return this._observation_space2_d; } }
  }
}
