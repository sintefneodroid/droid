using droid.Runtime.Utilities.Misc.Extensions;
using droid.Runtime.Utilities.Sensors;
using UnityEngine;

namespace droid.Runtime.Prototyping.Evaluation {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      EvaluationComponentMenuPath._ComponentMenuPath
      + "PunishmentFunction"
      + EvaluationComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Rigidbody))]
  public class PunishmentFunction : ObjectiveFunction {
    [SerializeField] string _avoid_tag = "balls";
    [SerializeField] int _hits;

    //[SerializeField] LayerMask _layer_mask;

    [SerializeField] GameObject _player;

    // Use this for initialization
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PostSetup() {
      this.ResetHits();

      var tagged_gos = GameObject.FindGameObjectsWithTag(this._avoid_tag);

      foreach (var ball in tagged_gos) {
        if (ball)
        {
          var publisher = ball.GetComponent<ChildCollider3DSensor>();
          if(!publisher || publisher.Caller != this)
          {
            publisher = ball.AddComponent<ChildCollider3DSensor>();
          }
          publisher.Caller = this;
          publisher.OnCollisionEnterDelegate = this.OnChildCollision;
        }
      }
    }

    void OnChildCollision(GameObject childSensorGameObject, Collision collision) {
      if (collision.collider.name == this._player.name) {
        this._hits += 1;
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log(this._hits);
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
