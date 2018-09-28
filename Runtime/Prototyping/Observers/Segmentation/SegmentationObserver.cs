using System;
using System.Linq;
using Neodroid.Runtime.Utilities.Segmentation;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Observers.Segmentation {
  /// <inheritdoc cref="Observer" />
  ///  <summary>
  ///  </summary>
  [AddComponentMenu(
       ObserverComponentMenuPath._ComponentMenuPath
       + "Segmentation"
       + ObserverComponentMenuPath._Postfix), ExecuteInEditMode, RequireComponent(typeof(Segmenter))]
  public class SegmentationObserver : StringObserver {
    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    Segmenter _segmenter;

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override void UpdateObservation() {
      this.ObservationValue = this._segmenter != null
                                    ? this._segmenter.ColorsDict.Select(c => $"{c.Key}: {c.Value.ToString()}")
                                        .Aggregate("", (current, next) => $"{current}, {next}")
                                    : "Nothing";
    }

    public override String ToString() { return this.ObservationValue; }
  }
}
