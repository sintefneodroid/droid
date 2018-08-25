using System;
using Neodroid.Prototyping.Motors;
using Neodroid.Utilities.Interfaces;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  public class TargetRigidbodyObserver : Observer,
                                         IHasDouble {
    TargetRigidbodyMotor _motor;

    protected override void PreSetup() {
      base.PreSetup();
      this._motor = this.GetComponent<TargetRigidbodyMotor>();
    }

    public override void UpdateObservation() {
      this.FloatEnumerable = new[] {this.ObservationValue.x, this.ObservationValue.y};
    }

    public Vector2 ObservationValue {
      get { return new Vector2(this._motor.MovementSpeed, this._motor.RotationSpeed); }
    }

    public Space2 ObservationSpace2D { get; }


  }
}
