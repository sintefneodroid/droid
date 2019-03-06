using droid.Runtime.Environments;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Utilities.Debugging;
using droid.Runtime.Utilities.Misc;
using UnityEngine;
using Random = System.Random;

namespace droid.Runtime.Prototyping.Configurables.Experimental
{
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
    ConfigurableComponentMenuPath._ComponentMenuPath
    + "StandardShaderMaterial"
    + ConfigurableComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Renderer))]
  public class StandardShaderMaterialConfigurable : Configurable
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

    /// <summary>
    ///   Red
    /// </summary>
    string _r;

    string _g;
    string _b;
    string _a;

    /// <summary>
    /// </summary>
    Renderer _renderer;

    static readonly int _glossiness = Shader.PropertyToID("_Glossiness");
    static readonly int _glossy_reflections = Shader.PropertyToID("_GlossyReflections");

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

      this._renderer = this.GetComponent<Renderer>();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent()
    {
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
        (PrototypingEnvironment) this.ParentEnvironment,
        (Configurable) this,
        this._r);
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
        (PrototypingEnvironment) this.ParentEnvironment,
        (Configurable) this,
        this._g);
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
        (PrototypingEnvironment) this.ParentEnvironment,
        (Configurable) this,
        this._b);
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
        (PrototypingEnvironment) this.ParentEnvironment,
        (Configurable) this,
        this._a);
      /*this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
        (PrototypingEnvironment) this.ParentEnvironment,
        (Configurable) this,
        this._texture);*/
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
        (PrototypingEnvironment) this.ParentEnvironment,
        (Configurable) this,
        this._reflection);
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
        (PrototypingEnvironment) this.ParentEnvironment,
        (Configurable) this,
        this._smoothness);
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
    }

    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration configuration)
    {
#if NEODROID_DEBUG
      if (this.Debugging)
      {
        DebugPrinting.ApplyPrint(this.Debugging, configuration, this.Identifier);
      }
#endif

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
        }else if (configuration.ConfigurableName == this._smoothness)
        {
          mat.SetFloat(_glossiness,configuration.ConfigurableValue);
        }else if (configuration.ConfigurableName == this._reflection)
        {
          mat.SetFloat(_glossy_reflections,configuration.ConfigurableValue);
        }

        mat.color = c;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="random_generator"></param>
    /// <returns></returns>
    public override IConfigurableConfiguration SampleConfiguration(Random random_generator)
    {
      var sample = random_generator.Next(0, 5);

      switch (sample)
      {
        case 0:
          return new Configuration(this._r, (float) random_generator.NextDouble());
        case 1:
          return new Configuration(this._g, (float) random_generator.NextDouble());
        case 2:
          return new Configuration(this._b, (float) random_generator.NextDouble());
        case 3:
          return new Configuration(this._a, (float) random_generator.NextDouble());
        //case 4:
          //return new Configuration(this._texture, (float) random_generator.NextDouble());
        case 4:
          return new Configuration(this._reflection, (float) random_generator.NextDouble());
        case 5:
          return new Configuration(this._smoothness, (float) random_generator.NextDouble());
      }

      return new Configuration(this._r, (float) random_generator.NextDouble());
    }
  }
}