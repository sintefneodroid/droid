using System;
using System.Collections.Generic;
using Neodroid.Runtime.Environments;
using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Messaging.Messages;
using Neodroid.Runtime.Utilities.Debugging;
using Neodroid.Runtime.Utilities.Misc;
using UnityEngine;
using Random = System.Random;

namespace Neodroid.Runtime.Prototyping.Configurables.Experimental {
  /// <inheritdoc cref="Configurable" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      ConfigurableComponentMenuPath._ComponentMenuPath + "Mesh" + ConfigurableComponentMenuPath._Postfix)]
  public class MeshConfigurable : Configurable, IHasFloatEnumerable {
    string _mesh_str;

    [SerializeField] Texture _texture;
    [SerializeField] float[] _observation_value;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() { this._mesh_str = this.Identifier + "Mesh"; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._mesh_str);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>n
    protected override void UnRegisterComponent() {
      this.ParentEnvironment?.UnRegister(this, this._mesh_str);
    }

    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration configuration) {
      #if NEODROID_DEBUG
        DebugPrinting.ApplyPrint(this.Debugging, configuration, this.Identifier);
      #endif

      if (configuration.ConfigurableName == this._mesh_str) {
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
      return new Configuration(this._mesh_str, (float)random_generator.NextDouble());
    }

    public IEnumerable<Single> FloatEnumerable { get { return this._observation_value; } }
  }
}
