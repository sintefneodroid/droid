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
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "StandardShaderMaterial"
                    + ConfigurableComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Renderer))]
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
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          (Configurable)this,
                                                          this._r);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          (Configurable)this,
                                                          this._g);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          (Configurable)this,
                                                          this._b);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          (Configurable)this,
                                                          this._a);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          (Configurable)this,
                                                          this._reflection);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          (Configurable)this,
                                                          this._smoothness);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          (Configurable)this,
                                                          this._offset_x);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          (Configurable)this,
                                                          this._offset_y);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          (Configurable)this,
                                                          this._tiling_x);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          (Configurable)this,
                                                          this._tiling_y);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(this, this._r);
      this.ParentEnvironment.UnRegister(this, this._g);
      this.ParentEnvironment.UnRegister(this, this._b);
      this.ParentEnvironment.UnRegister(this, this._a);
      this.ParentEnvironment.UnRegister(this, this._reflection);
      this.ParentEnvironment.UnRegister(this, this._smoothness);
      this.ParentEnvironment.UnRegister(this, this._offset_x);
      this.ParentEnvironment.UnRegister(this, this._offset_y);
      this.ParentEnvironment.UnRegister(this, this._tiling_x);
      this.ParentEnvironment.UnRegister(this, this._tiling_y);
    }

    /// <summary>
    ///
    /// </summary>
    public override ISamplable ConfigurableValueSpace { get { return this._tiling_space; } }

    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration configuration) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        DebugPrinting.ApplyPrint(this.Debugging, configuration, this.Identifier);
      }
      #endif

      if (!this._use_shared) {
        foreach (var mat in this._renderer.materials) {
          var c = mat.color;

          if (configuration.ConfigurableName.Equals(this._r, StringComparison.Ordinal)) {
            c.r = configuration.ConfigurableValue;
          } else if (string.Equals(configuration.ConfigurableName, this._g, StringComparison.Ordinal)) {
            c.g = configuration.ConfigurableValue;
          } else if (string.Equals(configuration.ConfigurableName, this._b, StringComparison.Ordinal)) {
            c.b = configuration.ConfigurableValue;
          } else if (string.Equals(configuration.ConfigurableName, this._a, StringComparison.Ordinal)) {
            c.a = configuration.ConfigurableValue;
          } else if (string.Equals(configuration.ConfigurableName,
                                   this._smoothness,
                                   StringComparison.Ordinal)) {
            mat.SetFloat(_glossiness, configuration.ConfigurableValue);
          } else if (string.Equals(configuration.ConfigurableName,
                                   this._reflection,
                                   StringComparison.Ordinal)) {
            mat.SetFloat(_glossy_reflections, configuration.ConfigurableValue);
          } else if (string.Equals(configuration.ConfigurableName,
                                   this._offset_x,
                                   StringComparison.Ordinal)) {
            var a = mat.GetTextureOffset(_main_tex);
            a.x = configuration.ConfigurableValue;
            mat.SetTextureOffset(_main_tex, a);
          } else if (string.Equals(configuration.ConfigurableName,
                                   this._offset_y,
                                   StringComparison.Ordinal)) {
            var a = mat.GetTextureOffset(_main_tex);
            a.y = configuration.ConfigurableValue;
            mat.SetTextureOffset(_main_tex, a);
          } else if (string.Equals(configuration.ConfigurableName,
                                   this._tiling_x,
                                   StringComparison.Ordinal)) {
            var a = mat.GetTextureScale(_main_tex);
            a.x = configuration.ConfigurableValue;
            mat.SetTextureScale(_main_tex, a);
          } else if (string.Equals(configuration.ConfigurableName,
                                   this._tiling_y,
                                   StringComparison.Ordinal)) {
            var a = mat.GetTextureScale(_main_tex);
            a.y = configuration.ConfigurableValue;
            mat.SetTextureScale(_main_tex, a);
          }

          mat.color = c;
        }
      } else {
        foreach (var mat in this._renderer.sharedMaterials) {
          var c = mat.color;

          if (string.Equals(configuration.ConfigurableName, this._r, StringComparison.Ordinal)) {
            c.r = configuration.ConfigurableValue;
          } else if (string.Equals(configuration.ConfigurableName, this._g, StringComparison.Ordinal)) {
            c.g = configuration.ConfigurableValue;
          } else if (string.Equals(configuration.ConfigurableName, this._b, StringComparison.Ordinal)) {
            c.b = configuration.ConfigurableValue;
          } else if (string.Equals(configuration.ConfigurableName, this._a, StringComparison.Ordinal)) {
            c.a = configuration.ConfigurableValue;
          } else if (string.Equals(configuration.ConfigurableName,
                                   this._smoothness,
                                   StringComparison.Ordinal)) {
            mat.SetFloat(_glossiness, configuration.ConfigurableValue);
          } else if (string.Equals(configuration.ConfigurableName,
                                   this._reflection,
                                   StringComparison.Ordinal)) {
            mat.SetFloat(_glossy_reflections, configuration.ConfigurableValue);
          } else if (string.Equals(configuration.ConfigurableName,
                                   this._offset_x,
                                   StringComparison.Ordinal)) {
            var a = mat.GetTextureOffset(_main_tex);
            a.x = configuration.ConfigurableValue;
            mat.SetTextureOffset(_main_tex, a);
          } else if (string.Equals(configuration.ConfigurableName,
                                   this._offset_y,
                                   StringComparison.Ordinal)) {
            var a = mat.GetTextureOffset(_main_tex);
            a.y = configuration.ConfigurableValue;
            mat.SetTextureOffset(_main_tex, a);
          } else if (string.Equals(configuration.ConfigurableName,
                                   this._tiling_x,
                                   StringComparison.Ordinal)) {
            var a = mat.GetTextureScale(_main_tex);
            a.x = configuration.ConfigurableValue;
            mat.SetTextureScale(_main_tex, a);
          } else if (string.Equals(configuration.ConfigurableName,
                                   this._tiling_y,
                                   StringComparison.Ordinal)) {
            var a = mat.GetTextureScale(_main_tex);
            a.y = configuration.ConfigurableValue;
            mat.SetTextureScale(_main_tex, a);
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
                       new Configuration(this._r, cs1.x),
                       new Configuration(this._g, cs1.y),
                       new Configuration(this._b, cs1.z),
                       new Configuration(this._a, cs1.w),
                       new Configuration(this._reflection, this._reflection_space.Sample()),
                       new Configuration(this._smoothness, this._smoothness_space.Sample()),
                       new Configuration(this._tiling_x, tl1.x),
                       new Configuration(this._tiling_y, tl1.y),
                       new Configuration(this._offset_x, os1.x),
                       new Configuration(this._offset_y, os1.y)
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
          mat.SetTextureScale(_main_tex, this._tiling_space.Sample());
          mat.SetTextureOffset(_main_tex, this._offset_space.Sample());
          mat.SetFloat(_glossiness, this._smoothness_space.Sample());
          mat.SetFloat(_glossy_reflections, this._reflection_space.Sample());
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
