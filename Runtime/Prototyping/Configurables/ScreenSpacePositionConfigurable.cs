using System.Collections.Generic;
using Neodroid.Runtime.Environments;
using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Messaging.Messages;
using Neodroid.Runtime.Utilities.Misc;
using UnityEngine;
using Random = System.Random;

namespace Neodroid.Runtime.Prototyping.Configurables {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      ConfigurableComponentMenuPath._ComponentMenuPath + "ScreenSpacePosition" + ConfigurableComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Renderer))]
  public class ScreenSpacePositionConfigurable : Configurable {
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
    List<GameObject> _spawned= new List<GameObject>();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this._r = this.Identifier + "R";
      this._g = this.Identifier + "G";
      this._b = this.Identifier + "B";
      this._a = this.Identifier + "A";

      this._spawned= new List<GameObject>();
      var rand = new Random();

      if(this._prefabs != null && this._prefabs.Length>0) {
        for (var i = 0; i < 10; i++) {
          var banana = this._prefabs[(int)(rand.Next() * this._prefabs.Length)];


          var x = rand.Next();
          var y = rand.Next();
         var z =   rand.Next() * this._camera.farClipPlane;
          var a = new Vector2(x, y);


          var c = this._camera.ViewportToWorldPoint(a);
          c.z = z;

          var b = new Quaternion(rand.Next(), rand.Next(),rand.Next(), rand.Next());

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
        Debug.Log("Applying " + configuration + " To " + this.Identifier);
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
