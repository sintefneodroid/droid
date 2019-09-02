using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Rays.Lidar {
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
    [SerializeField] Space1 sweeping_range = Space1.MinusOneOne;

    [Header("Observation", order = 103)]
    [SerializeField]
    float[] _obs_array;

    [SerializeField] Space1 _space = new Space1 {Min = 0, Max = 5f};

    [SerializeField] bool ignore_rotation = false;

    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "SweepingLidar"; } }

    /// <summary>
    ///
    /// </summary>
    public override IEnumerable<float> FloatEnumerable { get { return new []{ this.ObservationValue}; } }


    /// <summary>
    /// </summary>
    protected override void PreSetup() { }


    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      float outs;
        if (Physics.Raycast(this.transform.position + this._space.Min * Vector3.forward,
                            this.transform.TransformDirection(Vector3.forward),
                            out this._hit,
                            this._space.Max)) {
          outs = this._space.ClipNormaliseRound(this._hit.distance);
        } else {
          outs = this._space.Max;
        }

    }

    #if UNITY_EDITOR
    [SerializeField] Color _color = Color.green;

    void OnDrawGizmosSelected() {
      if (this.enabled) {
        var position = this.transform.position;
        Debug.DrawLine(position,
                       this.transform.TransformDirection(position - Vector3.forward * this._space.Max),
                       this._color);
        Debug.DrawLine(position,
                       this.transform.TransformDirection(position - Vector3.left * this._space.Max),
                       this._color);
        Debug.DrawLine(position,
                       this.transform.TransformDirection(position - Vector3.right * this._space.Max),
                       this._color);
        Debug.DrawLine(position,
                       this.transform.TransformDirection(position - Vector3.back * this._space.Max),
                       this._color);
        Debug.DrawLine(position,
                       this.transform.TransformDirection(position
                                                         - (Vector3.forward + Vector3.left).normalized
                                                         * this._space.Max),
                       this._color);
        Debug.DrawLine(position,
                       this.transform.TransformDirection(position
                                                         - (Vector3.forward + Vector3.right).normalized
                                                         * this._space.Max),
                       this._color);
        Debug.DrawLine(position,
                       this.transform.TransformDirection(position
                                                         - (Vector3.back + Vector3.left).normalized
                                                         * this._space.Max),
                       this._color);
        Debug.DrawLine(position,
                       this.transform.TransformDirection(position
                                                         - (Vector3.back + Vector3.right).normalized
                                                         * this._space.Max),
                       this._color);
      }
    }
    #endif
    public Single ObservationValue { get; }
    public Space1 SingleSpace { get; }
  }
}
