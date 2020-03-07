using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace droid.Runtime.GameObjects.BoundingBoxes {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [RequireComponent(requiredComponent : typeof(Camera))]
  [ExecuteInEditMode]
  public class DrawScreenSpaceBoundingBox : MonoBehaviour {
    List<string> _names = new List<string>();
    List<Rect> _rects = new List<Rect>();
    Camera _camera = null;

    [SerializeField] bool _draw_label = true;
    [SerializeField] NeodroidBoundingBox[] bounding_boxes = null;
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

      this.bounding_boxes = FindObjectsOfType<NeodroidBoundingBox>();
    }

    void Compute() {
      this._rects.Clear();
      this._names.Clear();

      if (!this._cache_bounding_boxes) {
        this.bounding_boxes = FindObjectsOfType<NeodroidBoundingBox>();
      }

      for (var index = 0; index < this.bounding_boxes.Length; index++) {
        var bb = this.bounding_boxes[index];
        if (this._camera.WorldToScreenPoint(position : bb.Bounds.center).z < 0) {
          return;
        }

        var a = bb.ScreenSpaceBoundingRect(a_camera : this._camera);

        this._rects.Add(item : a);
        this._names.Add(item : bb.name);
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
      for (var index = 0; index < this._rects.Count; index++) {
        var rect = this._rects[index : index];
        var text = "";
        if (this._draw_label) {
          text += $"{this._names[index : i]}";
        }

        if (this._draw_coords) {
          text += $"\n{rect}\n{rect.center}";
        }

        var a = rect;
        a.y = Screen.height - (a.y + a.height);

        GUI.Box(position : a, text : text);

        i++;
      }
    }
  }
}
