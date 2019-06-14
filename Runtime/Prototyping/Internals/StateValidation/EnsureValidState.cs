﻿using droid.Runtime.Environments;
using droid.Runtime.Prototyping.Actors;
using droid.Runtime.Utilities.GameObjects.BoundingBoxes;
using droid.Runtime.Utilities.Misc.Extensions;
using UnityEngine;

namespace droid.Runtime.Prototyping.Internals.StateValidation {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class EnsureValidState : EnvironmentListener {
    [SerializeField] Actor _actor;

    [SerializeField] AbstractPrototypingEnvironment _environment;
    [SerializeField] Transform _goal;

    [SerializeField] Obstruction[] _obstructions;
    [SerializeField] bool _only_initial_state = true;

    [SerializeField] BoundingBox _playable_area;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "ValidityChecker"; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Clear() {
      if (!this._goal) {
        this._goal = FindObjectOfType<Transform>();
      }

      if (!this._actor) {
        this._actor = FindObjectOfType<Actor>();
      }

      if (!this._environment) {
        this._environment = FindObjectOfType<AbstractPrototypingEnvironment>();
      }

      if (this._obstructions.Length <= 0) {
        this._obstructions = FindObjectsOfType<Obstruction>();
      }

      if (!this._playable_area) {
        this._playable_area = FindObjectOfType<BoundingBox>();
      }
    }

    /// <summary>
    /// </summary>
    void ValidateState() {
      if (this._only_initial_state && this._environment.CurrentFrameNumber != 0) {
        return;
      }

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

    public override void EnvironmentReset() { }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreStep() { this.ValidateState(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Step() { this.ValidateState(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PostStep() { this.ValidateState(); }
  }
}
