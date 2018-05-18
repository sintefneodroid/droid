using System;
using droid.Neodroid.Utilities.Messaging.FBS;
using droid.Neodroid.Utilities.ScriptableObjects;

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
    FrameFinishes _frame_finishes;
    int _num_of_environments;
    int _reset_iterations;

    /// <summary>
    ///
    /// </summary>
    public int FrameSkips { get { return this._frame_skips;} set { this._frame_skips = value; } }

    public float TimeScale { get; set; }

    public bool FullScreen { get { return this._full_screen; } set { this._full_screen = value; } }

    public int Height { get { return this._height; } set { this._height = value; } }

    public int Width { get { return this._width; } set { this._width = value; } }

    public FrameFinishes Finishes {
      get { return this._frame_finishes; }
      set { this._frame_finishes = value; }
    }

    public int NumOfEnvironments {
      get { return this._num_of_environments; }
      set { this._num_of_environments = value; }
    }

    public int ResetIterations {
      get { return this._reset_iterations; }
      set { this._reset_iterations = value; }
    }
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
      this._frame_finishes = (FrameFinishes)flat_simulator_configuration.WaitEvery;
      this._num_of_environments = flat_simulator_configuration.NumOfEnvironments;
      this.TimeScale = flat_simulator_configuration.TimeScale;
      this._reset_iterations = flat_simulator_configuration.ResetIterations;
//TODO: Exhaust list!
    }
  }
}