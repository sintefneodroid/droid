#if UNITY_EDITOR
using System;
using droid.Editor.Utilities.Commands;
using UnityEditor;
using UnityEngine;

namespace droid.Editor.Windows.Repl {
  public class SystemReplWindow : EditorWindow {
    [MenuItem(itemName : EditorWindowMenuPath._WindowMenuPath+"/REPLs/" + "System Repl")]
    static void ShowWindow() { GetWindow<SystemReplWindow>(); }

    string _last_message = string.Empty;
    string _cmd = string.Empty;
    string _args = string.Empty;

    void OnEnable() { this.titleContent = new GUIContent("System Repl"); }

    void OnGUI() {
      this._cmd = GUILayout.TextField(text : this._cmd);
      this._args = GUILayout.TextField(text : this._args);
      if (GUILayout.Button("Run")) {
        this._last_message = Commands.SystemCommand(input : this._cmd, arguments : this._args);
      }

      GUILayout.Label(text : this._last_message);
    }
  }
}
#endif
