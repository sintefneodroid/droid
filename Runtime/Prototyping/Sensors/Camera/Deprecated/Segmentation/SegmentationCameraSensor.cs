﻿using System.Linq;
using droid.Runtime.Utilities.GameObjects.NeodroidCamera.Segmentation;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Camera.Deprecated.Segmentation {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "SegmentationCamera"
                    + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [RequireComponent(typeof(UnityEngine.Camera), typeof(Segmenter))]
  public class SegmentationCameraSensor : StringAugmentedCameraSensor {
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
      this.serialisedString = this._segmenter != null
                                  ? this._segmenter.ColorsDict.Select(c => $"{c.Key}: {c.Value.ToString()}")
                                        .Aggregate("", (current, next) => $"{current}, {next}")
                                  : "Nothing";
    }
  }
}
