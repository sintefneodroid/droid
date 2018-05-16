using System;
using Neodroid.Utilities.Interfaces;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  [AddComponentMenu(
      ObserverComponentMenuPath._ComponentMenuPath + "FixedLidar" + ObserverComponentMenuPath._Postfix)]
  public class FixedLidarObserver : Observer,
                               IHasArray {
    [SerializeField] float _range = 100.0f;
    [SerializeField] RaycastHit _hit;

    [SerializeField] bool _is_2_d;

    [Header("Observation", order = 103)]
    [SerializeField]
    float[] _obs_array;

    [SerializeField]
    ValueSpace _observation_value_space =
        new ValueSpace {_Decimal_Granularity = 10, _Min_Value = 0.0f, _Max_Value = 100.0f};

    public override string PrototypingType { get { return "Lidar"; } }

    public Single[] ObservationArray {
      get { return this._obs_array; }
      private set { this._obs_array = value; }
    }

    protected override void PreSetup() {
      if (this._is_2_d) {
        this.ObservationArray = new float[4];
      } else {
        this.ObservationArray = new float[6];
      }

      this.FloatEnumerable = this.ObservationArray;
    }

    public override void UpdateObservation() {
      if (this._is_2_d) {
        var vals = new float[4];
        if (Physics.Raycast(this.transform.position, Vector3.forward, out this._hit, this._range)) {
          vals[0] = this._hit.distance;
        } else {
          vals[0] = this._range;
        }

        vals[0] = this._observation_value_space.ClipNormaliseRound(vals[0]);
        if (Physics.Raycast(this.transform.position, Vector3.left, out this._hit, this._range)) {
          vals[1] = this._hit.distance;
        } else {
          vals[1] = this._range;
        }

        vals[1] = this._observation_value_space.ClipNormaliseRound(vals[1]);
        if (Physics.Raycast(this.transform.position, Vector3.right, out this._hit, this._range)) {
          vals[2] = this._hit.distance;
        } else {
          vals[2] = this._range;
        }

        vals[2] = this._observation_value_space.ClipNormaliseRound(vals[2]);
        if (Physics.Raycast(this.transform.position, Vector3.back, out this._hit, this._range)) {
          vals[3] = this._hit.distance;
        } else {
          vals[3] = this._range;
        }

        vals[3] = this._observation_value_space.ClipNormaliseRound(vals[3]);
        this.ObservationArray = vals;
      } else {
        var vals = new float[6];
        if (Physics.Raycast(this.transform.position, Vector3.forward, out this._hit, this._range)) {
          vals[0] = this._hit.distance;
        } else {
          vals[0] = this._range;
        }

        vals[0] = this._observation_value_space.ClipNormaliseRound(vals[0]);
        if (Physics.Raycast(this.transform.position, Vector3.left, out this._hit, this._range)) {
          vals[1] = this._hit.distance;
        } else {
          vals[1] = this._range;
        }

        vals[1] = this._observation_value_space.ClipNormaliseRound(vals[1]);
        if (Physics.Raycast(this.transform.position, Vector3.right, out this._hit, this._range)) {
          vals[2] = this._hit.distance;
        } else {
          vals[2] = this._range;
        }

        vals[2] = this._observation_value_space.ClipNormaliseRound(vals[2]);
        if (Physics.Raycast(this.transform.position, Vector3.back, out this._hit, this._range)) {
          vals[3] = this._hit.distance;
        } else {
          vals[3] = this._range;
        }

        vals[3] = this._observation_value_space.ClipNormaliseRound(vals[3]);
        if (Physics.Raycast(this.transform.position, Vector3.up, out this._hit, this._range)) {
          vals[4] = this._hit.distance;
        } else {
          vals[4] = this._range;
        }

        vals[4] = this._observation_value_space.ClipNormaliseRound(vals[4]);
        if (Physics.Raycast(this.transform.position, Vector3.down, out this._hit, this._range)) {
          vals[5] = this._hit.distance;
        } else {
          vals[5] = this._range;
        }

        vals[5] = this._observation_value_space.ClipNormaliseRound(vals[5]);
        this.ObservationArray = vals;
      }

      this.FloatEnumerable = this.ObservationArray;
    }

    #if UNITY_EDITOR
    [SerializeField] Color _color = Color.green;

    void OnDrawGizmosSelected() {
      Debug.DrawLine(
          this.transform.position,
          this.transform.position - Vector3.forward * this._range,
          this._color);
      Debug.DrawLine(
          this.transform.position,
          this.transform.position - Vector3.left * this._range,
          this._color);
      Debug.DrawLine(
          this.transform.position,
          this.transform.position - Vector3.right * this._range,
          this._color);
      Debug.DrawLine(
          this.transform.position,
          this.transform.position - Vector3.back * this._range,
          this._color);
      if (!this._is_2_d) {
        Debug.DrawLine(
            this.transform.position,
            this.transform.position - Vector3.up * this._range,
            this._color);
        Debug.DrawLine(
            this.transform.position,
            this.transform.position - Vector3.down * this._range,
            this._color);
      }
    }
    #endif
  }
}
