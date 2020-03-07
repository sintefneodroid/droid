using System;
using droid.Runtime.Enums;
using droid.Runtime.Messaging.Messages;
using UnityEngine;
using Object = System.Object;

namespace droid.Runtime.Interfaces {
  /// <summary>
  ///
  /// </summary>
  public interface ISimulatorConfiguration {
    /// <summary>
    ///
    /// </summary>
    bool SerialiseIndividualObservables { get; set; }

    /// <summary>
    ///
    /// </summary>
    bool SerialiseAggregatedFloatArray { get; set; }

    /// <summary>
    ///
    /// </summary>
    bool SerialiseUnobservables { get; set; }

    /// <summary>
    ///
    /// </summary>
    SimulationTypeEnum SimulationType { get; set; }

    /// <summary>
    ///
    /// </summary>
    int FrameSkips { get; set; }

    /// <summary>
    ///
    /// </summary>
    FrameFinishesEnum FrameFinishes { get; set; }

    /// <summary>
    ///
    /// </summary>
    int TargetFrameRate { get; set; }

    /// <summary>
    ///
    /// </summary>
    int QualityLevel { get; set; }

    /// <summary>
    ///
    /// </summary>
    float TimeScale { get; set; }

    /// <summary>
    ///
    /// </summary>
    int NumOfEnvironments { get; set; }

    /// <summary>
    ///
    /// </summary>
    int Width { get; set; }

    /// <summary>
    ///
    /// </summary>
    bool FullScreen { get; set; }

    /// <summary>
    ///
    /// </summary>
    int Height { get; set; }

    /// <summary>
    ///
    /// </summary>
    ExecutionPhaseEnum StepExecutionPhase { get; set; }

    /// <summary>
    ///
    /// </summary>
    bool UpdateFixedTimeScale { get; set; }

    /// <summary>
    ///
    /// </summary>
    string IpAddress { get; set; }

    /// <summary>
    ///
    /// </summary>
    int Port { get; set; }

    /// <summary>
    ///
    /// </summary>
    bool ReplayReactionInSkips { get; set; }

    /// <summary>
    ///
    /// </summary>
    bool ApplyResolutionSettings { get; set; }

    /// <summary>
    ///
    /// </summary>
    bool ApplyQualitySettings { get; set; }

    /// <summary>
    ///
    /// </summary>
    bool ResizableWindow { get; set; }

    /// <summary>
    ///
    /// </summary>
    ColorSpace UnityColorSpace { get; set; }

    /// <summary>
    ///
    /// </summary>
    int VSyncCount { get; set; }

    bool ManualRender { get; set; }
  }
}
