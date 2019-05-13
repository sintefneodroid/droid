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

    [SerializeField] Space4 rot_space = Space4.ZeroOne;

    [SerializeField] Space2 xy_space2 = Space2.ZeroOne;

    [SerializeField] Space1 depth_space1 = Space1.ZeroOne;
    [SerializeField] Space3 size_space = Space3.ZeroOne;

    /// <summary>
    /// </summary>
    [SerializeField]
    Camera _camera = null;

    [SerializeField] GameObject[] _prefabs = null;
    List<GameObject> _spawned = new List<GameObject>();
    bool fsafas = true;
    [SerializeField] int num_obstructions = 10;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this._r = this.Identifier + "R";
      this._g = this.Identifier + "G";
      this._b = this.Identifier + "B";
      this._a = this.Identifier + "A";

      if (Application.isPlaying && this.fsafas) {
        if (this._prefabs != null && this._prefabs.Length > 0 && this._camera) {
          for (var i = 0; i < this.num_obstructions; i++) {
            var prefab = this._prefabs[(int)(Space1.ZeroOne.Sample() * this._prefabs.Length)];

            var xy = this.xy_space2.Sample();
            var z = this._camera.nearClipPlane + this.depth_space1.Sample() * this._camera.farClipPlane;

            var a = new Vector3(xy.x, xy.y, z);

            var c = this._camera.ViewportToWorldPoint(a);

            var rot = this.rot_space.Sample();
            var b = new Quaternion(rot.x, rot.y, rot.z, rot.w);

            var d = Instantiate(prefab, c, b, this.transform);
            d.transform.localScale = this.size_space.Sample();

            this._spawned.Add(d);
          }
        }

        this.fsafas = false;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent(this.ParentEnvironment, (Configurable)this, this._r);
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent(this.ParentEnvironment, (Configurable)this, this._g);
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent(this.ParentEnvironment, (Configurable)this, this._b);
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent(this.ParentEnvironment, (Configurable)this, this._a);
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
      var aa = Random.Range(0, this._spawned.Count);
      var bb = this._spawned[aa];

      var xy = this.xy_space2.Sample();
      var z = this._camera.nearClipPlane + this.depth_space1.Sample() * this._camera.farClipPlane;

      var a = new Vector3(xy.x, xy.y, z);

      var c = this._camera.ViewportToWorldPoint(a);

      var rot = this.rot_space.Sample();
      var b = new Quaternion(rot.x, rot.y, rot.z, rot.w);

      bb.transform.localScale = this.size_space.Sample();

      bb.transform.position = c;

      bb.transform.rotation = b;

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
    public override Configuration[] SampleConfigurations() {
      var s = this.depth_space1.Sample();

      var sample = Space3.ZeroOne.Sample();

      return new[] {
                       new Configuration(this._r, sample.x),
                       new Configuration(this._b, sample.y),
                       new Configuration(this._g, sample.z)
                   };
    }
  }
}
