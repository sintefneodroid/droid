using droid.Runtime.Messaging.Messages;

namespace droid.Runtime.Interfaces {
  public interface IEnvironment : IRegisterable {
    Reaction LastReaction { get; }
    int CurrentFrameNumber { get; }
    bool Terminated { get; }
    string LastTerminationReason { get; }
    bool IsResetting { get; }

    EnvironmentState CollectState();
    void React(Reaction reaction);
    EnvironmentState ReactAndCollectState(Reaction reaction);
    Reaction SampleReaction();
    void Tick();
    void PostStep();
  }
}
