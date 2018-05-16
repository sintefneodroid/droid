using UnityEngine;

namespace Neodroid.Utilities.BoundingBoxes {
  public static class Corners {
    public static Vector3[] ExtractCorners(
        Vector3 v3_center,
        Vector3 v3_extents,
        Transform reference_transform = null) {
      var v3_front_top_left = new Vector3(
          v3_center.x - v3_extents.x,
          v3_center.y + v3_extents.y,
          v3_center.z - v3_extents.z); // Front top left corner
      var v3_front_top_right = new Vector3(
          v3_center.x + v3_extents.x,
          v3_center.y + v3_extents.y,
          v3_center.z - v3_extents.z); // Front top right corner
      var v3_front_bottom_left = new Vector3(
          v3_center.x - v3_extents.x,
          v3_center.y - v3_extents.y,
          v3_center.z - v3_extents.z); // Front bottom left corner
      var v3_front_bottom_right = new Vector3(
          v3_center.x + v3_extents.x,
          v3_center.y - v3_extents.y,
          v3_center.z - v3_extents.z); // Front bottom right corner
      var v3_back_top_left = new Vector3(
          v3_center.x - v3_extents.x,
          v3_center.y + v3_extents.y,
          v3_center.z + v3_extents.z); // Back top left corner
      var v3_back_top_right = new Vector3(
          v3_center.x + v3_extents.x,
          v3_center.y + v3_extents.y,
          v3_center.z + v3_extents.z); // Back top right corner
      var v3_back_bottom_left = new Vector3(
          v3_center.x - v3_extents.x,
          v3_center.y - v3_extents.y,
          v3_center.z + v3_extents.z); // Back bottom left corner
      var v3_back_bottom_right = new Vector3(
          v3_center.x + v3_extents.x,
          v3_center.y - v3_extents.y,
          v3_center.z + v3_extents.z); // Back bottom right corner
      if (reference_transform) {
        v3_front_top_left = reference_transform.TransformPoint(v3_front_top_left);
        v3_front_top_right = reference_transform.TransformPoint(v3_front_top_right);
        v3_front_bottom_left = reference_transform.TransformPoint(v3_front_bottom_left);
        v3_front_bottom_right = reference_transform.TransformPoint(v3_front_bottom_right);
        v3_back_top_left = reference_transform.TransformPoint(v3_back_top_left);
        v3_back_top_right = reference_transform.TransformPoint(v3_back_top_right);
        v3_back_bottom_left = reference_transform.TransformPoint(v3_back_bottom_left);
        v3_back_bottom_right = reference_transform.TransformPoint(v3_back_bottom_right);
      }

      return new[] {
          v3_front_top_left,
          v3_front_top_right,
          v3_front_bottom_left,
          v3_front_bottom_right,
          v3_back_top_left,
          v3_back_top_right,
          v3_back_bottom_left,
          v3_back_bottom_right
      };
    }

    public static void DrawBox(
        Vector3 v3_front_top_left,
        Vector3 v3_front_top_right,
        Vector3 v3_front_bottom_left,
        Vector3 v3_front_bottom_right,
        Vector3 v3_back_top_left,
        Vector3 v3_back_top_right,
        Vector3 v3_back_bottom_left,
        Vector3 v3_back_bottom_right,
        Color color) {
      Debug.DrawLine(v3_front_top_left, v3_front_top_right, color);
      Debug.DrawLine(v3_front_top_right, v3_front_bottom_right, color);
      Debug.DrawLine(v3_front_bottom_right, v3_front_bottom_left, color);
      Debug.DrawLine(v3_front_bottom_left, v3_front_top_left, color);

      Debug.DrawLine(v3_back_top_left, v3_back_top_right, color);
      Debug.DrawLine(v3_back_top_right, v3_back_bottom_right, color);
      Debug.DrawLine(v3_back_bottom_right, v3_back_bottom_left, color);
      Debug.DrawLine(v3_back_bottom_left, v3_back_top_left, color);

      Debug.DrawLine(v3_front_top_left, v3_back_top_left, color);
      Debug.DrawLine(v3_front_top_right, v3_back_top_right, color);
      Debug.DrawLine(v3_front_bottom_right, v3_back_bottom_right, color);
      Debug.DrawLine(v3_front_bottom_left, v3_back_bottom_left, color);
    }
  }
}
