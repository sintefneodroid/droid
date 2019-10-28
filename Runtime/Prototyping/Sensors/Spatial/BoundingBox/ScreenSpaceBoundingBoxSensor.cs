using System.Collections.Generic;
using droid.Runtime.GameObjects.BoundingBoxes.Experimental;
using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.BoundingBox {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "Experimental/ScreenSpaceBoundingBox"
                    + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  //[ExecuteAlways]
  [RequireComponent(typeof(GameObjects.BoundingBoxes.BoundingBox))]
  public class ScreenSpaceBoundingBoxSensor : Sensor,
                                              IHasString {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "BoundingBox"; } }

    GameObjects.BoundingBoxes.BoundingBox _bounding_box = null;
    [SerializeField] Camera _camera = null;
    [SerializeField] Rect _out_rect = new Rect();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._bounding_box = this.GetComponent<GameObjects.BoundingBoxes.BoundingBox>();
    }

    [SerializeField] bool NormaliseObservation = true;
    public override IEnumerable<float> FloatEnumerable { get { return new List<float>(); } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      if (this._camera) {
        var rect = this._bounding_box.ScreenSpaceBoundingRect(this._camera);

        if (this.NormaliseObservation) {
          float w;
          float h;
          var target = this._camera.targetTexture;

          if (target) {
            w = target.width;
            h = target.height;
          } else {
            var r = this._camera.pixelRect;
            w = r.width;
            h = r.height;
          }

          this._out_rect = rect.Normalise(w, h);
        } else {
          this._out_rect = rect;
        }
      }

      var str_rep =
          $"{{\"x\":{this._out_rect.x},\n\"y\":{this._out_rect.y},\n\"w\":{this._out_rect.width},\n\"h\":{this._out_rect.height}}}";

      this.ObservationValue = str_rep;
    }

    /// <summary>
    ///
    /// </summary>
    public string ObservationValue { get; set; }

    public override string ToString() { return this.ObservationValue; }
  }
}
