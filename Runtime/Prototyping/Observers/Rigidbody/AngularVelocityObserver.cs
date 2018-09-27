using System;
using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Observers.Rigidbody {
  public class AngularVelocityObserver : Observer,
                                         IHasTriple {
    [SerializeField] Space3 _angular_velocity_space = new Space3(10);
    [SerializeField] Vector3 _angular_velocity;

    [SerializeField] UnityEngine.Rigidbody _rigidbody;

    protected override void PreSetup() {
      this._rigidbody = this.GetComponent<UnityEngine.Rigidbody>();
      this.FloatEnumerable =
          new[] {this.ObservationValue.x, this.ObservationValue.y, this.ObservationValue.z};
    }

    public override void UpdateObservation() {
      this.ObservationValue = this._rigidbody.angularVelocity;

      this.FloatEnumerable =
          new[] {this.ObservationValue.x, this.ObservationValue.y, this.ObservationValue.z};
    }

    public Vector3 ObservationValue {
      get { return this._angular_velocity; }
      set { this._angular_velocity = value; }
    }

    public Space3 TripleSpace { get { return this._angular_velocity_space; } }
  }
}
