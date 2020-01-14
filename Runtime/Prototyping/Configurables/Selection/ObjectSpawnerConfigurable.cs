using System.Collections.Generic;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Structs.Space;
using droid.Runtime.Structs.Space.Sample;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables.Selection {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "ObjectSpawner"
                    + ConfigurableComponentMenuPath._Postfix)]
  public class ObjectSpawnerConfigurable : Configurable {
    [SerializeField] int _amount = 0;

    [SerializeField] AxisEnum _axisEnum = AxisEnum.X_;

    [SerializeField] GameObject _object_to_spawn = null;

    List<GameObject> _spawned_objects = null;
    [SerializeField] SampleSpace1 _configurable_value_space = new SampleSpace1 {_space = Space1.TwentyEighty};

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "ObjectSpawnerConfigurable"; } }

    public override void PreSetup() {
      this.DestroyObjects();
      this._spawned_objects = new List<GameObject>();
      this.SpawnObjects();
    }

    void DestroyObjects() {
      if (this._spawned_objects != null) {
        foreach (var o in this._spawned_objects) {
          Destroy(obj : o);
        }
      }

      foreach (Transform c in this.transform) {
        Destroy(obj : c.gameObject);
      }
    }

    void SpawnObjects() {
      if (this._object_to_spawn) {
        var dir = Vector3.up;
        if (this._axisEnum == AxisEnum.X_) {
          dir = Vector3.right;
        } else if (this._axisEnum == AxisEnum.Z_) {
          dir = Vector3.forward;
        }

        var transform1 = this.transform;
        for (var i = 0; i < this._amount; i++) {
          this._spawned_objects.Add(Instantiate(original : this._object_to_spawn,
                                                transform1.position + dir * i,
                                                rotation : Random.rotation,
                                                parent : transform1));
        }
      }
    }

    void OnApplicationQuit() { this.DestroyObjects(); }

    /// <summary>
    ///
    /// </summary>
    public ISamplable ConfigurableValueSpace { get { return this._configurable_value_space; } }

    public override Configuration[] SampleConfigurations() {
      return new Configuration[] {this.ConfigurableValueSpace.Sample()};
    }

    public override void UpdateCurrentConfiguration() { }

    public override void ApplyConfiguration(IConfigurableConfiguration obj) {
      if (this._spawned_objects.Count < obj.ConfigurableValue) {
        var go = Instantiate(original : this._object_to_spawn, parent : this.transform);
        this._spawned_objects.Add(item : go);
      } else if (this._spawned_objects.Count > obj.ConfigurableValue) {
        if (this._spawned_objects.Count > 0) {
          this._spawned_objects.RemoveAt(this._spawned_objects.Count - 1);
        }
      }
    }
  }
}
