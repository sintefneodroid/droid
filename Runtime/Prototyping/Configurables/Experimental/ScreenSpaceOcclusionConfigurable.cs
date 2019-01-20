using System.Collections.Generic;
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
      ConfigurableComponentMenuPath._ComponentMenuPath
      + "ScreenSpaceOcclusion"
      + ConfigurableComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Renderer))]
  public class ScreenSpaceOcclusionConfigurable : Configurable {
    /// <summary>
    ///   Alpha
    /// </summary>
    string _a;

    /// <summary>
    ///   Blue
    /// </summary>
    string _b;

    /// <summary>
    ///   Green
    /// </summary>
    string _g;

    /// <summary>
    ///   Red
    /// </summary>
    string _r;

    /// <summary>
    /// </summary>
    Camera _camera;

    GameObject[] _prefabs;
    List<GameObject> _spawned = new List<GameObject>();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this._r = this.Identifier + "R";
      this._g = this.Identifier + "G";
      this._b = this.Identifier + "B";
      this._a = this.Identifier + "A";

      var rand = new Random();

      if (this._prefabs != null && this._prefabs.Length > 0) {
        for (var i = 0; i < 10; i++) {
          var banana = this._prefabs[(int)(rand.Next() * this._prefabs.Length)];

          var x = rand.NextDouble();
          var y = rand.NextDouble();
          var z = rand.NextDouble() * this._camera.farClipPlane;
          var a = new Vector2((float)x, (float)y);

          var c = this._camera.ViewportToWorldPoint(a);
          c.z = (float)z;

          var b = new Quaternion(
              (float)rand.NextDouble(),
              (float)rand.NextDouble(),
              (float)rand.NextDouble(),
              (float)rand.NextDouble());

          var d = Instantiate(banana, c, b);

          this._spawned.Add(d);
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

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(this, this._r);
      this.ParentEnvironment.UnRegister(this, this._g);
      this.ParentEnvironment.UnRegister(this, this._b);
      this.ParentEnvironment.UnRegister(this, this._a);
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
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="random_generator"></param>
    /// <returns></returns>
    public override IConfigurableConfiguration SampleConfiguration(Random random_generator) {
      var sample = random_generator.NextDouble();

      if (sample < .33f) {
        return new Configuration(this._r, (float)random_generator.NextDouble());
      }

      if (sample > .66f) {
        return new Configuration(this._g, (float)random_generator.NextDouble());
      }

      return new Configuration(this._b, (float)random_generator.NextDouble());
    }
  }
}
