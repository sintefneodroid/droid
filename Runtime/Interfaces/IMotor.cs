using droid.Runtime.Utilities.Structs;

namespace droid.Runtime.Interfaces {
  public interface IMotor : IRegisterable {
    ValueSpace MotionValueSpace { get; set; }
    float Sample();
    float GetEnergySpend();
    void ApplyMotion(IMotorMotion motion);
    void EnvironmentReset();
  }
}
