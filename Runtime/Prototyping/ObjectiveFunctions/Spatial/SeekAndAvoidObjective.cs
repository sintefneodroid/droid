using System;
using System.Collections.Generic;
using droid.Runtime.GameObjects.ChildSensors;
using UnityEngine;
using Random = UnityEngine.Random;

namespace droid.Runtime.Prototyping.ObjectiveFunctions.Spatial {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class SeekAndAvoidObjective : ObjectiveFunction {
    [SerializeField] Transform _actor = null;
    [SerializeField] GameObject _avoidable = null;
    List<GameObject> _avoidables = null;
    [SerializeField] GameObject _collectible = null;
    List<GameObject> _collectibles = null;
    [SerializeField] int _end_game_radius = 10;
    Vector3 _initial_actor_position = Vector3.zero;
    [SerializeField] int _num_avoidables = 50;
    [SerializeField] int _num_collectibles = 50;
    [SerializeField] int _penalty = -1;
    [SerializeField] int _reward = 1;
    float _score;
    [SerializeField] int _spawn_radius = 10;
    List<Vector3> _spawned_locations = null;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override String PrototypingTypeName { get { return "SeekAndAvoidListener"; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override Single InternalEvaluate() {
      // The game ends if the number of good balls is 0, or if the robot is too far from start
      var actor = this._actor;
      if (actor != null) {
        var dist = Vector3.Distance(this._initial_actor_position, actor.position);
        var game_objects = this._collectibles;
        var is_over = game_objects != null && (game_objects.Count == 0 || dist > this._end_game_radius);

        if (is_over) {
          this.ParentEnvironment.Terminate($"Ending Game: Dist {dist} radius {this._spawn_radius}");
        }
      }

      return this._score;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void InternalReset() {
      if (!Application.isPlaying) {
        return;
      }

      var actor = this._actor;
      if (actor != null) {
        this._initial_actor_position = actor.position;
        var remote_sensor = this._actor.GetComponentInChildren<ChildColliderSensor<Collider, Collision>>();
        if (!remote_sensor) {
          var col = this._actor.GetComponentInChildren<Collider>();
          if (col) {
            remote_sensor = col.gameObject.AddComponent<ChildColliderSensor<Collider, Collision>>();
          }
        }

        if (remote_sensor) {
          remote_sensor.Caller = this;
          remote_sensor.OnTriggerEnterDelegate = this.OnChildTriggerEnter;
          remote_sensor.OnCollisionEnterDelegate = this.OnChildTriggerEnter;
        }
      }

      this.ClearEnvironment();
      this.SpawnCollectibles();
      this.SpawnAvoidables();
    }

    void OnChildTriggerEnter(GameObject child_game_object, Collision collision) {
      this.OnChildTriggerEnter(child_game_object, collision.collider);
    }

    void OnChildTriggerEnter(GameObject child_game_object, Collider collider1) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        print($"{child_game_object} is colliding with {collider1}");
      }
      #endif

      var collectible = this._collectible;
      if (collectible != null && collider1.gameObject.name.Contains(collectible.name)) {
        this._collectibles.Remove(collider1.gameObject);
        Destroy(collider1.gameObject);
        this._score += this._reward;
      }

      var game_object = this._avoidable;
      if (game_object != null && collider1.gameObject.name.Contains(game_object.name)) {
        this._avoidables.Remove(collider1.gameObject);
        this._score += this._penalty;
        Destroy(collider1.gameObject);
      }
    }

    void SpawnCollectibles() {
      for (var i = 0; i < this._num_collectibles; i++) {
        var game_object = this._collectible;
        if (game_object != null) {
          var collectible = this.RandomSpawn(this._collectible, this._initial_actor_position);
          this._collectibles.Add(collectible);
        }
      }
    }

    void SpawnAvoidables() {
      for (var i = 0; i < this._num_avoidables; i++) {
        var game_object = this._avoidable;
        if (game_object != null) {
          var avoidable = this.RandomSpawn(this._avoidable, this._initial_actor_position);
          this._avoidables.Add(avoidable);
        }
      }
    }

    GameObject RandomSpawn(GameObject prefab, Vector3 position) {
      this._spawned_locations.Add(position);

      if (prefab != null) {
        Vector3 location;
        do {
          location = this._actor.transform.position;
          location.x += Random.Range(-this._spawn_radius, this._spawn_radius);
          location.y = position.y + 1f;
          location.z += Random.Range(-this._spawn_radius, this._spawn_radius);
        } while (this._spawned_locations.Contains(location));

        this._spawned_locations.Add(location);

        return Instantiate(prefab,
                           location,
                           Quaternion.identity,
                           this.ParentEnvironment.Transform);
      }

      return null;
    }

    void ClearEnvironment() {
      if (this._spawned_locations != null) {
        this._spawned_locations.Clear();
      } else {
        this._spawned_locations = new List<Vector3>();
      }

      if (this._collectibles != null) {
        foreach (var obj in this._collectibles) {
          Destroy(obj);
        }

        this._collectibles.Clear();
      } else {
        this._collectibles = new List<GameObject>();
      }

      if (this._avoidables != null) {
        foreach (var obj in this._avoidables) {
          Destroy(obj);
        }

        this._avoidables.Clear();
      } else {
        this._avoidables = new List<GameObject>();
      }
    }
  }
}
