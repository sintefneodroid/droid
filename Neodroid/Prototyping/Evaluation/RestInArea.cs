using System.Collections;
using Neodroid.Prototyping.Actors;
using Neodroid.Prototyping.Observers;
using Neodroid.Utilities;
using Neodroid.Utilities.BoundingBoxes;
using UnityEngine;

namespace Neodroid.Prototyping.Evaluation {
  [AddComponentMenu(
      EvaluationComponentMenuPath._ComponentMenuPath + "RestInArea" + EvaluationComponentMenuPath._Postfix)]
  public class RestInArea : ObjectiveFunction {
    [SerializeField] float _resting_time = 3f;
    [SerializeField] Actor _actor;

    [SerializeField] Collider _area;
    [SerializeField] bool _is_resting;

    [SerializeField] Utilities.Unsorted.Obstruction[] _obstructions;

    //Used for.. if outside playable area then reset
    [SerializeField] ActorOverlapping _overlapping = ActorOverlapping.Outside_area_;

    [SerializeField] BoundingBox _playable_area;
    [SerializeField] Coroutine _wait_for_resting;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override float InternalEvaluate() {
      if (this._overlapping == ActorOverlapping.Inside_area_ && this._is_resting) {
        if (this._actor is KillableActor) {
          if (((KillableActor)this._actor).IsAlive) {
            this.ParentEnvironment.Terminate("Inside goal area");
            return 1f;
          }
        } else {
          this.ParentEnvironment.Terminate("Inside goal area");
          return 1f;
        }
      }

      if (this._playable_area && this._actor) {
        if (!this._playable_area.Bounds.Intersects(this._actor.GetComponent<Collider>().bounds)) {
          this.ParentEnvironment.Terminate("Actor is outside playable area");
        }
      }

      return 0f;
    }

    public override void InternalReset() {
      if (this._wait_for_resting != null) {
        this.StopCoroutine(this._wait_for_resting);
      }

      this._is_resting = false;
    }

    IEnumerator WaitForResting() {
      yield return new WaitForSeconds(this._resting_time);

      this._is_resting = true;
    }

    protected override void Setup() {
      if (!this._area) {
        this._area = FindObjectOfType<Observer>().gameObject.GetComponent<Collider>();
      }

      if (!this._actor) {
        this._actor = FindObjectOfType<Actor>();
      }

      if (this._obstructions.Length <= 0) {
        this._obstructions = FindObjectsOfType<Utilities.Unsorted.Obstruction>();
      }

      if (!this._playable_area) {
        this._playable_area = FindObjectOfType<BoundingBox>();
      }

      Utilities.Unsorted.NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren(
          this,
          this._area.transform,
          null,
          this.OnTriggerEnterChild,
          null,
          this.OnTriggerExitChild,
          null,
          this.OnTriggerStayChild,
          this.Debugging);

      Utilities.Unsorted.NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren(
          this,
          this._actor.transform,
          null,
          this.OnTriggerEnterChild,
          null,
          this.OnTriggerExitChild,
          null,
          this.OnTriggerStayChild,
          this.Debugging);
    }

    void OnTriggerEnterChild(GameObject child_game_object, Collider other_game_object) {
      if (this._actor) {
        if (child_game_object == this._area.gameObject
            && other_game_object.gameObject == this._actor.gameObject) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log("Actor is inside area");
          }
          #endif

          this._overlapping = ActorOverlapping.Inside_area_;
          if (this._wait_for_resting != null) {
            this.StopCoroutine(this._wait_for_resting);
          }

          this._wait_for_resting = this.StartCoroutine(this.WaitForResting());
        }
      }
    }

    void OnTriggerStayChild(GameObject child_game_object, Collider other_game_object) {
      if (this._actor) {
        if (child_game_object == this._area.gameObject
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

    void OnTriggerExitChild(GameObject child_game_object, Collider other_game_object) {
      if (this._actor) {
        if (child_game_object == this._area.gameObject
            && other_game_object.gameObject == this._actor.gameObject) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log("Actor is outside area");
          }
          #endif

          this._overlapping = ActorOverlapping.Outside_area_;
          if (this._wait_for_resting != null) {
            this.StopCoroutine(this._wait_for_resting);
          }
        }
      }
    }
  }
}
