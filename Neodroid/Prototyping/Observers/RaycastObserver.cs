using System;
using Neodroid.Utilities.Interfaces;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  [AddComponentMenu(
      ObserverComponentMenuPath._ComponentMenuPath + "Raycast" + ObserverComponentMenuPath._Postfix)]
  public class RaycastObserver : Observer,
                                 IHasSingle {
    [SerializeField] Vector3 _direction = Vector3.forward;

    [SerializeField] float _range = 100.0f;

    [SerializeField] RaycastHit _hit;
    [SerializeField] ValueSpace _observation_value_space;
    public ValueSpace SingleSpace { get { return this._observation_value_space; } }

    [SerializeField]
    ValueSpace _observation_space =
        new ValueSpace {_Decimal_Granularity = 3, _Min_Value = 0, _Max_Value = 100.0f};

    [Header("Observation", order = 103)]
    [SerializeField]
    float _observation_value;

    public override string PrototypingType {
      get { return "Raycast" + $"{this._direction.x}{this._direction.y}{this._direction.z}"; }
    }

    public Single ObservationValue {
      get { return this._observation_value; }
      private set {
        this._observation_value = this.NormaliseObservationUsingSpace
                                      ? this._observation_space.ClipNormaliseRound(value)
                                      : value;
      }
    }

    protected override void PreSetup() { this.FloatEnumerable = new[] {this.ObservationValue}; }

    public override void UpdateObservation() {
      if (Physics.Raycast(this.transform.position, this._direction, out this._hit, this._range)) {
        this.ObservationValue = this._hit.distance;
      } else {
        this.ObservationValue = this._range;
      }

      this.FloatEnumerable = new[] {this.ObservationValue};
    }

    #if UNITY_EDITOR
    [SerializeField] Color _color = Color.green;

    void OnDrawGizmosSelected() {
      Debug.DrawLine(
          this.transform.position,
          this.transform.position - this._direction * this._range,
          this._color);
    }
    #endif
  }
}
