using droid.Runtime.Utilities.Structs;

namespace droid.Runtime.Interfaces {
  public interface IMotor : IRegisterable {
    Space1 MotionSpace1 { get; set; }
    float Sample();
    float GetEnergySpend();
    void ApplyMotion(IMotorMotion motion);
    void EnvironmentReset();
  }
}
