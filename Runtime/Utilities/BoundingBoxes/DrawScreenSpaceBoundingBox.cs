using System;
using System.Collections.Generic;
using Neodroid.Runtime.Utilities.BoundingBoxes.Experimental;
using UnityEngine;
using Object = System.Object;

namespace Neodroid.Runtime.Utilities.BoundingBoxes {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [RequireComponent(typeof(Camera))]
  [ExecuteInEditMode]
  public class DrawScreenSpaceBoundingBox : MonoBehaviour {
    List<GameObject> _names = new List<GameObject>();

    List<Rect> _rects = new List<Rect>();
    Camera _camera;
    [SerializeField] bool _draw_label = true;
    List<Vector3> _screen_pos = new List<Vector3>();

    void Awake() { this._camera = this.GetComponent<Camera>(); }

    /// <summary>
    /// </summary>
    /// <param name="new_points"></param>
    /// <param name="new_color"></param>
    /// <param name="game_object"></param>
    public void AddBoundingBox(Bounds new_points, GameObject game_object) {
      if (new_points == null) {
        return;
      }

      var scr_rect = new Rect();
      /*
      var le = new_points.Length;
      var screen_pos = new Vector3[le];

      var screen_bounds = new Bounds();
      for (var i = 0; i < le; i++) {
        screen_pos[i] = this._camera.WorldToScreenPoint(new_points[i]);

        if (i == 0) {
          screen_bounds = new Bounds(screen_pos[0], Vector3.zero);
        } else {
          screen_bounds.Encapsulate(screen_pos[i]);
        }
      }


      var a = this._camera.WorldToScreenPoint(game_object.transform.position);

      scr_rect.xMin = screen_bounds.min.x;
      scr_rect.yMin = screen_bounds.min.y;

      scr_rect.xMax = screen_bounds.max.x;
      scr_rect.yMax = screen_bounds.max.y;
      this._screen_pos.Add(a);
      */

      scr_rect =   new_points.GetBoundsScreenRect(this._camera);

      this._rects.Add(scr_rect);
      this._names.Add((game_object));
    }

    void OnPreRender() {
      this._rects.Clear();
      this._names.Clear();

      var bounding_boxes = FindObjectsOfType<BoundingBox>();
      foreach (var bb in bounding_boxes) {
        this.AddBoundingBox(bb.Bounds, bb.gameObject);
      }
    }

    void OnGUI() {
      if (this._draw_label) {
        var i = 0;
        foreach (var rect in this._rects) {
          var text = this._names[i].name;

          //var x = this._screen_pos[i].x;

          var aa = rect;
          //aa.x = Screen.width - x;
          //aa.y = Screen.height - rect.y;

          GUI.Box(aa, text);

          i++;
        }
      }
    }
  }
}
