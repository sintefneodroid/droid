using System.Linq;
using droid.Runtime.Utilities.Enums;
using droid.Runtime.Utilities.NeodroidCamera.Segmentation;
using droid.Runtime.Utilities.NeodroidCamera.Segmentation.Obsolete;
using UnityEngine;

namespace droid.Runtime.Prototyping.Observers.Camera.Segmentation {
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
    Segmenter _segmenter=null;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      base.UpdateObservation();


      Debug.LogWarning(JsonUtility.ToJson(this._segmenter.ColorsDict));
      this.serialisedString = this._segmenter != null
                                    ? this._segmenter.ColorsDict.Select(c => $"{c.Key}: {c.Value.ToString()}")
                                        .Aggregate("", (current, next) => $"{current}, {next}")
                                    : "Nothing";
    }
  }
}
