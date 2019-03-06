using System;
using System.Collections.Generic;
using System.Linq;
using droid.Runtime.Utilities.NeodroidCamera.Segmentation;
using droid.Runtime.Utilities.NeodroidCamera.Segmentation.Obsolete;
using UnityEngine;

namespace droid.Runtime.Prototyping.Observers.Camera.Segmentation {
  /// <inheritdoc cref="Observer" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      ObserverComponentMenuPath._ComponentMenuPath + "Segmentation" + ObserverComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [RequireComponent(typeof(Segmenter))]
  public class SegmentationObserver : StringObserver {
    /// <summary>
    /// </summary>
    [SerializeField]
    Segmenter _segmenter;

    public override IEnumerable<float> FloatEnumerable { get {return new List<float>();} }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      this.ObservationValue = this._segmenter != null
                                  ? this._segmenter.ColorsDict.Select(c => $"{c.Key}: {c.Value.ToString()}")
                                      .Aggregate(
                                          "",
                                          (current, next) => current != "" ? $"{current}, {next}" : $"{next}")
                                  : "Nothing";
      //TODO:ADD this Type(COLOR) and ColorDict as serialisation option instead of a string
/*      if (this._segmenter != null) {
        this.ObservationValue += $", Outline: {this._segmenter.OutlineColor.ToString()}";
      }
      */
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override String ToString() { return this.ObservationValue; }
  }
}
