using System;
using Neodroid.Utilities.Messaging.Messages;
using Neodroid.Utilities.Unsorted.SearchableEnum;
using UnityEngine;

namespace Neodroid.Utilities.ScriptableObjects {
  /// <summary>
  /// Determines the discrete timesteps of the simulation environment.
  /// </summary>
  public enum SimulationType {
    /// <summary>
    /// Waiting for frame instead means stable physics(Multiple fixed updates) and camera has updated their rendertextures. Pauses the game after every reaction until next reaction is received.
    /// </summary>
    Frame_dependent_, // TODO: Sometimes some frame seems to be dropped with the frame dependent configuration at high frame rates. 

    /// <summary>
    /// Camera observers should be manually rendered to ensure validity and freshness with camera.Render()
    /// </summary>
    Physics_dependent_,

    /// <summary>
    ///  Continue simulation
    /// </summary>
    Independent_
  }

  /// <summary>
  /// Determines where in the monobehaviour cycle a frame/step is finished
  /// </summary>
  public enum FrameFinishes {
    /// <summary>
    /// When ever all scripts has run their respective updates
    /// NOTE: Not working as expected, does not seem to work with physics engine.
    /// </summary>
    Late_update_,

    /// <summary>
    ///  NOTE: Not working as expected, does not seem to work with physics engine.
    /// </summary>
    End_of_frame_,

    /// <summary>
    /// When ever the scene has been rendered
    /// </summary>
    On_render_image_,

    /// <summary>
    /// When ever the scene has been rendered, default
    /// </summary>
    On_post_render_
  }

  /// <inheritdoc />
  /// <summary>
  /// Contains everything relevant to configuring simulation environments engine specific settings
  /// </summary>
  [CreateAssetMenu(
       fileName = "SimulatorConfiguration",
       menuName = "Neodroid/ScriptableObjects/SimulatorConfiguration",
       order = 1), Serializable]
  public class SimulatorConfiguration : ScriptableObject {
    /// <summary>
    ///
    /// </summary>
    [Header("Graphics"), SerializeField]
    bool _apply_resolution_settings;

    [SerializeField] bool _full_screen;

    /// <summary>
    ///
    /// </summary>
    [SerializeField, Range(0, 9999)]
    int _width = 500;

    /// <summary>
    ///
    /// </summary>
    [SerializeField, Range(0, 9999)]
    int _height = 500;

    /// <summary>
    ///
    /// </summary>
    [SerializeField, Range(1, 4)]
    int _quality_level = 1;

    /// <summary>
    ///
    /// </summary>
    [Header("Simulation"), SerializeField]
    FrameFinishes _frame_finishes = FrameFinishes.On_post_render_;

    /// <summary>
    /// Allow relative transforms to reset a couple of times.
    /// </summary>
    [SerializeField, Range(1, 99)]
    int _reset_iterations = 1;

    /// <summary>
    ///
    /// </summary>
    [SerializeField, Range(0, 99)]
    int _frame_skips;

    /// <summary>
    ///
    /// </summary>
    [SerializeField, SearchableEnum]
    SimulationType _simulation_type = SimulationType.Frame_dependent_;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    ExecutionPhase _step_execution_phase = ExecutionPhase.Middle_;

    /// <summary>
    /// Allows physics loop to be run more often than frame loop
    /// </summary>
    [Header("Time"), SerializeField, Range(0f, float.MaxValue)]
    float _time_scale = 1;

    /// <summary>
    /// Target frame rate = -1 means that no waiting/v-syncing is done and the simulation can run as fast as possible.
    /// </summary>
    [SerializeField, Range(-1, 9999)]
    int _target_frame_rate = -1;

    /// <summary>
    ///
    /// </summary>
    [Header("Connection"), SerializeField]
    string _ip_address = "localhost";

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    int _port = 6969;

    [Header("Performance (Disable for faster serialisation, but with loss of functionality)"), SerializeField]
    bool _always_serialise_unobservables;

    [SerializeField] bool _always_serialise_indidual_observables;

    /// <summary>
    /// WARNING When _update_fixed_time_scale is true, MAJOR slow downs due to PHYSX updates on change.
    /// </summary>
    [Header("Experimental (Warning, it is important to read docs before use!)"), SerializeField]
    bool _update_fixed_time_scale;

    /// <summary>
    ///
    /// </summary>
    [SerializeField, Range(0, 9999)]
    float _max_reply_interval;

    /// <summary>
    ///
    /// </summary>
    [SerializeField, Range(0, 999)]
    int _num_of_environments;

    [SerializeField] bool _replay_reaction_in_skips;

    /// <summary>
    ///
    /// </summary>
    public void SetToDefault() {
      this.Width = 500;
      this.Height = 500;
      this.FullScreen = false;
      this.QualityLevel = 1;
      this.TimeScale = 1;
      this.TargetFrameRate = -1;
      this.SimulationType = SimulationType.Frame_dependent_;
      this.FrameFinishes = FrameFinishes.Late_update_;
      this.FrameSkips = 0;
      this.ResetIterations = 1;
      this.MaxReplyInterval = 0;
      this.NumOfEnvironments = 1;
    }

    #region Getter Setters

    /// <summary>
    ///
    /// </summary>
    public int FrameSkips {
      get { return this._frame_skips; }
      set {
        if (value >= 0) {
          this._frame_skips = value;
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    public int ResetIterations {
      get { return this._reset_iterations; }
      set {
        if (value >= 1) {
          this._reset_iterations = value;
        }
      }
    }
    //When resetting transforms we run multiple times to ensure that we properly reset hierachies of objects

    /// <summary>
    ///
    /// </summary>
    public SimulationType SimulationType {
      get { return this._simulation_type; }
      set { this._simulation_type = value; }
    }

    public bool ApplyResolutionSettings {
      get { return this._apply_resolution_settings; }
      set { this._apply_resolution_settings = value; }
    }

    /// <summary>
    ///
    /// </summary>
    public int Width {
      get { return this._width; }
      set {
        if (value >= 0) {
          this._width = value;
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    public int Height {
      get { return this._height; }
      set {
        if (value >= 0) {
          this._height = value;
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    public bool FullScreen { get { return this._full_screen; } set { this._full_screen = value; } }

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
    public int TargetFrameRate {
      get { return this._target_frame_rate; }
      set {
        if (value >= -1) {
          this._target_frame_rate = value;
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    public int QualityLevel {
      get { return this._quality_level; }
      set {
        if (value >= 1 && value <= 4) {
          this._quality_level = value;
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    public float TimeScale {
      get { return this._time_scale; }
      set {
        if (value >= 0) {
          this._time_scale = value;
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    public float MaxReplyInterval {
      get { return this._max_reply_interval; }
      set { this._max_reply_interval = value; }
    }

    /// <summary>
    ///
    /// </summary>
    public FrameFinishes FrameFinishes {
      get { return this._frame_finishes; }
      set { this._frame_finishes = value; }
    }

    /// <summary>
    ///
    /// </summary>
    public ExecutionPhase StepExecutionPhase {
      get { return this._step_execution_phase; }
      set { this._step_execution_phase = value; }
    }

    /// <summary>
    /// WARNING When _update_fixed_time_scale is true, MAJOR slow downs due to PHYSX updates on change.
    /// </summary>
    ///
    public bool UpdateFixedTimeScale {
      get { return this._update_fixed_time_scale; }
      set { this._update_fixed_time_scale = value; }
    }

    public bool AlwaysSerialiseUnobservables {
      get { return this._always_serialise_unobservables; }
      set { this._always_serialise_unobservables = value; }
    }

    public bool AlwaysSerialiseIndidualObservables {
      get { return this._always_serialise_indidual_observables; }
      set { this._always_serialise_indidual_observables = value; }
    }

    public bool ReplayReactionInSkips {
      get { return this._replay_reaction_in_skips; }
      set { this._replay_reaction_in_skips = value; }
    }

    /// <summary>
    ///
    /// </summary>
    public Int32 Port { get { return this._port; } set { this._port = value; } }

    /// <summary>
    ///
    /// </summary>
    public String IpAddress { get { return this._ip_address; } set { this._ip_address = value; } }

    #endregion
  }
}
