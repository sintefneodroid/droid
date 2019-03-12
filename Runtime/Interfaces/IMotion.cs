namespace droid.Runtime.Interfaces {
  public interface IMotion {
    string ActuatorName { get; }
    string ActorName { get; }
    float Strength { get; set; }
  }
}
