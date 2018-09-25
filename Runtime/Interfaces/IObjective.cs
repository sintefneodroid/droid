namespace Neodroid.Runtime.Interfaces {
  public interface IObjective : IRegisterable {
    float Evaluate();
    float SolvedThreshold { get; set; }
    void EnvironmentReset();
  }
}