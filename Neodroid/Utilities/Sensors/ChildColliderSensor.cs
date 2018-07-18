using UnityEngine;

namespace Neodroid.Utilities.Sensors {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class ChildColliderSensor : MonoBehaviour {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="child_game_object"></param>
    /// <param name="collision"></param>
    public delegate void OnChildCollisionEnterDelegate(GameObject child_game_object, Collision collision);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="child_game_object"></param>
    /// <param name="collision"></param>
    public delegate void OnChildCollisionExitDelegate(GameObject child_game_object, Collision collision);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="child_game_object"></param>
    /// <param name="collision"></param>
    public delegate void OnChildCollisionStayDelegate(GameObject child_game_object, Collision collision);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="child_game_object"></param>
    /// <param name="collider"></param>
    public delegate void OnChildTriggerEnterDelegate(GameObject child_game_object, Collider collider);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="child_game_object"></param>
    /// <param name="collider"></param>
    public delegate void OnChildTriggerExitDelegate(GameObject child_game_object, Collider collider);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="child_game_object"></param>
    /// <param name="collider"></param>
    public delegate void OnChildTriggerStayDelegate(GameObject child_game_object, Collider collider);

    [SerializeField] Component _caller;

    OnChildCollisionEnterDelegate _on_collision_enter_delegate;

    OnChildCollisionExitDelegate _on_collision_exit_delegate;

    OnChildCollisionStayDelegate _on_collision_stay_delegate;

    OnChildTriggerEnterDelegate _on_trigger_enter_delegate;

    OnChildTriggerExitDelegate _on_trigger_exit_delegate;

    OnChildTriggerStayDelegate _on_trigger_stay_delegate;

    /// <summary>
    /// 
    /// </summary>
    public OnChildCollisionEnterDelegate OnCollisionEnterDelegate {
      set { this._on_collision_enter_delegate = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    public OnChildTriggerEnterDelegate OnTriggerEnterDelegate {
      set { this._on_trigger_enter_delegate = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    public OnChildTriggerStayDelegate OnTriggerStayDelegate {
      set { this._on_trigger_stay_delegate = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    public OnChildCollisionStayDelegate OnCollisionStayDelegate {
      set { this._on_collision_stay_delegate = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    public OnChildCollisionExitDelegate OnCollisionExitDelegate {
      set { this._on_collision_exit_delegate = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    public OnChildTriggerExitDelegate OnTriggerExitDelegate {
      set { this._on_trigger_exit_delegate = value; }
    }

    /// <summary>
    /// 
    /// </summary>
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
