using System;
using Neodroid.Utilities.Messaging.Messages;
using Neodroid.Utilities.Unsorted;
using UnityEngine;
using Random = System.Random;

namespace Neodroid.Prototyping.Configurables {
  [AddComponentMenu(
       ConfigurableComponentMenuPath._ComponentMenuPath + "Color" + ConfigurableComponentMenuPath._Postfix),
   RequireComponent(typeof(Renderer))]
  public class ColorConfigurable : ConfigurableGameObject {
    /// <summary>
    /// Alpha
    /// </summary>
    string _a;

    /// <summary>
    /// Blue
    /// </summary>
    string _b;

    /// <summary>
    /// Green
    /// </summary>
    string _g;

    /// <summary>
    /// Red
    /// </summary>
    string _r;

    /// <summary>
    ///
    /// </summary>
    Renderer _renderer;

    /// <summary>
    ///
    /// </summary>
    public override string PrototypingTypeName { get { return "Color"; } }

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

    /// <summary>
    ///
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._r);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._g);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._b);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._a);
    }

    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment) {
        this.ParentEnvironment.UnRegisterConfigurable(this._r);
        this.ParentEnvironment.UnRegisterConfigurable(this._g);
        this.ParentEnvironment.UnRegisterConfigurable(this._b);
        this.ParentEnvironment.UnRegisterConfigurable(this._a);
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="configuration"></param>
    public override void ApplyConfiguration(Configuration configuration) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Applying " + configuration + " To " + this.Identifier);
      }
      #endif

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

    public override Configuration SampleConfiguration(Random random_generator) {
      throw new NotImplementedException();
    }
  }
}
