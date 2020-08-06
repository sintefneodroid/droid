using droid.Runtime.GameObjects.ChildSensors;
using UnityEngine;
using Object = System.Object;

namespace droid.Runtime.Prototyping.ObjectiveFunctions.Spatial {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(menuName : EvaluationComponentMenuPath._ComponentMenuPath
                               + "PunishmentFunction"
                               + EvaluationComponentMenuPath._Postfix)]
  [RequireComponent(requiredComponent : typeof(Rigidbody))]
  public class AvoidanceObjective : SpatialObjective {
    [SerializeField] string _avoid_tag = "balls";
    [SerializeField] int _hits = 0;

    //[SerializeField] LayerMask _layer_mask;

    [SerializeField] GameObject _player = null;
    [SerializeField] GameObject[] tagged_gos;

    // Use this for initialization
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void RemotePostSetup() {
      this.ResetHits();

      tagged_gos = GameObject.FindGameObjectsWithTag(tag : this._avoid_tag);

      foreach (var ball in tagged_gos) {
        if (ball) {
          var publisher = ball.GetComponent<ChildCollider3DSensor>();
          if (!publisher || publisher.Caller != this) {
            publisher = ball.AddComponent<ChildCollider3DSensor>();
          }

          publisher.Caller = this;
          publisher.OnCollisionEnterDelegate = this.OnChildCollision;
        }
      }
    }

    void OnDrawGizmosSelected() {
      var player_pos = this._player.transform.position;
      foreach (var o in this.tagged_gos) {
        Debug.DrawLine(start : player_pos , end : o.transform.position);
      }

    }
    
    void OnChildCollision(GameObject child_sensor_game_object, Collision collision) {
      if (collision.collider.name == this._player.name) {
        this._hits += 1;
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log(message : this._hits);
      }
      #endif
    }

    void ResetHits() { this._hits = 0; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void InternalReset() { this.ResetHits(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override float InternalEvaluate() { return this._hits * -1f; }
  }
}
