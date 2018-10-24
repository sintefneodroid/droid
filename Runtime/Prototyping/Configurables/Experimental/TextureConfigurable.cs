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
      ConfigurableComponentMenuPath._ComponentMenuPath + "Texture" + ConfigurableComponentMenuPath._Postfix)]
  public class TextureConfigurable : Configurable {
    /// <summary>
    ///   Red
    /// </summary>
    string _texture_str;

    [SerializeField] Texture _texture;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() { this._texture_str = this.Identifier + "Texture"; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._texture_str);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>n
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(this, this._texture_str);
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

      if (configuration.ConfigurableName == this._texture_str) {
        if (this._texture) {
          this._texture.anisoLevel = (int)configuration.ConfigurableValue;
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="random_generator"></param>
    /// <returns></returns>
    public override IConfigurableConfiguration SampleConfiguration(Random random_generator) {
      return new Configuration(this._texture_str, (float)random_generator.NextDouble());
    }
  }
}
