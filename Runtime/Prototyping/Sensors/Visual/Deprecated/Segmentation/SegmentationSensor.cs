using System;
using System.Collections.Generic;
using System.Linq;
using droid.Runtime.GameObjects.NeodroidCamera.Segmentation;
using droid.Runtime.Prototyping.Sensors.Strings;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Visual.Deprecated.Segmentation {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "Segmentation"
                    + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [RequireComponent(typeof(Segmenter))]
  public class SegmentationSensor : StringSensor {
    /// <summary>
    /// </summary>
    [SerializeField]
    Segmenter _segmenter = null;

    public override IEnumerable<float> FloatEnumerable { get { return new List<float>(); } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      this.ObservationValue = this._segmenter != null
                                  ? this._segmenter.ColorsDict.Select(c => $"{c.Key}: {c.Value.ToString()}")
                                        .Aggregate("",
                                                   (current, next) =>
                                                       current != "" ? $"{current}, {next}" : $"{next}")
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
