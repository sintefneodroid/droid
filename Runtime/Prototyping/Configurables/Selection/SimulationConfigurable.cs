using System;
using droid.Runtime.Interfaces;
using droid.Runtime.Managers;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables.Selection {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "Simulation"
                    + ConfigurableComponentMenuPath._Postfix)]
  [RequireComponent(typeof(NeodroidManager))]
  public class SimulationConfigurable : Configurable {
    string _fullscreen;
    string _height;

    string _quality_level;
    string _target_frame_rate;
    string _time_scale;
    string _width;

    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "SimulationConfigurable"; } }

    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._quality_level = this.Identifier + "QualityLevel";
      this._target_frame_rate = this.Identifier + "TargetFrameRate";
      this._time_scale = this.Identifier + "TimeScale";
      this._width = this.Identifier + "Width";
      this._height = this.Identifier + "Height";
      this._fullscreen = this.Identifier + "Fullscreen";
    }

    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          (Configurable)this,
                                                          this._quality_level);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          (Configurable)this,
                                                          this._target_frame_rate);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          (Configurable)this,
                                                          this._width);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          (Configurable)this,
                                                          this._height);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          (Configurable)this,
                                                          this._fullscreen);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          (Configurable)this,
                                                          this._time_scale);
    }

    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(this, this._quality_level);
      this.ParentEnvironment.UnRegister(this, this._target_frame_rate);
      this.ParentEnvironment.UnRegister(this, this._time_scale);
      this.ParentEnvironment.UnRegister(this, this._width);
      this.ParentEnvironment.UnRegister(this, this._height);
      this.ParentEnvironment.UnRegister(this, this._fullscreen);
    }

    public override ISamplable ConfigurableValueSpace { get; }

    /// <summary>
    ///
    /// </summary>
    /// <summary>
    /// </summary>
    /// <param name="simulator_configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration simulator_configuration) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Applying " + simulator_configuration + " To " + this.Identifier);
      }
      #endif

      if (simulator_configuration.ConfigurableName == this._quality_level) {
        QualitySettings.SetQualityLevel((int)simulator_configuration.ConfigurableValue, true);
      } else if (simulator_configuration.ConfigurableName == this._target_frame_rate) {
        Application.targetFrameRate = (int)simulator_configuration.ConfigurableValue;
      } else if (simulator_configuration.ConfigurableName == this._width) {
        Screen.SetResolution((int)simulator_configuration.ConfigurableValue, Screen.height, false);
      } else if (simulator_configuration.ConfigurableName == this._height) {
        Screen.SetResolution(Screen.width, (int)simulator_configuration.ConfigurableValue, false);
      } else if (simulator_configuration.ConfigurableName == this._fullscreen) {
        Screen.SetResolution(Screen.width,
                             Screen.height,
                             (int)simulator_configuration.ConfigurableValue != 0);
      } else if (simulator_configuration.ConfigurableName == this._time_scale) {
        Time.timeScale = simulator_configuration.ConfigurableValue;
      }
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public override Configuration[] SampleConfigurations() {
      return new[] {new Configuration(this._time_scale, this.ConfigurableValueSpace.Sample())};
    }
  }
}
