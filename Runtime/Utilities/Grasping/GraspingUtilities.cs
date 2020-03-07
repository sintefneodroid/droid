using UnityEngine;

namespace droid.Runtime.Utilities.Grasping {
  /// <summary>
  /// </summary>
  public static class GraspingUtilities {
    /// <summary>
    /// </summary>
    /// <param name="p"></param>
    /// <param name="r"></param>
    /// <param name="c"></param>
    public static void DrawBoxFromCenter(Vector3 p, float r, Color c) {
      // p is pos.yition of the center, r is "radius" and c is the color of the box
      //Bottom lines
      Debug.DrawLine(start : new Vector3(x : -r + p.x, y : -r + p.y, z : -r + p.z),
                     end : new Vector3(x : r + p.x, y : -r + p.y, z : -r + p.z),
                     color : c);
      Debug.DrawLine(start : new Vector3(x : -r + p.x, y : -r + p.y, z : -r + p.z),
                     end : new Vector3(x : -r + p.x, y : -r + p.y, z : r + p.z),
                     color : c);
      Debug.DrawLine(start : new Vector3(x : r + p.x, y : -r + p.y, z : r + p.z),
                     end : new Vector3(x : -r + p.x, y : -r + p.y, z : r + p.z),
                     color : c);
      Debug.DrawLine(start : new Vector3(x : r + p.x, y : -r + p.y, z : r + p.z),
                     end : new Vector3(x : r + p.x, y : -r + p.y, z : -r + p.z),
                     color : c);

      //Vertical lines
      Debug.DrawLine(start : new Vector3(x : -r + p.x, y : r + p.y, z : -r + p.z),
                     end : new Vector3(x : r + p.x, y : r + p.y, z : -r + p.z),
                     color : c);
      Debug.DrawLine(start : new Vector3(x : -r + p.x, y : r + p.y, z : -r + p.z),
                     end : new Vector3(x : -r + p.x, y : r + p.y, z : r + p.z),
                     color : c);
      Debug.DrawLine(start : new Vector3(x : r + p.x, y : r + p.y, z : r + p.z),
                     end : new Vector3(x : -r + p.x, y : r + p.y, z : r + p.z),
                     color : c);
      Debug.DrawLine(start : new Vector3(x : r + p.x, y : r + p.y, z : r + p.z),
                     end : new Vector3(x : r + p.x, y : r + p.y, z : -r + p.z),
                     color : c);

      //Top lines
      Debug.DrawLine(start : new Vector3(x : -r + p.x, y : -r + p.y, z : -r + p.z),
                     end : new Vector3(x : -r + p.x, y : r + p.y, z : -r + p.z),
                     color : c);
      Debug.DrawLine(start : new Vector3(x : -r + p.x, y : -r + p.y, z : r + p.z),
                     end : new Vector3(x : -r + p.x, y : r + p.y, z : r + p.z),
                     color : c);
      Debug.DrawLine(start : new Vector3(x : r + p.x, y : -r + p.y, z : -r + p.z),
                     end : new Vector3(x : r + p.x, y : r + p.y, z : -r + p.z),
                     color : c);
      Debug.DrawLine(start : new Vector3(x : r + p.x, y : -r + p.y, z : r + p.z),
                     end : new Vector3(x : r + p.x, y : r + p.y, z : r + p.z),
                     color : c);
    }

    /// <summary>
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
      Debug.DrawLine(start : new Vector3(x : -x + pos.x, y : -y + pos.y, z : -z + pos.z),
                     end : new Vector3(x : -x + pos.x, y : y + pos.y, z : -z + pos.z),
                     color : color);
      Debug.DrawLine(start : new Vector3(x : x + pos.x, y : -y + pos.y, z : -z + pos.z),
                     end : new Vector3(x : x + pos.x, y : y + pos.y, z : -z + pos.z),
                     color : color);
      Debug.DrawLine(start : new Vector3(x : -x + pos.x, y : -y + pos.y, z : z + pos.z),
                     end : new Vector3(x : -x + pos.x, y : y + pos.y, z : z + pos.z),
                     color : color);
      Debug.DrawLine(start : new Vector3(x : x + pos.x, y : -y + pos.y, z : z + pos.z),
                     end : new Vector3(x : x + pos.x, y : y + pos.y, z : z + pos.z),
                     color : color);

      //Horizontal top
      Debug.DrawLine(start : new Vector3(x : -x + pos.x, y : y + pos.y, z : -z + pos.z),
                     end : new Vector3(x : x + pos.x, y : y + pos.y, z : -z + pos.z),
                     color : color);
      Debug.DrawLine(start : new Vector3(x : -x + pos.x, y : y + pos.y, z : z + pos.z),
                     end : new Vector3(x : x + pos.x, y : y + pos.y, z : z + pos.z),
                     color : color);
      Debug.DrawLine(start : new Vector3(x : -x + pos.x, y : y + pos.y, z : -z + pos.z),
                     end : new Vector3(x : -x + pos.x, y : y + pos.y, z : z + pos.z),
                     color : color);
      Debug.DrawLine(start : new Vector3(x : x + pos.x, y : y + pos.y, z : -z + pos.z),
                     end : new Vector3(x : x + pos.x, y : y + pos.y, z : z + pos.z),
                     color : color);

      //Horizontal bottom
      Debug.DrawLine(start : new Vector3(x : -x + pos.x, y : -y + pos.y, z : -z + pos.z),
                     end : new Vector3(x : x + pos.x, y : -y + pos.y, z : -z + pos.z),
                     color : color);
      Debug.DrawLine(start : new Vector3(x : -x + pos.x, y : -y + pos.y, z : z + pos.z),
                     end : new Vector3(x : x + pos.x, y : -y + pos.y, z : z + pos.z),
                     color : color);
      Debug.DrawLine(start : new Vector3(x : -x + pos.x, y : -y + pos.y, z : -z + pos.z),
                     end : new Vector3(x : -x + pos.x, y : -y + pos.y, z : z + pos.z),
                     color : color);
      Debug.DrawLine(start : new Vector3(x : x + pos.x, y : -y + pos.y, z : -z + pos.z),
                     end : new Vector3(x : x + pos.x, y : -y + pos.y, z : z + pos.z),
                     color : color);
    }

    /// <summary>
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
      for (var index = 0; index < old_transforms.Length; index++) {
        var old = old_transforms[index];
        if (old.position != newly_acquired_transforms[i].position
            || old.rotation != newly_acquired_transforms[i].rotation) {
          return true;
        }

        i++;
      }

      return false;
    }

    /// <summary>
    /// </summary>
    /// <param name="object_transform"></param>
    /// <returns></returns>
    public static Bounds GetTotalMeshFilterBounds(Transform object_transform) {
      var mesh_filter = object_transform.GetComponent<MeshFilter>();

      var result = mesh_filter != null ? mesh_filter.mesh.bounds : new Bounds();

      foreach (Transform transform in object_transform) {
        var bounds = GetTotalMeshFilterBounds(object_transform : transform);
        result.Encapsulate(point : bounds.min);
        result.Encapsulate(point : bounds.max);
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
      scaled_min.Scale(scale : object_transform.localScale);
      result.min = scaled_min;
      var scaled_max = result.max;
      scaled_max.Scale(scale : object_transform.localScale);
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
        var bounds = GetTotalColliderBounds(object_transform : transform);
        result.Encapsulate(point : bounds.min);
        result.Encapsulate(point : bounds.max);
      }

      var scaled_min = result.min;
      scaled_min.Scale(scale : object_transform.localScale);
      result.min = scaled_min;
      var scaled_max = result.max;
      scaled_max.Scale(scale : object_transform.localScale);
      result.max = scaled_max;
      return result;
    }

    /// <summary>
    /// </summary>
    /// <param name="g"></param>
    /// <returns></returns>
    public static Bounds GetMaxBounds(GameObject g) {
      var b = new Bounds(center : g.transform.position, size : Vector3.zero);
      var rs = g.GetComponentsInChildren<Renderer>();
      for (var index = 0; index < rs.Length; index++) {
        var r = rs[index];
        b.Encapsulate(bounds : r.bounds);
      }

      return b;
    }
  }
}
