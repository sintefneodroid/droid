using Neodroid.Runtime.Environments;
using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Messaging.Messages;
using Neodroid.Runtime.Utilities.Misc;
using UnityEngine;
using Random = System.Random;

namespace Neodroid.Runtime.Prototyping.Configurables.Experimental {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      ConfigurableComponentMenuPath._ComponentMenuPath + "Light" + ConfigurableComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Light))]
  public class LightConfigurable : Configurable {

    string _shadow_strength;
    string _color;
    string _intensity;
    string _indirect_multiplier;

    Light _light;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this._shadow_strength = this.Identifier + "ShadowStrength";
      this._color = this.Identifier + "Color";
      this._intensity = this.Identifier + "Intensity";
      this._indirect_multiplier = this.Identifier + "IndirectMultiplier";

      this._light = this.GetComponent<Light>();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._shadow_strength);
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._color);
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._intensity);
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._indirect_multiplier);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(this, this._shadow_strength);
      this.ParentEnvironment.UnRegister(this, this._color);
      this.ParentEnvironment.UnRegister(this, this._intensity);
      this.ParentEnvironment.UnRegister(this, this._indirect_multiplier);
    }

    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration configuration) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Applying " + configuration + " To " + this.Identifier);
      }
      #endif



      if (configuration.ConfigurableName == this._shadow_strength) {
        this._light.shadowStrength = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._color) {
        var c = this._light.color;
        c.g = configuration.ConfigurableValue;
        this._light.color = c;
      } else if (configuration.ConfigurableName == this._intensity) {
        this._light.intensity = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._indirect_multiplier) {
        this._light.bounceIntensity = configuration.ConfigurableValue;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="random_generator"></param>
    /// <returns></returns>
    public override IConfigurableConfiguration SampleConfiguration(Random random_generator) {
      return new Configuration(this._intensity, (float)random_generator.NextDouble());
    }


  }
}
