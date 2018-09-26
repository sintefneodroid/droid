using UnityEngine;

namespace Neodroid.Runtime.Utilities.Misc.Drawing {
  /// <summary>
  /// 
  /// </summary>
  public static class DrawArrow {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="direction"></param>
    /// <param name="color"></param>
    /// <param name="arrow_head_length"></param>
    /// <param name="arrow_head_angle"></param>
    public static void ForGizmo(
        Vector3 pos,
        Vector3 direction,
        Color color = default(Color),
        float arrow_head_length = 0.25f,
        float arrow_head_angle = 20.0f) {
      Gizmos.DrawRay(pos, direction);
      DrawArrowEnd(true, pos, direction, color, arrow_head_length, arrow_head_angle);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="direction"></param>
    /// <param name="color"></param>
    /// <param name="arrow_head_length"></param>
    /// <param name="arrow_head_angle"></param>
    /// <param name="ray_duration"></param>
    public static void ForDebug(
        Vector3 pos,
        Vector3 direction,
        Color color = default(Color),
        float arrow_head_length = 0.25f,
        float arrow_head_angle = 20.0f,
        float ray_duration = 0f) {
      if (ray_duration > 0) {
        Debug.DrawRay(pos, direction, color, ray_duration);
      } else {
        Debug.DrawRay(pos, direction, color);
      }

      DrawArrowEnd(false, pos, direction, color, arrow_head_length, arrow_head_angle, ray_duration);
    }

    static void DrawArrowEnd(
        bool gizmos,
        Vector3 pos,
        Vector3 direction,
        Color color = default(Color),
        float arrow_head_length = 0.25f,
        float arrow_head_angle = 20.0f,
        float ray_duration = 0f) {
      var right = Quaternion.LookRotation(direction)
                  * Quaternion.Euler(arrow_head_angle, 0, 0)
                  * Vector3.back;
      var left = Quaternion.LookRotation(direction)
                 * Quaternion.Euler(-arrow_head_angle, 0, 0)
                 * Vector3.back;
      var up = Quaternion.LookRotation(direction) * Quaternion.Euler(0, arrow_head_angle, 0) * Vector3.back;
      var down = Quaternion.LookRotation(direction)
                 * Quaternion.Euler(0, -arrow_head_angle, 0)
                 * Vector3.back;
      if (gizmos) {
        Gizmos.color = color;
        Gizmos.DrawRay(pos + direction, right * arrow_head_length);
        Gizmos.DrawRay(pos + direction, left * arrow_head_length);
        Gizmos.DrawRay(pos + direction, up * arrow_head_length);
        Gizmos.DrawRay(pos + direction, down * arrow_head_length);
      } else {
        if (ray_duration > 0) {
          Debug.DrawRay(pos + direction, right * arrow_head_length, color, ray_duration);
          Debug.DrawRay(pos + direction, left * arrow_head_length, color, ray_duration);
          Debug.DrawRay(pos + direction, up * arrow_head_length, color, ray_duration);
          Debug.DrawRay(pos + direction, down * arrow_head_length, color, ray_duration);
        } else {
          Debug.DrawRay(pos + direction, right * arrow_head_length, color);
          Debug.DrawRay(pos + direction, left * arrow_head_length, color);
          Debug.DrawRay(pos + direction, up * arrow_head_length, color);
          Debug.DrawRay(pos + direction, down * arrow_head_length, color);
        }
      }

/*
      var arrow_size = 2;
      Handles.color = Handles.xAxisColor;
      Handles.ArrowHandleCap( 0, pos, Quaternion.Euler(direction), arrow_size,EventType.Repaint );
 */
    }
  }
}