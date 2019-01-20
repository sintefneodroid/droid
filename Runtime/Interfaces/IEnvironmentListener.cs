﻿namespace droid.Runtime.Interfaces {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public interface IEnvironmentListener : IResetable {
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
