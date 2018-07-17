using System.Collections;
using droid.Neodroid.Prototyping.Actors;
using droid.Neodroid.Prototyping.Observers;
using droid.Neodroid.Utilities.BoundingBoxes;
using droid.Neodroid.Utilities.Unsorted;
using UnityEngine;

namespace droid.Neodroid.Prototyping.Evaluation {
  /// <summary>
  /// 
  /// </summary>
  [AddComponentMenu(
      EvaluationComponentMenuPath._ComponentMenuPath + "RestInArea" + EvaluationComponentMenuPath._Postfix)]
  public class RestInArea : ObjectiveFunction {
    [SerializeField] float _resting_time = 3f;
    [SerializeField] Actor _actor;

    [SerializeField] Collider _area;
    [SerializeField] bool _is_resting;

    [SerializeField] Obstruction[] _obstructions;

    //Used for.. if outside playable area then reset
    [SerializeField] ActorOverlapping _overlapping = ActorOverlapping.Outside_area_;

    [SerializeField] BoundingBox _playable_area;
    [SerializeField] Coroutine _wait_for_resting;
    [SerializeField] bool _sparse;
    WaitForSeconds _wait_for_seconds = new WaitForSeconds(3f);

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <returns></returns>
    public override float InternalEvaluate() {
      var signal = 0f;

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

      if (!this._sparse) {
        signal += 1 / Vector3.Distance(this._actor.transform.position, this._area.transform.position);
      }

      if (this._playable_area && this._actor) {
        if (!this._playable_area.Bounds.Intersects(this._actor.GetComponent<Collider>().bounds)) {
          this.ParentEnvironment.Terminate("Actor is outside playable area");
        }
      }

      return signal;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void InternalReset() {
      if (this._wait_for_resting != null) {
        this.StopCoroutine(this._wait_for_resting);
      }

      this._is_resting = false;
    }

    IEnumerator WaitForResting() {
      yield return this._wait_for_seconds;

      this._is_resting = true;
    }

    protected override void PostSetup() {
      if (!this._area) {
        this._area = FindObjectOfType<Observer>().gameObject.GetComponent<Collider>();
      }

      if (!this._actor) {
        this._actor = FindObjectOfType<Actor>();
      }

      if (this._obstructions.Length <= 0) {
        this._obstructions = FindObjectsOfType<Obstruction>();
      }

      if (!this._playable_area) {
        this._playable_area = FindObjectOfType<BoundingBox>();
      }

      NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren(
          this,
          this._area.transform,
          null,
          this.OnTriggerEnterChild,
          null,
          this.OnTriggerExitChild,
          null,
          this.OnTriggerStayChild,
          this.Debugging);

      NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren(
          this,
          this._actor.transform,
          null,
          this.OnTriggerEnterChild,
          null,
          this.OnTriggerExitChild,
          null,
          this.OnTriggerStayChild,
          this.Debugging);
      this._wait_for_seconds = new WaitForSeconds(this._resting_time);
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
