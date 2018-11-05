using Neodroid.Runtime.Utilities.Sensors;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace Neodroid.Runtime.Utilities.BoundingBoxes.Experimental {
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

    public static void RegisterCollisionTriggerCallbacksOnChildren(
        Transform transform,
        ChildColliderSensor.OnChildCollisionEnterDelegate on_collision_enter_child,
        ChildColliderSensor.OnChildTriggerEnterDelegate on_trigger_enter_child,
        ChildColliderSensor.OnChildCollisionExitDelegate on_collision_exit_child,
        ChildColliderSensor.OnChildTriggerExitDelegate on_trigger_exit_child,
        bool debug = false) {
      var children_with_colliders = transform.GetComponentsInChildren<Collider>(transform.gameObject);

      foreach (var child in children_with_colliders) {
        var child_sensor = child.gameObject.AddComponent<ChildColliderSensor>();
        child_sensor.OnCollisionEnterDelegate = on_collision_enter_child;
        child_sensor.OnTriggerEnterDelegate = on_trigger_enter_child;
        child_sensor.OnCollisionExitDelegate = on_collision_exit_child;
        child_sensor.OnTriggerExitDelegate = on_trigger_exit_child;
        if (debug) {
          Debug.Log(transform.name + " has " + child_sensor.name + " registered");
        }
      }
    }

    public static void DrawRect(float x_size, float y_size, float z_size, Vector3 pos, Color color) {
      var x = x_size / 2;
      var y = y_size / 2;
      var z = z_size / 2;

      //Vertical lines
      Debug.DrawLine(
          new Vector3(-x + pos.x, -y + pos.y, -z + pos.z),
          new Vector3(-x + pos.x, y + pos.y, -z + pos.z),
          color);
      Debug.DrawLine(
          new Vector3(x + pos.x, -y + pos.y, -z + pos.z),
          new Vector3(x + pos.x, y + pos.y, -z + pos.z),
          color);
      Debug.DrawLine(
          new Vector3(-x + pos.x, -y + pos.y, z + pos.z),
          new Vector3(-x + pos.x, y + pos.y, z + pos.z),
          color);
      Debug.DrawLine(
          new Vector3(x + pos.x, -y + pos.y, z + pos.z),
          new Vector3(x + pos.x, y + pos.y, z + pos.z),
          color);

      //Horizontal top
      Debug.DrawLine(
          new Vector3(-x + pos.x, y + pos.y, -z + pos.z),
          new Vector3(x + pos.x, y + pos.y, -z + pos.z),
          color);
      Debug.DrawLine(
          new Vector3(-x + pos.x, y + pos.y, z + pos.z),
          new Vector3(x + pos.x, y + pos.y, z + pos.z),
          color);
      Debug.DrawLine(
          new Vector3(-x + pos.x, y + pos.y, -z + pos.z),
          new Vector3(-x + pos.x, y + pos.y, z + pos.z),
          color);
      Debug.DrawLine(
          new Vector3(x + pos.x, y + pos.y, -z + pos.z),
          new Vector3(x + pos.x, y + pos.y, z + pos.z),
          color);

      //Horizontal bottom
      Debug.DrawLine(
          new Vector3(-x + pos.x, -y + pos.y, -z + pos.z),
          new Vector3(x + pos.x, -y + pos.y, -z + pos.z),
          color);
      Debug.DrawLine(
          new Vector3(-x + pos.x, -y + pos.y, z + pos.z),
          new Vector3(x + pos.x, -y + pos.y, z + pos.z),
          color);
      Debug.DrawLine(
          new Vector3(-x + pos.x, -y + pos.y, -z + pos.z),
          new Vector3(-x + pos.x, -y + pos.y, z + pos.z),
          color);
      Debug.DrawLine(
          new Vector3(x + pos.x, -y + pos.y, -z + pos.z),
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
      scaled_min.Scale(object_transform.localScale);
      result.min = scaled_min;
      var scaled_max = result.max;
      scaled_max.Scale(object_transform.localScale);
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
      scaled_min.Scale(object_transform.localScale);
      result.min = scaled_min;
      var scaled_max = result.max;
      scaled_max.Scale(object_transform.localScale);
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




    public static Rect GetBoundsScreenRect(this Bounds bounds, Camera cam) {
      var cen = bounds.center;
      var ext = bounds.extents;

      Vector2 min = cam.WorldToScreenPoint(new Vector3(cen.x - ext.x, cen.y - ext.y, cen.z - ext.z));
      var max = min;

      //0
      var point = min;
      get_min_max(point, ref min, ref max);

      //1
      point = cam.WorldToScreenPoint(new Vector3(cen.x + ext.x, cen.y - ext.y, cen.z - ext.z));
      get_min_max(point, ref min, ref max);


      //2
      point = cam.WorldToScreenPoint(new Vector3(cen.x - ext.x, cen.y - ext.y, cen.z + ext.z));
      get_min_max(point, ref min, ref max);

      //3
      point = cam.WorldToScreenPoint(new Vector3(cen.x + ext.x, cen.y - ext.y, cen.z + ext.z));
      get_min_max(point, ref min, ref max);

      //4
      point = cam.WorldToScreenPoint(new Vector3(cen.x - ext.x, cen.y + ext.y, cen.z - ext.z));
      get_min_max(point, ref min, ref max);

      //5
      point = cam.WorldToScreenPoint(new Vector3(cen.x + ext.x, cen.y + ext.y, cen.z - ext.z));
      get_min_max(point, ref min, ref max);

      //6
      point = cam.WorldToScreenPoint(new Vector3(cen.x - ext.x, cen.y + ext.y, cen.z + ext.z));
      get_min_max(point, ref min, ref max);

      //7
      point = cam.WorldToScreenPoint(new Vector3(cen.x + ext.x, cen.y + ext.y, cen.z + ext.z));
      get_min_max(point, ref min, ref max);

      return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static void get_min_max(Vector2 point, ref Vector2 min, ref Vector2 max) {
      min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
      max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);
    }
  }
}
