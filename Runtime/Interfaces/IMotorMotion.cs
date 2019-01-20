namespace droid.Runtime.Interfaces {
  public interface IMotorMotion {
    string MotorName { get; }
    string ActorName { get; }
    float Strength { get; set; }
  }
}
