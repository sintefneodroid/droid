using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.FBS;

namespace droid.Runtime.Messaging.Messages {
  /// <summary>
  /// </summary>
  public partial class SimulatorConfigurationMessage {
    /// <summary>
    /// </summary>
    public int FrameSkips { get; set; }

    /// <summary>
    /// </summary>
    public float TimeScale { get; set; }

    /// <summary>
    /// </summary>
    public bool FullScreen { get; set; }

    /// <summary>
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// </summary>
    public int Finishes { get; set; }

    /// <summary>
    /// </summary>
    public int NumOfEnvironments { get; set; }

    /// <summary>
    /// </summary>
    public int ResetIterations { get; set; }

    /// <summary>
    /// </summary>
    public int QualityLevel { get; set; }

    /// <summary>
    /// </summary>
    public float TargetFrameRate { get; set; }

    /// <summary>
    /// </summary>
    public int SimulationType { get; set; }

    /// <summary>
    /// </summary>
    public bool DoSerialiseUnobservables { get; set; }

    /// <summary>
    /// </summary>
    public bool DoSerialiseIndividualSensors { get; set; }
  }

  /// <summary>
  /// </summary>
  public partial class SimulatorConfigurationMessage {
    /// <summary>
    /// </summary>
    /// <param name="simulator_configuration"></param>
    public SimulatorConfigurationMessage(ISimulatorConfiguration simulator_configuration) {
      this.FrameSkips = simulator_configuration.FrameSkips;
      this.FullScreen = simulator_configuration.FullScreen;
      this.Height = simulator_configuration.Height;
      this.Width = simulator_configuration.Width;
      this.Finishes = (int)simulator_configuration.FrameFinishes;
      this.NumOfEnvironments = simulator_configuration.NumOfEnvironments;
      this.TimeScale = simulator_configuration.TimeScale;
      this.QualityLevel = simulator_configuration.QualityLevel;
      this.TargetFrameRate = simulator_configuration.TargetFrameRate;
      this.SimulationType = (int)simulator_configuration.SimulationType;
      this.Finishes = (int)simulator_configuration.FrameFinishes;
      this.DoSerialiseIndividualSensors = simulator_configuration.AlwaysSerialiseIndividualObservables;
      this.DoSerialiseUnobservables = simulator_configuration.AlwaysSerialiseUnobservables;
      //TODO: CANNOT BE CHANGE while running
      //TODO: Exhaust list!
    }

    public SimulatorConfigurationMessage() { }

    /// <summary>
    /// </summary>
    /// <param name="flat_simulator_configuration"></param>
    public void FbsParse(FSimulatorConfiguration flat_simulator_configuration) {
      this.FrameSkips = flat_simulator_configuration.FrameSkips;
      this.FullScreen = flat_simulator_configuration.FullScreen;
      this.Height = flat_simulator_configuration.Height;
      this.Width = flat_simulator_configuration.Width;
      this.Finishes = (int)flat_simulator_configuration.SimulationType;
      this.NumOfEnvironments = flat_simulator_configuration.NumOfEnvironments;
      this.TimeScale = flat_simulator_configuration.TimeScale;
      this.ResetIterations = flat_simulator_configuration.ResetIterations;
      this.QualityLevel = flat_simulator_configuration.QualityLevel;
      this.TargetFrameRate = flat_simulator_configuration.TargetFrameRate;
      this.DoSerialiseIndividualSensors = flat_simulator_configuration.DoSerialiseIndividualSensors;
      this.DoSerialiseUnobservables = flat_simulator_configuration.DoSerialiseUnobservables;
      //TODO: Exhaust list!
    }
  }
}
