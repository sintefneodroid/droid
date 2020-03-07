#if UNITY_EDITOR
using System;
using droid.Editor.Utilities.Commands;
using UnityEditor;
using UnityEngine;

namespace droid.Editor.Windows.Repl {
  public class PythonReplWindow : EditorWindow {
    string _last_message = string.Empty;
    string _cmd= string.Empty;
    string _python = "/usr/bin/python";

    [MenuItem(itemName : EditorWindowMenuPath._WindowMenuPath+"/REPLs/" + "Python Repl")]
    static void ShowWindow() {
      GetWindow<PythonReplWindow>();
    }

    void OnEnable() {       this.titleContent = new GUIContent("Python Repl"); }
    void OnGUI() {
      this._python = GUILayout.TextField(text : this._python);
      this._cmd = GUILayout.TextField(text : this._cmd);
      if (GUILayout.Button("Run")) {
        this._last_message = Commands.PythonCommand(input : this._cmd,python_path: this._python);
      }

      GUILayout.Label(text : this._last_message);
    }
  }
}
#endif
