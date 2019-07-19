using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace droid.Runtime.GameObjects.BoundingBoxes {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [RequireComponent(typeof(Camera))]
  [ExecuteInEditMode]
  public class DrawScreenSpaceBoundingBox : MonoBehaviour {
    List<string> _names = new List<string>();
    List<Rect> _rects = new List<Rect>();
    Camera _camera = null;

    [SerializeField] bool _draw_label = true;
    [SerializeField] BoundingBox[] bounding_boxes = null;
    [SerializeField] bool _cache_bounding_boxes = false;
    [SerializeField] GUISkin gui_style = null;
    [SerializeField] bool _draw_coords = false;

    void Awake() {
      if (!this._camera) {
        this._camera = this.GetComponent<Camera>();
      }

      if (!this.gui_style) {
        this.gui_style = Resources.FindObjectsOfTypeAll<GUISkin>().First(a => a.name == "BoundingBox");
      }

      this.bounding_boxes = FindObjectsOfType<BoundingBox>();
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

    void Draw() {
      this.Compute();
      var i = 0;
      foreach (var rect in this._rects) {
        var text = "";
        if (this._draw_label) {
          text += $"{this._names[i]}";
        }

        if (this._draw_coords) {
          text += $"\n{rect}\n{rect.center}";
        }

        var a = rect;
        a.y = Screen.height - (a.y + a.height);

        GUI.Box(a, text);

        i++;
      }
    }
  }
}
