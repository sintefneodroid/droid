using UnityEditor;
using UnityEngine;

namespace droid.Editor.Utilities {
  /// <summary>
  /// </summary>
  public static class NeodroidEditorUtilities {
    static Color _link_color = new Color(0x00 / 255f,
                                         0x78 / 255f,
                                         0xDA / 255f,
                                         1f);

    static GUIStyle _default_link_style = new GUIStyle(EditorStyles.label) {
                                                                               fontSize = 14,
                                                                               wordWrap = false,
                                                                               normal = {
                                                                                            textColor =
                                                                                                _link_color
                                                                                        },
                                                                               stretchWidth = false
                                                                           };

    /// <summary>
    /// </summary>
    /// <param name="label"></param>
    /// <param name="link_style"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static bool LinkLabel(GUIContent label,
                                 GUIStyle link_style = null,
                                 params GUILayoutOption[] options) {
      if (link_style == null) {
        link_style = _default_link_style;
      }

      var position = GUILayoutUtility.GetRect(label, link_style, options);

      Handles.BeginGUI();

      Handles.color = link_style.normal.textColor;
      Handles.DrawLine(new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
      Handles.color = Color.white;
      Handles.EndGUI();

      EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);

      return GUI.Button(position, label, link_style);
    }
  }
}
