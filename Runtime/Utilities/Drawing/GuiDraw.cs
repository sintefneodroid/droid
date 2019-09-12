using UnityEngine;

namespace droid.Runtime.Utilities.Drawing {
  /// <summary>
  /// </summary>
  public static partial class NeodroidUtilities {
    static Texture2D _s_line_tex;

    static NeodroidUtilities() {
      _s_line_tex = new Texture2D(1,
                                  3,
                                  TextureFormat.ARGB32,
                                  true);
      _s_line_tex.SetPixel(0,
                           0,
                           new Color(1,
                                     1,
                                     1,
                                     0));
      _s_line_tex.SetPixel(0, 1, Color.white);
      _s_line_tex.SetPixel(0,
                           2,
                           new Color(1,
                                     1,
                                     1,
                                     0));
      _s_line_tex.Apply();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="p_point_a"></param>
    /// <param name="p_point_b"></param>
    /// <param name="p_width"></param>
    public static void DrawLine(Vector2 p_point_a, Vector2 p_point_b, float p_width) {
      var save_matrix = GUI.matrix;
      var save_color = GUI.color;

      var delta = p_point_b - p_point_a;
      GUIUtility.ScaleAroundPivot(new Vector2(delta.magnitude, p_width), Vector2.zero);
      GUIUtility.RotateAroundPivot(Vector2.Angle(delta, Vector2.right) * Mathf.Sign(delta.y), Vector2.zero);
      GUI.matrix = Matrix4x4.TRS(p_point_a, Quaternion.identity, Vector3.one) * GUI.matrix;

      GUI.DrawTexture(new Rect(Vector2.zero, Vector2.one), _s_line_tex);

      GUI.matrix = save_matrix;
      GUI.color = save_color;
    }
  }
}
