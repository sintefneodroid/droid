using System;
using System.Collections.Generic;
using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Utilities.Enums;
using UnityEngine;
using Random = System.Random;

namespace Neodroid.Runtime.Prototyping.Configurables {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      ConfigurableComponentMenuPath._ComponentMenuPath
      + "ObjectSpawner"
      + ConfigurableComponentMenuPath._Postfix)]
  public class ObjectSpawnerConfigurable : Configurable {
    [SerializeField] int _amount;

    [SerializeField] Axis _axis;

    [SerializeField] GameObject _object_to_spawn;

    List<GameObject> _spawned_objects;

    protected override void PreSetup() {
      this.DestroyObjects();
      this._spawned_objects = new List<GameObject>();
      this.SpawnObjects();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "ObjectSpawnerConfigurable"; } }

    void DestroyObjects() {
      if (this._spawned_objects != null) {
        foreach (var o in this._spawned_objects) {
          Destroy(o);
        }
      }

      foreach (Transform c in this.transform) {
        Destroy(c.gameObject);
      }
    }

    void SpawnObjects() {
      if (this._object_to_spawn) {
        var dir = Vector3.up;
        if (this._axis == Axis.X_) {
          dir = Vector3.right;
        } else if (this._axis == Axis.Z_) {
          dir = Vector3.forward;
        }

        for (var i = 0; i < this._amount; i++) {
          this._spawned_objects.Add(
              Instantiate(
                  this._object_to_spawn,
                  this.transform.position + dir * i,
                  UnityEngine.Random.rotation,
                  this.transform));
        }
      }
    }

    void OnApplicationQuit() { this.DestroyObjects(); }

    public override void ApplyConfiguration(IConfigurableConfiguration obj) {
      if (this._spawned_objects.Count < obj.ConfigurableValue) {
        var go = Instantiate(this._object_to_spawn, this.transform);
        this._spawned_objects.Add(go);
      } else if (this._spawned_objects.Count > obj.ConfigurableValue) {
        if (this._spawned_objects.Count > 0) {
          this._spawned_objects.RemoveAt(this._spawned_objects.Count - 1);
        }
      }
    }
  }
}
