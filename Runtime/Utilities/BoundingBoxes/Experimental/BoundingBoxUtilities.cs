using System;
using System.Runtime.CompilerServices;
using droid.Runtime.Utilities.Sensors;
using UnityEditor;
using UnityEngine;

namespace droid.Runtime.Utilities.BoundingBoxes.Experimental {
  public static class BbUtilities {
    public static void DrawBoxFromCenter(Vector3 p, float r, Color c) {
      // p is pos.yition of the center, r is "radius" and c is the color of the box
      //Bottom lines
      Debug.DrawLine(new Vector3(-r + p.x, -r + p.y, -r + p.z), new Vector3(r + p.x, -r + p.y, -r + p.z), c);
      Debug.DrawLine(new Vector3(-r + p.x, -r + p.y, -r + p.z), new Vector3(-r + p.x, -r + p.y, r + p.z), c);
      Debug.DrawLine(new Vector3(r + p.x, -r + p.y, r + p.z), new Vector3(-r + p.x, -r + p.y, r + p.z), c);
      Debug.DrawLine(new Vector3(r + p.x, -r + p.y, r + p.z), new Vector3(r + p.x, -r + p.y, -r + p.z), c);

      //Vertical lines
      Debug.DrawLine(new Vector3(-r + p.x, r + p.y, -r + p.z), new Vector3(r + p.x, r + p.y, -r + p.z), c);
      Debug.DrawLine(new Vector3(-r + p.x, r + p.y, -r + p.z), new Vector3(-r + p.x, r + p.y, r + p.z), c);
      Debug.DrawLine(new Vector3(r + p.x, r + p.y, r + p.z), new Vector3(-r + p.x, r + p.y, r + p.z), c);
      Debug.DrawLine(new Vector3(r + p.x, r + p.y, r + p.z), new Vector3(r + p.x, r + p.y, -r + p.z), c);

      //Top lines
      Debug.DrawLine(new Vector3(-r + p.x, -r + p.y, -r + p.z), new Vector3(-r + p.x, r + p.y, -r + p.z), c);
      Debug.DrawLine(new Vector3(-r + p.x, -r + p.y, r + p.z), new Vector3(-r + p.x, r + p.y, r + p.z), c);
      Debug.DrawLine(new Vector3(r + p.x, -r + p.y, -r + p.z), new Vector3(r + p.x, r + p.y, -r + p.z), c);
      Debug.DrawLine(new Vector3(r + p.x, -r + p.y, r + p.z), new Vector3(r + p.x, r + p.y, r + p.z), c);
    }

    public static void DrawRect(float x_size, float y_size, float z_size, Vector3 pos, Color color) {
      var x = x_size / 2;
      var y = y_size / 2;
      var z = z_size / 2;

      //Vertical lines
      Debug.DrawLine(new Vector3(-x + pos.x, -y + pos.y, -z + pos.z),
                     new Vector3(-x + pos.x, y + pos.y, -z + pos.z),
                     color);
      Debug.DrawLine(new Vector3(x + pos.x, -y + pos.y, -z + pos.z),
                     new Vector3(x + pos.x, y + pos.y, -z + pos.z),
                     color);
      Debug.DrawLine(new Vector3(-x + pos.x, -y + pos.y, z + pos.z),
                     new Vector3(-x + pos.x, y + pos.y, z + pos.z),
                     color);
      Debug.DrawLine(new Vector3(x + pos.x, -y + pos.y, z + pos.z),
                     new Vector3(x + pos.x, y + pos.y, z + pos.z),
                     color);

      //Horizontal top
      Debug.DrawLine(new Vector3(-x + pos.x, y + pos.y, -z + pos.z),
                     new Vector3(x + pos.x, y + pos.y, -z + pos.z),
                     color);
      Debug.DrawLine(new Vector3(-x + pos.x, y + pos.y, z + pos.z),
                     new Vector3(x + pos.x, y + pos.y, z + pos.z),
                     color);
      Debug.DrawLine(new Vector3(-x + pos.x, y + pos.y, -z + pos.z),
                     new Vector3(-x + pos.x, y + pos.y, z + pos.z),
                     color);
      Debug.DrawLine(new Vector3(x + pos.x, y + pos.y, -z + pos.z),
                     new Vector3(x + pos.x, y + pos.y, z + pos.z),
                     color);

      //Horizontal bottom
      Debug.DrawLine(new Vector3(-x + pos.x, -y + pos.y, -z + pos.z),
                     new Vector3(x + pos.x, -y + pos.y, -z + pos.z),
                     color);
      Debug.DrawLine(new Vector3(-x + pos.x, -y + pos.y, z + pos.z),
                     new Vector3(x + pos.x, -y + pos.y, z + pos.z),
                     color);
      Debug.DrawLine(new Vector3(-x + pos.x, -y + pos.y, -z + pos.z),
                     new Vector3(-x + pos.x, -y + pos.y, z + pos.z),
                     color);
      Debug.DrawLine(new Vector3(x + pos.x, -y + pos.y, -z + pos.z),
                     new Vector3(x + pos.x, -y + pos.y, z + pos.z),
                     color);
    }

    public static bool DidTransformsChange(
        Transform[] old_transforms,
        Transform[] newly_acquired_transforms) {
      if (old_transforms.Length != newly_acquired_transforms.Length) {
        return true;
      }

      var i = 0;
      foreach (var old in old_transforms) {
        if (old.position != newly_acquired_transforms[i].position
            || old.rotation != newly_acquired_transforms[i].rotation) {
          return true;
        }

        i++;
      }

      return false;
    }

    public static Bounds GetTotalMeshFilterBounds(Transform object_transform) {
      var mesh_filter = object_transform.GetComponent<MeshFilter>();

      var result = mesh_filter != null ? mesh_filter.mesh.bounds : new Bounds();

      foreach (Transform transform in object_transform) {
        var bounds = GetTotalMeshFilterBounds(transform);
        result.Encapsulate(bounds.min);
        result.Encapsulate(bounds.max);
      }

      /*var bounds1 = GetTotalColliderBounds(objectTransform);
      result.Encapsulate(bounds1.min);
      result.Encapsulate(bounds1.max);
      */
      /*
            foreach (Transform transform in objectTransform) {
              var bounds = GetTotalColliderBounds(transform);
              result.Encapsulate(bounds.min);
              result.Encapsulate(bounds.max);
            }
            */
      var scaled_min = result.min;
      var local_scale = object_transform.localScale;
      scaled_min.Scale(local_scale);
      result.min = scaled_min;
      var scaled_max = result.max;
      scaled_max.Scale(local_scale);
      result.max = scaled_max;
      return result;
    }

    /// <summary>
    /// </summary>
    /// <param name="object_transform"></param>
    /// <returns></returns>
    public static Bounds GetTotalColliderBounds(Transform object_transform) {
      var mesh_filter = object_transform.GetComponent<Collider>();

      var result = mesh_filter != null ? mesh_filter.bounds : new Bounds();

      foreach (Transform transform in object_transform) {
        var bounds = GetTotalColliderBounds(transform);
        result.Encapsulate(bounds.min);
        result.Encapsulate(bounds.max);
      }

      var scaled_min = result.min;
      var local_scale = object_transform.localScale;
      scaled_min.Scale(local_scale);
      result.min = scaled_min;
      var scaled_max = result.max;
      scaled_max.Scale(local_scale);
      result.max = scaled_max;
      return result;
    }

    /// <summary>
    /// </summary>
    /// <param name="g"></param>
    /// <returns></returns>
    public static Bounds GetMaxBounds(GameObject g) {
      var b = new Bounds(g.transform.position, Vector3.zero);
      foreach (var r in g.GetComponentsInChildren<Renderer>()) {
        b.Encapsulate(r.bounds);
      }

      return b;
    }



    public static Rect
        GetBoundsScreenRectEncapsulationSlow(this Bounds bounds, Camera cam, float margin = 0) {
      var rect = new Rect();

      var points = new Vector3[8];
      var screen_pos = new Vector3[8];

      var b = bounds; // reference object ex Simple
      points[0] = new Vector3(b.min.x, b.min.y, b.min.z);
      points[1] = new Vector3(b.max.x, b.min.y, b.min.z);
      points[2] = new Vector3(b.max.x, b.max.y, b.min.z);
      points[3] = new Vector3(b.min.x, b.max.y, b.min.z);
      points[4] = new Vector3(b.min.x, b.min.y, b.max.z);
      points[5] = new Vector3(b.max.x, b.min.y, b.max.z);
      points[6] = new Vector3(b.max.x, b.max.y, b.max.z);
      points[7] = new Vector3(b.min.x, b.max.y, b.max.z);

      var screen_bounds = new Bounds();
      for (var i = 0; i < 8; i++) {
        screen_pos[i] = cam.WorldToScreenPoint(points[i]);

        if (i == 0) {
          screen_bounds = new Bounds(screen_pos[0], Vector3.zero);
        }

        screen_bounds.Encapsulate(screen_pos[i]);
      }

      //Debug.Log(screen_bounds.ToString());

      rect.xMin = screen_bounds.min.x;
      rect.yMin = screen_bounds.min.y;
      rect.xMax = screen_bounds.max.x;
      rect.yMax = screen_bounds.max.y;

      return rect;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="bounds"></param>
    /// <param name="cam"></param>
    /// <param name="margin"></param>
    /// <returns></returns>
    public static Rect GetBoundingBoxScreenRect(this BoundingBox bounds, Camera cam, float margin = 0) {
      Vector2 min = cam.WorldToScreenPoint(bounds.transform.TransformPoint(bounds.Points[0]));
      var max = min;

      var point = min;
      get_min_max(point, ref min, ref max);

      for (var i = 1; i < bounds.Points.Length; i++) {
        point = cam.WorldToScreenPoint(bounds.transform.TransformPoint(bounds.Points[i]));
        get_min_max(point, ref min, ref max);
      }

      var r = Rect.MinMaxRect(min.x, min.y, max.x, max.y);
      r.xMin -= margin;
      r.xMax += margin;
      r.yMin -= margin;
      r.yMax += margin;

      return r;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="bounds"></param>
    /// <param name="cam"></param>
    /// <param name="margin"></param>
    /// <returns></returns>
    public static Rect GetBoundsScreenRect(this Bounds bounds, Camera cam, float margin = 0) {
      var cen = bounds.center;
      var ext = bounds.extents;

      var x_min = cen.x - ext.x;
      var y_min = cen.y - ext.y;
      var z_min = cen.z - ext.z;
      var x_max = cen.x + ext.x;
      var y_max = cen.y + ext.y;
      var z_max = cen.z + ext.z;

      Vector2 min = cam.WorldToScreenPoint(new Vector3(x_min, y_min, z_min));
      var max = min;

      var point = min;
      get_min_max(point, ref min, ref max);

      point = cam.WorldToScreenPoint(new Vector3(x_max, y_max, z_max));
      get_min_max(point, ref min, ref max);

      point = cam.WorldToScreenPoint(new Vector3(x_max, y_min, z_min));
      get_min_max(point, ref min, ref max);

      point = cam.WorldToScreenPoint(new Vector3(x_min, y_max, z_min));
      get_min_max(point, ref min, ref max);

      point = cam.WorldToScreenPoint(new Vector3(x_min, y_min, z_max));
      get_min_max(point, ref min, ref max);

      point = cam.WorldToScreenPoint(new Vector3(x_max, y_min, z_max));
      get_min_max(point, ref min, ref max);

      point = cam.WorldToScreenPoint(new Vector3(x_max, y_max, z_min));
      get_min_max(point, ref min, ref max);

      point = cam.WorldToScreenPoint(new Vector3(x_min, y_max, z_max));
      get_min_max(point, ref min, ref max);

      /*
       var width = max.x - min.x;
      var height = max.y - min.y;
      return new Rect(min.x, min.y, width, height);
      */

      var r = Rect.MinMaxRect(min.x, min.y, max.x, max.y);
      r.xMin -= margin;
      r.xMax += margin;
      r.yMin -= margin;
      r.yMax += margin;

      return r;
    }

    public static Rect Normalise(this Rect rect, float width, float height) {
      if (width < float.Epsilon || Math.Abs(height) < float.Epsilon) {
        return new Rect();
      }

      return new Rect {
                          x = rect.x / width,
                          width = rect.width / width,
                          y = rect.y / width,
                          height = rect.height / height
                      };
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="local_bounds"></param>
    /// <returns></returns>
    public static Bounds TransformBounds(this Transform transform, Bounds local_bounds) {
      var center = transform.TransformPoint(local_bounds.center);

      // transform the local extents' axes
      var extents = local_bounds.extents;
      var axis_x = transform.TransformVector(extents.x, 0, 0);
      var axis_y = transform.TransformVector(0, extents.y, 0);
      var axis_z = transform.TransformVector(0, 0, extents.z);

      // sum their absolute value to get the world extents
      extents.x = Mathf.Abs(axis_x.x) + Mathf.Abs(axis_y.x) + Mathf.Abs(axis_z.x);
      extents.y = Mathf.Abs(axis_x.y) + Mathf.Abs(axis_y.y) + Mathf.Abs(axis_z.y);
      extents.z = Mathf.Abs(axis_x.z) + Mathf.Abs(axis_y.z) + Mathf.Abs(axis_z.z);

      return new Bounds {center = center, extents = extents};
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static void get_min_max(Vector2 point, ref Vector2 min, ref Vector2 max) {
      min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
      max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);
    }
  }
}
