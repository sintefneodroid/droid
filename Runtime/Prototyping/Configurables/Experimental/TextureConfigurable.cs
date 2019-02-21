using droid.Runtime.Environments;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Utilities.Debugging;
using droid.Runtime.Utilities.Misc;
using UnityEngine;
using Random = System.Random;

namespace droid.Runtime.Prototyping.Configurables.Experimental {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      ConfigurableComponentMenuPath._ComponentMenuPath + "Texture" + ConfigurableComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Renderer))]
  public class TextureConfigurable : Configurable {
    /// <summary>
    ///   Red
    /// </summary>
    string _texture_str;

    [SerializeField] Texture[] _textures;

    [SerializeField] Texture _texture;
    Renderer _renderer;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this._texture_str = this.Identifier + "Texture";
      this._renderer = this.GetComponent<Renderer>();

      this._textures = Resources.LoadAll<Texture>("Textures");
    }

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
      this.ParentEnvironment?.UnRegister(this, this._texture_str);
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

      if (configuration.ConfigurableName == this._texture_str) {

          this._texture = this._textures[(int)configuration.ConfigurableValue];
          this._renderer.material.mainTexture = this._texture;

      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="random_generator"></param>
    /// <returns></returns>
    public override IConfigurableConfiguration SampleConfiguration(Random random_generator) {
      return new Configuration(this._texture_str, (float)random_generator.NextDouble()* this._textures.Length);
    }
  }
}
