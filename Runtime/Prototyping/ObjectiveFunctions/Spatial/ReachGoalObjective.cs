using droid.Runtime.GameObjects.ChildSensors;
using droid.Runtime.Prototyping.Actors;
using droid.Runtime.Utilities;
using droid.Runtime.Utilities.Grid;
using UnityEngine;

namespace droid.Runtime.Prototyping.ObjectiveFunctions.Spatial {
  /// <summary>
  ///
  /// </summary>
  [AddComponentMenu(EvaluationComponentMenuPath._ComponentMenuPath
                    + "ReachGoal"
                    + EvaluationComponentMenuPath._Postfix)]
  public class ReachGoalObjective : SpatialObjective {
    [SerializeField] Actor _actor = null;

    [SerializeField] bool _based_on_tags = false;

    [SerializeField] EmptyCell _goal = null;

    //Used for.. if outside playable area then reset
    [SerializeField] ActorOverlapping _overlapping = ActorOverlapping.Outside_area_;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override float InternalEvaluate() {
      var distance =
          Mathf.Abs(Vector3.Distance(this._goal.transform.position, this._actor.transform.position));

      if (this._overlapping == ActorOverlapping.Inside_area_ || distance < 0.5f) {
        this.ParentEnvironment.Terminate("Inside goal area");
        return 1f;
      }

      return 0f;
    }

    /// <summary>
    ///
    /// </summary>
    public override void InternalReset() {
      this.Setup();
      this._overlapping = ActorOverlapping.Outside_area_;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="goal"></param>
    public void SetGoal(EmptyCell goal) {
      this._goal = goal;
      this.InternalReset();
    }

    /// <summary>
    ///
    /// </summary>
    public override void RemotePostSetup() {
      if (!this._goal) {
        this._goal = FindObjectOfType<EmptyCell>();
      }

      if (!this._actor) {
        this._actor = FindObjectOfType<Actor>();
      }

      if (this._goal) {
        NeodroidRegistrationUtilities
            .RegisterCollisionTriggerCallbacksOnChildren<ChildCollider3DSensor, Collider, Collision>(this,
                                                                                                     this
                                                                                                         ._goal
                                                                                                         .transform,
                                                                                                     null,
                                                                                                     this
                                                                                                         .OnTriggerEnterChild);
      }

      if (this._actor) {
        NeodroidRegistrationUtilities
            .RegisterCollisionTriggerCallbacksOnChildren<ChildCollider3DSensor, Collider, Collision>(this,
                                                                                                     this
                                                                                                         ._actor
                                                                                                         .transform,
                                                                                                     null,
                                                                                                     this
                                                                                                         .OnTriggerEnterChild);
      }
    }

    void OnTriggerEnterChild(GameObject child_game_object, Collider other_game_object) {
      Debug.Log("triggered");
      if (this._actor) {
        if (this._based_on_tags) {
          if (other_game_object.CompareTag(this._actor.tag)) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log("Actor is inside area");
            }
            #endif

            this._overlapping = ActorOverlapping.Inside_area_;
          }
        } else {
          if (child_game_object == this._goal.gameObject
              && other_game_object.gameObject == this._actor.gameObject) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log("Actor is inside area");
            }
            #endif
            this._overlapping = ActorOverlapping.Inside_area_;
          }
        }
      }
    }

    /// <summary>
    /// </summary>
    enum ActorOverlapping {
      /// <summary>
      /// </summary>
      Inside_area_,

      /// <summary>
      /// </summary>
      Outside_area_
    }
  }
}
