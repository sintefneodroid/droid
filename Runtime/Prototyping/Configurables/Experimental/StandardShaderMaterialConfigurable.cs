using System;
using droid.Runtime.Environments;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Utilities.Misc;
using droid.Runtime.Utilities.Structs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace droid.Runtime.Prototyping.Configurables.Experimental
{
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "StandardShaderMaterial"
                    + ConfigurableComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Renderer))]
  public class StandardShaderMaterialConfigurable : Configurable,
    IHasArray
  {
    /// <summary>
    ///   Alpha
    /// </summary>
    //string _texture;

    /// <summary>
    ///   Blue
    /// </summary>
    string _reflection;

    /// <summary>
    ///   Green
    /// </summary>
    string _smoothness;

    string _tiling_x;
    string _tiling_y;
    string _offset_x;
    string _offset_y;

    /// <summary>
    ///   Red
    /// </summary>
    string _r;

    string _g;
    string _b;
    string _a;

    [SerializeField] Space2 _tiling_space = Space2.ZeroOne;
    [SerializeField] Space2 _offset_space = Space2.ZeroOne;

    [SerializeField] Space4 _color_space = Space4.ZeroOne;
    [SerializeField] Space1 _smoothness_space = Space1.ZeroOne;
    [SerializeField] Space1 _reflection_space = Space1.ZeroOne;

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
    protected override void PreSetup()
    {
      this._r = this.Identifier + "R";
      this._g = this.Identifier + "G";
      this._b = this.Identifier + "B";
      this._a = this.Identifier + "A";
      //this._texture = this.Identifier + "Texture";
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
    protected override void RegisterComponent()
    {
      this.ParentEnvironment =
        NeodroidUtilities.RegisterComponent(this.ParentEnvironment,
          (Configurable) this,
          this._r);
      this.ParentEnvironment =
        NeodroidUtilities.RegisterComponent(this.ParentEnvironment,
          (Configurable) this,
          this._g);
      this.ParentEnvironment =
        NeodroidUtilities.RegisterComponent(this.ParentEnvironment,
          (Configurable) this,
          this._b);
      this.ParentEnvironment =
        NeodroidUtilities.RegisterComponent(this.ParentEnvironment,
          (Configurable) this,
          this._a);
      /*this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
        (PrototypingEnvironment) this.ParentEnvironment,
        (Configurable) this,
        this._texture);*/
      this.ParentEnvironment =
        NeodroidUtilities.RegisterComponent(this.ParentEnvironment,
          (Configurable) this,
          this._reflection);
      this.ParentEnvironment =
        NeodroidUtilities.RegisterComponent(this.ParentEnvironment,
          (Configurable) this,
          this._smoothness);
      this.ParentEnvironment =
        NeodroidUtilities.RegisterComponent(this.ParentEnvironment,
          (Configurable) this,
          this._offset_x);
      this.ParentEnvironment =
        NeodroidUtilities.RegisterComponent(this.ParentEnvironment,
          (Configurable) this,
          this._offset_y);
      this.ParentEnvironment =
        NeodroidUtilities.RegisterComponent(this.ParentEnvironment,
          (Configurable) this,
          this._tiling_x);
      this.ParentEnvironment =
        NeodroidUtilities.RegisterComponent(this.ParentEnvironment,
          (Configurable) this,
          this._tiling_y);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent()
    {
      if (this.ParentEnvironment == null)
      {
        return;
      }

      this.ParentEnvironment.UnRegister(this, this._r);
      this.ParentEnvironment.UnRegister(this, this._g);
      this.ParentEnvironment.UnRegister(this, this._b);
      this.ParentEnvironment.UnRegister(this, this._a);
      //this.ParentEnvironment.UnRegister(this, this._texture);
      this.ParentEnvironment.UnRegister(this, this._reflection);
      this.ParentEnvironment.UnRegister(this, this._smoothness);
      this.ParentEnvironment.UnRegister(this, this._offset_x);
      this.ParentEnvironment.UnRegister(this, this._offset_y);
      this.ParentEnvironment.UnRegister(this, this._tiling_x);
      this.ParentEnvironment.UnRegister(this, this._tiling_y);
    }

    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration configuration)
    {
#if NEODROID_DEBUG
      if (this.Debugging) {
        DebugPrinting.ApplyPrint(this.Debugging, configuration, this.Identifier);
      }
#endif

      if (!this._use_shared)
      {
        foreach (var mat in this._renderer.materials)
        {
          var c = mat.color;

          if (configuration.ConfigurableName == this._r)
          {
            c.r = configuration.ConfigurableValue;
          }
          else if (configuration.ConfigurableName == this._g)
          {
            c.g = configuration.ConfigurableValue;
          }
          else if (configuration.ConfigurableName == this._b)
          {
            c.b = configuration.ConfigurableValue;
          }
          else if (configuration.ConfigurableName == this._a)
          {
            c.a = configuration.ConfigurableValue;
          }
          else if (configuration.ConfigurableName == this._smoothness)
          {
            mat.SetFloat(_glossiness, configuration.ConfigurableValue);
          }
          else if (configuration.ConfigurableName == this._reflection)
          {
            mat.SetFloat(_glossy_reflections, configuration.ConfigurableValue);
          }
          else if (configuration.ConfigurableName == this._offset_x)
          {
            var a = mat.GetTextureOffset(_main_tex);
            a.x = configuration.ConfigurableValue;
            mat.SetTextureOffset(_main_tex, a);
          }
          else if (configuration.ConfigurableName == this._offset_y)
          {
            var a = mat.GetTextureOffset(_main_tex);
            a.y = configuration.ConfigurableValue;
            mat.SetTextureOffset(_main_tex, a);
          }
          else if (configuration.ConfigurableName == this._tiling_x)
          {
            var a = mat.GetTextureScale(_main_tex);
            a.x = configuration.ConfigurableValue;
            mat.SetTextureScale(_main_tex, a);
          }
          else if (configuration.ConfigurableName == this._tiling_y)
          {
            var a = mat.GetTextureScale(_main_tex);
            a.y = configuration.ConfigurableValue;
            mat.SetTextureScale(_main_tex, a);
          }

          mat.color = c;
        }
      }
      else
      {
        foreach (var mat in this._renderer.sharedMaterials)
        {
          var c = mat.color;

          if (configuration.ConfigurableName == this._r)
          {
            c.r = configuration.ConfigurableValue;
          }
          else if (configuration.ConfigurableName == this._g)
          {
            c.g = configuration.ConfigurableValue;
          }
          else if (configuration.ConfigurableName == this._b)
          {
            c.b = configuration.ConfigurableValue;
          }
          else if (configuration.ConfigurableName == this._a)
          {
            c.a = configuration.ConfigurableValue;
          }
          else if (configuration.ConfigurableName == this._smoothness)
          {
            mat.SetFloat(_glossiness, configuration.ConfigurableValue);
          }
          else if (configuration.ConfigurableName == this._reflection)
          {
            mat.SetFloat(_glossy_reflections, configuration.ConfigurableValue);
          }
          else if (configuration.ConfigurableName == this._offset_x)
          {
            var a = mat.GetTextureOffset(_main_tex);
            a.x = configuration.ConfigurableValue;
            mat.SetTextureOffset(_main_tex, a);
          }
          else if (configuration.ConfigurableName == this._offset_y)
          {
            var a = mat.GetTextureOffset(_main_tex);
            a.y = configuration.ConfigurableValue;
            mat.SetTextureOffset(_main_tex, a);
          }
          else if (configuration.ConfigurableName == this._tiling_x)
          {
            var a = mat.GetTextureScale(_main_tex);
            a.x = configuration.ConfigurableValue;
            mat.SetTextureScale(_main_tex, a);
          }
          else if (configuration.ConfigurableName == this._tiling_y)
          {
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
    public override Configuration[] SampleConfigurations()
    {
      var cs1 = this._color_space.Sample();
      var tl1 = this._tiling_space.Sample();
      var os1 = this._offset_space.Sample();
      return new[]
      {
        new Configuration(this._r, cs1.x),
        new Configuration(this._g, cs1.y),
        new Configuration(this._b, cs1.z),
        new Configuration(this._a, cs1.w),
        new Configuration(this._reflection, this._reflection_space.Sample()),
        new Configuration(this._smoothness, this._smoothness_space.Sample()),
        new Configuration(this._tiling_x, tl1.x),
        new Configuration(this._tiling_y, tl1.y),
        new Configuration(this._offset_x, os1.x), new Configuration(this._offset_y, os1.y)
      };
    }

    /// <summary>
    ///
    /// </summary>
    public Single[] ObservationArray
    {
      get { return new float[] { }; }
    }

    /// <summary>
    ///
    /// </summary>
    public Space1[] ObservationSpace
    {
      get { return new[] {this._smoothness_space}; }
    }
  }
}