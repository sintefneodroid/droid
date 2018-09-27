using System;
using Neodroid.Runtime.Environments;
using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Messaging.Messages;
using Neodroid.Runtime.Utilities.Misc.Drawing;
using Neodroid.Runtime.Utilities.Misc.Grasping;
using UnityEngine;
using NeodroidUtilities = Neodroid.Runtime.Utilities.Misc.NeodroidUtilities;
using Random = System.Random;

namespace Neodroid.Runtime.Prototyping.Configurables {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
       ConfigurableComponentMenuPath._ComponentMenuPath + "Color" + ConfigurableComponentMenuPath._Postfix),
   RequireComponent(typeof(Renderer))]
  public class ColorConfigurable : Configurable {
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

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override string PrototypingTypeName { get { return "ColorConfigurable"; } }

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
    ///  <summary>
    ///  </summary>
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

    /// <summary>
    /// 
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) return;
      this.ParentEnvironment.UnRegister(this, this._r);
      this.ParentEnvironment.UnRegister(this, this._g);
      this.ParentEnvironment.UnRegister(this, this._b);
      this.ParentEnvironment.UnRegister(this, this._a);
    }

    /// <summary>
    ///
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

    public override IConfigurableConfiguration SampleConfiguration(Random random_generator) {
      return new Configuration(this._r, (float)random_generator.NextDouble());
    }
  }
}
