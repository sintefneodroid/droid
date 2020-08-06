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
    public override string PrototypingTypeName { get { return "SeekAndAvoidListener"; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override float InternalEvaluate() {
      // The game ends if the number of good balls is 0, or if the robot is too far from start
      var actor = this._actor;
      if (actor != null) {
        var dist = Vector3.Distance(a : this._initial_actor_position, b : actor.position);
        var game_objects = this._collectibles;
        var is_over = game_objects != null && (game_objects.Count == 0 || dist > this._end_game_radius);

        if (is_over) {
          this.ParentEnvironment.Terminate(reason : $"Ending Game: Dist {dist} radius {this._spawn_radius}");
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
      this.OnChildTriggerEnter(child_game_object : child_game_object, collider1 : collision.collider);
    }

    void OnChildTriggerEnter(GameObject child_game_object, Collider collider1) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        print(message : $"{child_game_object} is colliding with {collider1}");
      }
      #endif

      var collectible = this._collectible;
      if (collectible != null && collider1.gameObject.name.Contains(value : collectible.name)) {
        this._collectibles.Remove(item : collider1.gameObject);
        Destroy(obj : collider1.gameObject);
        this._score += this._reward;
      }

      var game_object = this._avoidable;
      if (game_object != null && collider1.gameObject.name.Contains(value : game_object.name)) {
        this._avoidables.Remove(item : collider1.gameObject);
        this._score += this._penalty;
        Destroy(obj : collider1.gameObject);
      }
    }

    void SpawnCollectibles() {
      for (var i = 0; i < this._num_collectibles; i++) {
        var game_object = this._collectible;
        if (game_object != null) {
          var collectible =
              this.RandomSpawn(prefab : this._collectible, position : this._initial_actor_position);
          this._collectibles.Add(item : collectible);
        }
      }
    }

    void SpawnAvoidables() {
      for (var i = 0; i < this._num_avoidables; i++) {
        var game_object = this._avoidable;
        if (game_object != null) {
          var avoidable = this.RandomSpawn(prefab : this._avoidable, position : this._initial_actor_position);
          this._avoidables.Add(item : avoidable);
        }
      }
    }

    GameObject RandomSpawn(GameObject prefab, Vector3 position) {
      this._spawned_locations.Add(item : position);

      if (prefab != null) {
        Vector3 location;
        do {
          location = this._actor.transform.position;
          location.x += Random.Range(min : -this._spawn_radius, max : this._spawn_radius);
          location.y = position.y + 1f;
          location.z += Random.Range(min : -this._spawn_radius, max : this._spawn_radius);
        } while (this._spawned_locations.Contains(item : location));

        this._spawned_locations.Add(item : location);

        return Instantiate(original : prefab,
                           position : location,
                           rotation : Quaternion.identity,
                           parent : this.ParentEnvironment.Transform);
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
          Destroy(obj : obj);
        }

        this._collectibles.Clear();
      } else {
        this._collectibles = new List<GameObject>();
      }

      if (this._avoidables != null) {
        foreach (var obj in this._avoidables) {
          Destroy(obj : obj);
        }

        this._avoidables.Clear();
      } else {
        this._avoidables = new List<GameObject>();
      }
    }
  }
}
