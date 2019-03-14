using System;
using System.Runtime.CompilerServices;
using droid.Runtime.Utilities.Sensors;
using UnityEditor;
using UnityEngine;

namespace droid.Runtime.Utilities.BoundingBoxes.Experimental {
  /// <summary>
  ///
  /// </summary>
  public static class BoundingBoxUtilities {
    /// <summary>
    ///
    /// </summary>
    /// <param name="p"></param>
    /// <param name="r"></param>
    /// <param name="c"></param>
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

    /// <summary>
    ///
    /// </summary>
    /// <param name="x_size"></param>
    /// <param name="y_size"></param>
    /// <param name="z_size"></param>
    /// <param name="pos"></param>
    /// <param name="color"></param>
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

    /// <summary>
    ///
    /// </summary>
    /// <param name="old_transforms"></param>
    /// <param name="newly_acquired_transforms"></param>
    /// <returns></returns>
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

    /// <summary>
    ///
    /// </summary>
    /// <param name="object_transform"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bounds"></param>
    /// <param name="cam"></param>
    /// <param name="margin"></param>
    /// <returns></returns>
    public static Rect GetBoundsScreenRectEncapsulationSlow(this Bounds bounds,
                                                            Camera cam,
                                                            float margin = 0) {
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
    /// <param name="bb"></param>
    /// <param name="cam"></param>
    /// <param name="margin"></param>
    /// <returns></returns>
    public static Rect GetBoundingBoxScreenRect(this BoundingBox bb, Camera cam, Single margin = 0) {
      var min = Vector2.zero;

      if (bb != null && bb.Points?.Length > 0) {
        if (bb._use_bb_transform && bb._bb_transform) {
          min = cam.WorldToScreenPoint(bb._bb_transform.TransformPoint(bb.Points[0]));
        } else {
          min = cam.WorldToScreenPoint(bb.Points[0]);
        }
      }

      var max = min;

      var point = min;
      get_min_max(point, ref min, ref max);

      if (bb != null) {
        for (var i = 1; i < bb.Points?.Length; i++) {
          if (bb._use_bb_transform && bb._bb_transform) {
            point = cam.WorldToScreenPoint(bb._bb_transform.TransformPoint(bb.Points[i]));
          } else {
            point = cam.WorldToScreenPoint(bb.Points[i]);
          }

          get_min_max(point, ref min, ref max);
        }
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
    /// <param name="a_cam"></param>
    /// <param name="a_area"></param>
    /// <param name="a_limits"></param>
    /// <param name="a_max_height"></param>
    public static void CalculateLimits(Camera a_cam,
                                       Bounds a_area,
                                       out Rect a_limits,
                                       out float a_max_height) {
      // Half the FOV angle in radians
      var angle = a_cam.fieldOfView * Mathf.Deg2Rad * 0.5f;

      // half the size of the viewing frustum at a distance of "1" from the camera
      var tan = Vector2.one * Mathf.Tan(angle);
      tan.x *= a_cam.aspect;

      // the center point of the area and it's extents
      // the center point is taken from the bottom center of the bounding box
      var dim = a_area.extents;
      var center = a_area.center - new Vector3(0, a_area.extents.y, 0);

      // the maximum distance the camera can be above the area plane for each direction
      var max_dist = new Vector2(dim.x / tan.x, dim.z / tan.y);

      // actual distance of the camera above our plane
      var dist = a_cam.transform.position.y - center.y;

      // the max movement range around the center of the plane
      dim.x *= 1f - dist / max_dist.x;
      dim.z *= 1f - dist / max_dist.y;

      // maximum world space y coordinate the camera can be moved to
      a_max_height = center.y + Mathf.Min(max_dist.x, max_dist.y);

      // the min and max x and z coordinates the camera can be at the current distance.
      a_limits = new Rect(center.x - dim.x, center.z - dim.z, dim.x * 2, dim.z * 2);
    }

    public static bool CullToFrustum(Mesh mesh, Camera cam, Transform trans, bool update_position) {
      var fov = cam.fieldOfView;
      cam.fieldOfView = fov * 0.97f;
      var planes = GeometryUtility.CalculateFrustumPlanes(cam);
      cam.fieldOfView = fov;
      if (GeometryUtility.TestPlanesAABB(planes, mesh.bounds)) {
        return true;
      } else {
        Debug.Log("Culling :" + trans.name);
        return false;
      }
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

    public static Vector3[] GetMinMaxPointsCollider(this MeshCollider mesh, Camera cam, float margin = 0) {
      return null;

    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="bounds"></param>
    /// <param name="cam"></param>
    /// <param name="margin"></param>
    /// <returns></returns>
    public static Vector3[] GetMinMaxPointsMesh(this Mesh mesh, Camera cam, float margin = 0) {
      /*var cen = bounds.center;
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


      var r = Rect.MinMaxRect(min.x, min.y, max.x, max.y);
      r.xMin -= margin;
      r.xMax += margin;
      r.yMin -= margin;
      r.yMax += margin;

      return r;*/
      return null;
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static void get_min_max(Vector2 point, ref Vector2 min, ref Vector2 max) {
      min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
      max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);
    }
  }
}
