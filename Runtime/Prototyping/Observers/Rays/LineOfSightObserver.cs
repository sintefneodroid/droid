using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Prototyping.Observers.Rays {
  /// <inheritdoc cref="Observer" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      ObserverComponentMenuPath._ComponentMenuPath + "LineOfSight" + ObserverComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [Serializable]
  public class LineOfSightObserver : Observer,
                                     IHasSingle {
    RaycastHit _hit;

    [SerializeField] float _obs_value;

    /// <summary>
    /// </summary>
    [SerializeField]
    ValueSpace _observation_value_space;

    [Header("Specific", order = 102)]
    [SerializeField]
    UnityEngine.Transform _target;

    public override string PrototypingTypeName { get { return "LineOfSight"; } }

    public float ObservationValue { get { return this._obs_value; } private set { this._obs_value = value; } }

    public ValueSpace SingleSpace { get { return this._observation_value_space; } }

    protected override void PreSetup() {

    }

    public override IEnumerable<float> FloatEnumerable
    {
      get { return new[] {this.ObservationValue}; }
    }

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
    }
  }
}
