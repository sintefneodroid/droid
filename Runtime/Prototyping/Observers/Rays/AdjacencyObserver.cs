using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Prototyping.Observers.Rays {
  /// <inheritdoc cref="Observer" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ObserverComponentMenuPath._ComponentMenuPath
                    + "Adjacency"
                    + ObserverComponentMenuPath._Postfix)]
  public class AdjacencyObserver : Observer,
                                   IHasArray {
    [SerializeField] RaycastHit _hit;

    [SerializeField] bool _is_2_d = false;

    [Header("Observation", order = 103)]
    [SerializeField]
    float[] _obs_array;

    [SerializeField] float _range = 1.0f;

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

    public ValueSpace[] ObservationSpace { get; set; }

    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      if (this._is_2_d) {
        this.ObservationArray = new float[8];
        this.ObservationSpace = new ValueSpace[8];
      } else {
        this.ObservationArray = new float[27];
        this.ObservationSpace = new ValueSpace[27];
      }
    }

    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      if (this._is_2_d) {
        var vals = new float[8];
        if (Physics.Raycast(this.transform.position, Vector3.forward, out this._hit, this._range)) {
          vals[0] = 1;
        } else {
          vals[0] = 0;
        }

        if (Physics.Raycast(this.transform.position, Vector3.left, out this._hit, this._range)) {
          vals[1] = 1;
        } else {
          vals[1] = 0;
        }

        if (Physics.Raycast(this.transform.position, Vector3.right, out this._hit, this._range)) {
          vals[2] = 1;
        } else {
          vals[2] = 0;
        }

        if (Physics.Raycast(this.transform.position, Vector3.back, out this._hit, this._range)) {
          vals[3] = 1;
        } else {
          vals[3] = 0;
        }

        if (Physics.Raycast(this.transform.position,
                            (Vector3.forward + Vector3.left).normalized,
                            out this._hit,
                            this._range)) {
          vals[4] = 1;
        } else {
          vals[4] = 0;
        }

        if (Physics.Raycast(this.transform.position,
                            (Vector3.forward + Vector3.right).normalized,
                            out this._hit,
                            this._range)) {
          vals[5] = 1;
        } else {
          vals[5] = 0;
        }

        if (Physics.Raycast(this.transform.position,
                            (Vector3.back + Vector3.left).normalized,
                            out this._hit,
                            this._range)) {
          vals[6] = 1;
        } else {
          vals[6] = 0;
        }

        if (Physics.Raycast(this.transform.position,
                            (Vector3.back + Vector3.right).normalized,
                            out this._hit,
                            this._range)) {
          vals[7] = 1;
        } else {
          vals[7] = 0;
        }

        //TODO: Normalise observations with observation space

        this.ObservationArray = vals;
      } else {
        var vals = new float[27];
        if (Physics.Raycast(this.transform.position, Vector3.forward, out this._hit, this._range)) {
          vals[0] = 1;
        } else {
          vals[0] = 0;
        }

        if (Physics.Raycast(this.transform.position, Vector3.left, out this._hit, this._range)) {
          vals[1] = 1;
        } else {
          vals[1] = 0;
        }

        if (Physics.Raycast(this.transform.position, Vector3.right, out this._hit, this._range)) {
          vals[2] = 1;
        } else {
          vals[2] = 0;
        }

        if (Physics.Raycast(this.transform.position, Vector3.back, out this._hit, this._range)) {
          vals[3] = 1;
        } else {
          vals[3] = 0;
        }

        if (Physics.Raycast(this.transform.position, Vector3.up, out this._hit, this._range)) {
          vals[4] = 1;
        } else {
          vals[4] = 0;
        }

        if (Physics.Raycast(this.transform.position, Vector3.down, out this._hit, this._range)) {
          vals[5] = 1;
        } else {
          vals[5] = 0;
        }

        //TODO:Missing combinations Vector3.down+Vector3.left...

        //TODO: Normalise observations with observation space

        this.ObservationArray = vals;
      }
    }

    #if UNITY_EDITOR
    [SerializeField] Color _color = Color.green;

    void OnDrawGizmosSelected() {
      if (this.enabled) {
        var position = this.transform.position;
        Debug.DrawLine(position, position - Vector3.forward * this._range, this._color);
        Debug.DrawLine(position, position - Vector3.left * this._range, this._color);
        Debug.DrawLine(position, position - Vector3.right * this._range, this._color);
        Debug.DrawLine(position, position - Vector3.back * this._range, this._color);
        Debug.DrawLine(position,
                       position - (Vector3.forward + Vector3.left).normalized * this._range,
                       this._color);
        Debug.DrawLine(position,
                       position - (Vector3.forward + Vector3.right).normalized * this._range,
                       this._color);
        Debug.DrawLine(position,
                       position - (Vector3.back + Vector3.left).normalized * this._range,
                       this._color);
        Debug.DrawLine(position,
                       position - (Vector3.back + Vector3.right).normalized * this._range,
                       this._color);
        if (!this._is_2_d) {
          var position1 = this.transform.position;
          Debug.DrawLine(position1, position1 - Vector3.up * this._range, this._color);
          Debug.DrawLine(position1, position1 - Vector3.down * this._range, this._color);

          Debug.DrawLine(position1,
                         position1 - (Vector3.up + Vector3.left).normalized * this._range,
                         this._color);
          Debug.DrawLine(position1,
                         position1 - (Vector3.up + Vector3.right).normalized * this._range,
                         this._color);
          Debug.DrawLine(position1,
                         position1 - (Vector3.up + Vector3.forward).normalized * this._range,
                         this._color);
          Debug.DrawLine(position1,
                         position1 - (Vector3.up + Vector3.back).normalized * this._range,
                         this._color);

          Debug.DrawLine(position1,
                         position1 - (Vector3.down + Vector3.left).normalized * this._range,
                         this._color);

          Debug.DrawLine(position1,
                         position1 - (Vector3.down + Vector3.right).normalized * this._range,
                         this._color);

          Debug.DrawLine(position1,
                         position1 - (Vector3.down + Vector3.forward).normalized * this._range,
                         this._color);

          Debug.DrawLine(position1,
                         position1 - (Vector3.down + Vector3.back).normalized * this._range,
                         this._color);

          Debug.DrawLine(position1,
                         position1 - (Vector3.down + Vector3.forward + Vector3.left).normalized * this._range,
                         this._color);

          Debug.DrawLine(position1,
                         position1
                         - (Vector3.down + Vector3.forward + Vector3.right).normalized * this._range,
                         this._color);
          Debug.DrawLine(position1,
                         position1 - (Vector3.down + Vector3.back + Vector3.left).normalized * this._range,
                         this._color);

          Debug.DrawLine(position1,
                         position1 - (Vector3.down + Vector3.back + Vector3.right).normalized * this._range,
                         this._color);

          Debug.DrawLine(position1,
                         position1 - (Vector3.up + Vector3.forward + Vector3.left).normalized * this._range,
                         this._color);

          Debug.DrawLine(position1,
                         position1 - (Vector3.up + Vector3.forward + Vector3.right).normalized * this._range,
                         this._color);
          Debug.DrawLine(position1,
                         position1 - (Vector3.up + Vector3.back + Vector3.left).normalized * this._range,
                         this._color);

          Debug.DrawLine(position1,
                         position1 - (Vector3.up + Vector3.back + Vector3.right).normalized * this._range,
                         this._color);
        }
      }
    }
    #endif
  }
}
