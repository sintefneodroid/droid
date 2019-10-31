namespace droid.Runtime.Interfaces {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public interface IEpisodicObjectiveFunction : IObjectiveFunction {

    /// <summary>
    /// The length of an episode
    /// </summary>
    int EpisodeLength { get; set; }

  }
}
