using System.Collections.Generic;
using UnityEngine;

namespace droid.Runtime.GameObjects.BoundingBoxes {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [RequireComponent(typeof(Camera))]
  [ExecuteInEditMode]
  public class Draw3DBoundingBox : MonoBehaviour {
    List<Color> _colors = new List<Color>();
    [SerializeField] Material _line_material;
    List<GameObject> _names = new List<GameObject>();

    List<Vector3[,]> _outlines = new List<Vector3[,]>();

    //List<Vector3[,]> _triangles = new List<Vector3[,]>();
    //[SerializeField] GUISkin _gui_skin;

    Camera _camera;
    [SerializeField] bool _draw_label = true;
    BoundingBox[] _bounding_boxes;
    [SerializeField] bool _cacheBoundingBoxes = true;

    void Awake() {
      if (!this._line_material) {
        var shader = Shader.Find("Unlit/Color");
        this._line_material = new Material(shader);
      }

      //GUI.skin = this._gui_skin;

      this._camera = this.GetComponent<Camera>();
    }

    void OnPostRender() {
      if (this._outlines == null) {
        return;
      }

      if (this._line_material) {
        this._line_material.SetPass(0);
      }

      GL.Begin(GL.LINES);
      for (var j = 0; j < this._outlines.Count; j++) {
        GL.Color(this._colors[j]);
        for (var i = 0; i < this._outlines[j].GetLength(0); i++) {
          GL.Vertex(this._outlines[j][i, 0]);
          GL.Vertex(this._outlines[j][i, 1]);
        }
      }

      GL.End();
/*
      GL.Begin(GL.TRIANGLES);

      for (var j = 0; j < this._triangles.Count; j++) {
        GL.Color(this._colors[j]);
        for (var i = 0; i < this._triangles[j].GetLength(0); i++) {
          GL.Vertex(this._triangles[j][i, 0]);
          GL.Vertex(this._triangles[j][i, 1]);
          GL.Vertex(this._triangles[j][i, 2]);
        }
      }

      GL.End();
      */
    }

    /// <summary>
    /// </summary>
    /// <param name="new_outlines"></param>
    /// <param name="new_color"></param>
    /// <param name="game_object"></param>
    public void SetOutlines(Vector3[,] new_outlines, Color new_color, GameObject game_object) {
      if (new_outlines == null) {
        return;
      }

      if (this._outlines == null) {
        return;
      }

      if (new_outlines.GetLength(0) > 0) {
        this._outlines.Add(new_outlines);
        this._colors.Add(new_color);
        this._names.Add(game_object);
      }
    }
/*
    /// <summary>
    /// </summary>
    /// <param name="new_color"></param>
    /// <param name="new_triangles"></param>
    /// <param name="game_object"></param>
    public void SetTriangles(Vector3[,] new_triangles, Color new_color, GameObject game_object) {
      if (new_triangles == null) {
        return;
      }

      if (this._triangles == null) {
        return;
      }

      if (new_triangles.GetLength(0) > 0) {
        this._colors.Add(new_color);
        this._triangles.Add(new_triangles);
        this._names.Add(game_object);
      }
    }
*/

    void OnPreRender() {
      this._outlines.Clear();
      this._colors.Clear();
      this._names.Clear();
      //this._triangles.Clear();
      if (!this._cacheBoundingBoxes || this._bounding_boxes == null || this._bounding_boxes.Length == 0) {
        this._bounding_boxes = FindObjectsOfType<BoundingBox>();
      }

      foreach (var bb in this._bounding_boxes) {
        if (bb) {
          this.SetOutlines(bb.Lines, bb.EditorPreviewLineColor, bb.gameObject);
        }
      }
    }

    const float _padding = 4;

    void OnGUI() {
      if (this._draw_label) {
        var i = 0;
        foreach (var t in this._outlines) {
          var point = t[0, 0];
          var box_position = this._camera.WorldToScreenPoint(point);
          box_position.y = Screen.height - box_position.y;

          var text = this._names[i].name;

          var content = GUI.skin.box.CalcSize(new GUIContent(text));
          content.x = content.x + _padding;
          content.y = content.y + _padding;
          var rect = new Rect(box_position.x - content.x / 2,
                              box_position.y - content.y / 2,
                              content.x,
                              content.y);
          GUI.Box(rect, text);
          //GUI.Label(this._rect, text);
          i++;
        }
      }
    }
  }
}
