using droid.Runtime.Utilities.Structs;

namespace droid.Runtime.Interfaces {
  public interface IActuator : IRegisterable {
    Space1 MotionSpace { get; set; }
    float Sample();
    float GetEnergySpend();
    void ApplyMotion(IMotion motion);
    void EnvironmentReset();
  }
}
