using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Utilities.Misc;
using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "Color"
                    + ConfigurableComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Renderer))]
  public class ColorConfigurable : Configurable {
    /// <summary>
    ///   Alpha
    /// </summary>
    const char _a = 'A';

    /// <summary>
    ///   Blue
    /// </summary>
    const char _b = 'B';

    /// <summary>
    ///   Green
    /// </summary>
    const char _g = 'G';

    /// <summary>
    ///   Red
    /// </summary>
    const char _r = 'R';

    string r_id;
    string b_id;
    string g_id;
    string a_id;

    /// <summary>
    /// </summary>
    Renderer _renderer;

    [SerializeField] Space4 _space = Space4.TwentyEighty;

    [SerializeField] bool use_shared = false;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this.r_id = this.Identifier + _r;
      this.b_id = this.Identifier + _b;
      this.g_id = this.Identifier + _g;
      this.a_id = this.Identifier + _a;

      this._renderer = this.GetComponent<Renderer>();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent(this.ParentEnvironment, (Configurable)this, this.r_id);
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent(this.ParentEnvironment, (Configurable)this, this.g_id);
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent(this.ParentEnvironment, (Configurable)this, this.b_id);
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent(this.ParentEnvironment, (Configurable)this, this.a_id);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(this, this.r_id);
      this.ParentEnvironment.UnRegister(this, this.b_id);
      this.ParentEnvironment.UnRegister(this, this.g_id);
      this.ParentEnvironment.UnRegister(this, this.a_id);
    }

    public override ISpace ConfigurableValueSpace { get { return this._space; } }

    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration configuration) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        DebugPrinting.ApplyPrint(this.Debugging, configuration, this.Identifier);
      }
      #endif

      if (this.use_shared) {
        foreach (var mat in this._renderer.sharedMaterials) {
          var c = mat.color;

          switch (configuration.ConfigurableName[configuration.ConfigurableName.Length - 1]) {
            case _r:
              c.r = configuration.ConfigurableValue;
              break;
            case _g:
              c.g = configuration.ConfigurableValue;
              break;
            case _b:
              c.b = configuration.ConfigurableValue;
              break;
            case _a:
              c.a = configuration.ConfigurableValue;
              break;
          }

          mat.color = c;
        }
      } else {
        foreach (var mat in this._renderer.materials) {
          var c = mat.color;

          switch (configuration.ConfigurableName[configuration.ConfigurableName.Length - 1]) {
            case _r:
              c.r = configuration.ConfigurableValue;
              break;
            case _g:
              c.g = configuration.ConfigurableValue;
              break;
            case _b:
              c.b = configuration.ConfigurableValue;
              break;
            case _a:
              c.a = configuration.ConfigurableValue;
              break;
          }

          mat.color = c;
        }
      }
    }

    protected override void Randomise() {
      if (this.use_shared) {
        foreach (var mat in this._renderer.sharedMaterials) {
          mat.color = this._space.Sample();
        }
      } else {
        foreach (var mat in this._renderer.materials) {
          mat.color = this._space.Sample();
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override Configuration[] SampleConfigurations() {
      var v = this._space.Sample();

      return new[] {
                       new Configuration(this.r_id, v.x),
                       new Configuration(this.g_id, v.y),
                       new Configuration(this.b_id, v.z),
                       new Configuration(this.a_id, v.w)
                   };
    }
  }
}
