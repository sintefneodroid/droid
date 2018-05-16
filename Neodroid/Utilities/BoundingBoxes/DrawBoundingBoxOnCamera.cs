using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Utilities.BoundingBoxes {
  [RequireComponent(typeof(Camera))]
  [ExecuteInEditMode]
  public class DrawBoundingBoxOnCamera : MonoBehaviour {
    List<Color> _colors;
    public Color _L_Color = Color.green;
    public Material _Line_Material;
    List<Vector3[,]> _outlines;
    List<Vector3[,]> _triangles;

    void Awake() {
      this._outlines = new List<Vector3[,]>();
      this._colors = new List<Color>();
      this._triangles = new List<Vector3[,]>();
    }

    void OnPostRender() {
      if (this._outlines == null) {
        return;
      }

      this._Line_Material.SetPass(0);
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
      this._outlines = new List<Vector3[,]>();
      this._colors = new List<Color>();
      this._triangles = new List<Vector3[,]>();
    }
  }
}
