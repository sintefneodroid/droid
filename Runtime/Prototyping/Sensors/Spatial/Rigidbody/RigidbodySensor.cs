using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.Rigidbody {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                               + "Rigidbody"
                               + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [RequireComponent(requiredComponent : typeof(UnityEngine.Rigidbody))]
  public class RigidbodySensor : Sensor,
                                 IHasRigidbody {
    [SerializeField] Space3 _angular_space = Space3.ZeroOne;

    [Header("Observation", order = 100)]
    [SerializeField]
    Vector3 _angular_velocity = Vector3.zero;

    [SerializeField] bool _differential = false;

    [SerializeField] float _last_update_time = 0;

    [Header("Configuration", order = 110)]
    [SerializeField]
    UnityEngine.Rigidbody _rigidbody = null;

    [SerializeField] Vector3 _velocity = Vector3.zero;
    [SerializeField] Space3 _velocity_space = Space3.ZeroOne;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName {
      get {
        if (this._differential) {
          return "RigidbodyDifferential";
        }

        return "Rigidbody";
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Vector3 Velocity {
      get { return this._velocity; }
      set { this._velocity = this._velocity_space.Project(v : value); }
    }

    void OnDrawGizmosSelected() {
      var rb_pos = this._rigidbody.position;
      Debug.DrawLine(start : rb_pos, end : rb_pos + this._rigidbody.angularVelocity);
      Debug.DrawLine(start : rb_pos, end : rb_pos + this._rigidbody.velocity,color : Color.red);
    }

    
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Vector3 AngularVelocity {
      get { return this._angular_velocity; }
      set { this._angular_velocity = this._angular_space.Project(v : value); }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Space3 VelocitySpace { get { return this._velocity_space; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Space3 AngularSpace { get { return this._angular_space; } }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override IEnumerable<float> FloatEnumerable {
      get {
        yield return this.Velocity.x;
        yield return this.Velocity.y;
        yield return this.Velocity.z;
        yield return this.AngularVelocity.x;
        yield return this.AngularVelocity.y;
        yield return this.AngularVelocity.z;
      }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override void UpdateObservation() {
      var update_time_difference = Time.time - this._last_update_time;
      if (this._differential && update_time_difference > 0) {
        var vel_diff = this.Velocity - this._rigidbody.velocity;
        var ang_diff = this.AngularVelocity - this._rigidbody.angularVelocity;

        var vel_magnitude = vel_diff.magnitude;
        if (vel_magnitude > 0) {
          this.Velocity = vel_diff / (update_time_difference + float.Epsilon);
        } else {
          this.Velocity = vel_diff;
        }

        var ang_magnitude = ang_diff.magnitude;
        if (ang_magnitude > 0) {
          this.AngularVelocity = ang_diff / (update_time_difference + float.Epsilon);
        } else {
          this.AngularVelocity = ang_diff;
        }
      } else {
        this.Velocity = this._rigidbody.velocity;
        this.AngularVelocity = this._rigidbody.angularVelocity;
      }

      this._last_update_time = Time.realtimeSinceStartup;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() { this._rigidbody = this.GetComponent<UnityEngine.Rigidbody>(); }
  }
}
