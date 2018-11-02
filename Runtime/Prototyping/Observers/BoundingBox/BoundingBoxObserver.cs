using System;
using Neodroid.Runtime.Interfaces;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Observers.BoundingBox {
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
    public override string PrototypingTypeName { get { return "BoundingBox"; } }

    Utilities.BoundingBoxes.BoundingBox _bounding_box;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this._bounding_box = this.GetComponent<Utilities.BoundingBoxes.BoundingBox>();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      this.ObservationValue = this._bounding_box.BoundingBoxCoordinatesAsJson;
    }

    /// <summary>
    ///
    /// </summary>
    public String ObservationValue { get; set; }
  }
}