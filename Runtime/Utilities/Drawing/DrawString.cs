using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace droid.Runtime.Utilities.Drawing {
  public static partial class NeodroidUtilities {
    /// <summary>
    /// </summary>
    /// <param name="text"></param>
    /// <param name="world_pos"></param>
    /// <param name="color"></param>
    /// <param name="o_x"></param>
    /// <param name="o_y"></param>
    public static void DrawString(string text,
                                  Vector3 world_pos,
                                  Color? color = null,
                                  float o_x = 0,
                                  float o_y = 0) {
      Handles.BeginGUI();

      var restore_color = GUI.color;

      if (color.HasValue) {
        GUI.color = color.Value;
      }

      var view = SceneView.currentDrawingSceneView;
      var screen_pos = view.camera.WorldToScreenPoint(world_pos);

      if (screen_pos.y < 0
          || screen_pos.y > Screen.height
          || screen_pos.x < 0
          || screen_pos.x > Screen.width
          || screen_pos.z < 0) {
        GUI.color = restore_color;
        Handles.EndGUI();
        return;
      }

      Handles.Label(TransformByPixel(world_pos, o_x, o_y), text);

      GUI.color = restore_color;
      Handles.EndGUI();
    }

    /// <summary>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static Vector3 TransformByPixel(Vector3 position, float x, float y) {
      return TransformByPixel(position, new Vector3(x, y));
    }

    /// <summary>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="translate_by"></param>
    /// <returns></returns>
    public static Vector3 TransformByPixel(Vector3 position, Vector3 translate_by) {
      var cam = SceneView.currentDrawingSceneView.camera;
      return cam ? cam.ScreenToWorldPoint(cam.WorldToScreenPoint(position) + translate_by) : position;
    }
  }
}
#endif
