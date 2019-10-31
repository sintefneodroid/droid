using droid.Runtime.Prototyping.ObjectiveFunctions;
using droid.Runtime.Prototyping.Unobservables;
#if UNITY_EDITOR && NEODROID_DEBUG
using droid.Runtime.Utilities.InternalReactions;
using droid.Runtime.Environments;
using droid.Runtime.Managers;
using droid.Runtime.Prototyping.Actors;
using droid.Runtime.Prototyping.Configurables;
using droid.Runtime.Prototyping.Displayers;
using droid.Runtime.Prototyping.Actuators;
using droid.Runtime.Prototyping.Sensors;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace droid.Editor.Windows {
  /// <summary>
  ///
  /// </summary>
  public class DebugWindow : EditorWindow {
    Actor[] _actors;

    Configurable[] _configurables;
    bool _debug_all;

    Displayer[] _displayers;

    NeodroidEnvironment[] _environments;

    Texture _icon;

    Unobservable[] _listeners;

    AbstractNeodroidManager _manager;

    Actuator[] _actuators;

    ObjectiveFunction[] _objective_functions_function;

    Sensor[] _sensors;

    PlayerReactions _player_reactions;

    Vector2 _scroll_position;

    bool _show_actors_debug;
    bool _show_configurables_debug;
    bool _show_displayers_debug;
    bool _show_environments_debug;
    bool _show_listeners_debug;
    bool _show_actuators_debug;
    bool _show_objective_functions_debug;
    bool _show_sensors_debug;
    bool _show_player_reactions_debug;
    bool _show_simulation_manager_debug;

    /// <summary>
    ///
    /// </summary>
    [MenuItem(EditorWindowMenuPath._WindowMenuPath + "DebugWindow")]
    [MenuItem(EditorWindowMenuPath._ToolMenuPath + "DebugWindow")]
    public static void ShowWindow() {
      GetWindow<DebugWindow>(); //Show existing window instance. If one doesn't exist, make one.
    }

    void OnEnable() {
      this.FindObjects();
      this._icon =
          (Texture2D)AssetDatabase.LoadAssetAtPath(NeodroidSettings.Current.NeodroidImportLocationProp
                                                   + "Gizmos/Icons/information.png",
                                                   typeof(Texture2D));
      this.titleContent = new GUIContent("Neo:Debug", this._icon, "Window for controlling debug messages");
    }

    void FindObjects() {
      this._manager = FindObjectOfType<NeodroidManager>();
      this._environments = FindObjectsOfType<NeodroidEnvironment>();
      this._actors = FindObjectsOfType<Actor>();
      this._actuators = FindObjectsOfType<Actuator>();
      this._sensors = FindObjectsOfType<Sensor>();
      this._configurables = FindObjectsOfType<Configurable>();
      this._objective_functions_function = FindObjectsOfType<ObjectiveFunction>();
      this._displayers = FindObjectsOfType<Displayer>();
      this._listeners = FindObjectsOfType<Unobservable>();
      this._player_reactions = FindObjectOfType<PlayerReactions>();
    }

    void EnableAll() {
      this._show_simulation_manager_debug = true;
      this._show_player_reactions_debug = true;
      this._show_environments_debug = true;
      this._show_actors_debug = true;
      this._show_actuators_debug = true;
      this._show_sensors_debug = true;
      this._show_configurables_debug = true;
      this._show_objective_functions_debug = true;
      this._show_displayers_debug = true;
      this._show_listeners_debug = true;
    }

    void DisableAll() {
      this._show_simulation_manager_debug = false;
      this._show_player_reactions_debug = false;
      this._show_environments_debug = false;
      this._show_actors_debug = false;
      this._show_actuators_debug = false;
      this._show_sensors_debug = false;
      this._show_configurables_debug = false;
      this._show_objective_functions_debug = false;
      this._show_displayers_debug = false;
      this._show_listeners_debug = false;
    }

    bool AreAllChecked() {
      if (this._show_simulation_manager_debug
          && this._show_player_reactions_debug
          && this._show_environments_debug
          && this._show_actors_debug
          && this._show_actuators_debug
          && this._show_sensors_debug
          && this._show_configurables_debug
          && this._show_objective_functions_debug
          && this._show_displayers_debug
          && this._show_listeners_debug) {
        return true;
      }

      return false;
    }

    void OnGUI() {
      this.FindObjects();

      var prev_debug_all = this._debug_all;
      this._debug_all = EditorGUILayout.Toggle("Debug everything", this._debug_all);
      if (this._debug_all != prev_debug_all) {
        if (this._debug_all) {
          this.EnableAll();
        } else {
          this.DisableAll();
        }
      }

      EditorGUILayout.Separator();

      this._scroll_position = EditorGUILayout.BeginScrollView(this._scroll_position);
      EditorGUILayout.BeginVertical("Box");

      this._show_simulation_manager_debug =
          EditorGUILayout.Toggle("Debug simulation manager", this._show_simulation_manager_debug);
      this._show_player_reactions_debug =
          EditorGUILayout.Toggle("Debug player reactions", this._show_player_reactions_debug);
      this._show_environments_debug =
          EditorGUILayout.Toggle("Debug all environments", this._show_environments_debug);
      this._show_actors_debug = EditorGUILayout.Toggle("Debug all actors", this._show_actors_debug);
      this._show_actuators_debug = EditorGUILayout.Toggle("Debug all Actuators", this._show_actuators_debug);
      this._show_sensors_debug = EditorGUILayout.Toggle("Debug all sensors", this._show_sensors_debug);
      this._show_configurables_debug =
          EditorGUILayout.Toggle("Debug all configurables", this._show_configurables_debug);
      this._show_objective_functions_debug =
          EditorGUILayout.Toggle("Debug all objective functions", this._show_objective_functions_debug);
      this._show_displayers_debug =
          EditorGUILayout.Toggle("Debug all displayers", this._show_displayers_debug);
      this._show_listeners_debug = EditorGUILayout.Toggle("Debug all listeners", this._show_listeners_debug);

      EditorGUILayout.EndVertical();

      EditorGUILayout.EndScrollView();

      this._debug_all = this.AreAllChecked();

      if (GUILayout.Button("Apply")) {
        if (this._manager != null) {
          this._manager.Debugging = this._show_simulation_manager_debug;
        }

        if (this._player_reactions != null) {
          this._player_reactions.Debugging = this._show_player_reactions_debug;
        }

        foreach (var environment in this._environments) {
          environment.Debugging = this._show_environments_debug;
        }

        foreach (var actor in this._actors) {
          actor.Debugging = this._show_actors_debug;
        }

        foreach (var actuator in this._actuators) {
          actuator.Debugging = this._show_actuators_debug;
        }

        foreach (var observer in this._sensors) {
          observer.Debugging = this._show_sensors_debug;
        }

        foreach (var configurable in this._configurables) {
          configurable.Debugging = this._show_configurables_debug;
        }

        foreach (var objective_functions in this._objective_functions_function) {
          objective_functions.Debugging = this._show_objective_functions_debug;
        }

        foreach (var displayer in this._displayers) {
          displayer.Debugging = this._show_displayers_debug;
        }

        foreach (var listener in this._listeners) {
          listener.Debugging = this._show_listeners_debug;
        }
      }

      if (GUI.changed && !Application.isPlaying) {
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        // Unity not tracking changes to properties of a GameObject made through this window automatically and
        // are not saved unless other changes are made from a working inspector window
      }
    }

    /// <summary>
    ///
    /// </summary>
    public void OnInspectorUpdate() { this.Repaint(); }
  }
}
#endif
