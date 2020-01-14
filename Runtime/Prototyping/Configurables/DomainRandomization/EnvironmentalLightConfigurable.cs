using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Sampling;
using droid.Runtime.Structs.Space;
using droid.Runtime.Structs.Space.Sample;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables.DomainRandomization {
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
    ISamplable _intensity_space = new SampleSpace2 {
                                                       Space =
                                                           new Space2 {
                                                                          Min = Vector3.one * 0.0f,
                                                                          Max = Vector3.one * 1f
                                                                      },
                                                       _distribution_sampler =
                                                           new DistributionSampler(distribution_enum : DistributionEnum.Linear_) {
                                                                                                                 DistributionParameter
                                                                                                                     = -1
                                                                                                             }
                                                   };

    [SerializeField]
    ISamplable _color_space = new SampleSpace3 {
                                                   _space = new Space3 {
                                                                           Min = Vector3.one * 0.6f,
                                                                           Max = Vector3.one * 1f
                                                                       }
                                               };

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
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
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          (Configurable)this,
                                                          identifier : this._color_r);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          (Configurable)this,
                                                          identifier : this._color_b);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          (Configurable)this,
                                                          identifier : this._color_g);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          (Configurable)this,
                                                          identifier : this._intensity);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          (Configurable)this,
                                                          identifier : this._reflection_intensity);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(this, identifier : this._color_r);
      this.ParentEnvironment.UnRegister(this, identifier : this._color_g);
      this.ParentEnvironment.UnRegister(this, identifier : this._color_b);
      this.ParentEnvironment.UnRegister(this, identifier : this._intensity);
      this.ParentEnvironment.UnRegister(this, identifier : this._reflection_intensity);
    }

    public ISamplable ConfigurableValueSpace { get; }

    public override void UpdateCurrentConfiguration() {  }

    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration configuration) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        DebugPrinting.ApplyPrint(debugging : this.Debugging, configuration : configuration, identifier : this.Identifier);
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
        RenderSettings.reflectionIntensity = Mathf.Clamp01(value : configuration.ConfigurableValue);
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
                       new Configuration(configurable_name : this._color_r, configurable_value : v.x),
                       new Configuration(configurable_name : this._color_g, configurable_value : v.y),
                       new Configuration(configurable_name : this._color_b, configurable_value : v.z),
                       new Configuration(configurable_name : this._intensity, configurable_value : o.x),
                       new Configuration(configurable_name : this._reflection_intensity, configurable_value : o.y)
                   };
    }
  }
}
