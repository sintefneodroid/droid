using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Observers.Rigidbody {
  /// <summary>
  /// </summary>
  [AddComponentMenu(
       ObserverComponentMenuPath._ComponentMenuPath + "Rigidbody" + ObserverComponentMenuPath._Postfix),
   ExecuteInEditMode, RequireComponent(typeof(UnityEngine.Rigidbody))]
  public class RigidbodyObserver : Observer,
                                   IHasRigidbody {
    [Header("Observation", order = 100), SerializeField]
    Vector3 _angular_velocity;

    [SerializeField] Vector3 _velocity;

    [SerializeField] float _last_update_time;

    [Header("Configuration", order = 110), SerializeField]
    UnityEngine.Rigidbody _rigidbody;

    [SerializeField] bool _differential;
    [SerializeField] Space3 _velocity_space = new Space3(10);
    [SerializeField] Space3 _angular_space = new Space3(10);

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
      set {
        this._velocity = this.NormaliseObservation ? this._velocity_space.ClipNormaliseRound(value) : value;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Vector3 AngularVelocity {
      get { return this._angular_velocity; }
      set {
        this._angular_velocity =
            this.NormaliseObservation ? this._angular_space.ClipNormaliseRound(value) : value;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Space3 VelocitySpace {
      get { return this._velocity_space; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Space3 AngularSpace {
      get { return this._angular_space; }
    }

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

      this._last_update_time = Time.time;

      this.FloatEnumerable = new[] {
          this.Velocity.x,
          this.Velocity.y,
          this.Velocity.z,
          this.AngularVelocity.x,
          this.AngularVelocity.y,
          this.AngularVelocity.z
      };
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this._rigidbody = this.GetComponent<UnityEngine.Rigidbody>();
      this.FloatEnumerable = new[] {
          this.Velocity.x,
          this.Velocity.y,
          this.Velocity.z,
          this.AngularVelocity.x,
          this.AngularVelocity.y,
          this.AngularVelocity.z
      };
    }
  }
}