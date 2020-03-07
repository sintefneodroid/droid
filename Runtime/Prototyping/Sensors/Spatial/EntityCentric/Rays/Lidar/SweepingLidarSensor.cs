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
  [AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                               + "SweepingLidar"
                               + SensorComponentMenuPath._Postfix)]
  public class SweepingLidarSensor : Sensor,
                                     IHasSingle {
    [SerializeField] RaycastHit _hit;
    [SerializeField] Vector3 current_direction = Vector3.forward;

    [Header("Observation", order = 103)]
    [SerializeField]
    Space1 _ray_space = new Space1 {Min = 0, Max = 5f, DecimalGranularity = 2};

    [SerializeField] Space1 _sweeping_space = new Space1 {Min = -180, Max = 180, DecimalGranularity = 5};

    [SerializeField] AxisEnum _sweeping_axisEnum = AxisEnum.Rot_y_;
    [SerializeField] int tick_i;
    [Range(0.001f, 999)] [SerializeField] float speed = 1;

    /// <summary>
    /// Does not use the defined sweep space
    /// </summary>
    [SerializeField]
    bool loop = false;

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
        a = this._sweeping_space.Reproject(v : Mathf.Cos(f : a) * .5f + .5f);
      }

      switch (this._sweeping_axisEnum) {
        case AxisEnum.Rot_x_:
          c = Quaternion.Euler(x : a, 0, 0) * c;
          break;
        case AxisEnum.Rot_y_:
          c = Quaternion.Euler(0, y : a, 0) * c;
          break;

        default: throw new ArgumentOutOfRangeException();
      }

      this.current_direction = c;

      this.tick_i += 1;
    }

    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      if (Physics.Raycast(origin : this.transform.position + this._ray_space.Min * this.current_direction,
                          direction : this.transform.TransformDirection(direction : this.current_direction),
                          hitInfo : out this._hit,
                          maxDistance : this._ray_space.Max)) {
        this.ObservationValue = this._ray_space.Project(v : this._hit.distance);
      } else {
        this.ObservationValue = this._ray_space.Max;
      }
    }

    #if UNITY_EDITOR
    [SerializeField] Color _color = Color.green;

    void OnDrawGizmosSelected() {
      if (this.enabled) {
        var position = this.transform.position;
        Debug.DrawLine(start : position,
                       end : position
                             + this.transform.TransformDirection(direction : this.current_direction)
                             * this._ray_space.Max,
                       color : this._color);
      }
    }
    #endif
    /// <summary>
    ///
    /// </summary>
    [field : SerializeField]
    public Single ObservationValue { get; set; }

    public Space1 SingleSpace { get { return this._ray_space; } }
  }
}
