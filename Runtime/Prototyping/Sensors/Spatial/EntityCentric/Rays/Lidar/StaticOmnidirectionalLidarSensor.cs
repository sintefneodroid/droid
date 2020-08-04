using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.EntityCentric.Rays.Lidar {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                               + "StaticOmnidirectionalLidar"
                               + SensorComponentMenuPath._Postfix)]
  public class StaticOmnidirectionalLidarSensor : Sensor,
                                                  IHasFloatArray {
    [SerializeField] RaycastHit _hit;

    [SerializeField] bool _is_2_d = false;

    [SerializeField] Space1 _space = new Space1 {Min = 0, Max = 5f};

    [SerializeField] bool ignore_rotation = false;

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override IEnumerable<Single> FloatEnumerable { get { return this.ObservationArray; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [field : Header("Observation", order = 103)]
    [field : SerializeField]
    public float[] ObservationArray { get; private set; }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public Space1[] ObservationSpace { get { return new[] {this._space}; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      if (this._is_2_d) {
        this.ObservationArray = new float[8];
      } else {
        this.ObservationArray = new float[27];
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      if (this._is_2_d) {
        var vals = new float[8];
        if (Physics.Raycast(origin : this.transform.position + this._space.Min * Vector3.forward,
                            direction : this.transform.TransformDirection(direction : Vector3.forward),
                            hitInfo : out this._hit,
                            maxDistance : this._space.Max)) {
          vals[0] = this._space.Project(v : this._hit.distance);
        } else {
          vals[0] = this._space.Max;
        }

        if (Physics.Raycast(origin : this.transform.position + this._space.Min * Vector3.left,
                            direction : this.transform.TransformDirection(direction : Vector3.left),
                            hitInfo : out this._hit,
                            maxDistance : this._space.Max)) {
          vals[1] = this._space.Project(v : this._hit.distance);
        } else {
          vals[1] = this._space.Max;
        }

        if (Physics.Raycast(origin : this.transform.position + this._space.Min * Vector3.right,
                            direction : this.transform.TransformDirection(direction : Vector3.right),
                            hitInfo : out this._hit,
                            maxDistance : this._space.Max)) {
          vals[2] = this._space.Project(v : this._hit.distance);
        } else {
          vals[2] = this._space.Max;
        }

        if (Physics.Raycast(origin : this.transform.position + this._space.Min * Vector3.back,
                            direction : this.transform.TransformDirection(direction : Vector3.back),
                            hitInfo : out this._hit,
                            maxDistance : this._space.Max)) {
          vals[3] = this._space.Project(v : this._hit.distance);
        } else {
          vals[3] = this._space.Max;
        }

        if (Physics.Raycast(origin : this.transform.position
                                     + this._space.Min * (Vector3.forward + Vector3.left).normalized,
                            direction :
                            this.transform.TransformDirection(direction : (Vector3.forward + Vector3.left)
                                                              .normalized),
                            hitInfo : out this._hit,
                            maxDistance : this._space.Max)) {
          vals[4] = this._space.Project(v : this._hit.distance);
        } else {
          vals[4] = this._space.Max;
        }

        if (Physics.Raycast(origin : this.transform.position
                                     + this._space.Min * (Vector3.forward + Vector3.right).normalized,
                            direction :
                            this.transform.TransformDirection(direction : (Vector3.forward + Vector3.right)
                                                              .normalized),
                            hitInfo : out this._hit,
                            maxDistance : this._space.Max)) {
          vals[5] = this._space.Project(v : this._hit.distance);
        } else {
          vals[5] = this._space.Max;
        }

        if (Physics.Raycast(origin : this.transform.position
                                     + this._space.Min * (Vector3.back + Vector3.left).normalized,
                            direction :
                            this.transform.TransformDirection(direction : (Vector3.back + Vector3.left)
                                                              .normalized),
                            hitInfo : out this._hit,
                            maxDistance : this._space.Max)) {
          vals[6] = this._space.Project(v : this._hit.distance);
        } else {
          vals[6] = this._space.Max;
        }

        if (Physics.Raycast(origin : this.transform.position
                                     + this._space.Min * (Vector3.back + Vector3.right).normalized,
                            direction :
                            this.transform.TransformDirection(direction : (Vector3.back + Vector3.right)
                                                              .normalized),
                            hitInfo : out this._hit,
                            maxDistance : this._space.Max)) {
          vals[7] = this._space.Project(v : this._hit.distance);
        } else {
          vals[7] = this._space.Max;
        }

        this.ObservationArray = vals;
      } else {
        var vals = new float[27];
        if (Physics.Raycast(origin : this.transform.position + this._space.Min * Vector3.forward,
                            direction : this.transform.TransformDirection(direction : Vector3.forward),
                            hitInfo : out this._hit,
                            maxDistance : this._space.Max)) {
          vals[0] = this._space.Project(v : this._hit.distance);
        } else {
          vals[0] = this._space.Max;
        }

        if (Physics.Raycast(origin : this.transform.position + this._space.Min * Vector3.left,
                            direction : this.transform.TransformDirection(direction : Vector3.left),
                            hitInfo : out this._hit,
                            maxDistance : this._space.Max)) {
          vals[1] = this._space.Project(v : this._hit.distance);
        } else {
          vals[1] = this._space.Max;
        }

        if (Physics.Raycast(origin : this.transform.position + this._space.Min * Vector3.right,
                            direction : this.transform.TransformDirection(direction : Vector3.right),
                            hitInfo : out this._hit,
                            maxDistance : this._space.Max)) {
          vals[2] = this._space.Project(v : this._hit.distance);
        } else {
          vals[2] = this._space.Max;
        }

        if (Physics.Raycast(origin : this.transform.position + this._space.Min * Vector3.back,
                            direction : this.transform.TransformDirection(direction : Vector3.back),
                            hitInfo : out this._hit,
                            maxDistance : this._space.Max)) {
          vals[3] = this._space.Project(v : this._hit.distance);
        } else {
          vals[3] = this._space.Max;
        }

        if (Physics.Raycast(origin : this.transform.position
                                     + this._space.Min * (Vector3.forward + Vector3.left).normalized,
                            direction :
                            this.transform.TransformDirection(direction : (Vector3.forward + Vector3.left)
                                                              .normalized),
                            hitInfo : out this._hit,
                            maxDistance : this._space.Max)) {
          vals[4] = this._space.Project(v : this._hit.distance);
        } else {
          vals[4] = this._space.Max;
        }

        if (Physics.Raycast(origin : this.transform.position
                                     + this._space.Min * (Vector3.forward + Vector3.right).normalized,
                            direction :
                            this.transform.TransformDirection(direction : (Vector3.forward + Vector3.right)
                                                              .normalized),
                            hitInfo : out this._hit,
                            maxDistance : this._space.Max)) {
          vals[5] = this._space.Project(v : this._hit.distance);
        } else {
          vals[5] = this._space.Max;
        }

        if (Physics.Raycast(origin : this.transform.position
                                     + this._space.Min * (Vector3.back + Vector3.left).normalized,
                            direction :
                            this.transform.TransformDirection(direction : (Vector3.back + Vector3.left)
                                                              .normalized),
                            hitInfo : out this._hit,
                            maxDistance : this._space.Max)) {
          vals[6] = this._space.Project(v : this._hit.distance);
        } else {
          vals[6] = this._space.Max;
        }

        if (Physics.Raycast(origin : this.transform.position
                                     + this._space.Min * (Vector3.back + Vector3.right).normalized,
                            direction :
                            this.transform.TransformDirection(direction : (Vector3.back + Vector3.right)
                                                              .normalized),
                            hitInfo : out this._hit,
                            maxDistance : this._space.Max)) {
          vals[7] = this._space.Project(v : this._hit.distance);
        } else {
          vals[7] = this._space.Max;
        }

        //TODO:Missing combinations Vector3.down+Vector3.left...

        this.ObservationArray = vals;
      }
    }

    #if UNITY_EDITOR
    [SerializeField] Color _color = Color.green;

    void OnDrawGizmosSelected() {
      if (this.enabled) {
        var position = this.transform.position;
        Debug.DrawLine(start : position,
                       end : this.transform.TransformDirection(direction : position
                                                                           - Vector3.forward
                                                                           * this._space.Max),
                       color : this._color);
        Debug.DrawLine(start : position,
                       end : this.transform.TransformDirection(direction : position
                                                                           - Vector3.left * this._space.Max),
                       color : this._color);
        Debug.DrawLine(start : position,
                       end : this.transform.TransformDirection(direction : position
                                                                           - Vector3.right * this._space.Max),
                       color : this._color);
        Debug.DrawLine(start : position,
                       end : this.transform.TransformDirection(direction : position
                                                                           - Vector3.back * this._space.Max),
                       color : this._color);
        Debug.DrawLine(start : position,
                       end : this.transform.TransformDirection(direction : position
                                                                           - (Vector3.forward + Vector3.left)
                                                                           .normalized
                                                                           * this._space.Max),
                       color : this._color);
        Debug.DrawLine(start : position,
                       end : this.transform.TransformDirection(direction : position
                                                                           - (Vector3.forward + Vector3.right)
                                                                           .normalized
                                                                           * this._space.Max),
                       color : this._color);
        Debug.DrawLine(start : position,
                       end : this.transform.TransformDirection(direction : position
                                                                           - (Vector3.back + Vector3.left)
                                                                           .normalized
                                                                           * this._space.Max),
                       color : this._color);
        Debug.DrawLine(start : position,
                       end : this.transform.TransformDirection(direction : position
                                                                           - (Vector3.back + Vector3.right)
                                                                           .normalized
                                                                           * this._space.Max),
                       color : this._color);
        if (!this._is_2_d) {
          var position1 = this.transform.position;
          Debug.DrawLine(start : position1,
                         end : this.transform.TransformDirection(direction : position1
                                                                             - Vector3.up * this._space.Max),
                         color : this._color);
          Debug.DrawLine(start : position1,
                         end : this.transform.TransformDirection(direction : position1
                                                                             - Vector3.down
                                                                             * this._space.Max),
                         color : this._color);

          Debug.DrawLine(start : position1,
                         end : this.transform.TransformDirection(direction : position1
                                                                             - (Vector3.up + Vector3.left)
                                                                             .normalized
                                                                             * this._space.Max),
                         color : this._color);
          Debug.DrawLine(start : position1,
                         end : this.transform.TransformDirection(direction : position1
                                                                             - (Vector3.up + Vector3.right)
                                                                             .normalized
                                                                             * this._space.Max),
                         color : this._color);
          Debug.DrawLine(start : position1,
                         end : this.transform.TransformDirection(direction : position1
                                                                             - (Vector3.up + Vector3.forward)
                                                                             .normalized
                                                                             * this._space.Max),
                         color : this._color);
          Debug.DrawLine(start : position1,
                         end : this.transform.TransformDirection(direction : position1
                                                                             - (Vector3.up + Vector3.back)
                                                                             .normalized
                                                                             * this._space.Max),
                         color : this._color);

          Debug.DrawLine(start : position1,
                         end : this.transform.TransformDirection(direction : position1
                                                                             - (Vector3.down + Vector3.left)
                                                                             .normalized
                                                                             * this._space.Max),
                         color : this._color);

          Debug.DrawLine(start : position1,
                         end : this.transform.TransformDirection(direction : position1
                                                                             - (Vector3.down + Vector3.right)
                                                                             .normalized
                                                                             * this._space.Max),
                         color : this._color);

          Debug.DrawLine(start : position1,
                         end : this.transform.TransformDirection(direction : position1
                                                                             - (Vector3.down
                                                                                + Vector3.forward).normalized
                                                                             * this._space.Max),
                         color : this._color);

          Debug.DrawLine(start : position1,
                         end : this.transform.TransformDirection(direction : position1
                                                                             - (Vector3.down + Vector3.back)
                                                                             .normalized
                                                                             * this._space.Max),
                         color : this._color);

          Debug.DrawLine(start : position1,
                         end : this.transform.TransformDirection(direction : position1
                                                                             - (Vector3.down
                                                                                + Vector3.forward
                                                                                + Vector3.left).normalized
                                                                             * this._space.Max),
                         color : this._color);

          Debug.DrawLine(start : position1,
                         end : this.transform.TransformDirection(direction : position1
                                                                             - (Vector3.down
                                                                                + Vector3.forward
                                                                                + Vector3.right).normalized
                                                                             * this._space.Max),
                         color : this._color);
          Debug.DrawLine(start : position1,
                         end : this.transform.TransformDirection(direction : position1
                                                                             - (Vector3.down
                                                                                + Vector3.back
                                                                                + Vector3.left).normalized
                                                                             * this._space.Max),
                         color : this._color);

          Debug.DrawLine(start : position1,
                         end : this.transform.TransformDirection(direction : position1
                                                                             - (Vector3.down
                                                                                + Vector3.back
                                                                                + Vector3.right).normalized
                                                                             * this._space.Max),
                         color : this._color);

          Debug.DrawLine(start : position1,
                         end : this.transform.TransformDirection(direction : position1
                                                                             - (Vector3.up
                                                                                + Vector3.forward
                                                                                + Vector3.left).normalized
                                                                             * this._space.Max),
                         color : this._color);

          Debug.DrawLine(start : position1,
                         end : this.transform.TransformDirection(direction : position1
                                                                             - (Vector3.up
                                                                                + Vector3.forward
                                                                                + Vector3.right).normalized
                                                                             * this._space.Max),
                         color : this._color);
          Debug.DrawLine(start : position1,
                         end : this.transform.TransformDirection(direction : position1
                                                                             - (Vector3.up
                                                                                + Vector3.back
                                                                                + Vector3.left).normalized
                                                                             * this._space.Max),
                         color : this._color);

          Debug.DrawLine(start : position1,
                         end : this.transform.TransformDirection(direction : position1
                                                                             - (Vector3.up
                                                                                + Vector3.back
                                                                                + Vector3.right).normalized
                                                                             * this._space.Max),
                         color : this._color);
        }
      }
    }
    #endif
  }
}
