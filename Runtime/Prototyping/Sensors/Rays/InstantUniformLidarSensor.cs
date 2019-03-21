using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Rays {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "InstantUniformLidar"
                    + SensorComponentMenuPath._Postfix)]
  public class InstantUniformLidarSensor : Sensor,
                             IHasArray {
    [SerializeField] RaycastHit _hit;

    [SerializeField] bool _is_2_d = false;

    [Header("Observation", order = 103)]
    [SerializeField]
    float[] _obs_array;

    [SerializeField] Space1 _space = new Space1() {_Min_Value = 0, _Max_Value = 5f};

    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "Adjacency"; } }

    /// <summary>
    /// </summary>
    public RaycastHit Hit { get { return this._hit; } set { this._hit = value; } }

    public override IEnumerable<float> FloatEnumerable { get { return this.ObservationArray; } }

    /// <summary>
    /// </summary>
    public float[] ObservationArray {
      get { return this._obs_array; }
      private set { this._obs_array = value; }
    }

    public Space1[] ObservationSpace { get { return new[] {this._space}; } }

    /// <summary>
    /// </summary>
    protected override void PreSetup() {
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
        if (Physics.Raycast(this.transform.position + this._space._Min_Value * Vector3.forward,
                            Vector3.forward,
                            out this._hit,
                            this._space._Max_Value)) {
          vals[0] = 1;
        } else {
          vals[0] = this._space._Min_Value;
        }

        if (Physics.Raycast(this.transform.position + this._space._Min_Value * Vector3.left,
                            Vector3.left,
                            out this._hit,
                            this._space._Max_Value)) {
          vals[1] = this._space.ClipNormaliseRound(this._hit.distance);
        } else {
          vals[1] = this._space._Min_Value;
        }

        if (Physics.Raycast(this.transform.position + this._space._Min_Value * Vector3.right,
                            Vector3.right,
                            out this._hit,
                            this._space._Max_Value)) {
          vals[2] = this._space.ClipNormaliseRound(this._hit.distance);
        } else {
          vals[2] = this._space._Min_Value;
        }

        if (Physics.Raycast(this.transform.position + this._space._Min_Value * Vector3.back,
                            Vector3.back,
                            out this._hit,
                            this._space._Max_Value)) {
          vals[3] = this._space.ClipNormaliseRound(this._hit.distance);
        } else {
          vals[3] = this._space._Min_Value;
        }

        if (Physics.Raycast(this.transform.position
                            + this._space._Min_Value * (Vector3.forward + Vector3.left).normalized,
                            (Vector3.forward + Vector3.left).normalized,
                            out this._hit,
                            this._space._Max_Value)) {
          vals[4] = this._space.ClipNormaliseRound(this._hit.distance);
        } else {
          vals[4] = this._space._Min_Value;
        }

        if (Physics.Raycast(this.transform.position
                            + this._space._Min_Value * (Vector3.forward + Vector3.right).normalized,
                            (Vector3.forward + Vector3.right).normalized,
                            out this._hit,
                            this._space._Max_Value)) {
          vals[5] = this._space.ClipNormaliseRound(this._hit.distance);
        } else {
          vals[5] = this._space._Min_Value;
        }

        if (Physics.Raycast(this.transform.position
                            + this._space._Min_Value * (Vector3.back + Vector3.left).normalized,
                            (Vector3.back + Vector3.left).normalized,
                            out this._hit,
                            this._space._Max_Value)) {
          vals[6] = this._space.ClipNormaliseRound(this._hit.distance);
        } else {
          vals[6] = this._space._Min_Value;
        }

        if (Physics.Raycast(this.transform.position
                            + this._space._Min_Value * (Vector3.back + Vector3.right).normalized,
                            (Vector3.back + Vector3.right).normalized,
                            out this._hit,
                            this._space._Max_Value)) {
          vals[7] = this._space.ClipNormaliseRound(this._hit.distance);
        } else {
          vals[7] = this._space._Min_Value;
        }

        this.ObservationArray = vals;
      } else {
        var vals = new float[27];
        if (Physics.Raycast(this.transform.position + this._space._Min_Value * Vector3.forward,
                            Vector3.forward,
                            out this._hit,
                            this._space._Max_Value)) {
          vals[0] = 1;
        } else {
          vals[0] = this._space._Min_Value;
        }

        if (Physics.Raycast(this.transform.position + this._space._Min_Value * Vector3.left,
                            Vector3.left,
                            out this._hit,
                            this._space._Max_Value)) {
          vals[1] = this._space.ClipNormaliseRound(this._hit.distance);
        } else {
          vals[1] = this._space._Min_Value;
        }

        if (Physics.Raycast(this.transform.position + this._space._Min_Value * Vector3.right,
                            Vector3.right,
                            out this._hit,
                            this._space._Max_Value)) {
          vals[2] = this._space.ClipNormaliseRound(this._hit.distance);
        } else {
          vals[2] = this._space._Min_Value;
        }

        if (Physics.Raycast(this.transform.position + this._space._Min_Value * Vector3.back,
                            Vector3.back,
                            out this._hit,
                            this._space._Max_Value)) {
          vals[3] = this._space.ClipNormaliseRound(this._hit.distance);
        } else {
          vals[3] = this._space._Min_Value;
        }

        if (Physics.Raycast(this.transform.position
                            + this._space._Min_Value * (Vector3.forward + Vector3.left).normalized,
                            (Vector3.forward + Vector3.left).normalized,
                            out this._hit,
                            this._space._Max_Value)) {
          vals[4] = this._space.ClipNormaliseRound(this._hit.distance);
        } else {
          vals[4] = this._space._Min_Value;
        }

        if (Physics.Raycast(this.transform.position
                            + this._space._Min_Value * (Vector3.forward + Vector3.right).normalized,
                            (Vector3.forward + Vector3.right).normalized,
                            out this._hit,
                            this._space._Max_Value)) {
          vals[5] = this._space.ClipNormaliseRound(this._hit.distance);
        } else {
          vals[5] = this._space._Min_Value;
        }

        if (Physics.Raycast(this.transform.position
                            + this._space._Min_Value * (Vector3.back + Vector3.left).normalized,
                            (Vector3.back + Vector3.left).normalized,
                            out this._hit,
                            this._space._Max_Value)) {
          vals[6] = this._space.ClipNormaliseRound(this._hit.distance);
        } else {
          vals[6] = this._space._Min_Value;
        }

        if (Physics.Raycast(this.transform.position
                            + this._space._Min_Value * (Vector3.back + Vector3.right).normalized,
                            (Vector3.back + Vector3.right).normalized,
                            out this._hit,
                            this._space._Max_Value)) {
          vals[7] = this._space.ClipNormaliseRound(this._hit.distance);
        } else {
          vals[7] = this._space._Min_Value;
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
        Debug.DrawLine(position, position - Vector3.forward * this._space._Max_Value, this._color);
        Debug.DrawLine(position, position - Vector3.left * this._space._Max_Value, this._color);
        Debug.DrawLine(position, position - Vector3.right * this._space._Max_Value, this._color);
        Debug.DrawLine(position, position - Vector3.back * this._space._Max_Value, this._color);
        Debug.DrawLine(position,
                       position - (Vector3.forward + Vector3.left).normalized * this._space._Max_Value,
                       this._color);
        Debug.DrawLine(position,
                       position - (Vector3.forward + Vector3.right).normalized * this._space._Max_Value,
                       this._color);
        Debug.DrawLine(position,
                       position - (Vector3.back + Vector3.left).normalized * this._space._Max_Value,
                       this._color);
        Debug.DrawLine(position,
                       position - (Vector3.back + Vector3.right).normalized * this._space._Max_Value,
                       this._color);
        if (!this._is_2_d) {
          var position1 = this.transform.position;
          Debug.DrawLine(position1, position1 - Vector3.up * this._space._Max_Value, this._color);
          Debug.DrawLine(position1, position1 - Vector3.down * this._space._Max_Value, this._color);

          Debug.DrawLine(position1,
                         position1 - (Vector3.up + Vector3.left).normalized * this._space._Max_Value,
                         this._color);
          Debug.DrawLine(position1,
                         position1 - (Vector3.up + Vector3.right).normalized * this._space._Max_Value,
                         this._color);
          Debug.DrawLine(position1,
                         position1 - (Vector3.up + Vector3.forward).normalized * this._space._Max_Value,
                         this._color);
          Debug.DrawLine(position1,
                         position1 - (Vector3.up + Vector3.back).normalized * this._space._Max_Value,
                         this._color);

          Debug.DrawLine(position1,
                         position1 - (Vector3.down + Vector3.left).normalized * this._space._Max_Value,
                         this._color);

          Debug.DrawLine(position1,
                         position1 - (Vector3.down + Vector3.right).normalized * this._space._Max_Value,
                         this._color);

          Debug.DrawLine(position1,
                         position1 - (Vector3.down + Vector3.forward).normalized * this._space._Max_Value,
                         this._color);

          Debug.DrawLine(position1,
                         position1 - (Vector3.down + Vector3.back).normalized * this._space._Max_Value,
                         this._color);

          Debug.DrawLine(position1,
                         position1
                         - (Vector3.down + Vector3.forward + Vector3.left).normalized
                         * this._space._Max_Value,
                         this._color);

          Debug.DrawLine(position1,
                         position1
                         - (Vector3.down + Vector3.forward + Vector3.right).normalized
                         * this._space._Max_Value,
                         this._color);
          Debug.DrawLine(position1,
                         position1
                         - (Vector3.down + Vector3.back + Vector3.left).normalized * this._space._Max_Value,
                         this._color);

          Debug.DrawLine(position1,
                         position1
                         - (Vector3.down + Vector3.back + Vector3.right).normalized * this._space._Max_Value,
                         this._color);

          Debug.DrawLine(position1,
                         position1
                         - (Vector3.up + Vector3.forward + Vector3.left).normalized * this._space._Max_Value,
                         this._color);

          Debug.DrawLine(position1,
                         position1
                         - (Vector3.up + Vector3.forward + Vector3.right).normalized * this._space._Max_Value,
                         this._color);
          Debug.DrawLine(position1,
                         position1
                         - (Vector3.up + Vector3.back + Vector3.left).normalized * this._space._Max_Value,
                         this._color);

          Debug.DrawLine(position1,
                         position1
                         - (Vector3.up + Vector3.back + Vector3.right).normalized * this._space._Max_Value,
                         this._color);
        }
      }
    }
    #endif
  }
}
