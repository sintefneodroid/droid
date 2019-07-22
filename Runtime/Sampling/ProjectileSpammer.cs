using UnityEngine;

namespace droid.Runtime.Sampling {
  public class ProjectileSpammer : MonoBehaviour {
    [SerializeField] string _assigned_tag = "Obstruction";
    float _last_spawn = 0f;
    [SerializeField] [Range(.0f, 3.0f)] float _life_time = 2f;
    [SerializeField] Vector2 _mass_range = new Vector2(1f, 4f);
    [SerializeField] float _projectile_multiplier = 100f;
    [SerializeField] [Range(.0f, 1.0f)] float _scale_modifier = 0.2f;
    [SerializeField] float _spawn_radius = 20f;
    [SerializeField] float _spawn_rate = 0.5f;
    [SerializeField] Transform _target = null;

    void Update() {
      if (this._last_spawn + 1 / this._spawn_rate < Time.time) {
        if (this._spawn_rate > 1) {
          for (var i = 0; i < this._spawn_rate; i++) {
            this.SpawnRandomProjectile();
          }
        } else {
          this.SpawnRandomProjectile();
        }
      }
    }

    void SpawnRandomProjectile() {
      var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
      cube.tag = this._assigned_tag;
      cube.transform.position = this._target.transform.position + Random.onUnitSphere * this._spawn_radius;
      cube.transform.rotation = Random.rotation;
      cube.transform.localScale = (Vector3.one - Random.insideUnitSphere) / 2 * this._scale_modifier;
      var rb = cube.AddComponent<Rigidbody>();
      rb.AddForce((this._target.position - cube.transform.position) * this._projectile_multiplier);
      rb.AddTorque(Random.insideUnitSphere);
      rb.mass = Random.Range(this._mass_range.x, this._mass_range.y);
      var sf = cube.AddComponent<SelfDestruct>();
      sf.LifeTime = this._life_time;
    }
  }
}
