using System;
using System.Collections.Generic;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.EntityCentric.Rays.Lidar {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "SweepingLidar"
                    + SensorComponentMenuPath._Postfix)]
  public class SweepingLidarSensor : Sensor,
                                     IHasSingle {
    [SerializeField] RaycastHit _hit;
    [SerializeField] Vector3 current_direction = Vector3.forward;

    [Header("Observation", order = 103)]
    [SerializeField]
    Space1 _ray_space = new Space1 {Min = 0, Max = 5f, DecimalGranularity = 2};

    [SerializeField]
    Space1 _sweeping_space =
        new Space1 {Min = -180, Max = 180, DecimalGranularity = 5};

    [SerializeField] Axis sweeping_axis = Axis.Rot_y_;
    [SerializeField] int tick_i;
    [Range(0.001f,999)][SerializeField] float speed = 1;

    /// <summary>
    /// Does not use the defined sweep space
    /// </summary>
    [SerializeField] bool loop = false;

    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "SweepingLidar"; } }

    /// <summary>
    ///
    /// </summary>
    public override IEnumerable<float> FloatEnumerable { get { return new[] {this.ObservationValue}; } }

    /// <summary>
    /// </summary>
    public override void PreSetup() { this.PrototypingReset(); }

    public override void PrototypingReset() { this.tick_i = 0; }

    /// <summary>
    ///
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public override void Tick() {
      var c = Vector3.forward;
      float a = this._sweeping_space.Precision * this.tick_i * this.speed;
      if (!this.loop) {
        a = this._sweeping_space.Reproject(Mathf.Cos(a) * .5f + .5f);
      }

      switch (this.sweeping_axis) {
        case Axis.Rot_x_:
          c = Quaternion.Euler(a, 0, 0) * c;
          break;
        case Axis.Rot_y_:
          c = Quaternion.Euler(0, a, 0) * c;
          break;

        default: throw new ArgumentOutOfRangeException();
      }

      this.current_direction = c;

      this.tick_i += 1;
    }

    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      if (Physics.Raycast(this.transform.position + this._ray_space.Min * this.current_direction,
                          this.transform.TransformDirection(this.current_direction),
                          out this._hit,
                          this._ray_space.Max)) {
        this.ObservationValue = this._ray_space.Project(this._hit.distance);
      } else {
        this.ObservationValue = this._ray_space.Max;
      }
    }

    #if UNITY_EDITOR
    [SerializeField] Color _color = Color.green;
    [SerializeField] Single _observation_value;

    void OnDrawGizmosSelected() {
      if (this.enabled) {
        var position = this.transform.position;
        Debug.DrawLine(position,
                       position + this.transform.TransformDirection(this.current_direction) * this._ray_space.Max,
                       this._color);
      }
    }
    #endif
    /// <summary>
    ///
    /// </summary>
    public Single ObservationValue {
      get { return this._observation_value; }
      set { this._observation_value = value; }
    }

    public Space1 SingleSpace { get { return this._ray_space; } }
  }
}
