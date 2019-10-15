using UnityEngine;

namespace droid.Runtime.Prototyping.ObjectiveFunctions.Spatial {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class VelocityObjective : SpatialObjective {
    /// <summary>
    /// </summary>
    [SerializeField]
    Rigidbody _rigidbody = null;

    /// <summary>
    /// </summary>
    public override void InternalReset() { }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override float InternalEvaluate() {
      var vel_mag = this._rigidbody.velocity.magnitude;

      this.IsOutsideBound();

      return vel_mag;
    }

    /// <summary>
    /// </summary>
    void IsOutsideBound() {
      if (this.ParentEnvironment.PlayableArea && this._rigidbody) {
        var env_bounds = this.ParentEnvironment.PlayableArea.Bounds;
        var rb_bounds = this._rigidbody.GetComponent<Collider>().bounds;
        var intersects = env_bounds.Intersects(rb_bounds);

        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"{this.ParentEnvironment.Identifier} - {env_bounds}");
          Debug.Log($"{this._rigidbody.name} - {rb_bounds}");
          Debug.Log($"Is intersecting - {intersects}");
        }
        #endif

        if (!intersects) {
          this.ParentEnvironment.Terminate("Actor is outside playable area");
        }
      }
    }
  }
}
