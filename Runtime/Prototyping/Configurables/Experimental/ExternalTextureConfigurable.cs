using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Structs.Space.Sample;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables.Experimental {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "ExternalTexture"
                    + ConfigurableComponentMenuPath._Postfix)]
  public class ExternalTextureConfigurable : Configurable {
    /// <summary>
    ///   Red
    /// </summary>
    string _texture_str;

    [SerializeField] Texture _texture = null;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() { this._texture_str = this.Identifier + "Texture"; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          (Configurable)this,
                                                          this._texture_str);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>n
    protected override void UnRegisterComponent() {
      this.ParentEnvironment?.UnRegister(this, this._texture_str);
    }

    public override ISamplable ConfigurableValueSpace { get { return new SampleSpace1(); } }

    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration configuration) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        DebugPrinting.ApplyPrint(this.Debugging, configuration, this.Identifier);
      }
      #endif

      if (configuration.ConfigurableName == this._texture_str) {
        if (this._texture) {
          this._texture.anisoLevel = (int)configuration.ConfigurableValue;
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override Configuration[] SampleConfigurations() {
      return new[] {new Configuration(this._texture_str, this.ConfigurableValueSpace.Sample())};
    }
  }
}
