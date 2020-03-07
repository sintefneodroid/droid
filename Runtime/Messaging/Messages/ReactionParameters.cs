using System;

namespace droid.Runtime.Messaging.Messages {
  /// <summary>
  /// </summary>
  public enum ExecutionPhaseEnum {
    /// <summary>
    /// </summary>
    Before_tick_,

    /// <summary>
    /// </summary>
    On_tick_,

    /// <summary>
    /// </summary>
    After_tick_
  }

  /// <summary>
  ///
  /// </summary>
  public enum ReactionTypeEnum {
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
    public ReactionParameters(ReactionTypeEnum reaction_type,
                              bool terminable = false,
                              bool configure = false,
                              bool episode_count = false,
                              bool render = false,
                              bool describe = true) {
      this.Terminable = terminable;
      this.ReactionType = reaction_type;
      this.Configure = configure;
      this.Describe = describe;
      this.Render = render;
      this.EpisodeCount = episode_count;
    }

    public bool Render { get; }

    /// <summary>
    /// </summary>
    public bool EpisodeCount { get; }

    /// <summary>
    /// </summary>
    public ExecutionPhaseEnum PhaseEnum { get; set; } = ExecutionPhaseEnum.On_tick_;

    /// <summary>
    /// </summary>
    public bool Terminable { get; }

    /// <summary>
    /// </summary>
    public bool Describe { get; }

    /// <summary>
    ///
    /// </summary>
    public ReactionTypeEnum ReactionType { get; }

    /// <summary>
    /// </summary>
    public bool Configure { get; }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      return "<ReactionParameters>\n "
             + $"Terminable:{this.Terminable},\n"
             + $"StepResetObserveEnu:{this.ReactionType},\n"
             + $"Configure:{this.Configure},\n"
             + $"Describe:{this.Describe}\n"
             + $"Describe:{this.Render}\n"
             + $"EpisodeCount:{this.EpisodeCount}"
             + "\n</ReactionParameters>\n";
    }
  }
}
