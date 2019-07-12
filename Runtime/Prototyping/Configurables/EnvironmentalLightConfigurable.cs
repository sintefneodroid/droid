using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Structs.Space;
using droid.Runtime.Utilities;
using droid.Runtime.Utilities.Misc;
using droid.Runtime.Utilities.Sampling;
using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "EnvironmentalLight"
                    + ConfigurableComponentMenuPath._Postfix)]
  [DisallowMultipleComponent]
  public class EnvironmentalLightConfigurable : Configurable {
    string _color_r;
    string _color_g;
    string _color_b;
    string _intensity;
    string _reflection_intensity;

    [SerializeField]
    Space2 _intensity_space = new Space2 {
                                             DecimalGranularity = 2,
                                             MinValues = Vector3.one * 0.0f,
                                             MaxValues = Vector3.one * 1f,
                                             DistributionSampler =
                                                 new DistributionSampler(DistributionEnum.Linear_) {
                                                                                                       _factor
                                                                                                           = -1
                                                                                                   }
                                         };

    [SerializeField]
    Space3 _color_space = new Space3 {MinValues = Vector3.one * 0.6f, MaxValues = Vector3.one * 1f};

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this._color_r = this.Identifier + "ColorR";
      this._color_g = this.Identifier + "ColorG";
      this._color_b = this.Identifier + "ColorB";
      this._intensity = this.Identifier + "Intensity";
      this._reflection_intensity = this.Identifier + "ReflectionIntensity";
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent(this.ParentEnvironment, (Configurable)this, this._color_r);
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent(this.ParentEnvironment, (Configurable)this, this._color_b);
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent(this.ParentEnvironment, (Configurable)this, this._color_g);
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent(this.ParentEnvironment, (Configurable)this, this._intensity);
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent(this.ParentEnvironment,
                                              (Configurable)this,
                                              this._reflection_intensity);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(this, this._color_r);
      this.ParentEnvironment.UnRegister(this, this._color_g);
      this.ParentEnvironment.UnRegister(this, this._color_b);
      this.ParentEnvironment.UnRegister(this, this._intensity);
      this.ParentEnvironment.UnRegister(this, this._reflection_intensity);
    }

    public override ISpace ConfigurableValueSpace { get; }

    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration configuration) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        DebugPrinting.ApplyPrint(this.Debugging, configuration, this.Identifier);
      }
      #endif
      var c = RenderSettings.ambientLight;
      if (configuration.ConfigurableName == this._color_r) {
        c.r = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._color_g) {
        c.g = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._color_b) {
        c.b = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._intensity) {
        //c.a = configuration.ConfigurableValue;
        RenderSettings.ambientIntensity = configuration.ConfigurableValue;
        RenderSettings.reflectionIntensity = Mathf.Clamp01(configuration.ConfigurableValue);
        //RenderSettings.skybox.SetFloat("_Exposure", configuration.ConfigurableValue);
      } else if (configuration.ConfigurableName == this._reflection_intensity) {
        //c.a = configuration.ConfigurableValue;
//        RenderSettings.reflectionIntensity = configuration.ConfigurableValue;
        //RenderSettings.skybox.SetFloat("_Exposure", configuration.ConfigurableValue);
      }

      RenderSettings.ambientLight = c;
      DynamicGI.UpdateEnvironment();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override Configuration[] SampleConfigurations() {
      var o = this._intensity_space.Sample();
      var v = this._color_space.Sample();

      return new[] {
                       new Configuration(this._color_r, v.x),
                       new Configuration(this._color_g, v.y),
                       new Configuration(this._color_b, v.z),
                       new Configuration(this._intensity, o.x),
                       new Configuration(this._reflection_intensity, o.y)
                   };
    }
  }
}
