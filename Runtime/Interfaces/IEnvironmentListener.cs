namespace droid.Runtime.Interfaces {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public interface IEnvironmentListener : IRegisterable {
    /// <summary>
    /// </summary>
    void EnvironmentReset();

    /// <summary>
    /// </summary>
    void PreStep();

    /// <summary>
    /// </summary>
    void Step();

    /// <summary>
    /// </summary>
    void PostStep();
  }
}
