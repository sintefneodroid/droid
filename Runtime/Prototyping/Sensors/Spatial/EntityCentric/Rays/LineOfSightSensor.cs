using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.EntityCentric.Rays {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "LineOfSight"
                    + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [Serializable]
  public class LineOfSightSensor : Sensor,
                                   IHasSingle {
    RaycastHit _hit = new RaycastHit();

    [SerializeField] float _obs_value = 0;

    /// <summary>
    /// </summary>
    [SerializeField]
    Space1 _observation_value_space = Space1.ZeroOne;

    [Header("Specific", order = 102)]
    [SerializeField]
    UnityEngine.Transform _target = null;

    public override string PrototypingTypeName { get { return "LineOfSight"; } }

    public float ObservationValue { get { return this._obs_value; } private set { this._obs_value = value; } }

    public Space1 SingleSpace { get { return this._observation_value_space; } }


    public override IEnumerable<float> FloatEnumerable { get { return new[] {this.ObservationValue}; } }

    public override void UpdateObservation() {
      var distance = Vector3.Distance(this.transform.position, this._target.position);
      if (Physics.Raycast(this.transform.position,
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
