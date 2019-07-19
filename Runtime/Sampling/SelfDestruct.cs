using UnityEngine;

namespace droid.Runtime.Sampling {
  public class SelfDestruct : MonoBehaviour {
    float _spawn_time;
    public float LifeTime { get; set; } = 10f;

    void Awake() { this._spawn_time = Time.time; }

    void Update() {
      if (this._spawn_time + this.LifeTime < Time.time) {
        Destroy(this.gameObject);
      }
    }
  }
}
