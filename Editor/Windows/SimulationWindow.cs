using droid.Runtime;
using droid.Runtime.Managers;
using droid.Runtime.Messaging.Messages;
using UnityEditor;
using UnityEngine;

namespace droid.Editor.Windows {
  public class SimulationWindow : EditorWindow {
    [MenuItem(EditorWindowMenuPath._WindowMenuPath + "SimulationWindow")]
    [MenuItem(EditorWindowMenuPath._ToolMenuPath + "SimulationWindow")]
    public static void ShowWindow() {
      GetWindow(typeof(SimulationWindow)); //Show existing window instance. If one doesn't exist, make one.
      //window.Show();
    }

    Texture _icon;
    PausableManager _simulation_manager;

    /// <summary>
    /// </summary>
    void OnEnable() {
      this._icon =
          (Texture2D)AssetDatabase.LoadAssetAtPath(NeodroidEditorInfo.ImportLocation
                                                   + "Gizmos/Icons/clock.png",
                                                   typeof(Texture2D));
      this.titleContent = new GUIContent("Neo:Sim", this._icon, "Window for controlling simulation");
      this.Setup();
    }

    void Setup() {
      var serialised_object = new SerializedObject(this);
      this._simulation_manager = FindObjectOfType<PausableManager>();

      serialised_object.ApplyModifiedProperties();
    }

    void OnGUI() {
      EditorGUI.BeginDisabledGroup(!Application.isPlaying);

      if (GUILayout.Button("Step")) {
        this._simulation_manager.ReactAndCollectStates(new Reaction(new ReactionParameters(true,
                                                                                           true,
                                                                                           episode_count :
                                                                                           true),
                                                                    null,
                                                                    null,
                                                                    null,
                                                                    null,
                                                                    ""));
      }

      if (GUILayout.Button("Reset")) {
        this._simulation_manager.ReactAndCollectStates(new Reaction(new ReactionParameters(true,
                                                                                           false,
                                                                                           true,
                                                                                           episode_count :
                                                                                           true),
                                                                    null,
                                                                    null,
                                                                    null,
                                                                    null,
                                                                    ""));
      }

      EditorGUI.EndDisabledGroup();
    }
  }
}
