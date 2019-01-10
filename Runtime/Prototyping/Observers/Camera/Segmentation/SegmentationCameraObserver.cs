using System.Linq;
using Neodroid.Runtime.Utilities.Enums;
using Neodroid.Runtime.Utilities.NeodroidCamera.Segmentation;
using Neodroid.Runtime.Utilities.ScriptableObjects;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Observers.Camera.Segmentation {
  /// <inheritdoc cref="Observer" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      ObserverComponentMenuPath._ComponentMenuPath
      + "SegmentationCamera"
      + ObserverComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [RequireComponent(typeof(UnityEngine.Camera), typeof(Segmenter))]
  public class SegmentationCameraObserver : StringAugmentedCameraObserver {
    /// <summary>
    /// </summary>
    [SerializeField]
    Segmenter _segmenter;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      this._Grab = true;
      if (this._Manager?.SimulatorConfiguration?.SimulationType != SimulationType.Frame_dependent_) {
        this._Camera.Render();
        this.UpdateBytes();
      }

      this._Serialised_String = this._segmenter != null
                                    ? this._segmenter.ColorsDict.Select(c => $"{c.Key}: {c.Value.ToString()}")
                                        .Aggregate("", (current, next) => $"{current}, {next}")
                                    : "Nothing";
    }
  }
}