#if UNITY_EDITOR
using System;
using droid.Editor.Utilities.Commands;
using UnityEditor;
using UnityEngine;

namespace droid.Editor.Windows.Agent {
  public class AgentWindow : EditorWindow {
    [MenuItem(itemName : EditorWindowMenuPath._WindowMenuPath+"/Agents")]
    static void ShowWindow() { GetWindow<AgentWindow>(); }

    //string _last_message = String.Empty;

    void OnEnable() { this.titleContent = new GUIContent("Agents"); }

    void OnGUI() {
      if (GUILayout.Button("DQN-Agent")) {
        Commands.RunDqnAgent();
      }

      if (GUILayout.Button("PG-Agent")) {
        Commands.RunPgAgent();
      }

      //GUILayout.Label(text : this._last_message);
    }
  }
}
#endif
