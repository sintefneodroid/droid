using Neodroid.Managers;
using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Prototyping.Configurables {
  [AddComponentMenu(
      ConfigurableComponentMenuPath._ComponentMenuPath
      + "Simulation"
      + ConfigurableComponentMenuPath._Postfix)]
  [RequireComponent(typeof(PausableManager))]
  public class SimulationConfigurable : ConfigurableGameObject {
    string _fullscreen;
    string _height;

    string _quality_level;
    string _target_frame_rate;
    string _time_scale;
    string _width;

    protected override void Setup() {
      base.Setup();
      this._quality_level = this.Identifier + "QualityLevel";
      this._target_frame_rate = this.Identifier + "TargetFrameRate";
      this._time_scale = this.Identifier + "TimeScale";
      this._width = this.Identifier + "Width";
      this._height = this.Identifier + "Height";
      this._fullscreen = this.Identifier + "Fullscreen";
    }

    public override string PrototypingType { get { return "Simulation"; } }

    protected override void RegisterComponent() {
      this.ParentEnvironment = Utilities.Unsorted.NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._quality_level);
      this.ParentEnvironment = Utilities.Unsorted.NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._target_frame_rate);
      this.ParentEnvironment = Utilities.Unsorted.NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._width);
      this.ParentEnvironment = Utilities.Unsorted.NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._height);
      this.ParentEnvironment = Utilities.Unsorted.NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._fullscreen);
      this.ParentEnvironment = Utilities.Unsorted.NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._time_scale);
    }

    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment) {
        this.ParentEnvironment.UnRegisterConfigurable(this._quality_level);
        this.ParentEnvironment.UnRegisterConfigurable(this._target_frame_rate);
        this.ParentEnvironment.UnRegisterConfigurable(this._time_scale);
        this.ParentEnvironment.UnRegisterConfigurable(this._width);
        this.ParentEnvironment.UnRegisterConfigurable(this._height);
        this.ParentEnvironment.UnRegisterConfigurable(this._fullscreen);
      }
    }

    public override void ApplyConfiguration(Utilities.Messaging.Messages.Configuration configuration) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Applying " + configuration + " To " + this.Identifier);
      }
      #endif

      if (configuration.ConfigurableName == this._quality_level) {
        QualitySettings.SetQualityLevel((int)configuration.ConfigurableValue, true);
      } else if (configuration.ConfigurableName == this._target_frame_rate) {
        Application.targetFrameRate = (int)configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._width) {
        Screen.SetResolution((int)configuration.ConfigurableValue, Screen.height, false);
      } else if (configuration.ConfigurableName == this._height) {
        Screen.SetResolution(Screen.width, (int)configuration.ConfigurableValue, false);
      } else if (configuration.ConfigurableName == this._fullscreen) {
        Screen.SetResolution(Screen.width, Screen.height, (int)configuration.ConfigurableValue != 0);
      } else if (configuration.ConfigurableName == this._time_scale) {
        Time.timeScale = configuration.ConfigurableValue;
      }
    }

    public override Utilities.Messaging.Messages.Configuration SampleConfiguration(
        System.Random random_generator) {
      throw new System.NotImplementedException();
    }
  }
}
