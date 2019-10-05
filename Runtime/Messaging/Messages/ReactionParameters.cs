namespace droid.Runtime.Messaging.Messages {
  /// <summary>
  /// </summary>
  public enum ExecutionPhase {
    /// <summary>
    /// </summary>
    Before_,

    /// <summary>
    /// </summary>
    Middle_,

    /// <summary>
    /// </summary>
    After_
  }

  public enum StepResetObserve {
    /// <summary>
    /// </summary>
    Step_,

    /// <summary>
    /// </summary>
    Reset_,

    /// <summary>
    /// </summary>
    Observe_
  }

  /// <summary>
  /// </summary>
  public class ReactionParameters {
    public ReactionParameters(StepResetObserve sro = StepResetObserve.Observe_,
                              bool terminable = false,
                              bool configure = false,
                              bool describe = false,
                              bool episode_count = true) {
      this.Terminable = terminable;
      this.StepResetObserveEnu = sro;
      this.Configure = configure;
      this.Describe = describe;
      this.EpisodeCount = episode_count;
    }

    /// <summary>
    /// </summary>
    public bool EpisodeCount { get; }

    /// <summary>
    /// </summary>
    public ExecutionPhase Phase { get; set; } = ExecutionPhase.Middle_;

    /// <summary>
    /// </summary>
    public bool Terminable { get; }

    /// <summary>
    /// </summary>
    public bool Describe { get; }

    public StepResetObserve StepResetObserveEnu { get; }

    /// <summary>
    /// </summary>
    public bool Configure { get; }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      return "<ReactionParameters>\n "
             + $"Terminable:{this.Terminable},\n"
             + $"StepResetObserveEnu:{this.StepResetObserveEnu},\n"
             + $"Configure:{this.Configure},\n"
             + $"Describe:{this.Describe}\n"
             + $"EpisodeCount:{this.EpisodeCount}"
             + "\n</ReactionParameters>\n";
    }
  }
}
