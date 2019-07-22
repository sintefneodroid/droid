//------------------------------//
//  ProceduralCapsule.cs        //
//  Written by Jay Kay          //
//  2016/05/27                  //
//------------------------------//

using UnityEngine;

namespace droid.Runtime.Utilities.Procedural {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
  [ExecuteInEditMode]
  public class ProceduralCapsule : MonoBehaviour {
    /// <summary>
    /// </summary>
    public float _Height = 2f;

    /// <summary>
    /// </summary>
    public float _Radius = 0.5f;

    /// <summary>
    /// </summary>
    public int _Segments = 24;
    #if UNITY_EDITOR
    /// <summary>
    /// </summary>
    [ContextMenu("Generate Procedural Capsule")]
    public void GenerateProceduralCapsule() { this.CreateMesh(); }
    #endif

    void Start() {
      var mesh_filter = this.GetComponent<MeshFilter>();
      if (!mesh_filter.sharedMesh
          || mesh_filter.sharedMesh.vertexCount == 0
          || mesh_filter.sharedMesh.name != "ProceduralCapsule") {
        this.CreateMesh();
      }
    }

    /// <summary>
    /// </summary>
    public void CreateMesh() {
      // make segments an even number
      if (this._Segments % 2 != 0) {
        this._Segments++;
      }

      // extra vertex on the seam
      var points = this._Segments + 1;

      // calculate points around a circle
      var p_x = new float[points];
      var p_z = new float[points];
      var p_y = new float[points];
      var p_r = new float[points];

      var calc_h = 0f;
      var calc_v = 0f;

      for (var i = 0; i < points; i++) {
        p_x[i] = Mathf.Sin(calc_h * Mathf.Deg2Rad);
        p_z[i] = Mathf.Cos(calc_h * Mathf.Deg2Rad);
        p_y[i] = Mathf.Cos(calc_v * Mathf.Deg2Rad);
        p_r[i] = Mathf.Sin(calc_v * Mathf.Deg2Rad);

        calc_h += 360f / this._Segments;
        calc_v += 180f / this._Segments;
      }

      // - Vertices and UVs -

      var vertices = new Vector3[points * (points + 1)];
      var uvs = new Vector2[vertices.Length];
      var ind = 0;

      // Y-offset is half the height minus the diameter
      // float yOff = ( height - ( radius * 2f ) ) * 0.5f;
      var y_off = (this._Height - this._Radius) * 0.5f;
      if (y_off < 0) {
        y_off = 0;
      }

      // uv calculations
      var step_x = 1f / (points - 1);
      float uv_x, uv_y;

      // Top Hemisphere
      var top = Mathf.CeilToInt(points * 0.5f);

      for (var y = 0; y < top; y++) {
        for (var x = 0; x < points; x++) {
          vertices[ind] = new Vector3(p_x[x] * p_r[y], p_y[y], p_z[x] * p_r[y]) * this._Radius;
          vertices[ind].y = y_off + vertices[ind].y;

          uv_x = 1f - step_x * x;
          uv_y = (vertices[ind].y + this._Height * 0.5f) / this._Height;
          uvs[ind] = new Vector2(uv_x, uv_y);

          ind++;
        }
      }

      // Bottom Hemisphere
      var btm = Mathf.FloorToInt(points * 0.5f);

      for (var y = btm; y < points; y++) {
        for (var x = 0; x < points; x++) {
          vertices[ind] = new Vector3(p_x[x] * p_r[y], p_y[y], p_z[x] * p_r[y]) * this._Radius;
          vertices[ind].y = -y_off + vertices[ind].y;

          uv_x = 1f - step_x * x;
          uv_y = (vertices[ind].y + this._Height * 0.5f) / this._Height;
          uvs[ind] = new Vector2(uv_x, uv_y);

          ind++;
        }
      }

      // - Triangles -

      var triangles = new int[this._Segments * (this._Segments + 1) * 2 * 3];

      for (int y = 0, t = 0; y < this._Segments + 1; y++) {
        for (var x = 0; x < this._Segments; x++, t += 6) {
          triangles[t + 0] = (y + 0) * (this._Segments + 1) + x + 0;
          triangles[t + 1] = (y + 1) * (this._Segments + 1) + x + 0;
          triangles[t + 2] = (y + 1) * (this._Segments + 1) + x + 1;

          triangles[t + 3] = (y + 0) * (this._Segments + 1) + x + 1;
          triangles[t + 4] = (y + 0) * (this._Segments + 1) + x + 0;
          triangles[t + 5] = (y + 1) * (this._Segments + 1) + x + 1;
        }
      }

      // - Assign Mesh -

      var mf = this.gameObject.GetComponent<MeshFilter>();
      var mesh = mf.sharedMesh;
      if (!mesh) {
        mesh = new Mesh();
        mf.sharedMesh = mesh;
      }

      mesh.Clear();

      mesh.name = "ProceduralCapsule";

      mesh.vertices = vertices;
      mesh.uv = uvs;
      mesh.triangles = triangles;

      mesh.RecalculateBounds();
      mesh.RecalculateNormals();
    }
  }
}
