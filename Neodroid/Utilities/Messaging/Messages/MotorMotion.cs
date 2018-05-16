using System;

namespace Neodroid.Utilities.Messaging.Messages {
  [Serializable]
  public class MotorMotion {
    // Has a possible direction given by the sign of the float

    public MotorMotion(string actor_name, string motor_name, float strength) {
      this.ActorName = actor_name;
      this.MotorName = motor_name;
      this.Strength = strength;
    }

    public float Strength { get; }

    public string ActorName { get; }

    public string MotorName { get; }

    public override string ToString() {
      return "<MotorMotion> "
             + this.ActorName
             + ", "
             + this.MotorName
             + ", "
             + this.Strength
             + " </MotorMotion>";
    }
  }
}
