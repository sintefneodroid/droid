using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Structs.Space;
using droid.Runtime.Structs.Space.Sample;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables.DomainRandomization {
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

    string _r_id;
    string _b_id;
    string _g_id;
    string _a_id;

    /// <summary>
    /// </summary>
    Renderer _renderer;

    [SerializeField] SampleSpace4 _space = new SampleSpace4 {_space = Space4.TwentyEighty};

    [SerializeField] bool use_shared = false;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._r_id = this.Identifier + _r;
      this._b_id = this.Identifier + _b;
      this._g_id = this.Identifier + _g;
      this._a_id = this.Identifier + _a;

      this._renderer = this.GetComponent<Renderer>();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          this,
                                                          this._r_id);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          this,
                                                          this._g_id);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          this,
                                                          this._b_id);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          this,
                                                          this._a_id);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(this, this._r_id);
      this.ParentEnvironment.UnRegister(this, this._b_id);
      this.ParentEnvironment.UnRegister(this, this._g_id);
      this.ParentEnvironment.UnRegister(this, this._a_id);
    }

    /// <summary>
    ///
    /// </summary>
    public override ISamplable ConfigurableValueSpace { get { return this._space; } }

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

    /// <summary>
    ///
    /// </summary>
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
                       new Configuration(this._r_id, v.x),
                       new Configuration(this._g_id, v.y),
                       new Configuration(this._b_id, v.z),
                       new Configuration(this._a_id, v.w)
                   };
    }
  }
}
