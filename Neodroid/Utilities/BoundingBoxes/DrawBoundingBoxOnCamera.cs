using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Utilities.BoundingBoxes {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [RequireComponent(typeof(Camera)), ExecuteInEditMode]
  public class DrawBoundingBoxOnCamera : MonoBehaviour {
    List<Color> _colors = new List<Color>();
    [SerializeField] Material _line_material;
    List<Vector3[,]> _outlines = new List<Vector3[,]>();
    List<Vector3[,]> _triangles = new List<Vector3[,]>();

    void Awake() {
      if (!this._line_material) {
        var shader = Shader.Find("Unlit/Color");
        this._line_material = new Material(shader);
      }
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
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="new_outlines"></param>
    /// <param name="newcolor"></param>
    public void SetOutlines(Vector3[,] new_outlines, Color newcolor) {
      if (new_outlines == null) {
        return;
      }

      if (this._outlines == null) {
        return;
      }

      if (new_outlines.GetLength(0) > 0) {
        this._outlines.Add(new_outlines);
        this._colors.Add(newcolor);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="new_outlines"></param>
    /// <param name="newcolor"></param>
    /// <param name="new_triangles"></param>
    public void SetOutlines(Vector3[,] new_outlines, Color newcolor, Vector3[,] new_triangles) {
      if (new_outlines == null) {
        return;
      }

      if (this._outlines == null) {
        return;
      }

      if (new_outlines.GetLength(0) > 0) {
        this._outlines.Add(new_outlines);
        this._colors.Add(newcolor);
        this._triangles.Add(new_triangles);
      }
    }

    void Update() {
      this._outlines.Clear();
      this._colors.Clear();
      this._triangles.Clear();
    }
  }
}
