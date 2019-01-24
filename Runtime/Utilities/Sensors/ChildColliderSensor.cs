using UnityEngine;

namespace droid.Runtime.Utilities.Sensors
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract class ChildColliderSensor<TCollider, TCollision> : MonoBehaviour where TCollider : Component
    {
        /// <summary>
        /// </summary>
        /// <param name="child_sensor_game_object"></param>
        /// <param name="collision"></param>
        public delegate void OnChildCollisionEnterDelegate(
            GameObject child_sensor_game_object,
            TCollision collision );

        /// <summary>
        /// </summary>
        /// <param name="child_sensor_game_object"></param>
        /// <param name="collision"></param>
        public delegate void OnChildCollisionExitDelegate(
            GameObject child_sensor_game_object,
            TCollision collision );

        /// <summary>
        /// </summary>
        /// <param name="child_sensor_game_object"></param>
        /// <param name="collision"></param>
        public delegate void OnChildCollisionStayDelegate(
            GameObject child_sensor_game_object,
            TCollision collision );

        /// <summary>
        /// </summary>
        /// <param name="child_sensor_game_object"></param>
        /// <param name="collider"></param>
        public delegate void OnChildTriggerEnterDelegate( GameObject child_sensor_game_object, TCollider collider );

        /// <summary>
        /// </summary>
        /// <param name="child_sensor_game_object"></param>
        /// <param name="collider"></param>
        public delegate void OnChildTriggerExitDelegate( GameObject child_sensor_game_object, TCollider collider );

        /// <summary>
        /// </summary>
        /// <param name="child_sensor_game_object"></param>
        /// <param name="collider"></param>
        public delegate void OnChildTriggerStayDelegate( GameObject child_sensor_game_object, TCollider collider );

        [SerializeField] protected Component _caller;

        [SerializeField] protected OnChildCollisionEnterDelegate _on_collision_enter_delegate;

        [SerializeField] protected OnChildCollisionExitDelegate _on_collision_exit_delegate;

        [SerializeField] protected OnChildCollisionStayDelegate _on_collision_stay_delegate;

        [SerializeField] protected OnChildTriggerEnterDelegate _on_trigger_enter_delegate;

        [SerializeField] protected OnChildTriggerExitDelegate _on_trigger_exit_delegate;

        [SerializeField] protected OnChildTriggerStayDelegate _on_trigger_stay_delegate;

        /// <summary>
        /// </summary>
        public OnChildCollisionEnterDelegate OnCollisionEnterDelegate
        {
            set { this._on_collision_enter_delegate = value; }
        }

        /// <summary>
        /// </summary>
        public OnChildTriggerEnterDelegate OnTriggerEnterDelegate
        {
            set { this._on_trigger_enter_delegate = value; }
        }

        /// <summary>
        /// </summary>
        public OnChildTriggerStayDelegate OnTriggerStayDelegate
        {
            set { this._on_trigger_stay_delegate = value; }
        }

        /// <summary>
        /// </summary>
        public OnChildCollisionStayDelegate OnCollisionStayDelegate
        {
            set { this._on_collision_stay_delegate = value; }
        }

        /// <summary>
        /// </summary>
        public OnChildCollisionExitDelegate OnCollisionExitDelegate
        {
            set { this._on_collision_exit_delegate = value; }
        }

        /// <summary>
        /// </summary>
        public OnChildTriggerExitDelegate OnTriggerExitDelegate
        {
            set { this._on_trigger_exit_delegate = value; }
        }

        /// <summary>
        /// </summary>
        public Component Caller { get { return this._caller; } set { this._caller = value; } }
    }
}
