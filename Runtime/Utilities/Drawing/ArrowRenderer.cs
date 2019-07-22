using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace droid.Runtime.Utilities.Drawing {
// Put this script on a Camera
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public class ArrowRenderer : MonoBehaviour {
    // Fill/drag these in from the editor

    // Choose the Unlit/Color shader in the Material Settings
    // You can change that color, to change the color of the connecting lines
    /// <summary>
    /// </summary>
    [SerializeField]
    Material _line_mat = null;

    /// <summary>
    /// </summary>
    [SerializeField]
    GameObject _main_point = null;

    [SerializeField] float _offset = 0.3f;

    /// <summary>
    /// </summary>
    [SerializeField]
    GameObject[] _points = null;

    [SerializeField] Vector3[] _vec3_points = null;

    // Connect all of the `points` to the `main_point_pos`
    void DrawConnectingLines(IReadOnlyCollection<Tuple<Vector3, Vector3>> vec_pairs) {
      if (vec_pairs.Count > 0) {
        // Loop through each point to connect to the mainPoint
        foreach (var point in vec_pairs) {
          var main_point_pos = point.Item1;
          var point_pos = point.Item2;

          GL.Begin(GL.LINES);
          this._line_mat.SetPass(0);
          GL.Color(new Color(this._line_mat.color.r,
                             this._line_mat.color.g,
                             this._line_mat.color.b,
                             this._line_mat.color.a));
          GL.Vertex3(main_point_pos.x, main_point_pos.y, main_point_pos.z);
          GL.Vertex3(point_pos.x, point_pos.y, point_pos.z);
          //
          GL.Vertex3(point_pos.x - this._offset, point_pos.y, point_pos.z);
          GL.Vertex3(point_pos.x, point_pos.y - this._offset, point_pos.z);
          GL.Vertex3(point_pos.x, point_pos.y, point_pos.z - this._offset);
          GL.Vertex3(point_pos.x + this._offset, point_pos.y, point_pos.z);
          GL.Vertex3(point_pos.x, point_pos.y + this._offset, point_pos.z);
          GL.Vertex3(point_pos.x, point_pos.y, point_pos.z + this._offset);
          //
          GL.End();
        }
      }
    }

    // To show the lines in the game window whne it is running
    void OnPostRender() {
      this._vec3_points = this._points.Select(v => v.transform.position).ToArray();
      var s = this._vec3_points
                  .Select(v => new Tuple<Vector3, Vector3>(this._main_point.transform.position, v)).ToArray();
      this.DrawConnectingLines(s);
    }

    // To show the lines in the editor
    void OnDrawGizmos() {
      this._vec3_points = this._points.Select(v => v.transform.position).ToArray();
      var s = this._vec3_points
                  .Select(v => new Tuple<Vector3, Vector3>(this._main_point.transform.position, v)).ToArray();
      this.DrawConnectingLines(s);
    }
  }
}
