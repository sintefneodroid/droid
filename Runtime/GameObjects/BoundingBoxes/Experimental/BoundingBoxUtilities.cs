using System;
using UnityEngine;

namespace droid.Runtime.GameObjects.BoundingBoxes.Experimental {
  /// <summary>
  ///
  /// </summary>
  public static class BoundingBoxUtilities {
    /// <summary>
    ///
    /// </summary>
    public const int _Num_Points_Per_Line = 2;

    public const int _Num_Lines = 12;
    public static Vector3 _Top_Front_Right = new Vector3(1, 1, 1);
    public static Vector3 _Top_Front_Left = new Vector3(-1, 1, 1);
    public static Vector3 _Bottom_Back_Right = new Vector3(1, -1, -1);
    public static Vector3 _Bottom_Back_Left = new Vector3(-1, -1, -1);
    public static Vector3 _Bottom_Front_Left = new Vector3(-1, -1, 1);
    public static Vector3 _Bottom_Front_Right = new Vector3(1, -1, 1);
    public static Vector3 _Top_Back_Right = new Vector3(1, 1, -1);
    public static Vector3 _Top_Back_Left = new Vector3(-1, 1, -1);

    /// <summary>
    ///
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="t"></param>
    /// <param name="cam"></param>
    /// <param name="max"></param>
    /// <param name="use_view_port"></param>
    /// <param name="min"></param>
    /// <returns></returns>
    public static Vector3[] GetCameraMinMaxPoints(this Mesh mesh,
                                                  Transform t,
                                                  Camera cam,
                                                  Vector3 min,
                                                  Vector3 max,
                                                  bool use_view_port = false) {
      var a = mesh.vertices;

      Vector3 point;

      foreach (var t1 in a) {
        if (use_view_port) {
          point = cam.WorldToViewportPoint(t.TransformPoint(t1));
        } else {
          point = cam.WorldToScreenPoint(t.TransformPoint(t1));
        }

        point.GetMinMax(ref min, ref max);
      }

      var size = max - min;

      return new[] {min, max, size};
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="t"></param>
    /// <param name="cam"></param>
    /// <param name="use_view_port"></param>
    /// <returns></returns>
    public static Vector3[] GetCameraMinMaxPoints(this Mesh mesh,
                                                  Transform t,
                                                  Camera cam,
                                                  bool use_view_port = false) {
      var a = mesh.vertices;

      Vector3 min;
      if (use_view_port) {
        min = cam.WorldToViewportPoint(t.TransformPoint(a[0]));
      } else {
        min = cam.WorldToScreenPoint(t.TransformPoint(a[0]));
      }

      var max = min;

      var point = min;
      point.GetMinMax(ref min, ref max);

      for (var i = 1; i < a.Length; i++) {
        if (use_view_port) {
          point = cam.WorldToViewportPoint(t.TransformPoint(a[i]));
        } else {
          point = cam.WorldToScreenPoint(t.TransformPoint(a[i]));
        }

        point.GetMinMax(ref min, ref max);
      }

      var size = max - min;

      return new[] {min, max, size};
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="t"></param>
    /// <param name="cam"></param>
    /// <param name="margin"></param>
    /// <returns></returns>
    public static Rect GetCameraMinMaxRect(this Mesh mesh,
                                           Transform t,
                                           Camera cam,
                                           float margin = 0,
                                           bool use_viewport = false) {
      var cen = mesh.GetCameraMinMaxPoints(t, cam, use_viewport);
      var min = cen[0];
      var max = cen[1];

      var r = Rect.MinMaxRect(min.x,
                              min.y,
                              max.x,
                              max.y);
      r.xMin -= margin;
      r.xMax += margin;
      r.yMin -= margin;
      r.yMax += margin;

      return r;
    }

    public static Rect GetMinMaxRect(Vector3 min, Vector3 max, float margin = 0) {
      var r = Rect.MinMaxRect(min.x,
                              min.y,
                              max.x,
                              max.y);
      r.xMin -= margin;
      r.xMax += margin;
      r.yMin -= margin;
      r.yMax += margin;

      return r;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
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
  }
}
