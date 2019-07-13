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
  }
}
