using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables.DomainRandomization {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "Texture"
                    + ConfigurableComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Renderer))]
  public class TextureConfigurable : Configurable {
    [SerializeField] Texture[] _textures = null;
    [SerializeField] bool load_from_resources_if_empty = true;
    [SerializeField] Texture _texture = null;
    [SerializeField] Renderer _renderer = null;
    [SerializeField] bool use_shared = false;
    Material _mat;
    [SerializeField] int _last_sample;
    [SerializeField] string load_path = "Textures";
    static readonly int _main_tex = Shader.PropertyToID("_MainTex");

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._renderer = this.GetComponent<Renderer>();
      if (Application.isPlaying) {
        if (this.use_shared) {
          this._mat = this._renderer?.sharedMaterial;
        } else {
          this._mat = this._renderer?.material;
        }
      }

      if (this.load_from_resources_if_empty) {
        if (this._textures == null || this._textures.Length == 0) {
          this._textures = Resources.LoadAll<Texture>(this.load_path);
        }
      }
    }

    public override ISamplable ConfigurableValueSpace { get; }

    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration configuration) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        DebugPrinting.ApplyPrint(this.Debugging, configuration, this.Identifier);
      }
      #endif

      this._texture = this._textures[(int)configuration.ConfigurableValue];

      this._mat.SetTexture(_main_tex, this._texture);
    }

    /// <summary>
    ///
    /// </summary>
    protected override void Randomise() {
      this._texture = this._textures[Random.Range(0, this._textures.Length)];

      this._mat.SetTexture(_main_tex, this._texture);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override Configuration[] SampleConfigurations() {
      this._last_sample = Random.Range(0, this._textures.Length);

      return new[] {new Configuration(this.Identifier, this._last_sample)};
    }
  }
}
