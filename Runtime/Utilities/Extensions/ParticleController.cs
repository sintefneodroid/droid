//[ExecuteInEditMode]

using UnityEngine;

namespace droid.Runtime.Utilities.Extensions {
  [RequireComponent(typeof(ParticleSystem))]
  public class ParticleController : MonoBehaviour {
    ParticleSystem _particle_system;

    // Use this for initialization
    void Start() { this._particle_system = this.GetComponent<ParticleSystem>(); }

    // Update is called once per frame
    void Update() {
      if (Input.GetKey(KeyCode.Space)) {
        if (this._particle_system.isPlaying) {
          return;
        }

        this._particle_system.Play(true);
      } else {
        //_particle_system.Pause (true);
        this._particle_system.Stop(true);
      }
    }
  }
}
