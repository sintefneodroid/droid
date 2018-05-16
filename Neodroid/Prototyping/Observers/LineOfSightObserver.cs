using System;
using Neodroid.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  [AddComponentMenu(
      ObserverComponentMenuPath._ComponentMenuPath + "LineOfSight" + ObserverComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [Serializable]
  public class LineOfSightObserver : Observer,
                                     IHasSingle {
    RaycastHit _hit;

    [SerializeField] float _obs_value;

    [Header("Specfic", order = 102)]
    [SerializeField]
    Transform _target;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    Utilities.Structs.ValueSpace _observation_value_space;

    public override string PrototypingType { get { return "LineOfSight"; } }

    public Single ObservationValue {
      get { return this._obs_value; }
      private set { this._obs_value = value; }
    }

    public Utilities.Structs.ValueSpace SingleSpace { get { return this._observation_value_space; } }

    protected override void PreSetup() { this.FloatEnumerable = new[] {this.ObservationValue}; }

    public override void UpdateObservation() {
      var distance = Vector3.Distance(this.transform.position, this._target.position);
      if (Physics.Raycast(
          this.transform.position,
          this._target.position - this.transform.position,
          out this._hit,
          distance)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(this._hit.distance);
        }
        #endif

        if (this._hit.collider.gameObject != this._target.gameObject) {
          this.ObservationValue = 0;
        } else {
          this.ObservationValue = 1;
        }
      } else {
        this.ObservationValue = 1;
      }

      this.FloatEnumerable = new[] {this.ObservationValue};
    }
  }
}
