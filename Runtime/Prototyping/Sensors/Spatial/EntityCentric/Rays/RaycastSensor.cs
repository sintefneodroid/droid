using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.EntityCentric.Rays {
  /// <summary>
  ///
  /// </summary>
  [AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                               + "Raycast"
                               + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  public class RaycastSensor : Sensor,
                               IHasSingle {
    [SerializeField] Vector3 _direction = Vector3.forward;

    [SerializeField] RaycastHit _hit = new RaycastHit();

    [SerializeField] Space1 _observation_space = new Space1 {DecimalGranularity = 3, Min = 0f, Max = 100.0f};

    [Header("Observation", order = 103)]
    [SerializeField]
    float _observation_value = 0;

    /// <summary>
    ///
    /// </summary>
    public override string PrototypingTypeName {
      get { return "Raycast" + $"{this._direction.x}{this._direction.y}{this._direction.z}"; }
    }

    /// <summary>
    ///
    /// </summary>
    public Space1 SingleSpace { get { return this._observation_space; } }

    /// <summary>
    ///
    /// </summary>
    public float ObservationValue {
      get { return this._observation_value; }
      private set { this._observation_value = this.SingleSpace.Project(v : value); }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override IEnumerable<float> FloatEnumerable { get { yield return this.ObservationValue; } }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override void UpdateObservation() {
      if (Physics.Raycast(origin : this.transform.position,
                          direction : this.transform.TransformDirection(direction : this._direction),
                          hitInfo : out this._hit,
                          maxDistance : this._observation_space.Max)) {
        this.ObservationValue = this._hit.distance;
      } else {
        this.ObservationValue = this._observation_space.Max;
      }
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log(message : $"Raycast hit at distance {this._hit.distance}");
      }
      #endif
    }

    #if UNITY_EDITOR
    [SerializeField] Color _color = Color.green;

    void OnDrawGizmosSelected() {
      if (this.enabled) {
        var position = this.transform.position;
        Debug.DrawLine(start : position,
                       end : position
                             + this.transform.TransformDirection(direction : this._direction)
                             * this._observation_space.Max,
                       color : this._color);
      }
    }
    #endif
  }
}
