using Neodroid.Runtime.Messaging.Messages;

namespace Neodroid.Runtime.Interfaces {
  public interface IEnvironment : IRegisterable {
    Reaction LastReaction { get; }
    int CurrentFrameNumber { get; }
    bool Terminated { get; }
    string LastTerminationReason { get; }
    int EpisodeLength { get; }
    bool IsResetting { get; }

    EnvironmentState CollectState();
    void React(Reaction reaction);
    EnvironmentState ReactAndCollectState(Reaction reaction);
    Reaction SampleReaction();
    void Tick();
    void PostStep();
  }
}