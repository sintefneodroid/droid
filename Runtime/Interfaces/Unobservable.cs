namespace droid.Runtime.Interfaces {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public interface IUnobservable : IRegisterable {

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
