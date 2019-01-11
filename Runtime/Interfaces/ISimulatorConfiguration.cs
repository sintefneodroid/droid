using Neodroid.Runtime.Messaging.Messages;
using Neodroid.Runtime.Utilities.Enums;
using Neodroid.Runtime.Utilities.ScriptableObjects;

namespace Neodroid.Runtime.Interfaces {
  public interface ISimulatorConfiguration {
    int ResetIterations { get; set; }
    bool AlwaysSerialiseIndividualObservables { get; set; }
    bool AlwaysSerialiseUnobservables { get; set; }
    SimulationType SimulationType { get; set; }
    int FrameSkips { get; set; }
    FrameFinishes FrameFinishes { get; set; }
    int TargetFrameRate { get; set; }
    int QualityLevel { get; set; }
    float TimeScale { get; set; }
    int NumOfEnvironments { get; set; }
    int Width { get; set; }
    bool FullScreen { get; set; }
    int Height { get; set; }
    ExecutionPhase StepExecutionPhase { get; set; }
    bool UpdateFixedTimeScale { get; set; }
    string IpAddress { get; set; }
    int Port { get; set; }
    bool ReplayReactionInSkips { get; set; }
    bool ApplyResolutionSettings { get; set; }
  }
}