using System;

namespace Neodroid.Prototyping.Observers.Rigidbody {
  public class AngularVelocityObserver : Observer {
    protected override void PreSetup() {
      base.PreSetup();
      throw new NotImplementedException();
    }

    public override void UpdateObservation() { throw new NotImplementedException(); }

    public override String PrototypingTypeName { get; }
  }
}
