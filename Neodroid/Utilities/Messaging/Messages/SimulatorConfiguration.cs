using droid.Neodroid.Utilities.Messaging.FBS;

namespace droid.Neodroid.Utilities.Messaging.Messages
{
  /// <summary>
  ///
  /// </summary>
  public partial class SimulatorConfiguration {
    int _frame_skips;
    bool _full_screen;
    int _height;
    int _width;
    ScriptableObjects.FrameFinishes _frame_finishes;
    int _num_of_environments;
    int _reset_iterations;

    public SimulatorConfiguration() {}

    /// <summary>
    ///
    /// </summary>
    public int FrameSkips { get { return this._frame_skips;} set { this._frame_skips = value; } }


    public System.Single TimeScale { get; set; }
  }

  /// <summary>
  ///
  /// </summary>
  public partial class SimulatorConfiguration {
    /// <summary>
    ///
    /// </summary>
    /// <param name="flat_simulator_configuration"></param>
    public void FbsParse(FSimulatorConfiguration flat_simulator_configuration) {
      this._frame_skips = flat_simulator_configuration.FrameSkips;
      this._full_screen = flat_simulator_configuration.FullScreen;
      this._height = flat_simulator_configuration.Height;
      this._width = flat_simulator_configuration.Width;
      this._frame_finishes = (ScriptableObjects.FrameFinishes)flat_simulator_configuration.WaitEvery;
      this._num_of_environments = flat_simulator_configuration.NumOfEnvironments;
      this.TimeScale = flat_simulator_configuration.TimeScale;
      this._reset_iterations = flat_simulator_configuration.ResetIterations;
//TODO: Exhaust list!
    }
  }
}