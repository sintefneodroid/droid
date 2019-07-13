using UnityEngine;

namespace droid.Runtime.GameObjects.Plotting {
  public class Meshes {
    public static Mesh ConeMesh() {
      var mesh = new Mesh();
      mesh.Clear();

      const float height = 1f;
      const float bottom_radius = .25f;
      const float top_radius = .05f;
      const int nb_sides = 18;
      const int nb_height_seg = 1; // Not implemented yet

      const int nb_vertices_cap = nb_sides + 1;

      #region Vertices

// bottom + top + sides
      var vertices = new Vector3[nb_vertices_cap + nb_vertices_cap + nb_sides * nb_height_seg * 2 + 2];
      var vert = 0;
      const float _2_pi = Mathf.PI * 2f;

// Bottom cap
      vertices[vert++] = new Vector3(0f, 0f, 0f);
      while (vert <= nb_sides) {
        var rad = (float)vert / nb_sides * _2_pi;
        vertices[vert] = new Vector3(Mathf.Cos(rad) * bottom_radius, 0f, Mathf.Sin(rad) * bottom_radius);
        vert++;
      }

// Top cap
      vertices[vert++] = new Vector3(0f, height, 0f);
      while (vert <= nb_sides * 2 + 1) {
        var rad = (float)(vert - nb_sides - 1) / nb_sides * _2_pi;
        vertices[vert] = new Vector3(Mathf.Cos(rad) * top_radius, height, Mathf.Sin(rad) * top_radius);
        vert++;
      }

// Sides
      var v = 0;
      while (vert <= vertices.Length - 4) {
        var rad = (float)v / nb_sides * _2_pi;
        vertices[vert] = new Vector3(Mathf.Cos(rad) * top_radius, height, Mathf.Sin(rad) * top_radius);
        vertices[vert + 1] = new Vector3(Mathf.Cos(rad) * bottom_radius, 0, Mathf.Sin(rad) * bottom_radius);
        vert += 2;
        v++;
      }

      vertices[vert] = vertices[nb_sides * 2 + 2];
      vertices[vert + 1] = vertices[nb_sides * 2 + 3];

      #endregion

      #region Normales

// bottom + top + sides
      var normales = new Vector3[vertices.Length];
      vert = 0;

// Bottom cap
      while (vert <= nb_sides) {
        normales[vert++] = Vector3.down;
      }

// Top cap
      while (vert <= nb_sides * 2 + 1) {
        normales[vert++] = Vector3.up;
      }

// Sides
      v = 0;
      while (vert <= vertices.Length - 4) {
        var rad = (float)v / nb_sides * _2_pi;
        var cos = Mathf.Cos(rad);
        var sin = Mathf.Sin(rad);

        normales[vert] = new Vector3(cos, 0f, sin);
        normales[vert + 1] = normales[vert];

        vert += 2;
        v++;
      }

      normales[vert] = normales[nb_sides * 2 + 2];
      normales[vert + 1] = normales[nb_sides * 2 + 3];

      #endregion

      #region UVs

      var uvs = new Vector2[vertices.Length];

// Bottom cap
      var u = 0;
      uvs[u++] = new Vector2(0.5f, 0.5f);
      while (u <= nb_sides) {
        var rad = (float)u / nb_sides * _2_pi;
        uvs[u] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
        u++;
      }

// Top cap
      uvs[u++] = new Vector2(0.5f, 0.5f);
      while (u <= nb_sides * 2 + 1) {
        var rad = (float)u / nb_sides * _2_pi;
        uvs[u] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
        u++;
      }

// Sides
      var u_sides = 0;
      while (u <= uvs.Length - 4) {
        var t = (float)u_sides / nb_sides;
        uvs[u] = new Vector3(t, 1f);
        uvs[u + 1] = new Vector3(t, 0f);
        u += 2;
        u_sides++;
      }

      uvs[u] = new Vector2(1f, 1f);
      uvs[u + 1] = new Vector2(1f, 0f);

      #endregion

      #region Triangles

      var nb_triangles = nb_sides + nb_sides + nb_sides * 2;
      var triangles = new int[nb_triangles * 3 + 3];

// Bottom cap
      var tri = 0;
      var i = 0;
      while (tri < nb_sides - 1) {
        triangles[i] = 0;
        triangles[i + 1] = tri + 1;
        triangles[i + 2] = tri + 2;
        tri++;
        i += 3;
      }

      triangles[i] = 0;
      triangles[i + 1] = tri + 1;
      triangles[i + 2] = 1;
      tri++;
      i += 3;

// Top cap
//tri++;
      while (tri < nb_sides * 2) {
        triangles[i] = tri + 2;
        triangles[i + 1] = tri + 1;
        triangles[i + 2] = nb_vertices_cap;
        tri++;
        i += 3;
      }

      triangles[i] = nb_vertices_cap + 1;
      triangles[i + 1] = tri + 1;
      triangles[i + 2] = nb_vertices_cap;
      tri++;
      i += 3;
      tri++;

// Sides
      while (tri <= nb_triangles) {
        triangles[i] = tri + 2;
        triangles[i + 1] = tri + 1;
        triangles[i + 2] = tri + 0;
        tri++;
        i += 3;

        triangles[i] = tri + 1;
        triangles[i + 1] = tri + 2;
        triangles[i + 2] = tri + 0;
        tri++;
        i += 3;
      }

      #endregion

      mesh.vertices = vertices;
      mesh.normals = normales;
      mesh.uv = uvs;
      mesh.triangles = triangles;

      mesh.RecalculateBounds();
//mesh.Optimize();

      return mesh;
    }

    public static Mesh SphereMesh() {
      var mesh = new Mesh();
      mesh.Clear();

      var radius = 1f;
// Longitude |||
      var nb_long = 24;
// Latitude ---
      var nb_lat = 16;

      #region Vertices

      var vertices = new Vector3[(nb_long + 1) * nb_lat + 2];
      const float pi = Mathf.PI;
      const float _2_pi = pi * 2f;

      vertices[0] = Vector3.up * radius;
      for (var lat = 0; lat < nb_lat; lat++) {
        var a1 = pi * (lat + 1) / (nb_lat + 1);
        var sin1 = Mathf.Sin(a1);
        var cos1 = Mathf.Cos(a1);

        for (var lon = 0; lon <= nb_long; lon++) {
          var a2 = _2_pi * (lon == nb_long ? 0 : lon) / nb_long;
          var sin2 = Mathf.Sin(a2);
          var cos2 = Mathf.Cos(a2);

          vertices[lon + lat * (nb_long + 1) + 1] = new Vector3(sin1 * cos2, cos1, sin1 * sin2) * radius;
        }
      }

      vertices[vertices.Length - 1] = Vector3.up * -radius;

      #endregion

      #region Normales

      var normales = new Vector3[vertices.Length];
      for (var n = 0; n < vertices.Length; n++) {
        normales[n] = vertices[n].normalized;
      }

      #endregion

      #region UVs

      var uvs = new Vector2[vertices.Length];
      uvs[0] = Vector2.up;
      uvs[uvs.Length - 1] = Vector2.zero;
      for (var lat = 0; lat < nb_lat; lat++) {
        for (var lon = 0; lon <= nb_long; lon++) {
          uvs[lon + lat * (nb_long + 1) + 1] =
              new Vector2((float)lon / nb_long, 1f - (float)(lat + 1) / (nb_lat + 1));
        }
      }

      #endregion

      #region Triangles

      var nb_faces = vertices.Length;
      var nb_triangles = nb_faces * 2;
      var nb_indexes = nb_triangles * 3;
      var triangles = new int[nb_indexes];

//Top Cap
      var i = 0;
      for (var lon = 0; lon < nb_long; lon++) {
        triangles[i++] = lon + 2;
        triangles[i++] = lon + 1;
        triangles[i++] = 0;
      }

//Middle
      for (var lat = 0; lat < nb_lat - 1; lat++) {
        for (var lon = 0; lon < nb_long; lon++) {
          var current = lon + lat * (nb_long + 1) + 1;
          var next = current + nb_long + 1;

          triangles[i++] = current;
          triangles[i++] = current + 1;
          triangles[i++] = next + 1;

          triangles[i++] = current;
          triangles[i++] = next + 1;
          triangles[i++] = next;
        }
      }

//Bottom Cap
      for (var lon = 0; lon < nb_long; lon++) {
        triangles[i++] = vertices.Length - 1;
        triangles[i++] = vertices.Length - (lon + 2) - 1;
        triangles[i++] = vertices.Length - (lon + 1) - 1;
      }

      #endregion

      mesh.vertices = vertices;
      mesh.normals = normales;
      mesh.uv = uvs;
      mesh.triangles = triangles;

      mesh.RecalculateBounds();
      return mesh;
    }
  }
}
