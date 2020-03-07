using droid.Runtime.GameObjects.BoundingBoxes;
using droid.Runtime.GameObjects.ChildSensors;
using droid.Runtime.Utilities.Extensions;
using UnityEngine;

namespace droid.Runtime.Prototyping.ObjectiveFunctions.Spatial {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(menuName : EvaluationComponentMenuPath._ComponentMenuPath
                               + "ConstantSpatial"
                               + EvaluationComponentMenuPath._Postfix)]
  public class ConstantSpatialObjective : SpatialObjective {
    public override void InternalReset() { }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override float InternalEvaluate() {
      var signal = this.DefaultSignal;

      return signal;
    }
  }
}
