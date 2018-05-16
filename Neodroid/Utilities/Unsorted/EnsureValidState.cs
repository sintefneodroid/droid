using Neodroid.Environments;
using Neodroid.Prototyping.Actors;
using Neodroid.Utilities.BoundingBoxes;
using UnityEngine;

namespace Neodroid.Utilities.Unsorted {
  /// <summary>
  ///
  /// </summary>
  public class EnsureValidState : Prototyping.Internals.EnvironmentListener {
    [SerializeField] Actor _actor;

    [SerializeField] PrototypingEnvironment _environment;
    [SerializeField] Transform _goal;

    [SerializeField] Obstruction[] _obstructions;

    [SerializeField] BoundingBox _playable_area;

    public PrototypingEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    protected override void Clear() {
      if (!this._goal) {
        this._goal = FindObjectOfType<Transform>();
      }

      if (!this._actor) {
        this._actor = FindObjectOfType<Actor>();
      }

      if (!this._environment) {
        this._environment = FindObjectOfType<PrototypingEnvironment>();

      }

      if (this._obstructions.Length <= 0) {
        this._obstructions = FindObjectsOfType<Obstruction>();
      }

      if (!this._playable_area) {
        this._playable_area = FindObjectOfType<BoundingBox>();
      }
    }

    /// <summary>
    ///
    /// </summary>
    void ValidateState() {
      if (this._playable_area != null && !this._playable_area.Bounds.Intersects(this._actor.ActorBounds)) {
        this._environment.Terminate("Actor outside playable area");
      }

      if (this._playable_area != null
          && !this._playable_area.Bounds.Intersects(this._goal.GetComponent<Collider>().bounds)) {
        this._environment.Terminate("Goal outside playable area");
      }

      foreach (var obstruction in this._obstructions) {
        if (obstruction != null
            && obstruction.GetComponent<Collider>().bounds.Intersects(this._actor.ActorBounds)) {
          this._environment.Terminate("Actor overlapping obstruction");
        }

        if (obstruction != null
            && obstruction.GetComponent<Collider>().bounds
                .Intersects(this._goal.GetComponent<Collider>().bounds)) {
          this._environment.Terminate("Goal overlapping obstruction");
        }
      }
    }

    protected override void PreStep() { this.ValidateState();  }
    protected override void Step() { this.ValidateState(); }
    protected override void PostStep() { this.ValidateState(); }
    public override string PrototypingType { get { return "ValidityChecker"; } }
  }
}
