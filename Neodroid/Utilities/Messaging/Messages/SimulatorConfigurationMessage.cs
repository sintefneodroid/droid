using Neodroid.Utilities.Messaging.FBS;
using Neodroid.Utilities.ScriptableObjects;

namespace Neodroid.Utilities.Messaging.Messages {
  /// <summary>
  ///
  /// </summary>
  public partial class SimulatorConfigurationMessage {
    int _frame_skips;
    bool _full_screen;
    int _height;
    int _width;
    int _frame_finishes;
    int _num_of_environments;
    int _reset_iterations;

    /// <summary>
    ///
    /// </summary>
    public int FrameSkips { get { return this._frame_skips; } set { this._frame_skips = value; } }

    /// <summary>
    /// 
    /// </summary>
    public float TimeScale { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool FullScreen { get { return this._full_screen; } set { this._full_screen = value; } }

    /// <summary>
    /// 
    /// </summary>
    public int Height { get { return this._height; } set { this._height = value; } }

    /// <summary>
    /// 
    /// </summary>
    public int Width { get { return this._width; } set { this._width = value; } }

    /// <summary>
    /// 
    /// </summary>
    public int Finishes { get { return this._frame_finishes; } set { this._frame_finishes = value; } }

    /// <summary>
    /// 
    /// </summary>
    public int NumOfEnvironments {
      get { return this._num_of_environments; }
      set { this._num_of_environments = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    public int ResetIterations {
      get { return this._reset_iterations; }
      set { this._reset_iterations = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    public int QualityLevel { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public float TargetFrameRate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int SimulationType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool DoSerialiseUnobservables { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool DoSerialiseIndidualObservables { get; set; }
  }

  /// <summary>
  ///
  /// </summary>
  public partial class SimulatorConfigurationMessage {
    /// <summary>
    ///
    /// </summary>
    /// <param name="simulator_configuration"></param>
    public SimulatorConfigurationMessage(SimulatorConfiguration simulator_configuration) {
      this._frame_skips = simulator_configuration.FrameSkips;
      this._full_screen = simulator_configuration.FullScreen;
      this._height = simulator_configuration.Height;
      this._width = simulator_configuration.Width;
      this._frame_finishes = (int)simulator_configuration.FrameFinishes;
      this._num_of_environments = simulator_configuration.NumOfEnvironments;
      this.TimeScale = simulator_configuration.TimeScale;
      this._reset_iterations = simulator_configuration.ResetIterations;
      this.QualityLevel = simulator_configuration.QualityLevel;
      this.TargetFrameRate = simulator_configuration.TargetFrameRate;
      this.SimulationType = (int)simulator_configuration.SimulationType;
      this.Finishes = (int)simulator_configuration.FrameFinishes;
      this.DoSerialiseIndidualObservables = simulator_configuration.AlwaysSerialiseIndidualObservables;
      this.DoSerialiseUnobservables = simulator_configuration.AlwaysSerialiseUnobservables;
      //TODO: CANT BE CHANGE while running
      //TODO: Exhaust list!
    }

    public SimulatorConfigurationMessage() { }

    /// <summary>
    ///
    /// </summary>
    /// <param name="flat_simulator_configuration"></param>
    public void FbsParse(FSimulatorConfiguration flat_simulator_configuration) {
      this._frame_skips = flat_simulator_configuration.FrameSkips;
      this._full_screen = flat_simulator_configuration.FullScreen;
      this._height = flat_simulator_configuration.Height;
      this._width = flat_simulator_configuration.Width;
      this._frame_finishes = (int)flat_simulator_configuration.WaitEvery;
      this._num_of_environments = flat_simulator_configuration.NumOfEnvironments;
      this.TimeScale = flat_simulator_configuration.TimeScale;
      this._reset_iterations = flat_simulator_configuration.ResetIterations;
      this.QualityLevel = flat_simulator_configuration.QualityLevel;
      this.TargetFrameRate = flat_simulator_configuration.TargetFrameRate;
      this.DoSerialiseIndidualObservables = flat_simulator_configuration.DoSerialiseIndidualObservables;
      this.DoSerialiseUnobservables = flat_simulator_configuration.DoSerialiseUnobservables;
      //TODO: Exhaust list!
    }
  }
}
