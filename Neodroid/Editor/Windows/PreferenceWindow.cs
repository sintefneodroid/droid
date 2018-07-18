using UnityEditor;
using UnityEngine;

namespace Neodroid.Editor.Windows {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class PreferenceWindow : MonoBehaviour {
    static bool _prefs_loaded = false;

    /// <summary>
    ///
    /// </summary>
    public static bool _BoolPreference = false;

    const string _debug_pref_key = "EnableNeodroidDebug";

    /// <summary>
    ///
    /// </summary>
    [PreferenceItem("Neodroid")]
    public static void PreferencesGui() {
      if (!_prefs_loaded) {
        _BoolPreference = EditorPrefs.GetBool(_debug_pref_key, false);
        _prefs_loaded = true;
      }

      _BoolPreference = EditorGUILayout.Toggle("Bool Preference", _BoolPreference);

      if (_BoolPreference) {
        print("sss");
      }

      if (GUI.changed) {
        EditorPrefs.SetBool(_debug_pref_key, _BoolPreference);
      }
    }
  }
}
