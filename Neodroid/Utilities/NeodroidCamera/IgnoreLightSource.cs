using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Utilities.NeodroidCamera {
  [ExecuteInEditMode]
  public class IgnoreLightSource : MonoBehaviour {
    [SerializeField] bool _ignore_infrared_if_empty = true;

    [SerializeField] Light[] _lights_to_ignore;

    // Use this for initialization
    void Start() {
      if (this._lights_to_ignore == null
          || this._lights_to_ignore.Length == 0 && this._ignore_infrared_if_empty) {
        var infrared_light_sources = FindObjectsOfType<InfraredLightSource>();
        var lights = new List<Light>();
        foreach (var ils in infrared_light_sources) {
          lights.Add(ils.GetComponent<Light>());
        }

        this._lights_to_ignore = lights.ToArray();
      }
    }

    // Update is called once per frame
    void Update() { }

    void OnPreCull() {
      if (this._lights_to_ignore != null) {
        foreach (var l in this._lights_to_ignore) {
          if (l) {
            l.enabled = false;
          }
        }
      }
    }

    void OnPreRender() {
      if (this._lights_to_ignore != null) {
        foreach (var l in this._lights_to_ignore) {
          if (l) {
            l.enabled = false;
          }
        }
      }
    }

    void OnPostRender() {
      if (this._lights_to_ignore != null) {
        foreach (var l in this._lights_to_ignore) {
          if (l) {
            l.enabled = true;
          }
        }
      }
    }
  }
}
