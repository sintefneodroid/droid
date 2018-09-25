using Neodroid.Runtime.Utilities.Structs;

namespace Neodroid.Runtime.Interfaces {
  public interface IMotor : IRegisterable {
    float Sample();
    float GetEnergySpend();
    void ApplyMotion(IMotorMotion motion);
    void EnvironmentReset();
    ValueSpace MotionValueSpace { get; set; }
  }
}