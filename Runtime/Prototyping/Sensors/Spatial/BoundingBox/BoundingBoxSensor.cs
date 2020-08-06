using System;
using System.Collections.Generic;
using droid.Runtime.GameObjects.BoundingBoxes;
using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.BoundingBox {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                               + "Experimental/BoundingBox"
                               + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [RequireComponent(requiredComponent : typeof(NeodroidBoundingBox))]
  public class BoundingBoxSensor : Sensor,
                                   IHasString {
    NeodroidBoundingBox _neodroid_bounding_box;
    [SerializeField] string _observationValue;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._neodroid_bounding_box = this.GetComponent<NeodroidBoundingBox>();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override IEnumerable<float> FloatEnumerable { get { return null; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      this.ObservationValue = this._neodroid_bounding_box.BoundingBoxCoordinatesWorldSpaceAsJson;
    }

    /// <summary>
    ///
    /// </summary>
    public string ObservationValue {
      get { return this._observationValue; }
      set { this._observationValue = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() { return this.ObservationValue; }
  }
}
