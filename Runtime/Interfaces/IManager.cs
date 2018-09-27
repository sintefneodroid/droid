namespace Neodroid.Runtime.Interfaces {
  public interface IManager : IHasRegister<IEnvironment> {
    ISimulatorConfiguration SimulatorConfiguration { get; }
    bool IsSyncingEnvironments { get; }
  }
}
