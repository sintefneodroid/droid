using droid.Neodroid.Utilities.Unsorted;
using UnityEngine;

namespace droid.Neodroid.Prototyping.Evaluation {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      EvaluationComponentMenuPath._ComponentMenuPath
      + "PunishmentFunction"
      + EvaluationComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Rigidbody))]
  public class PunishmentFunction : ObjectiveFunction {
    [SerializeField] int _hits;

    //[SerializeField] LayerMask _layer_mask;

    [SerializeField] GameObject _player;
    [SerializeField] string _avoid_tag = "balls";

    // Use this for initialization
    /// <summary>
    /// 
    /// </summary>
    protected override void PostSetup() {
      this.ResetHits();

      var tagged_gos = GameObject.FindGameObjectsWithTag(this._avoid_tag);

      foreach (var ball in tagged_gos) {
        if (ball) {
          var publisher = ball.AddComponent<ChildCollisionPublisher>();
          publisher.CollisionDelegate = this.OnChildCollision;
        }
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

    public override void InternalReset() { this.ResetHits(); }
    
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override float InternalEvaluate() { return this._hits * -1f; }
  }
}
