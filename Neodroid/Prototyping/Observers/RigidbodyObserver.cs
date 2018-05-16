using Neodroid.Utilities.Interfaces;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  [AddComponentMenu(
      ObserverComponentMenuPath._ComponentMenuPath + "Rigidbody" + ObserverComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [RequireComponent(typeof(Rigidbody))]
  public class RigidbodyObserver : Observer,
                                   IHasRigidbody {
    [SerializeField] Space3 _angular_space = new Space3(10);

    [Header("Observation", order = 103)]
    [SerializeField]
    Vector3 _angular_velocity;

    [SerializeField] bool _differential;
    [SerializeField] float _last_update_time;

    [Header("Specfic", order = 103)]
    [SerializeField]
    Rigidbody _rigidbody;

    [SerializeField] Vector3 _velocity;
    [SerializeField] Space3 _velocity_space = new Space3(10);

    public override string PrototypingType {
      get {
        if (this._differential) {
          return "RigidbodyDifferential";
        }

        return "Rigidbody";
      }
    }

    public Vector3 Velocity {
      get { return this._velocity; }
      set {
        this._velocity = this.NormaliseObservationUsingSpace
                             ? this._velocity_space.ClipNormaliseRound(value)
                             : value;
      }
    }

    public Vector3 AngularVelocity {
      get { return this._angular_velocity; }
      set {
        this._angular_velocity = this.NormaliseObservationUsingSpace
                                     ? this._angular_space.ClipNormaliseRound(value)
                                     : value;
      }
    }

    public Space3 VelocitySpace { get; }
    public Space3 AngularSpace { get; }

    protected override void Setup() { this._rigidbody = this.GetComponent<Rigidbody>(); }

    public override void UpdateObservation() {
      var update_time_difference = Time.time - this._last_update_time;
      if (this._differential && update_time_difference > 0) {
        var vel_diff = this.Velocity - this._rigidbody.velocity;
        var ang_diff = this.AngularVelocity - this._rigidbody.angularVelocity;
        if (vel_diff.magnitude > 0) {
          this.Velocity = vel_diff / (update_time_difference + float.Epsilon);
        } else {
          this.Velocity = vel_diff;
        }

        if (ang_diff.magnitude > 0) {
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

    protected override void PreSetup() {
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
