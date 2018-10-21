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
      ConfigurableComponentMenuPath._ComponentMenuPath + "StandardShaderMaterial" + ConfigurableComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Renderer))]
  public class StandardShaderMaterialConfigurable : Configurable {
    /// <summary>
    ///   Alpha
    /// </summary>
    string _texture;

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

    /// <summary>
    /// </summary>
    Renderer _renderer;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this._r = this.Identifier + "R";
      this._texture = this.Identifier + "G";
      this._reflection = this.Identifier + "B";
      this._smoothness = this.Identifier + "A";

      this._renderer = this.GetComponent<Renderer>();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._r);
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._texture);
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._reflection);
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._smoothness);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(this, this._r);
      this.ParentEnvironment.UnRegister(this, this._texture);
      this.ParentEnvironment.UnRegister(this, this._reflection);
      this.ParentEnvironment.UnRegister(this, this._smoothness);
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

      foreach (var mat in this._renderer.materials) {
        var c = mat.color;

        if (configuration.ConfigurableName == this._r) {
          c.r = configuration.ConfigurableValue;
        } else if (configuration.ConfigurableName == this._texture) {
          c.g = configuration.ConfigurableValue;
        } else if (configuration.ConfigurableName == this._reflection) {
          c.b = configuration.ConfigurableValue;
        } else if (configuration.ConfigurableName == this._smoothness) {
          c.a = configuration.ConfigurableValue;
        }

        mat.color = c;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="random_generator"></param>
    /// <returns></returns>
    public override IConfigurableConfiguration SampleConfiguration(Random random_generator) {
      var sample = random_generator.NextDouble();

      if (sample < .33f) {
        return new Configuration(this._r, (float)random_generator.NextDouble());
      }

      if (sample > .66f) {
        return new Configuration(this._reflection, (float)random_generator.NextDouble());
      }

      return new Configuration(this._smoothness, (float)random_generator.NextDouble());
    }
  }
}
