using Neodroid.Utilities.BoundingBoxes;
using UnityEngine;

namespace Neodroid.Prototyping.Observers.Experimental {
  /// <summary>
  /// 
  /// </summary>
  [AddComponentMenu(
      ObserverComponentMenuPath._ComponentMenuPath
      + "Experimental/BoundingBox"
      + ObserverComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [RequireComponent(typeof(BoundingBox))]
  public class BoundingBoxObserver : Observer {
    /// <summary>
    /// 
    /// </summary>
    public override string PrototypingType { get { return "BoundingBox"; } }
    //BoundingBox _bounding_box;

    /// <summary>
    /// 
    /// </summary>
    protected override void Setup() {
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
