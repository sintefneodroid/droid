using System.Linq;
using droid.Runtime.GameObjects.NeodroidCamera.Segmentation;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Visual.Deprecated.Segmentation {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "SegmentationCamera"
                    + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [RequireComponent(typeof(Camera), typeof(Segmenter))]
  public class SegmentationEncodedCameraSensor : StringAugmentedEncodedCameraSensor {
    /// <summary>
    /// </summary>
    [SerializeField]
    Segmenter _segmenter = null;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      base.UpdateObservation();

      Debug.LogWarning(JsonUtility.ToJson(this._segmenter.ColorsDict));
      this.serialised_string = this._segmenter != null
                                   ? this._segmenter.ColorsDict.Select(c => $"{c.Key}: {c.Value.ToString()}")
                                         .Aggregate("", (current, next) => $"{current}, {next}")
                                   : "Nothing";
    }
  }
}
