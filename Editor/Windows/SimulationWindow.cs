using droid.Runtime.Managers;
using droid.Runtime.Messaging.Messages;
using UnityEditor;
using UnityEngine;

namespace droid.Editor.Windows {
  /// <summary>
  ///
  /// </summary>
  public class SimulationWindow : EditorWindow {
    /// <summary>
    ///
    /// </summary>
    [MenuItem(EditorWindowMenuPath._WindowMenuPath + "SimulationWindow")]
    [MenuItem(EditorWindowMenuPath._ToolMenuPath + "SimulationWindow")]
    public static void ShowWindow() {
      GetWindow(typeof(SimulationWindow)); //Show existing window instance. If one doesn't exist, make one.
      //window.Show();
    }

    Texture _icon;
    NeodroidManager _simulation_manager;

    /// <summary>
    /// </summary>
    void OnEnable() {
      this._icon =
          (Texture2D)AssetDatabase.LoadAssetAtPath(NeodroidSettings.Current.NeodroidImportLocationProp
                                                   + "Gizmos/Icons/clock.png",
                                                   typeof(Texture2D));
      this.titleContent = new GUIContent("Neo:Sim", this._icon, "Window for controlling simulation");
    }

    void OnFocus() { this.Setup(); }

    void Setup() {
      var serialised_object = new SerializedObject(this);
      if (this._simulation_manager == null) {
        this._simulation_manager = FindObjectOfType<NeodroidManager>();
      }

      serialised_object.ApplyModifiedProperties();
    }

    void OnGUI() {
      EditorGUILayout.ObjectField(this._simulation_manager, typeof(AbstractNeodroidManager), true);
      EditorGUI.BeginDisabledGroup(!Application.isPlaying);

      if (GUILayout.Button("Step")) {
        this._simulation_manager?.SendToEnvironments(new[] {
                                                               new
                                                                   Reaction(new
                                                                                ReactionParameters(StepResetObserve
                                                                                                       .Step_,
                                                                                                   true,
                                                                                                   configure :
                                                                                                   true),
                                                                            null,
                                                                            null,
                                                                            null,
                                                                            null,
                                                                            "")
                                                           });
      }

      if (GUILayout.Button("Reset")) {
        this._simulation_manager?.ResetAllEnvironments();
      }

      if (this._simulation_manager) {
        this._simulation_manager.TestActuators =
            EditorGUILayout.Toggle("Test Actuators", this._simulation_manager.TestActuators);
      }

      EditorGUI.EndDisabledGroup();
    }
  }
}
