using droid.Runtime.Environments;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Utilities.Debugging;
using droid.Runtime.Utilities.Misc;
using JetBrains.Annotations;
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

    [SerializeField] Texture[] _textures=null;
    [SerializeField] bool load_from_resources_if_empty = true;
    [SerializeField] Texture _texture=null;
    [CanBeNull] Renderer _renderer=null;
    [SerializeField] bool use_shared=false;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this._texture_str = this.Identifier + "Texture";
      this._renderer = this.GetComponent<Renderer>();

      if(this.load_from_resources_if_empty) {
        if(this._textures == null || this._textures.Length == 0) {
          this._textures = Resources.LoadAll<Texture>("Textures");
        }
      }
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
          if(this.use_shared)
          {
            this._renderer.sharedMaterial.mainTexture = this._texture;
          }
          else
          {
            this._renderer.material.mainTexture = this._texture;
          }


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
