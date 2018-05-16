using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Prototyping.Evaluation {
  [AddComponentMenu(
      EvaluationComponentMenuPath._ComponentMenuPath
      + "PunishmentFunction"
      + EvaluationComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Rigidbody))]
  public class PunishmentFunction : ObjectiveFunction {
    [SerializeField] int _hits;

    //[SerializeField] LayerMask _layer_mask;

    [SerializeField] GameObject _player;

    // Use this for initialization
    protected override void Setup() {
      this.ResetHits();
      var balls = GameObject.FindGameObjectsWithTag("balls");

      foreach (var ball in balls) {
        ball.AddComponent<Utilities.Unsorted.ChildCollisionPublisher>().CollisionDelegate = this.OnChildCollision;
      }
    }

    void OnChildCollision(Collision collision) {
      if (collision.collider.name == this._player.name) {
        this._hits += 1;
      }

      if (true) {
        Debug.Log(this._hits);
      }
    }

    void ResetHits() { this._hits = 0; }

    public override float InternalEvaluate() { return this._hits * -1f; }
  }
}
