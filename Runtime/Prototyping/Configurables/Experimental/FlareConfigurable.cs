using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Structs.Space;
using droid.Runtime.Structs.Space.Sample;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables.Experimental {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "Flare"
                    + ConfigurableComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Light))]
  public class FlareConfigurable : Configurable {
    string _color_r;
    string _color_g;
    string _color_b;
    string _shadow_strength;
    string _intensity;
    string _indirect_multiplier;

    Light _light;
    Flare _flare;

    [SerializeField]
    SampleSpace3 _color_space = new SampleSpace3 {
                                                     _space = new Space3 {
                                                                             DecimalGranularity = 2,
                                                                             Min = Vector3.one * 0.7f,
                                                                             Max = Vector3.one * 1f
                                                                         }
                                                 };

    [SerializeField]
    SampleSpace3 _int_ind_sha_space = new SampleSpace3 {_space = Space3.TwentyEighty + Vector3.one * 0.4f};

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._shadow_strength = this.Identifier + "ShadowStrength";
      this._color_r = this.Identifier + "ColorR";
      this._color_g = this.Identifier + "ColorG";
      this._color_b = this.Identifier + "ColorB";
      this._intensity = this.Identifier + "Intensity";
      this._indirect_multiplier = this.Identifier + "IndirectMultiplier";

      this._light = this.GetComponent<Light>();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          (Configurable)this,
                                                          identifier : this._shadow_strength);
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
                                                          identifier : this._indirect_multiplier);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(this, identifier : this._shadow_strength);
      this.ParentEnvironment.UnRegister(this, identifier : this._color_r);
      this.ParentEnvironment.UnRegister(this, identifier : this._color_g);
      this.ParentEnvironment.UnRegister(this, identifier : this._color_b);
      this.ParentEnvironment.UnRegister(this, identifier : this._intensity);
      this.ParentEnvironment.UnRegister(this, identifier : this._indirect_multiplier);
    }

    public ISamplable ConfigurableValueSpace { get { return this._color_space; } }

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

      if (configuration.ConfigurableName == this._shadow_strength) {
        this._light.shadowStrength = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._color_r) {
        var c = this._light.color;
        c.r = configuration.ConfigurableValue;
        this._light.color = c;
      } else if (configuration.ConfigurableName == this._color_g) {
        var c = this._light.color;
        c.g = configuration.ConfigurableValue;
        this._light.color = c;
      } else if (configuration.ConfigurableName == this._color_b) {
        var c = this._light.color;
        c.b = configuration.ConfigurableValue;
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
    /// <returns></returns>
    public override Configuration[] SampleConfigurations() {
      var o = this._int_ind_sha_space.Sample();
      var v = this._color_space.Sample();

      return new[] {
                       new Configuration(configurable_name : this._color_r, configurable_value : v.x),
                       new Configuration(configurable_name : this._color_g, configurable_value : v.y),
                       new Configuration(configurable_name : this._color_b, configurable_value : v.z),
                       new Configuration(configurable_name : this._intensity, configurable_value : o.x),
                       new Configuration(configurable_name : this._indirect_multiplier, configurable_value : o.y),
                       new Configuration(configurable_name : this._shadow_strength, configurable_value : o.z)
                   };
    }
  }
}
