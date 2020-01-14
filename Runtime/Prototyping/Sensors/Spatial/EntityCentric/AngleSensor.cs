using droid.Runtime.Prototyping.Sensors.Experimental;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.Transform {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath + "Angle" + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  public class AngleSensor : SingleValueSensor {
    [SerializeField] Vector3 reference = Vector3.up;
    [SerializeField] Vector3 axis = -Vector3.forward;

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <exception cref="T:System.ArgumentOutOfRangeException"></exception>
    public override void UpdateObservation() {
      float val;
      if (false) {
        val = Vector3.Dot(this.transform.TransformVector(vector : this.reference), rhs : this.reference);
      } else {
        var t = this.transform.TransformDirection(direction : this.reference);
        //var axis = Vector3.Cross(t, this.reference);
        val = Vector3.SignedAngle(@from : t, to : this.reference, axis : this.axis);
      }

      this.ObservationValue = val;
    }
  }
}
