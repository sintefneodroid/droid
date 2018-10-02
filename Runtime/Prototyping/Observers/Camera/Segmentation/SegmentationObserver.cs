using System;
using System.Linq;
using Neodroid.Runtime.Utilities.NeodroidCamera.Segmentation;
using Newtonsoft.Json;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Observers.Camera.Segmentation {
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

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      this.ObservationValue = this._segmenter != null
                                  ? this._segmenter.ColorsDict.Select(c => $"{c.Key}: {c.Value.ToString()}")
                                      .Aggregate("", (current, next) =>  current!="" ? $"{current}, {next}" : $"{next}")
                                  : "Nothing";
      //TODO:ADD this Type(COLOR) and ColorDict as serialisation option instead of a string
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override String ToString() { return this.ObservationValue; }
  }
}
