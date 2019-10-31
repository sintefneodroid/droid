using System;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Structs.Space.Sample;
using droid.Runtime.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace droid.Runtime.Prototyping.Configurables.Experimental {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "NormalMap"
                    + ConfigurableComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Renderer))]
  public class NormalMapConfigurable : Configurable {
    [SerializeField] Texture[] _textures = null;
    [SerializeField] bool load_from_resources_if_empty = true;
    [SerializeField] Texture _texture = null;
    [SerializeField] Renderer _renderer = null;
    [SerializeField] bool use_shared = false;
    [SerializeField] Material _mat;
    [SerializeField] int _last_sample;
    static readonly Int32 _main_tex = Shader.PropertyToID("_BumpMap");

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
          this._textures = Resources.LoadAll<Texture>("Textures");
        }
      }
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

      this._texture = this._textures[(int)configuration.ConfigurableValue];

      this._mat.SetTexture(_main_tex, this._texture);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override Configuration[] SampleConfigurations() {
      this._last_sample = int.Parse(Random.Range(0, this._textures.Length).ToString());

      return new[] {new Configuration(this.Identifier, this._last_sample)};
    }
  }
}
