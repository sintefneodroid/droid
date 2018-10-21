using System;
using System.Text;
using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Utilities.BoundingBoxes;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Observers.Experimental {
  /// <inheritdoc cref="Observer" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      ObserverComponentMenuPath._ComponentMenuPath
      + "Experimental/ScreenSpaceBoundingBox"
      + ObserverComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [RequireComponent(typeof(BoundingBox))]
  public class ScreenSpaceBoundingBoxObserver : Observer, IHasString {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "BoundingBox"; } }
    BoundingBox _bounding_box;
    [SerializeField] UnityEngine.Camera _camera;
    Rect _scr_rect;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this._bounding_box = this.GetComponent<BoundingBox> ();
    }

    /// <summary>
    /// </summary>
    public override void UpdateObservation() {

      var points = new Vector3[8];
      var screen_pos = new Vector3[8];

      var b = this._bounding_box.Bounds; // reference object ex Simple
      points[0] = new Vector3( b.min.x, b.min.y, b.min.z );
      points[1] = new Vector3( b.max.x, b.min.y, b.min.z );
      points[2] = new Vector3( b.max.x, b.max.y, b.min.z );
      points[3] = new Vector3( b.min.x, b.max.y, b.min.z );
      points[4] = new Vector3( b.min.x, b.min.y, b.max.z );
      points[5] = new Vector3( b.max.x, b.min.y, b.max.z );
      points[6] = new Vector3( b.max.x, b.max.y, b.max.z );
      points[7] = new Vector3( b.min.x, b.max.y, b.max.z );

      var screen_bounds = new Bounds();
      for( var i = 0; i < 8; i++ ){
        screen_pos[i] = this._camera.WorldToScreenPoint( points[i] );

        if( i == 0 )
          screen_bounds = new Bounds( screen_pos[0], Vector3.zero);

        screen_bounds.Encapsulate( screen_pos[i] );
      }

      Debug.Log(screen_bounds.ToString());

      this._scr_rect.xMin = screen_bounds.min.x;
      this._scr_rect.yMin = screen_bounds.min.y;
      this._scr_rect.xMax = screen_bounds.max.x;
      this._scr_rect.yMax = screen_bounds.max.y;




      this.ObservationValue = this._bounding_box.BoundingBoxCoordinatesAsJson;
    }

    /// <summary>
    ///
    /// </summary>
    public String ObservationValue { get; set; }
  }
}
