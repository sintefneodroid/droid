using System;
using droid.Runtime.Environments;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Utilities.Debugging;
using droid.Runtime.Utilities.Misc;
using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables.Experimental {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "StandardShaderMaterial"
                    + ConfigurableComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Renderer))]
  public class StandardShaderMaterialConfigurable : Configurable,
                                                    IHasArray {
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
    static readonly Int32 _offset = Shader.PropertyToID("_MainTex");
    static readonly Int32 _tiling = Shader.PropertyToID("_MainTex");

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
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
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent((PrototypingEnvironment)this.ParentEnvironment,
                                              (Configurable)this,
                                              this._r);
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent((PrototypingEnvironment)this.ParentEnvironment,
                                              (Configurable)this,
                                              this._g);
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent((PrototypingEnvironment)this.ParentEnvironment,
                                              (Configurable)this,
                                              this._b);
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent((PrototypingEnvironment)this.ParentEnvironment,
                                              (Configurable)this,
                                              this._a);
      /*this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
        (PrototypingEnvironment) this.ParentEnvironment,
        (Configurable) this,
        this._texture);*/
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent((PrototypingEnvironment)this.ParentEnvironment,
                                              (Configurable)this,
                                              this._reflection);
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent((PrototypingEnvironment)this.ParentEnvironment,
                                              (Configurable)this,
                                              this._smoothness);
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent((PrototypingEnvironment)this.ParentEnvironment,
                                              (Configurable)this,
                                              this._offset_x);
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent((PrototypingEnvironment)this.ParentEnvironment,
                                              (Configurable)this,
                                              this._offset_y);
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent((PrototypingEnvironment)this.ParentEnvironment,
                                              (Configurable)this,
                                              this._tiling_x);
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent((PrototypingEnvironment)this.ParentEnvironment,
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
    public override void ApplyConfiguration(IConfigurableConfiguration configuration) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        DebugPrinting.ApplyPrint(this.Debugging, configuration, this.Identifier);
      }
      #endif

      if (!this._use_shared) {
        foreach (var mat in this._renderer.materials) {
          var c = mat.color;

          if (configuration.ConfigurableName == this._r) {
            c.r = configuration.ConfigurableValue;
          } else if (configuration.ConfigurableName == this._g) {
            c.g = configuration.ConfigurableValue;
          } else if (configuration.ConfigurableName == this._b) {
            c.b = configuration.ConfigurableValue;
          } else if (configuration.ConfigurableName == this._a) {
            c.a = configuration.ConfigurableValue;
          } else if (configuration.ConfigurableName == this._smoothness) {
            mat.SetFloat(_glossiness, configuration.ConfigurableValue);
          } else if (configuration.ConfigurableName == this._reflection) {
            mat.SetFloat(_glossy_reflections, configuration.ConfigurableValue);
          } else if (configuration.ConfigurableName == this._offset_x) {
            var a = mat.GetTextureOffset(_offset);
            a.x = configuration.ConfigurableValue;
            mat.SetTextureOffset(_offset, a);
          } else if (configuration.ConfigurableName == this._offset_y) {
            var a = mat.GetTextureOffset(_offset);
            a.y = configuration.ConfigurableValue;
            mat.SetTextureOffset(_offset, a);
          } else if (configuration.ConfigurableName == this._tiling_x) {
            var a = mat.GetTextureScale(_tiling);
            a.x = configuration.ConfigurableValue;
            mat.SetTextureScale(_tiling, a);
          } else if (configuration.ConfigurableName == this._tiling_y) {
            var a = mat.GetTextureScale(_tiling);
            a.y = configuration.ConfigurableValue;
            mat.SetTextureScale(_tiling, a);
          }

          mat.color = c;
        }
      } else {
        foreach (var mat in this._renderer.sharedMaterials) {
          var c = mat.color;

          if (configuration.ConfigurableName == this._r) {
            c.r = configuration.ConfigurableValue;
          } else if (configuration.ConfigurableName == this._g) {
            c.g = configuration.ConfigurableValue;
          } else if (configuration.ConfigurableName == this._b) {
            c.b = configuration.ConfigurableValue;
          } else if (configuration.ConfigurableName == this._a) {
            c.a = configuration.ConfigurableValue;
          } else if (configuration.ConfigurableName == this._smoothness) {
            mat.SetFloat(_glossiness, configuration.ConfigurableValue);
          } else if (configuration.ConfigurableName == this._reflection) {
            mat.SetFloat(_glossy_reflections, configuration.ConfigurableValue);
          } else if (configuration.ConfigurableName == this._offset_x) {
            var a = mat.GetTextureOffset(_offset);
            a.x = configuration.ConfigurableValue;
            mat.SetTextureOffset(_offset, a);
          } else if (configuration.ConfigurableName == this._offset_y) {
            var a = mat.GetTextureOffset(_offset);
            a.y = configuration.ConfigurableValue;
            mat.SetTextureOffset(_offset, a);
          } else if (configuration.ConfigurableName == this._tiling_x) {
            var a = mat.GetTextureScale(_tiling);
            a.x = configuration.ConfigurableValue;
            mat.SetTextureScale(_tiling, a);
          } else if (configuration.ConfigurableName == this._tiling_y) {
            var a = mat.GetTextureScale(_tiling);
            a.y = configuration.ConfigurableValue;
            mat.SetTextureScale(_tiling, a);
          }

          mat.color = c;
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override IConfigurableConfiguration SampleConfiguration() {
      var sample = (int)(UnityEngine.Random.Range(0, 9 + 1));

      switch (sample) {
        case 0:
          var cs1 = this._color_space.Sample();
          return new Configuration(this._r, cs1.x);
        case 1:
          var cs2 = this._color_space.Sample();
          return new Configuration(this._g, cs2.y);
        case 2:
          var cs3 = this._color_space.Sample();
          return new Configuration(this._b, cs3.z);
        case 3:
          var cs4 = this._color_space.Sample();
          return new Configuration(this._a, cs4.w);
        case 4:
          return new Configuration(this._reflection, this._reflection_space.Sample());
        case 5:
          return new Configuration(this._smoothness, this._smoothness_space.Sample());
        case 6:
          return new Configuration(this._tiling_x, this._tiling_space.Sample().x);
        case 7:
          return new Configuration(this._tiling_y, this._tiling_space.Sample().y);
        case 8:
          return new Configuration(this._offset_x, this._offset_space.Sample().x);
        case 9:
          return new Configuration(this._offset_y, this._offset_space.Sample().y);
        //case 6:
        //return new Configuration(this._texture, (float) Space1.ZeroOne.Sample());
        default:
          throw new IndexOutOfRangeException();
      }

      var cs6 = this._color_space.Sample();
      return new Configuration(this._r, cs6.x);
    }

    public Single[] ObservationArray { get; }
    public Space1[] ObservationSpace { get; }
  }
}
