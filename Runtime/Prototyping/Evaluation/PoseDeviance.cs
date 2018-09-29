using Neodroid.Runtime.Utilities.BoundingBoxes;
using Neodroid.Runtime.Utilities.Misc.Extensions;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Evaluation {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      EvaluationComponentMenuPath._ComponentMenuPath + "PoseDeviance" + EvaluationComponentMenuPath._Postfix)]
  public class PoseDeviance : ObjectiveFunction {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override float InternalEvaluate() {
      var reward = this._default_reward;

      /*if (this._playable_area != null && !this._playable_area.Bounds.Intersects(this._actor_transform.ActorBounds)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("Outside playable area");
        }
        #endif
        this.ParentEnvironment.Terminate("Outside playable area");
      }*/

      var distance = Mathf.Abs(
          Vector3.Distance(this._goal.transform.position, this._actor_transform.transform.position));
      var angle = Quaternion.Angle(this._goal.transform.rotation, this._actor_transform.transform.rotation);
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"Distance: {distance}");
        Debug.Log($"Angle: {angle}");
      }
      #endif

      if (!this._sparse) {
        reward += this._distance_nominator / (Mathf.Pow(this._distance_base, distance) + float.Epsilon);
        reward += this._angle_nominator / (Mathf.Pow(this._angle_base, angle) + float.Epsilon);

        if (this._state_full) {
          if (reward <= this._peak_reward) {
            reward = 0.0f;
          } else {
            this._peak_reward = reward;
          }
        }
      }

      if (distance < this._goal_reached_radius) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("Within range of goal");
        }
        #endif

        reward += this._solved_reward;
        if (this._terminate_on_goal_reached) {
          this.ParentEnvironment?.Terminate("Within range of goal");
        }
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log(
            $"Frame Number: {this.ParentEnvironment?.CurrentFrameNumber}, "
            + $"Terminated: {this.ParentEnvironment?.Terminated}, "
            + $"Last Reason: {this.ParentEnvironment?.LastTerminationReason}, "
            + $"Internal Feedback Signal: {reward}, "
            + $"Distance: {distance}");
      }
      #endif

      return reward;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void InternalReset() { this._peak_reward = 0.0f; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PostSetup() {
      if (!this._goal) {
        this._goal = FindObjectOfType<Transform>();
      }

      if (!this._actor_transform) {
        this._actor_transform = FindObjectOfType<Transform>();
      }

      if (this._obstructions == null || this._obstructions.Length <= 0) {
        this._obstructions = FindObjectsOfType<Obstruction>();
      }

      if (!this._playable_area) {
        this._playable_area = FindObjectOfType<BoundingBox>();
      }
    }

    #region Fields

    [Header("Specific", order = 102)]
    [SerializeField]
    float _peak_reward;

    [SerializeField] [Range(0.1f, 10f)] float _distance_base = 2f;
    [SerializeField] [Range(0.1f, 10f)] float _distance_nominator = 5f;
    [SerializeField] [Range(0.1f, 10f)] float _angle_base = 6f;
    [SerializeField] [Range(0.1f, 10f)] float _angle_nominator = 3f;

    [SerializeField] bool _sparse = true;

    [SerializeField] Transform _goal;

    [SerializeField] Transform _actor_transform;

    [SerializeField] BoundingBox _playable_area;

    [SerializeField] Obstruction[] _obstructions;

    [SerializeField] bool _state_full;
    [SerializeField] float _goal_reached_radius = 0.01f; // Equivalent to 1 cm.

    /// <summary>
    /// </summary>
    [SerializeField]
    float _solved_reward = 1000.0f;

    /// <summary>
    /// </summary>
    [SerializeField]
    float _default_reward = -0.01f;

    [SerializeField] bool _terminate_on_obstruction_collision; //TODO: implement
    [SerializeField] bool _terminate_on_goal_reached = true;

    #endregion
  }
}
