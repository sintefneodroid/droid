using UnityEngine;

namespace Neodroid.Utilities.Sensors {
  public class ChildSensor : MonoBehaviour {
    //public GameObject Caller{ get { return _caller; } set { _caller = value; } }

    public delegate void OnChildCollisionEnterDelegate(GameObject child_game_object, Collision collision);

    public delegate void OnChildCollisionExitDelegate(GameObject child_game_object, Collision collision);

    public delegate void OnChildCollisionStayDelegate(GameObject child_game_object, Collision collision);

    public delegate void OnChildTriggerEnterDelegate(GameObject child_game_object, Collider collider);

    public delegate void OnChildTriggerExitDelegate(GameObject child_game_object, Collider collider);

    public delegate void OnChildTriggerStayDelegate(GameObject child_game_object, Collider collider);

    [SerializeField] Component _caller;

    OnChildCollisionEnterDelegate _on_collision_enter_delegate;

    OnChildCollisionExitDelegate _on_collision_exit_delegate;

    OnChildCollisionStayDelegate _on_collision_stay_delegate;

    OnChildTriggerEnterDelegate _on_trigger_enter_delegate;

    OnChildTriggerExitDelegate _on_trigger_exit_delegate;

    OnChildTriggerStayDelegate _on_trigger_stay_delegate;

    public OnChildCollisionEnterDelegate OnCollisionEnterDelegate {
      set { this._on_collision_enter_delegate = value; }
    }

    public OnChildTriggerEnterDelegate OnTriggerEnterDelegate {
      set { this._on_trigger_enter_delegate = value; }
    }

    public OnChildTriggerStayDelegate OnTriggerStayDelegate {
      set { this._on_trigger_stay_delegate = value; }
    }

    public OnChildCollisionStayDelegate OnCollisionStayDelegate {
      set { this._on_collision_stay_delegate = value; }
    }

    public OnChildCollisionExitDelegate OnCollisionExitDelegate {
      set { this._on_collision_exit_delegate = value; }
    }

    public OnChildTriggerExitDelegate OnTriggerExitDelegate {
      set { this._on_trigger_exit_delegate = value; }
    }

    public Component Caller { get { return this._caller; } set { this._caller = value; } }

    void OnCollisionEnter(Collision collision) {
      this._on_collision_enter_delegate?.Invoke(this.gameObject, collision);
    }

    void OnTriggerEnter(Collider other) { this._on_trigger_enter_delegate?.Invoke(this.gameObject, other); }

    void OnTriggerStay(Collider other) { this._on_trigger_stay_delegate?.Invoke(this.gameObject, other); }

    void OnCollisionStay(Collision collision) {
      this._on_collision_stay_delegate?.Invoke(this.gameObject, collision);
    }

    void OnTriggerExit(Collider other) { this._on_trigger_exit_delegate?.Invoke(this.gameObject, other); }

    void OnCollisionExit(Collision collision) {
      this._on_collision_exit_delegate?.Invoke(this.gameObject, collision);
    }
  }
}
