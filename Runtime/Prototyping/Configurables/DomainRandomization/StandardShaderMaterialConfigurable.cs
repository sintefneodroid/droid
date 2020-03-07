using System;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Structs.Space;
using droid.Runtime.Structs.Space.Sample;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables.DomainRandomization {
  /// <inheritdoc cref="Configurable" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                               + "StandardShaderMaterial"
                               + ConfigurableComponentMenuPath._Postfix)]
  [RequireComponent(requiredComponent : typeof(Renderer))]
  public class StandardShaderMaterialConfigurable : Configurable,
                                                    IHasTArray {
    string _reflection;
    string _smoothness;
    string _tiling_x;
    string _tiling_y;
    string _offset_x;
    string _offset_y;
    string _r;
    string _g;
    string _b;
    string _a;

    [SerializeField] SampleSpace2 _tiling_space = new SampleSpace2 {Space = Space2.TwentyEighty};
    [SerializeField] SampleSpace2 _offset_space = new SampleSpace2 {Space = Space2.TwentyEighty};

    [SerializeField] SampleSpace4 _color_space = new SampleSpace4 {Space = Space4.TwentyEighty};
    [SerializeField] SampleSpace1 _smoothness_space = new SampleSpace1 {Space = Space1.TwentyEighty};
    [SerializeField] SampleSpace1 _reflection_space = new SampleSpace1 {Space = Space1.TwentyEighty};

    /// <summary>
    /// </summary>
    Renderer _renderer;

    [SerializeField] bool _use_shared = false;

    static readonly int _glossiness = Shader.PropertyToID("_Glossiness");
    static readonly int _glossy_reflections = Shader.PropertyToID("_GlossyReflections");
    static readonly int _main_tex = Shader.PropertyToID("_MainTex");

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._r = this.Identifier + "R";
      this._g = this.Identifier + "G";
      this._b = this.Identifier + "B";
      this._a = this.Identifier + "A";
      this._reflection = this.Identifier + "Reflection";
      this._smoothness = this.Identifier + "Smoothness";
      this._tiling_x = this.Identifier + "TilingX";
      this._tiling_y = this.Identifier + "TilingY";
      this._offset_x = this.Identifier + "OffsetX";
      this._offset_y = this.Identifier + "OffsetY";

      this._renderer = this.GetComponent<Renderer>();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          c : (Configurable)this,
                                                          identifier : this._r);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          c : (Configurable)this,
                                                          identifier : this._g);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          c : (Configurable)this,
                                                          identifier : this._b);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          c : (Configurable)this,
                                                          identifier : this._a);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          c : (Configurable)this,
                                                          identifier : this._reflection);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          c : (Configurable)this,
                                                          identifier : this._smoothness);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          c : (Configurable)this,
                                                          identifier : this._offset_x);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          c : (Configurable)this,
                                                          identifier : this._offset_y);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          c : (Configurable)this,
                                                          identifier : this._tiling_x);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          c : (Configurable)this,
                                                          identifier : this._tiling_y);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(t : this, identifier : this._r);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._g);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._b);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._a);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._reflection);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._smoothness);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._offset_x);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._offset_y);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._tiling_x);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._tiling_y);
    }

    /// <summary>
    ///
    /// </summary>
    public ISamplable ConfigurableValueSpace { get { return this._tiling_space; } }

    public override void UpdateCurrentConfiguration() { }

    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration configuration) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        DebugPrinting.ApplyPrint(debugging : this.Debugging,
                                 configuration : configuration,
                                 identifier : this.Identifier);
      }
      #endif

      if (!this._use_shared) {
        for (var index = 0; index < this._renderer.materials.Length; index++) {
          var mat = this._renderer.materials[index];
          var c = mat.color;

          if (configuration.ConfigurableName.Equals(value : this._r,
                                                    comparisonType : StringComparison.Ordinal)) {
            c.r = configuration.ConfigurableValue;
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._g,
                                   comparisonType : StringComparison.Ordinal)) {
            c.g = configuration.ConfigurableValue;
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._b,
                                   comparisonType : StringComparison.Ordinal)) {
            c.b = configuration.ConfigurableValue;
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._a,
                                   comparisonType : StringComparison.Ordinal)) {
            c.a = configuration.ConfigurableValue;
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._smoothness,
                                   comparisonType : StringComparison.Ordinal)) {
            mat.SetFloat(nameID : _glossiness, value : configuration.ConfigurableValue);
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._reflection,
                                   comparisonType : StringComparison.Ordinal)) {
            mat.SetFloat(nameID : _glossy_reflections, value : configuration.ConfigurableValue);
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._offset_x,
                                   comparisonType : StringComparison.Ordinal)) {
            var a = mat.GetTextureOffset(nameID : _main_tex);
            a.x = configuration.ConfigurableValue;
            mat.SetTextureOffset(nameID : _main_tex, value : a);
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._offset_y,
                                   comparisonType : StringComparison.Ordinal)) {
            var a = mat.GetTextureOffset(nameID : _main_tex);
            a.y = configuration.ConfigurableValue;
            mat.SetTextureOffset(nameID : _main_tex, value : a);
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._tiling_x,
                                   comparisonType : StringComparison.Ordinal)) {
            var a = mat.GetTextureScale(nameID : _main_tex);
            a.x = configuration.ConfigurableValue;
            mat.SetTextureScale(nameID : _main_tex, value : a);
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._tiling_y,
                                   comparisonType : StringComparison.Ordinal)) {
            var a = mat.GetTextureScale(nameID : _main_tex);
            a.y = configuration.ConfigurableValue;
            mat.SetTextureScale(nameID : _main_tex, value : a);
          }

          mat.color = c;
        }
      } else {
        foreach (var mat in this._renderer.sharedMaterials) {
          var c = mat.color;

          if (string.Equals(a : configuration.ConfigurableName,
                            b : this._r,
                            comparisonType : StringComparison.Ordinal)) {
            c.r = configuration.ConfigurableValue;
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._g,
                                   comparisonType : StringComparison.Ordinal)) {
            c.g = configuration.ConfigurableValue;
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._b,
                                   comparisonType : StringComparison.Ordinal)) {
            c.b = configuration.ConfigurableValue;
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._a,
                                   comparisonType : StringComparison.Ordinal)) {
            c.a = configuration.ConfigurableValue;
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._smoothness,
                                   comparisonType : StringComparison.Ordinal)) {
            mat.SetFloat(nameID : _glossiness, value : configuration.ConfigurableValue);
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._reflection,
                                   comparisonType : StringComparison.Ordinal)) {
            mat.SetFloat(nameID : _glossy_reflections, value : configuration.ConfigurableValue);
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._offset_x,
                                   comparisonType : StringComparison.Ordinal)) {
            var a = mat.GetTextureOffset(nameID : _main_tex);
            a.x = configuration.ConfigurableValue;
            mat.SetTextureOffset(nameID : _main_tex, value : a);
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._offset_y,
                                   comparisonType : StringComparison.Ordinal)) {
            var a = mat.GetTextureOffset(nameID : _main_tex);
            a.y = configuration.ConfigurableValue;
            mat.SetTextureOffset(nameID : _main_tex, value : a);
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._tiling_x,
                                   comparisonType : StringComparison.Ordinal)) {
            var a = mat.GetTextureScale(nameID : _main_tex);
            a.x = configuration.ConfigurableValue;
            mat.SetTextureScale(nameID : _main_tex, value : a);
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._tiling_y,
                                   comparisonType : StringComparison.Ordinal)) {
            var a = mat.GetTextureScale(nameID : _main_tex);
            a.y = configuration.ConfigurableValue;
            mat.SetTextureScale(nameID : _main_tex, value : a);
          }

          mat.color = c;
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override Configuration[] SampleConfigurations() {
      var cs1 = this._color_space.Sample();
      var tl1 = this._tiling_space.Sample();
      var os1 = this._offset_space.Sample();
      return new[] {
                       new Configuration(configurable_name : this._r, configurable_value : cs1.x),
                       new Configuration(configurable_name : this._g, configurable_value : cs1.y),
                       new Configuration(configurable_name : this._b, configurable_value : cs1.z),
                       new Configuration(configurable_name : this._a, configurable_value : cs1.w),
                       new Configuration(configurable_name : this._reflection,
                                         configurable_value : this._reflection_space.Sample()),
                       new Configuration(configurable_name : this._smoothness,
                                         configurable_value : this._smoothness_space.Sample()),
                       new Configuration(configurable_name : this._tiling_x, configurable_value : tl1.x),
                       new Configuration(configurable_name : this._tiling_y, configurable_value : tl1.y),
                       new Configuration(configurable_name : this._offset_x, configurable_value : os1.x),
                       new Configuration(configurable_name : this._offset_y, configurable_value : os1.y)
                   };
    }

    /// <summary>
    ///
    /// </summary>
    protected override void Randomise() {
      Material[] materials;
      if (this._use_shared) {
        materials = this._renderer.sharedMaterials;
      } else {
        materials = this._renderer.materials;
      }

      foreach (var mat in materials) {
        if (mat) {
          mat.color = this._color_space.Sample();
          mat.SetTextureScale(nameID : _main_tex, value : this._tiling_space.Sample());
          mat.SetTextureOffset(nameID : _main_tex, value : this._offset_space.Sample());
          mat.SetFloat(nameID : _glossiness, value : this._smoothness_space.Sample());
          mat.SetFloat(nameID : _glossy_reflections, value : this._reflection_space.Sample());
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    public dynamic[] ObservationArray { get { return new dynamic[] { }; } }

    /// <summary>
    ///
    /// </summary>
    public ISpace[] ObservationSpace { get { return new ISpace[] {this._tiling_space.Space}; } }
  }
}
