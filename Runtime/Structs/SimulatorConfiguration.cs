using System;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.ScriptableObjects;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.Structs {
  /// <inheritdoc cref="ScriptableObject" />
  /// <summary>
  ///   Contains everything relevant to configuring simulation environments engine specific settings
  /// </summary>
  [CreateAssetMenu(fileName = "SimulatorConfiguration",
                   menuName = ScriptableObjectMenuPath._ScriptableObjectMenuPath + "SimulatorConfiguration",
                   order = 1)]
  [Serializable]
  public class SimulatorConfiguration : ScriptableObject,
                                        ISimulatorConfiguration {
    [Header("Performance (Disable for faster serialisation, but with loss of functionality)")]
    [SerializeField]
    bool _always_serialise_unobservables = true;

    [SerializeField] bool _always_serialise_individual_observables = true;

    [SerializeField] bool _always_serialise_aggregated_float_array = true;

    [Header("Graphics")] [SerializeField] bool _apply_resolution_settings = false;

    [SerializeField] bool _optimiseWindow_for_speed = false;

    [SerializeField] bool _apply_quality_settings = false;

    [SerializeField] [Range(1, 9999)] int _height = 500;

    [SerializeField] [Range(1, 9999)] int _width = 500;
    [SerializeField] [Range(1, 4)] int _quality_level = 1;
    [SerializeField] [Range(0, 4)] int _v_sync_count = 0;
    [SerializeField] bool _full_screen = false;
    [SerializeField] bool _resizable_window = true;
    [SerializeField] ColorSpace _unityColor_space = ColorSpace.Linear;

    /// <summary>
    /// </summary>
    [Header("Simulation")]
    [SerializeField]
    [SearchableEnum]
    FrameFinishesEnum _frame_finishes = FrameFinishesEnum.Late_update_;

    /// <summary>
    /// </summary>
    [SerializeField]
    [Range(0, 99)]
    int _frame_skips = 0;

    /// <summary>
    /// </summary>
    [Header("Connection")]
    [SerializeField]
    string _ip_address = "localhost";

    /// <summary>
    /// </summary>
    [SerializeField]
    [Range(0, 9999)]
    float _max_reply_interval = 0;

    /// <summary>
    /// </summary>
    [SerializeField]
    [Range(0, 999)]
    int _num_of_environments = 0;

    /// <summary>
    /// </summary>
    [SerializeField]
    int _port = 6969;

    [SerializeField] bool _replay_reaction_in_skips;

    /// <summary>
    /// </summary>
    [SerializeField]
    [SearchableEnum]
    SimulationTypeEnum _simulation_type = SimulationTypeEnum.Independent_;

    /// <summary>
    /// </summary>
    [SerializeField]
    //[SearchableEnum]
    ExecutionPhaseEnum _step_execution_phase = ExecutionPhaseEnum.On_tick_;

    /// <summary>
    ///   Target frame rate = -1 means that no waiting/v-syncing is done and the simulation can run as fast as
    ///   possible.
    /// </summary>
    [SerializeField]
    [Range(-1, 9999)]
    int _target_frame_rate = -1;

    /// <summary>
    ///   Allows physics loop to be run more often than frame loop
    /// </summary>
    [Header("Time")]
    [SerializeField]
    [Range(0f, 100f)]
    float _time_scale = 1;

    /// <summary>
    ///   WARNING When _update_fixed_time_scale is true, MAJOR slow downs due to PHYSX updates on change.
    /// </summary>
    [Header("Experimental (Warning, it is important to read docs before use!)")]
    [SerializeField]
    bool _update_fixed_time_scale = false;

    [SerializeField] Boolean _manual_render = false;

    /// <summary>
    /// </summary>
    public void SetToDefault() {
      this.Width = 500;
      this.Height = 500;
      this.FullScreen = false;
      this.QualityLevel = 1;
      this.TimeScale = 1;
      this.TargetFrameRate = -1;
      this.SimulationType = SimulationTypeEnum.Frame_dependent_;
      this.FrameFinishes = FrameFinishesEnum.Late_update_;
      this.FrameSkips = 0;
      this.MaxReplyInterval = 0;
      this.NumOfEnvironments = 1;
      this.ResizableWindow = true;
      this.UnityColorSpace = ColorSpace.Linear;
      this.VSyncCount = 0;
    }

    #region Getter Setters

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public int FrameSkips {
      get { return this._frame_skips; }
      set {
        if (value >= 0) {
          this._frame_skips = value;
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public SimulationTypeEnum SimulationType {
      get { return this._simulation_type; }
      set { this._simulation_type = value; }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public bool ApplyResolutionSettings {
      get { return this._apply_resolution_settings; }
      set { this._apply_resolution_settings = value; }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public bool ApplyQualitySettings {
      get { return this._apply_quality_settings; }
      set { this._apply_quality_settings = value; }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public bool ResizableWindow {
      get { return this._resizable_window; }
      set { this._resizable_window = value; }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public ColorSpace UnityColorSpace {
      get { return this._unityColor_space; }
      set { this._unityColor_space = value; }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public int VSyncCount { get { return this._v_sync_count = 0; } set { this._v_sync_count = value; } }

    public bool ManualRender { get { return this._manual_render; } set { this._manual_render = value; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public int Width {
      get { return this._width; }
      set {
        if (value >= 0) {
          this._width = value;
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public int Height {
      get { return this._height; }
      set {
        if (value >= 0) {
          this._height = value;
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public bool FullScreen { get { return this._full_screen; } set { this._full_screen = value; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public int NumOfEnvironments {
      get { return this._num_of_environments; }
      set { this._num_of_environments = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public int TargetFrameRate {
      get { return this._target_frame_rate; }
      set {
        if (value >= -1) {
          this._target_frame_rate = value;
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public int QualityLevel {
      get { return this._quality_level; }
      set {
        if (value >= 1 && value <= 4) {
          this._quality_level = value;
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
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
    /// </summary>
    public float MaxReplyInterval {
      get { return this._max_reply_interval; }
      set { this._max_reply_interval = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public FrameFinishesEnum FrameFinishes {
      get { return this._frame_finishes; }
      set { this._frame_finishes = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public ExecutionPhaseEnum StepExecutionPhase {
      get { return this._step_execution_phase; }
      set { this._step_execution_phase = value; }
    }

    /// <inheritdoc />
    /// <summary>
    ///   WARNING When _update_fixed_time_scale is true, MAJOR slow downs due to PHYSX updates on change.
    /// </summary>
    public bool UpdateFixedTimeScale {
      get { return this._update_fixed_time_scale; }
      set { this._update_fixed_time_scale = value; }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public bool SerialiseAggregatedFloatArray {
      get { return this._always_serialise_aggregated_float_array; }
      set { this._always_serialise_aggregated_float_array = value; }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public bool SerialiseUnobservables {
      get { return this._always_serialise_unobservables; }
      set { this._always_serialise_unobservables = value; }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public bool SerialiseIndividualObservables {
      get { return this._always_serialise_individual_observables; }
      set { this._always_serialise_individual_observables = value; }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public bool ReplayReactionInSkips {
      get { return this._replay_reaction_in_skips; }
      set { this._replay_reaction_in_skips = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Int32 Port { get { return this._port; } set { this._port = value; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public String IpAddress { get { return this._ip_address; } set { this._ip_address = value; } }

    /// <summary>
    ///
    /// </summary>
    public Boolean OptimiseWindowForSpeed {
      get { return this._optimiseWindow_for_speed; }
      set { this._optimiseWindow_for_speed = value; }
    }

    #endregion

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <returns></returns>
    public override String ToString() {
      var e = "";

      e += this.name;
      e += ", ";
      e += this.SimulationType.ToString();

      return e;
    }
  }
}
