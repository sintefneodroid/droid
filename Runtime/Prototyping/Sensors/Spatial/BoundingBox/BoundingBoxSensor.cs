using System.Collections.Generic;
using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.BoundingBox {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "Experimental/BoundingBox"
                    + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [RequireComponent(typeof(GameObjects.BoundingBoxes.BoundingBox))]
  public class BoundingBoxSensor : Sensor,
                                   IHasString {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "BoundingBox"; } }

    GameObjects.BoundingBoxes.BoundingBox _bounding_box;
    [SerializeField] string _observationValue;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._bounding_box = this.GetComponent<GameObjects.BoundingBoxes.BoundingBox>();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override IEnumerable<float> FloatEnumerable { get { return new List<float>(); } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      this.ObservationValue = this._bounding_box.BoundingBoxCoordinatesWorldSpaceAsJson;
    }

    /// <summary>
    ///
    /// </summary>
    public string ObservationValue {
      get { return this._observationValue; }
      set { this._observationValue = value; }
    }

    public override string ToString() { return this.ObservationValue; }
  }
}
