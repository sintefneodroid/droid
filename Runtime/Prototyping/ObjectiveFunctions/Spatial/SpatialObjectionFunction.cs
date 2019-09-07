using droid.Runtime.GameObjects.BoundingBoxes;
using UnityEngine;

namespace droid.Runtime.Prototyping.Evaluation.Spatial {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public abstract class SpatialObjectionFunction : ObjectiveFunction {
    /// <summary>
    /// </summary>
    [SerializeField]
    protected Transform[] terminatingTransforms;

    // TODO: Look at how to simplify a way to describe which objects should be in this list
    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    protected BoundingBox boundingBox;

    /// <summary>
    ///
    /// </summary>
    protected override void PostSetup() {
      base.PostSetup();

      if (this.boundingBox == null) {
        if (this.ParentEnvironment.PlayableArea) {
          this.boundingBox = this.ParentEnvironment.PlayableArea;
        } else {
          this.boundingBox = this.gameObject.GetComponent<BoundingBox>();
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override float Evaluate() {
      var signal = 0.0f;
      signal += this.InternalEvaluate();

      if (this.EpisodeLength > 0 && this._environment.CurrentFrameNumber >= this.EpisodeLength) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Maximum episode length reached, Length {this._environment.CurrentFrameNumber}");
        }
        #endif

        signal = this._failed_reward;

        this._environment.Terminate("Maximum episode length reached");
      }

      if (this.boundingBox) {
        foreach (var t in this.terminatingTransforms) {
          if (!this.boundingBox.Bounds.Contains(t.position)) {
            signal = this._failed_reward;

            #if NEODROID_DEBUG
            if (this.Debugging) {
              Debug.Log($"The transform {t} outside bounds, terminating {this._environment}");
            }
            #endif

            this._environment
                .Terminate($"The transform {t} is not inside {this.boundingBox.gameObject} bounds");
          }
        }
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log(signal);
      }
      #endif

      this._last_signal = signal;

      this._Episode_Return += signal;

      return signal;
    }

    /// <summary>
    /// </summary>
    public abstract override void InternalReset();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public abstract override float InternalEvaluate();
  }
}
