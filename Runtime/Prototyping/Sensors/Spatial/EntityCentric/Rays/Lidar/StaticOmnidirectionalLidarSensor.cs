using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.EntityCentric.Rays.Lidar {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "StaticOmnidirectionalLidar"
                    + SensorComponentMenuPath._Postfix)]
  public class StaticOmnidirectionalLidarSensor : Sensor,
                                            IHasFloatArray {
    [SerializeField] RaycastHit _hit;

    [SerializeField] bool _is_2_d = false;

    [Header("Observation", order = 103)]
    [SerializeField]
    float[] _obs_array;

    [SerializeField] Space1 _space = new Space1 {Min = 0, Max = 5f};

    [SerializeField] bool ignore_rotation = false;

    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "OmnidirectionalLidar"; } }

    /// <summary>
    ///
    /// </summary>
    public override IEnumerable<float> FloatEnumerable { get { return this.ObservationArray; } }

    /// <summary>
    /// </summary>
    public float[] ObservationArray {
      get { return this._obs_array; }
      private set { this._obs_array = value; }
    }

    /// <summary>
    ///
    /// </summary>
    public Space1[] ObservationSpace { get { return new[] {this._space}; } }

    /// <summary>
    /// </summary>
    public override void PreSetup() {
      if (this._is_2_d) {
        this.ObservationArray = new float[8];
      } else {
        this.ObservationArray = new float[27];
      }
    }

    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      if (this._is_2_d) {
        var vals = new float[8];
        if (Physics.Raycast(this.transform.position + this._space.Min * Vector3.forward,
                            this.transform.TransformDirection(Vector3.forward),
                            out this._hit,
                            this._space.Max)) {
          vals[0] = this._space.Project(this._hit.distance);
        } else {
          vals[0] = this._space.Max;
        }

        if (Physics.Raycast(this.transform.position + this._space.Min * Vector3.left,
                            this.transform.TransformDirection(Vector3.left),
                            out this._hit,
                            this._space.Max)) {
          vals[1] = this._space.Project(this._hit.distance);
        } else {
          vals[1] = this._space.Max;
        }

        if (Physics.Raycast(this.transform.position + this._space.Min * Vector3.right,
                            this.transform.TransformDirection(Vector3.right),
                            out this._hit,
                            this._space.Max)) {
          vals[2] = this._space.Project(this._hit.distance);
        } else {
          vals[2] = this._space.Max;
        }

        if (Physics.Raycast(this.transform.position + this._space.Min * Vector3.back,
                            this.transform.TransformDirection(Vector3.back),
                            out this._hit,
                            this._space.Max)) {
          vals[3] = this._space.Project(this._hit.distance);
        } else {
          vals[3] = this._space.Max;
        }

        if (Physics.Raycast(this.transform.position
                            + this._space.Min * (Vector3.forward + Vector3.left).normalized,
                            this.transform.TransformDirection((Vector3.forward + Vector3.left).normalized),
                            out this._hit,
                            this._space.Max)) {
          vals[4] = this._space.Project(this._hit.distance);
        } else {
          vals[4] = this._space.Max;
        }

        if (Physics.Raycast(this.transform.position
                            + this._space.Min * (Vector3.forward + Vector3.right).normalized,
                            this.transform.TransformDirection((Vector3.forward + Vector3.right).normalized),
                            out this._hit,
                            this._space.Max)) {
          vals[5] = this._space.Project(this._hit.distance);
        } else {
          vals[5] = this._space.Max;
        }

        if (Physics.Raycast(this.transform.position
                            + this._space.Min * (Vector3.back + Vector3.left).normalized,
                            this.transform.TransformDirection((Vector3.back + Vector3.left).normalized),
                            out this._hit,
                            this._space.Max)) {
          vals[6] = this._space.Project(this._hit.distance);
        } else {
          vals[6] = this._space.Max;
        }

        if (Physics.Raycast(this.transform.position
                            + this._space.Min * (Vector3.back + Vector3.right).normalized,
                            this.transform.TransformDirection((Vector3.back + Vector3.right).normalized),
                            out this._hit,
                            this._space.Max)) {
          vals[7] = this._space.Project(this._hit.distance);
        } else {
          vals[7] = this._space.Max;
        }

        this.ObservationArray = vals;
      } else {
        var vals = new float[27];
        if (Physics.Raycast(this.transform.position + this._space.Min * Vector3.forward,
                            this.transform.TransformDirection(Vector3.forward),
                            out this._hit,
                            this._space.Max)) {
          vals[0] = this._space.Project(this._hit.distance);
        } else {
          vals[0] = this._space.Max;
        }

        if (Physics.Raycast(this.transform.position + this._space.Min * Vector3.left,
                            this.transform.TransformDirection(Vector3.left),
                            out this._hit,
                            this._space.Max)) {
          vals[1] = this._space.Project(this._hit.distance);
        } else {
          vals[1] = this._space.Max;
        }

        if (Physics.Raycast(this.transform.position + this._space.Min * Vector3.right,
                            this.transform.TransformDirection(Vector3.right),
                            out this._hit,
                            this._space.Max)) {
          vals[2] = this._space.Project(this._hit.distance);
        } else {
          vals[2] = this._space.Max;
        }

        if (Physics.Raycast(this.transform.position + this._space.Min * Vector3.back,
                            this.transform.TransformDirection(Vector3.back),
                            out this._hit,
                            this._space.Max)) {
          vals[3] = this._space.Project(this._hit.distance);
        } else {
          vals[3] = this._space.Max;
        }

        if (Physics.Raycast(this.transform.position
                            + this._space.Min * (Vector3.forward + Vector3.left).normalized,
                            this.transform.TransformDirection((Vector3.forward + Vector3.left).normalized),
                            out this._hit,
                            this._space.Max)) {
          vals[4] = this._space.Project(this._hit.distance);
        } else {
          vals[4] = this._space.Max;
        }

        if (Physics.Raycast(this.transform.position
                            + this._space.Min * (Vector3.forward + Vector3.right).normalized,
                            this.transform.TransformDirection((Vector3.forward + Vector3.right).normalized),
                            out this._hit,
                            this._space.Max)) {
          vals[5] = this._space.Project(this._hit.distance);
        } else {
          vals[5] = this._space.Max;
        }

        if (Physics.Raycast(this.transform.position
                            + this._space.Min * (Vector3.back + Vector3.left).normalized,
                            this.transform.TransformDirection((Vector3.back + Vector3.left).normalized),
                            out this._hit,
                            this._space.Max)) {
          vals[6] = this._space.Project(this._hit.distance);
        } else {
          vals[6] = this._space.Max;
        }

        if (Physics.Raycast(this.transform.position
                            + this._space.Min * (Vector3.back + Vector3.right).normalized,
                            this.transform.TransformDirection((Vector3.back + Vector3.right).normalized),
                            out this._hit,
                            this._space.Max)) {
          vals[7] = this._space.Project(this._hit.distance);
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
        Debug.DrawLine(position,
                       this.transform.TransformDirection(position - Vector3.forward * this._space.Max),
                       this._color);
        Debug.DrawLine(position,
                       this.transform.TransformDirection(position - Vector3.left * this._space.Max),
                       this._color);
        Debug.DrawLine(position,
                       this.transform.TransformDirection(position - Vector3.right * this._space.Max),
                       this._color);
        Debug.DrawLine(position,
                       this.transform.TransformDirection(position - Vector3.back * this._space.Max),
                       this._color);
        Debug.DrawLine(position,
                       this.transform.TransformDirection(position
                                                         - (Vector3.forward + Vector3.left).normalized
                                                         * this._space.Max),
                       this._color);
        Debug.DrawLine(position,
                       this.transform.TransformDirection(position
                                                         - (Vector3.forward + Vector3.right).normalized
                                                         * this._space.Max),
                       this._color);
        Debug.DrawLine(position,
                       this.transform.TransformDirection(position
                                                         - (Vector3.back + Vector3.left).normalized
                                                         * this._space.Max),
                       this._color);
        Debug.DrawLine(position,
                       this.transform.TransformDirection(position
                                                         - (Vector3.back + Vector3.right).normalized
                                                         * this._space.Max),
                       this._color);
        if (!this._is_2_d) {
          var position1 = this.transform.position;
          Debug.DrawLine(position1,
                         this.transform.TransformDirection(position1 - Vector3.up * this._space.Max),
                         this._color);
          Debug.DrawLine(position1,
                         this.transform.TransformDirection(position1 - Vector3.down * this._space.Max),
                         this._color);

          Debug.DrawLine(position1,
                         this.transform.TransformDirection(position1
                                                           - (Vector3.up + Vector3.left).normalized
                                                           * this._space.Max),
                         this._color);
          Debug.DrawLine(position1,
                         this.transform.TransformDirection(position1
                                                           - (Vector3.up + Vector3.right).normalized
                                                           * this._space.Max),
                         this._color);
          Debug.DrawLine(position1,
                         this.transform.TransformDirection(position1
                                                           - (Vector3.up + Vector3.forward).normalized
                                                           * this._space.Max),
                         this._color);
          Debug.DrawLine(position1,
                         this.transform.TransformDirection(position1
                                                           - (Vector3.up + Vector3.back).normalized
                                                           * this._space.Max),
                         this._color);

          Debug.DrawLine(position1,
                         this.transform.TransformDirection(position1
                                                           - (Vector3.down + Vector3.left).normalized
                                                           * this._space.Max),
                         this._color);

          Debug.DrawLine(position1,
                         this.transform.TransformDirection(position1
                                                           - (Vector3.down + Vector3.right).normalized
                                                           * this._space.Max),
                         this._color);

          Debug.DrawLine(position1,
                         this.transform.TransformDirection(position1
                                                           - (Vector3.down + Vector3.forward).normalized
                                                           * this._space.Max),
                         this._color);

          Debug.DrawLine(position1,
                         this.transform.TransformDirection(position1
                                                           - (Vector3.down + Vector3.back).normalized
                                                           * this._space.Max),
                         this._color);

          Debug.DrawLine(position1,
                         this.transform.TransformDirection(position1
                                                           - (Vector3.down + Vector3.forward + Vector3.left)
                                                           .normalized
                                                           * this._space.Max),
                         this._color);

          Debug.DrawLine(position1,
                         this.transform.TransformDirection(position1
                                                           - (Vector3.down + Vector3.forward + Vector3.right)
                                                           .normalized
                                                           * this._space.Max),
                         this._color);
          Debug.DrawLine(position1,
                         this.transform.TransformDirection(position1
                                                           - (Vector3.down + Vector3.back + Vector3.left)
                                                           .normalized
                                                           * this._space.Max),
                         this._color);

          Debug.DrawLine(position1,
                         this.transform.TransformDirection(position1
                                                           - (Vector3.down + Vector3.back + Vector3.right)
                                                           .normalized
                                                           * this._space.Max),
                         this._color);

          Debug.DrawLine(position1,
                         this.transform.TransformDirection(position1
                                                           - (Vector3.up + Vector3.forward + Vector3.left)
                                                           .normalized
                                                           * this._space.Max),
                         this._color);

          Debug.DrawLine(position1,
                         this.transform.TransformDirection(position1
                                                           - (Vector3.up + Vector3.forward + Vector3.right)
                                                           .normalized
                                                           * this._space.Max),
                         this._color);
          Debug.DrawLine(position1,
                         this.transform.TransformDirection(position1
                                                           - (Vector3.up + Vector3.back + Vector3.left)
                                                           .normalized
                                                           * this._space.Max),
                         this._color);

          Debug.DrawLine(position1,
                         this.transform.TransformDirection(position1
                                                           - (Vector3.up + Vector3.back + Vector3.right)
                                                           .normalized
                                                           * this._space.Max),
                         this._color);
        }
      }
    }
    #endif
  }
}
