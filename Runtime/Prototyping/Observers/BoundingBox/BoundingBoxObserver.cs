using System;
using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Prototyping.Observers.BoundingBox {
  /// <inheritdoc cref="Observer" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      ObserverComponentMenuPath._ComponentMenuPath
      + "Experimental/BoundingBox"
      + ObserverComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [RequireComponent(typeof(Utilities.BoundingBoxes.BoundingBox))]
  public class BoundingBoxObserver : Observer,
                                     IHasString {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName => "BoundingBox";

    Utilities.BoundingBoxes.BoundingBox _boundingBox;
    [SerializeField] string _observationValue;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this._boundingBox = this.GetComponent<Utilities.BoundingBoxes.BoundingBox>();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      this.ObservationValue = this._boundingBox.BoundingBoxCoordinatesWorldSpaceAsJson;

      this.FloatEnumerable = new float[] { };
    }

    /// <summary>
    ///
    /// </summary>
    public string ObservationValue
    {
      get => this._observationValue;
      set => this._observationValue = value;
    }

    public override string ToString()
    {
      return this.ObservationValue;
    }
  }
}
