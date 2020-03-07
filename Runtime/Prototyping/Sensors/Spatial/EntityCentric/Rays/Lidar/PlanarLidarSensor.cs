using System;
using System.Collections.Generic;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.EntityCentric.Rays.Lidar {
  /// <inheritdoc />
  ///  <summary>
  ///  </summary>
  public class PlanarLidarSensor : Sensor,
                                   IHasFloatArray {
    [SerializeField] [Range(1, 90)] int RaysToShoot = 30;
    [SerializeField] Space1 _observation_space = new Space1 {Max = 30};
    [SerializeField] AxisEnum _axisEnum = AxisEnum.Y_;

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override IEnumerable<Single> FloatEnumerable { get { return this.ObservationArray; } }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override void UpdateObservation() {
      var res = new float[this.RaysToShoot];
      float angle = 0;
      for (var i = 0; i < this.RaysToShoot; i++) {
        var x = Mathf.Sin(f : angle);
        var y = Mathf.Cos(f : angle);
        angle += 2 * Mathf.PI / this.RaysToShoot;

        var position = this.transform.position;

        var dir = new Vector3(x : position.x + x, y : position.y + y, z : 0);
        if (this._axisEnum == AxisEnum.Y_) {
          dir = new Vector3(x : position.x + x, y : 0, z : position.z + y);
        } else if (this._axisEnum == AxisEnum.X_) {
          dir = new Vector3(0, y : position.y + y, z : position.z + x);
        }

        Debug.DrawLine(start : position, end : dir, color : Color.red);
        if (Physics.Raycast(origin : this.transform.position,
                            direction : dir,
                            hitInfo : out var a,
                            maxDistance : (float)this._observation_space.Max)) {
          res[i] = this._observation_space.Project(v : a.distance);
        } else {
          res[i] = this._observation_space.Max;
        }
      }

      this.ObservationArray = res;
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    [field : SerializeField]
    public Single[] ObservationArray { get; private set; }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public Space1[] ObservationSpace { get { return new[] {this._observation_space}; } }

    void OnDrawGizmosSelected() {
      float angle = 0;
      for (var i = 0; i < this.RaysToShoot; i++) {
        var x = Mathf.Sin(f : angle) * this._observation_space.Max;
        var y = Mathf.Cos(f : angle) * this._observation_space.Max;

        angle += 2 * Mathf.PI / this.RaysToShoot;

        var position = this.transform.position;
        var dir = new Vector3(x : position.x + x, y : position.y + y, z : 0);
        if (this._axisEnum == AxisEnum.Y_) {
          dir = new Vector3(x : position.x + x, y : 0, z : position.z + y);
        } else if (this._axisEnum == AxisEnum.X_) {
          dir = new Vector3(0, y : position.y + y, z : position.z + x);
        }

        Debug.DrawLine(start : position, end : dir, color : Color.red);
      }
    }
  }
}
