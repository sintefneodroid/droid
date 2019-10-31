using droid.Runtime.GameObjects.BoundingBoxes;
using droid.Runtime.GameObjects.ChildSensors;
using droid.Runtime.Utilities.Extensions;
using UnityEngine;

namespace droid.Runtime.Prototyping.ObjectiveFunctions.Spatial {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(EvaluationComponentMenuPath._ComponentMenuPath
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

      var distance =
          Mathf.Abs(Vector3.Distance(this._goal.transform.position,
                                     this._actor_transform.transform.position));
      var angle = Quaternion.Angle(this._goal.transform.rotation, this._actor_transform.transform.rotation);
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"Distance: {distance}");
        Debug.Log($"Angle: {angle}");
      }
      #endif

      if (!this._sparse) {
        if (this._inverse) {
          signal -= Mathf.Pow(this._distance_base, distance);
          signal -= Mathf.Pow(this._angle_base, angle);
        } else {
          signal += this._distance_nominator / (Mathf.Pow(this._distance_base, distance) + float.Epsilon);
          signal += this._angle_nominator / (Mathf.Pow(this._angle_base, angle) + float.Epsilon);

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
        Debug.Log($"Frame Number: {this.ParentEnvironment?.StepI}, "
                  + $"Terminated: {this.ParentEnvironment?.Terminated}, "
                  + $"Last Reason: {this.ParentEnvironment?.LastTerminationReason}, "
                  + $"Internal Feedback Signal: {signal}, "
                  + $"Distance: {distance}");
      }
      #endif

      return signal;
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
      if (!this._goal) {
        this._goal = FindObjectOfType<Transform>();
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
        this._playable_area = FindObjectOfType<BoundingBox>();
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
    [SerializeField] Transform _goal = null;
    [SerializeField] Transform _actor_transform = null;
    [SerializeField] BoundingBox _playable_area = null;
    [SerializeField] Obstruction[] _obstructions = null;
    [SerializeField] bool _state_full = false;
    [SerializeField] float _goal_reached_radius = 0.01f; // Equivalent to 1 cm.
    [SerializeField] bool _terminate_on_obstruction_collision = true; //TODO: implement
    [SerializeField] bool _has_collided = false;
    [SerializeField] bool _terminate_on_goal_reached = true;

    #endregion
  }
}
