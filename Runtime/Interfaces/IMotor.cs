using Neodroid.Runtime.Utilities.Structs;

namespace Neodroid.Runtime.Interfaces {
  public interface IMotor : IRegisterable {
    ValueSpace MotionValueSpace { get; set; }
    float Sample();
    float GetEnergySpend();
    void ApplyMotion(IMotorMotion motion);
    void EnvironmentReset();
  }
}
