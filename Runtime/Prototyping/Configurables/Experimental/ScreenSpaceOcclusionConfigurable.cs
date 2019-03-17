using System.Collections.Generic;
using droid.Runtime.Environments;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Utilities.Misc;
using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables.Experimental {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
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
    [SerializeField]
    Camera _camera = null;

    [SerializeField] GameObject[] _prefabs = null;
    List<GameObject> _spawned = new List<GameObject>();
    [SerializeField] int num_obstructions = 10;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this._r = this.Identifier + "R";
      this._g = this.Identifier + "G";
      this._b = this.Identifier + "B";
      this._a = this.Identifier + "A";

      if (Application.isPlaying) {
        if (this._prefabs != null && this._prefabs.Length > 0 && this._camera) {
          for (var i = 0; i < this.num_obstructions; i++) {
            var prefab = this._prefabs[(int)(Space1.ZeroOne.Sample() * this._prefabs.Length)];

            var x = Space1.ZeroOne.Sample();
            var y = Space1.ZeroOne.Sample();
            var z = Space1.ZeroOne.Sample() * this._camera.farClipPlane;
            var a = new Vector2(x, y);

            var c = this._camera.ViewportToWorldPoint(a);
            c.z = z;

            var b = new Quaternion(Space1.ZeroOne.Sample(),
                                   Space1.ZeroOne.Sample(),
                                   Space1.ZeroOne.Sample(),
                                   Space1.ZeroOne.Sample());

            var d = Instantiate(prefab, c, b, this.transform);

            this._spawned.Add(d);
          }
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent((PrototypingEnvironment)this.ParentEnvironment,
                                              (Configurable)this,
                                              this._r);
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent((PrototypingEnvironment)this.ParentEnvironment,
                                              (Configurable)this,
                                              this._g);
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent((PrototypingEnvironment)this.ParentEnvironment,
                                              (Configurable)this,
                                              this._b);
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent((PrototypingEnvironment)this.ParentEnvironment,
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
    /// <returns></returns>
    public override IConfigurableConfiguration SampleConfiguration() {
      var sample = Space1.ZeroOne.Sample();

      if (sample < .33f) {
        return new Configuration(this._r, Space1.ZeroOne.Sample());
      }

      if (sample > .66f) {
        return new Configuration(this._g, Space1.ZeroOne.Sample());
      }

      return new Configuration(this._b, Space1.ZeroOne.Sample());
    }
  }
}
