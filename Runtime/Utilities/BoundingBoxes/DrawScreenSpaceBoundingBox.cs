using System;
using System.Collections.Generic;
using droid.Runtime.Utilities.BoundingBoxes.Experimental;
using UnityEngine;
using Object = System.Object;

namespace droid.Runtime.Utilities.BoundingBoxes {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [RequireComponent(typeof(Camera))]
  [ExecuteInEditMode]
  public class DrawScreenSpaceBoundingBox : MonoBehaviour {
    List<string> _names = new List<string>();

    List<Rect> _rects = new List<Rect>();
    Camera _camera;
    [SerializeField] bool _draw_label = true;
    List<Vector3> _screen_pos = new List<Vector3>();
    [SerializeField] BoundingBox[] bounding_boxes;
    bool _cache_bounding_boxes;
    [SerializeField] GUISkin gui_style;
    [SerializeField] bool _draw_coords;

    void Awake() {
      if (!this._camera) {
        this._camera = this.GetComponent<Camera>();
      }
    }

    void Start() { this.bounding_boxes = FindObjectsOfType<BoundingBox>(); }

    /// <summary>
    /// </summary>
    /// <param name="new_points"></param>
    /// <param name="game_object"></param>
    public void AddBoundingBox(BoundingBox new_points, string name) {
      if (new_points == null) {
        return;
      }

      if (this._camera.WorldToScreenPoint(new_points.Bounds.center).z < 0) {
        return;
      }

      /*
      new_points.EncapsulatePoints(this._camera);

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

      //var scr_rect = new_points.GetBoundsScreenRect(this._camera);
      //scr_rect.center = this._camera.WorldToScreenPoint(go.transform.TransformPoint(new_points.center));
      //scr_rect.y = Screen.height - scr_rect.y;

      this._rects.Add(new_points.ScreenSpaceBoundingRect(this._camera));
      this._names.Add(name);
    }

    void OnPreRender() {
      //      this.Compute();
    }

    void Compute() {
      this._rects.Clear();
      this._names.Clear();

      if (!this._cache_bounding_boxes) {
        this.bounding_boxes = FindObjectsOfType<BoundingBox>();
      }

      foreach (var bb in this.bounding_boxes) {
        if (this._camera.WorldToScreenPoint(bb.Bounds.center).z < 0) {
          return;
        }

        var a = bb.ScreenSpaceBoundingRect(this._camera);
        this._rects.Add(a);
        this._names.Add(bb.name);
      }
    }

    void OnGUI() {
      if (this.gui_style) {
        GUI.skin = this.gui_style;
      }

      this.Draw();
    }

    void LateUpdate() {
      //this.Draw();
    }

    void Draw() {
      this.Compute();
      var i = 0;
      foreach (var rect in this._rects) {
        var text = $"";
        if (this._draw_label) {
          text += $"{this._names[i]}";
        }

        if (this._draw_coords) {
          text += $"\n{rect}\n{rect.center}";
        }

        var a = rect;
        a.y = Screen.height - (a.y+a.height);

        GUI.Box(a, text);

        i++;
      }
    }
  }
}
