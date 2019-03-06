using droid.Runtime.Environments;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Prototyping.Observers.Transform;
using droid.Runtime.Utilities.Debugging;
using droid.Runtime.Utilities.Misc;
using droid.Runtime.Utilities.Structs;
using UnityEngine;
using Random = System.Random;

namespace droid.Runtime.Prototyping.Configurables {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      ConfigurableComponentMenuPath._ComponentMenuPath + "Color" + ConfigurableComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Renderer))]
  public class ColorConfigurable : Configurable {
    /// <summary>
    ///   Alpha
    /// </summary>
    string _a;

    /// <summary>
    ///   Blue
    /// </summary>
    string _b;

    /// <summary>
    ///   Green
    /// </summary>
    string _g;

    /// <summary>
    ///   Red
    /// </summary>
    string _r;

    /// <summary>
    /// </summary>
    Renderer _renderer;

    [SerializeField] ValueSpace _space = new ValueSpace(){_Min_Value = 0.3f, _Max_Value = 1f};

    [SerializeField] bool use_shared = false;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this._r = this.Identifier + "R";
      this._g = this.Identifier + "G";
      this._b = this.Identifier + "B";
      this._a = this.Identifier + "A";

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
          this._g);
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._b);
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._a);
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

      if(this.use_shared)
      {
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
          }

          mat.color = c;
        }
      }
      else
      {
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
          }

          mat.color = c;
        }
      }


    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="random_generator"></param>
    /// <returns></returns>
    public override IConfigurableConfiguration SampleConfiguration(Random random_generator) {
      var sample = random_generator.NextDouble();


      var v = this._space.Sample();

      if (sample < .33f) {
        return new Configuration(this._r, v);
      }

      if (sample > .66f) {
        return new Configuration(this._g, v);
      }

      return new Configuration(this._b, v);
    }
  }
}
