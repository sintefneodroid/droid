using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Prototyping.Observers.Rays {
  [AddComponentMenu(ObserverComponentMenuPath._ComponentMenuPath
                    + "FixedLidar"
                    + ObserverComponentMenuPath._Postfix)]
  public class FixedLidarObserver : Observer,
                                    IHasArray {
    [SerializeField] RaycastHit _hit = new RaycastHit();

    [SerializeField] bool _is_2_d = false;

    [Header("Observation", order = 103)]
    [SerializeField]
    float[] _obs_array = null;

    [SerializeField] float _range = 100.0f;

    public override string PrototypingTypeName { get { return "Lidar"; } }

    public float[] ObservationArray {
      get { return this._obs_array; }
      private set { this._obs_array = value; }
    }

    [SerializeField]
    Space1 _observationSpace1 =
        new Space1 {_Decimal_Granularity = 10, _Min_Value = 0.0f, _Max_Value = 100.0f};

    public Space1[] ObservationSpace { get { return new Space1[this.ObservationArray.Length]; } }

    protected override void PreSetup() {
      if (this._is_2_d) {
        this.ObservationArray = new float[4];
      } else {
        this.ObservationArray = new float[6];
      }
    }

    public override IEnumerable<float> FloatEnumerable { get { return this.ObservationArray; } }

    public override void UpdateObservation() {
      if (this._is_2_d) {
        var vals = new float[4];
        if (Physics.Raycast(this.transform.position, Vector3.forward, out this._hit, this._range)) {
          vals[0] = this._hit.distance;
        } else {
          vals[0] = this._range;
        }

        vals[0] = this._observationSpace1.ClipNormaliseRound(vals[0]);
        if (Physics.Raycast(this.transform.position, Vector3.left, out this._hit, this._range)) {
          vals[1] = this._hit.distance;
        } else {
          vals[1] = this._range;
        }

        vals[1] = this._observationSpace1.ClipNormaliseRound(vals[1]);
        if (Physics.Raycast(this.transform.position, Vector3.right, out this._hit, this._range)) {
          vals[2] = this._hit.distance;
        } else {
          vals[2] = this._range;
        }

        vals[2] = this._observationSpace1.ClipNormaliseRound(vals[2]);
        if (Physics.Raycast(this.transform.position, Vector3.back, out this._hit, this._range)) {
          vals[3] = this._hit.distance;
        } else {
          vals[3] = this._range;
        }

        vals[3] = this._observationSpace1.ClipNormaliseRound(vals[3]);
        this.ObservationArray = vals;
      } else {
        var vals = new float[6];
        if (Physics.Raycast(this.transform.position, Vector3.forward, out this._hit, this._range)) {
          vals[0] = this._hit.distance;
        } else {
          vals[0] = this._range;
        }

        vals[0] = this._observationSpace1.ClipNormaliseRound(vals[0]);
        if (Physics.Raycast(this.transform.position, Vector3.left, out this._hit, this._range)) {
          vals[1] = this._hit.distance;
        } else {
          vals[1] = this._range;
        }

        vals[1] = this._observationSpace1.ClipNormaliseRound(vals[1]);
        if (Physics.Raycast(this.transform.position, Vector3.right, out this._hit, this._range)) {
          vals[2] = this._hit.distance;
        } else {
          vals[2] = this._range;
        }

        vals[2] = this._observationSpace1.ClipNormaliseRound(vals[2]);
        if (Physics.Raycast(this.transform.position, Vector3.back, out this._hit, this._range)) {
          vals[3] = this._hit.distance;
        } else {
          vals[3] = this._range;
        }

        vals[3] = this._observationSpace1.ClipNormaliseRound(vals[3]);
        if (Physics.Raycast(this.transform.position, Vector3.up, out this._hit, this._range)) {
          vals[4] = this._hit.distance;
        } else {
          vals[4] = this._range;
        }

        vals[4] = this._observationSpace1.ClipNormaliseRound(vals[4]);
        if (Physics.Raycast(this.transform.position, Vector3.down, out this._hit, this._range)) {
          vals[5] = this._hit.distance;
        } else {
          vals[5] = this._range;
        }

        vals[5] = this._observationSpace1.ClipNormaliseRound(vals[5]);
        this.ObservationArray = vals;
      }
    }

    #if UNITY_EDITOR
    [SerializeField] Color _color = Color.green;

    void OnDrawGizmosSelected() {
      if (this.enabled) {
        Debug.DrawLine(this.transform.position,
                       this.transform.position - Vector3.forward * this._range,
                       this._color);
        Debug.DrawLine(this.transform.position,
                       this.transform.position - Vector3.left * this._range,
                       this._color);
        Debug.DrawLine(this.transform.position,
                       this.transform.position - Vector3.right * this._range,
                       this._color);
        Debug.DrawLine(this.transform.position,
                       this.transform.position - Vector3.back * this._range,
                       this._color);
        if (!this._is_2_d) {
          Debug.DrawLine(this.transform.position,
                         this.transform.position - Vector3.up * this._range,
                         this._color);
          Debug.DrawLine(this.transform.position,
                         this.transform.position - Vector3.down * this._range,
                         this._color);
        }
      }
    }
    #endif
  }
}
