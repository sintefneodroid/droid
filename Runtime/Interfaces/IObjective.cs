namespace droid.Runtime.Interfaces {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public interface IObjective : IRegisterable {
    /// <summary>
    /// </summary>
    float SolvedThreshold { get; set; }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    float Evaluate();

    /// <summary>
    /// </summary>
    void EnvironmentReset();

    int EpisodeLength { get; set; }
  }
}
