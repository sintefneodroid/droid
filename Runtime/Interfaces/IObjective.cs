namespace Neodroid.Runtime.Interfaces {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public interface IObjective : IRegisterable {
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    float Evaluate();
    /// <summary>
    /// 
    /// </summary>
    float SolvedThreshold { get; set; }
    /// <summary>
    /// 
    /// </summary>
    void EnvironmentReset();
  }
}
