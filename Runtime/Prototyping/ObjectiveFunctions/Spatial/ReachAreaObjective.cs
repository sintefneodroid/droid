using droid.Runtime.GameObjects.BoundingBoxes;
using droid.Runtime.GameObjects.ChildSensors;
using droid.Runtime.Prototyping.Actors;
using droid.Runtime.Prototyping.Sensors;
using droid.Runtime.Utilities;
using droid.Runtime.Utilities.Extensions;
using UnityEngine;

namespace droid.Runtime.Prototyping.ObjectiveFunctions.Spatial {
  /// <summary>
  ///
  /// </summary>
  enum ActorOverlapping {
    Inside_area_,
    Outside_area_
  }

  /// <summary>
  ///
  /// </summary>
  enum ActorColliding {
    Not_colliding_,
    Colliding_
  }

  //[RequireComponent (typeof(BoundingBox))]
  //[RequireComponent (typeof(BoxCollider))]
  [AddComponentMenu(EvaluationComponentMenuPath._ComponentMenuPath
                    + "ReachArea"
                    + EvaluationComponentMenuPath._Postfix)]
  public class ReachAreaObjective : SpatialObjective {
    [SerializeField] Collider _actor = null;

    [SerializeField] Collider _area = null;

    [SerializeField] bool _based_on_tags = false;
    [SerializeField] ActorColliding _colliding = ActorColliding.Not_colliding_;

    [SerializeField] Obstruction[] _obstructions;

    //Used for.. if outside playable area then reset
    [SerializeField] ActorOverlapping _overlapping = ActorOverlapping.Outside_area_;

    [SerializeField] BoundingBox _playable_area;

    public override void InternalReset() { }

    public override float InternalEvaluate() {
      /*var regularising_term = 0f;

            foreach (var ob in _obstructions) {
              RaycastHit ray_hit;
              Physics.Raycast (_actor.transform.position, (ob.transform.position - _actor.transform.position).normalized, out ray_hit, LayerMask.NameToLayer ("Obstruction"));
              regularising_term += -Mathf.Abs (Vector3.Distance (ray_hit.point, _actor.transform.position));
              //regularising_term += -Mathf.Abs (Vector3.Distance (ob.transform.position, _actor.transform.position));
            }

            reward += 0.2 * regularising_term;*/

      //reward += 1 / Mathf.Abs (Vector3.Distance (_area.transform.position, _actor.transform.position)); // Inversely porpotional to the absolute distance, closer higher reward

      if (this._overlapping == ActorOverlapping.Inside_area_) {
        this.ParentEnvironment.Terminate("Inside goal area");
        return 1f;
      }

      if (this._colliding == ActorColliding.Colliding_) {
        this.ParentEnvironment.Terminate("Actor colliding with obstruction");
      }

      if (this._playable_area && this._actor) {
        if (!this._playable_area.Bounds.Intersects(this._actor.GetComponent<Collider>().bounds)) {
          this.ParentEnvironment.Terminate("Actor is outside playable area");
        }
      }

      return 0f;
    }

    public override void RemotePostSetup() {
      if (!this._area) {
        this._area = FindObjectOfType<Sensor>().gameObject.GetComponent<Collider>();
      }

      if (!this._actor) {
        this._actor = FindObjectOfType<Actor>().gameObject.GetComponent<Collider>();
      }

      if (this._obstructions.Length <= 0) {
        this._obstructions = FindObjectsOfType<Obstruction>();
      }

      if (!this._playable_area) {
        this._playable_area = FindObjectOfType<BoundingBox>();
      }

      NeodroidRegistrationUtilities
          .RegisterCollisionTriggerCallbacksOnChildren<ChildCollider3DSensor, Collider, Collision>(this,
                                                                                                   this
                                                                                                       ._area
                                                                                                       .transform,
                                                                                                   this
                                                                                                       .OnCollisionEnterChild,
                                                                                                   this
                                                                                                       .OnTriggerEnterChild,
                                                                                                   this
                                                                                                       .OnCollisionExitChild,
                                                                                                   this
                                                                                                       .OnTriggerExitChild,
                                                                                                   this
                                                                                                       .OnCollisionStayChild,
                                                                                                   this
                                                                                                       .OnTriggerStayChild);

      NeodroidRegistrationUtilities
          .RegisterCollisionTriggerCallbacksOnChildren<ChildCollider3DSensor, Collider, Collision>(this,
                                                                                                   this
                                                                                                       ._actor
                                                                                                       .transform,
                                                                                                   this
                                                                                                       .OnCollisionEnterChild,
                                                                                                   this
                                                                                                       .OnTriggerEnterChild,
                                                                                                   this
                                                                                                       .OnCollisionExitChild,
                                                                                                   this
                                                                                                       .OnTriggerExitChild,
                                                                                                   this
                                                                                                       .OnCollisionStayChild,
                                                                                                   this
                                                                                                       .OnTriggerStayChild);
    }

    void OnTriggerEnterChild(GameObject child_game_object, Collider other_game_object) {
      if (this._actor) {
        if (this._based_on_tags) {
          if (child_game_object.CompareTag(this._area.tag) && other_game_object.CompareTag(this._actor.tag)) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log("Actor is inside area");
            }
            #endif

            this._overlapping = ActorOverlapping.Inside_area_;
          }

          if (child_game_object.CompareTag(this._actor.tag) && other_game_object.CompareTag("Obstruction")) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log("Actor is colliding");
            }
            #endif

            this._colliding = ActorColliding.Colliding_;
          }
        } else {
          if (child_game_object == this._area.gameObject
              && other_game_object.gameObject == this._actor.gameObject) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log("Actor is inside area");
            }
            #endif

            this._overlapping = ActorOverlapping.Inside_area_;
          }

          if (child_game_object == this._actor.gameObject && other_game_object.CompareTag("Obstruction")) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log("Actor is colliding");
            }
            #endif

            this._colliding = ActorColliding.Colliding_;
          }
        }
      }
    }

    void OnTriggerStayChild(GameObject child_game_object, Collider other_game_object) {
      if (this._actor) {
        if (this._based_on_tags) {
          if (child_game_object.CompareTag(this._area.tag) && other_game_object.CompareTag(this._actor.tag)) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log("Actor is inside area");
            }
            #endif

            this._overlapping = ActorOverlapping.Inside_area_;
          }

          if (child_game_object.CompareTag(this._actor.tag) && other_game_object.CompareTag("Obstruction")) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log("Actor is colliding");
            }
            #endif

            this._colliding = ActorColliding.Colliding_;
          }
        } else {
          if (child_game_object == this._area.gameObject
              && other_game_object.gameObject == this._actor.gameObject) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log("Actor is inside area");
            }
            #endif

            this._overlapping = ActorOverlapping.Inside_area_;
          }

          if (child_game_object == this._actor.gameObject && other_game_object.CompareTag("Obstruction")) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log("Actor is colliding");
            }
            #endif

            this._colliding = ActorColliding.Colliding_;
          }
        }
      }
    }

    void OnTriggerExitChild(GameObject child_game_object, Collider other_game_object) {
      if (this._actor) {
        if (this._based_on_tags) {
          if (child_game_object.CompareTag(this._area.tag) && other_game_object.CompareTag(this._actor.tag)) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log("Actor is outside area");
            }
            #endif

            this._overlapping = ActorOverlapping.Outside_area_;
          }

          if (child_game_object.CompareTag(this._actor.tag) && other_game_object.CompareTag("Obstruction")) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log("Actor is not colliding");
            }
            #endif

            this._colliding = ActorColliding.Not_colliding_;
          }
        } else {
          if (child_game_object == this._area.gameObject
              && other_game_object.gameObject == this._actor.gameObject) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log("Actor is outside area");
            }
            #endif

            this._overlapping = ActorOverlapping.Outside_area_;
          }

          if (child_game_object == this._actor.gameObject && other_game_object.CompareTag("Obstruction")) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log("Actor is not colliding");
            }
            #endif

            this._colliding = ActorColliding.Not_colliding_;
          }
        }
      }
    }

    void OnCollisionEnterChild(GameObject child_game_object, Collision collision) { }

    void OnCollisionStayChild(GameObject child_game_object, Collision collision) { }

    void OnCollisionExitChild(GameObject child_game_object, Collision collision) { }
  }
}
