using UnityEngine;

namespace Neodroid.Utilities.Noise {
  public class SelfDestruct : MonoBehaviour {
    public float LifeTime { get; set; } = 10f;
    float _spawn_time;

    void Awake() { this._spawn_time = Time.time; }

    void Update() {
      if (this._spawn_time + this.LifeTime < Time.time) {
        Destroy(this.gameObject);
      }
    }
  }
}
