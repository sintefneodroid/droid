namespace droid.Runtime.Interfaces {
  /// <summary>
  ///
  /// </summary>
  public interface IMotion {
    /// <summary>
    ///
    /// </summary>
    string ActuatorName { get; }

    /// <summary>
    ///
    /// </summary>
    string ActorName { get; }

    /// <summary>
    ///
    /// </summary>
    float Strength { get; set; }
  }
}
