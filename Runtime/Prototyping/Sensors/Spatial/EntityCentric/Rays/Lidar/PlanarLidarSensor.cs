using System;
using System.Collections.Generic;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.EntityCentric.Rays.Lidar {
  /// <summary>
  ///
  /// </summary>
  public class PlanarLidarSensor : Sensor,
                                   IHasFloatArray {
    [SerializeField] [Range(1, 90)] int RaysToShoot = 30;
    [SerializeField] Space1 _observation_space = new Space1 {Max = 30 };
    [SerializeField] Axis axis = Axis.Y_;
    [SerializeField] Single[] _observation_array;

    /// <summary>
    ///
    /// </summary>
    public override IEnumerable<Single> FloatEnumerable { get { return this._observation_array; } }

    /// <summary>
    ///
    /// </summary>
    public override void UpdateObservation() {
      var res = new float[this.RaysToShoot];
      float angle = 0;
      for (var i = 0; i < this.RaysToShoot; i++) {
        var x = Mathf.Sin(angle);
        var y = Mathf.Cos(angle);
        angle += 2 * Mathf.PI / this.RaysToShoot;

        var position = this.transform.position;

        var dir = new Vector3(position.x + x, position.y + y, 0);
        if (this.axis == Axis.Y_) {
          dir = new Vector3(position.x + x, 0, position.z + y);
        } else if (this.axis == Axis.X_) {
          dir = new Vector3(0, position.y + y, position.z + x);
        }

        Debug.DrawLine(position, dir, Color.red);
        if (Physics.Raycast(this.transform.position,
                            dir,
                            out var a,
                            (float)this._observation_space.Max)) {
          res[i] = this._observation_space.Project(a.distance);
        } else {
          res[i] = this._observation_space.Max;
        }
      }

      this.ObservationArray = res;
    }

    /// <summary>
    ///
    /// </summary>
    public Single[] ObservationArray {
      get { return this._observation_array; }
      set { this._observation_array = value; }
    }

    /// <summary>
    ///
    /// </summary>
    public Space1[] ObservationSpace { get { return new[] {this._observation_space}; } }

    void OnDrawGizmosSelected() {
      float angle = 0;
      for (var i = 0; i < this.RaysToShoot; i++) {
        var x = Mathf.Sin(angle) * this._observation_space.Max;
        var y = Mathf.Cos(angle) * this._observation_space.Max;

        angle += 2 * Mathf.PI / this.RaysToShoot;

        var position = this.transform.position;
        var dir = new Vector3(position.x + x, position.y + y, 0);
        if (this.axis == Axis.Y_) {
          dir = new Vector3(position.x + x, 0, position.z + y);
        } else if (this.axis == Axis.X_) {
          dir = new Vector3(0, position.y + y, position.z + x);
        }

        Debug.DrawLine(position, dir, Color.red);
      }
    }
  }
}
