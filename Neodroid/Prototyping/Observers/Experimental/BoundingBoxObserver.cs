using droid.Neodroid.Utilities.BoundingBoxes;
using UnityEngine;

namespace droid.Neodroid.Prototyping.Observers.Experimental {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      ObserverComponentMenuPath._ComponentMenuPath
      + "Experimental/BoundingBox"
      + ObserverComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [RequireComponent(typeof(BoundingBox))]
  public class BoundingBoxObserver : Observer {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingType { get { return "BoundingBox"; } }
    //BoundingBox _bounding_box;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      //_bounding_box = this.GetComponent<BoundingBox> ();
    }

    /// <summary>
    /// 
    /// </summary>
    public override void UpdateObservation() {
      //Data = Encoding.ASCII.GetBytes (_bounding_box.BoundingBoxCoordinatesAsJSON);
    }
  }
}
