using System;
using droid.Runtime.GameObjects.BoundingBoxes;
using droid.Runtime.GameObjects.ChildSensors;
using droid.Runtime.Utilities.Extensions;
using UnityEngine;

namespace droid.Runtime.Prototyping.ObjectiveFunctions.Spatial {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(menuName : EvaluationComponentMenuPath._ComponentMenuPath
                               + "PoseDeviance"
                               + EvaluationComponentMenuPath._Postfix)]
  public class PoseDevianceObjective : SpatialObjective {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override float InternalEvaluate() {
      var signal = this.DefaultSignal;

      /*if (this._playable_area != null && !this._playable_area.Bounds.Intersects(this._actor_transform.ActorBounds)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("Outside playable area");
        }
        #endif
        this.ParentEnvironment.Terminate("Outside playable area");
      }*/

      var distance = Mathf.Abs(f : Vector3.Distance(a : this._target_transform.position,
                                                    b : this._actor_transform.position));
      var angle = Quaternion.Angle(a : this._target_transform.rotation,
                                   b : this._actor_transform.rotation);
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log(message : $"Distance: {distance}");
        Debug.Log(message : $"Angle: {angle}");
      }
      #endif

      if (!this._sparse) {
        if (this._inverse) {
          signal -= distance;
          signal -= angle;
        } else {
          signal += 1 / (distance + 1);
          signal += 1 / (angle + 1);

          if (this._state_full) {
            if (signal <= this._peak_reward) {
              signal = 0.0f;
            } else {
              this._peak_reward = signal;
            }
          }
        }
      }

      if (distance < this._goal_reached_radius) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("Within range of goal");
        }
        #endif

        signal += this.SolvedSignal;
        if (this._terminate_on_goal_reached) {
          this.ParentEnvironment?.Terminate("Within range of goal");
        }
      }

      if (this._has_collided) {
        this.ParentEnvironment?.Terminate("Actor has collided");
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log(message : $"Frame Number: {this.ParentEnvironment?.StepI}, "
                            + $"Terminated: {this.ParentEnvironment?.Terminated}, "
                            + $"Last Reason: {this.ParentEnvironment?.LastTerminationReason}, "
                            + $"Internal Feedback Signal: {signal}, "
                            + $"Distance: {distance}");
      }
      #endif

      return signal;
    }

    void OnDrawGizmosSelected() {
      var goal_position = this._target_transform.position;
      var actor_position = this._actor_transform.position;
      Debug.DrawLine(start : actor_position, end : goal_position);
      
      
      var off_up = goal_position + Vector3.up * Vector3.SignedAngle(@from : this._target_transform.forward,
                                                                    to : this
                                                                                                                 ._actor_transform.forward, axis : Vector3.up)/180;
      Debug.DrawLine(start : goal_position, end : off_up);
      Debug.DrawLine(start : actor_position, end : off_up);

      var up = this
               ._actor_transform.up;
      var up1 = this._target_transform.up;
      var off_forward = goal_position + Vector3.forward * Vector3.SignedAngle(@from : up1,
                                                                              to : up, axis : Vector3.forward)/180;
      Debug.DrawLine(start : goal_position, end : off_forward);
      Debug.DrawLine(start : actor_position, end : off_forward);
      
      var off_left = goal_position + Vector3.left * Vector3.SignedAngle(@from : up1,
                                                                        to : up, axis : Vector3.left)/180;
      Debug.DrawLine(start : goal_position, end : off_left);
      Debug.DrawLine(start : actor_position, end : off_left);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void InternalReset() {
      this._peak_reward = 0.0f;
      this._has_collided = false;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void RemotePostSetup() {
      if (!this._target_transform) {
        this._target_transform = FindObjectOfType<Transform>();
      }

      if (!this._actor_transform) {
        this._actor_transform = FindObjectOfType<Transform>();

        var remote_sensor =
            this._actor_transform.GetComponentInChildren<ChildColliderSensor<Collider, Collision>>();
        if (!remote_sensor) {
          var col = this._actor_transform.GetComponentInChildren<Collider>();
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

      if (this._obstructions == null || this._obstructions.Length <= 0) {
        this._obstructions = FindObjectsOfType<Obstruction>();
      }

      if (!this._playable_area) {
        this._playable_area = FindObjectOfType<NeodroidBoundingBox>();
      }
    }

    void OnChildTriggerEnter(GameObject child_sensor_game_object, Collision collision) {
      if (collision.collider.CompareTag("Obstruction")) {
        if (this._terminate_on_obstruction_collision) {
          this.ParentEnvironment.Terminate("Collided with obstruction");
        }
      }

      this._has_collided = true;
    }

    void OnChildTriggerEnter(GameObject child_sensor_game_object, Collider collider1) {
      if (collider1.CompareTag("Obstruction")) {
        if (this._terminate_on_obstruction_collision) {
          this.ParentEnvironment.Terminate("Collided with obstruction");
        }
      }

      this._has_collided = true;
    }

    #region Fields

    [Header("Specific", order = 102)]
    [SerializeField]
    float _peak_reward = 0;

    [SerializeField] [Range(0.1f, 10f)] float _distance_base = 2f;
    [SerializeField] [Range(0.1f, 10f)] float _distance_nominator = 5f;
    [SerializeField] [Range(0.1f, 10f)] float _angle_base = 6f;
    [SerializeField] [Range(0.1f, 10f)] float _angle_nominator = 3f;
    [SerializeField] bool _sparse = true;
    [SerializeField] bool _inverse = false;
    [SerializeField] Transform _target_transform = null;
    [SerializeField] Transform _actor_transform = null;
    [SerializeField] NeodroidBoundingBox _playable_area = null;
    [SerializeField] Obstruction[] _obstructions = null;
    [SerializeField] bool _state_full = false;
    [SerializeField] float _goal_reached_radius = 0.01f; // Equivalent to 1 cm.
    [SerializeField] bool _terminate_on_obstruction_collision = true; //TODO: implement
    [SerializeField] bool _has_collided = false;
    [SerializeField] bool _terminate_on_goal_reached = true;

    #endregion
  }
}
