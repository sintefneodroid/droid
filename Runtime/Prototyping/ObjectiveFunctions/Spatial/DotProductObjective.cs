using droid.Runtime.GameObjects.BoundingBoxes;
using droid.Runtime.GameObjects.ChildSensors;
using droid.Runtime.Utilities.Extensions;
using UnityEngine;

namespace droid.Runtime.Prototyping.ObjectiveFunctions.Spatial {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(EvaluationComponentMenuPath._ComponentMenuPath
                    + "DotProduct"
                    + EvaluationComponentMenuPath._Postfix)]
  public class DotProductObjective : SpatialObjective {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override float InternalEvaluate() {
      var signal = this.DefaultSignal;

      var angle = Vector3.Dot(lhs : this.target_direction.transform.up,
                                     rhs : this._actor_transform.transform.up);
       #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"Distance: {angle}");
      }
      #endif

      if (this._inverse) {
        signal -= angle;
      } else {
        signal += angle;
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
                  + $"Distance: {angle}");
      }
      #endif

      return signal;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void InternalReset() {
      this._has_collided = false;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void RemotePostSetup() {
      if (!this.target_direction) {
        this.target_direction = FindObjectOfType<Transform>();
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
    [SerializeField] bool _inverse = false;
    [SerializeField] Transform target_direction = null;
    [SerializeField] Transform _actor_transform = null;
    [SerializeField] BoundingBox _playable_area = null;
    [SerializeField] Obstruction[] _obstructions = null;
    [SerializeField] bool _terminate_on_obstruction_collision = true;
    [SerializeField] bool _has_collided = false;

    #endregion
  }
}
