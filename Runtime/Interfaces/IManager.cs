namespace Neodroid.Runtime.Interfaces {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public interface IManager : IHasRegister<IEnvironment> {
    /// <summary>
    /// </summary>
    ISimulatorConfiguration SimulatorConfiguration { get; }

    /// <summary>
    /// </summary>
    bool IsSyncingEnvironments { get; }
  }
}
