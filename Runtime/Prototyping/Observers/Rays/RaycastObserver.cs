using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Prototyping.Observers.Rays {
  [AddComponentMenu(ObserverComponentMenuPath._ComponentMenuPath
                    + "Raycast"
                    + ObserverComponentMenuPath._Postfix)]
  public class RaycastObserver : Observer,
                                 IHasSingle {
    [SerializeField] Vector3 _direction = Vector3.forward;

    [SerializeField] RaycastHit _hit = new RaycastHit();

    [SerializeField]
    Space1 _observation_space = new Space1 {_Decimal_Granularity = 3, _Min_Value = 0, _Max_Value = 100.0f};

    [Header("Observation", order = 103)]
    [SerializeField]
    float _observation_value = 0;

    [SerializeField] Space1 _observation_value_space = Space1.ZeroOne;

    [SerializeField] float _range = 100.0f;

    public override string PrototypingTypeName {
      get { return "Raycast" + $"{this._direction.x}{this._direction.y}{this._direction.z}"; }
    }

    public Space1 SingleSpace { get { return this._observation_value_space; } }

    public float ObservationValue {
      get { return this._observation_value; }
      private set {
        this._observation_value = this.NormaliseObservation
                                      ? this._observation_space.ClipNormaliseRound(value)
                                      : value;
      }
    }

    protected override void PreSetup() { }

    public override IEnumerable<float> FloatEnumerable { get { return new[] {this.ObservationValue}; } }

    public override void UpdateObservation() {
      if (Physics.Raycast(this.transform.position, this._direction, out this._hit, this._range)) {
        this.ObservationValue = this._hit.distance;
      } else {
        this.ObservationValue = this._range;
      }
    }

    #if UNITY_EDITOR
    [SerializeField] Color _color = Color.green;

    void OnDrawGizmosSelected() {
      if (this.enabled) {
        var position = this.transform.position;
        Debug.DrawLine(position, position - this._direction * this._range, this._color);
      }
    }
    #endif
  }
}
