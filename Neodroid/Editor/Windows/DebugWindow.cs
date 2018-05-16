#if UNITY_EDITOR
using Neodroid.Environments;
using Neodroid.Managers;
using Neodroid.Prototyping.Actors;
using Neodroid.Prototyping.Configurables;
using Neodroid.Prototyping.Evaluation;
using Neodroid.Prototyping.Motors;
using Neodroid.Prototyping.Observers;
using Neodroid.Prototyping.Displayers;
using UnityEditor;
using UnityEngine;

namespace Neodroid.Editor.Windows {
  public class DebugWindow : EditorWindow {
    Actor[] _actors;

    ConfigurableGameObject[] _configurables;

    PrototypingEnvironment[] _environments;

    Texture _icon;

    Motor[] _motors;

    ObjectiveFunction[] _objective_functions;

    Observer[] _observers;

    Displayer[] _displayers;

    Prototyping.Internals.Resetable[] _resetables;

    Prototyping.Internals.EnvironmentListener[] _listeners;

    PlayerControls.PlayerReactions _player_reactions;

    NeodroidManager _manager;

    bool _show_actors_debug;
    bool _show_configurables_debug;
    bool _show_environments_debug;
    bool _show_motors_debug;
    bool _show_objective_functions_debug;
    bool _show_observers_debug;
    bool _show_simulation_manager_debug;
    bool _show_displayers_debug;
    bool _show_player_reactions_debug;
    bool _show_resetables_debug;
    bool _show_listeners_debug;
    bool _debug_all;

    [MenuItem(EditorWindowMenuPath._WindowMenuPath + "DebugWindow")]
    [MenuItem(EditorWindowMenuPath._ToolMenuPath + "DebugWindow")]
    public static void ShowWindow() {
      GetWindow<DebugWindow>(); //Show existing window instance. If one doesn't exist, make one.
    }

    void OnEnable() {
      this.FindObjects();
      this._icon = (Texture2D)AssetDatabase.LoadAssetAtPath(
          "Assets/Neodroid/Gizmos/Icons/information.png",
          typeof(Texture2D));
      this.titleContent = new GUIContent("Neo:Debug", this._icon, "Window for controlling debug messages");
    }

    void FindObjects() {
      this._manager = FindObjectOfType<PausableManager>();
      this._environments = FindObjectsOfType<PrototypingEnvironment>();
      this._actors = FindObjectsOfType<Actor>();
      this._motors = FindObjectsOfType<Motor>();
      this._observers = FindObjectsOfType<Observer>();
      this._configurables = FindObjectsOfType<ConfigurableGameObject>();
      this._objective_functions = FindObjectsOfType<ObjectiveFunction>();
      this._displayers = FindObjectsOfType<Displayer>();
      this._listeners = FindObjectsOfType<Prototyping.Internals.EnvironmentListener>();
      this._resetables = FindObjectsOfType<Prototyping.Internals.Resetable>();
      this._player_reactions = FindObjectOfType<PlayerControls.PlayerReactions>();
    }

    void EnableAll() {
      this._show_simulation_manager_debug = true;
      this._show_player_reactions_debug = true;
      this._show_environments_debug = true;
      this._show_actors_debug = true;
      this._show_motors_debug = true;
      this._show_observers_debug = true;
      this._show_configurables_debug = true;
      this._show_objective_functions_debug = true;
      this._show_displayers_debug = true;
      this._show_resetables_debug = true;
      this._show_listeners_debug = true;
    }

    void DisableAll() {
      this._show_simulation_manager_debug = false;
      this._show_player_reactions_debug = false;
      this._show_environments_debug = false;
      this._show_actors_debug = false;
      this._show_motors_debug = false;
      this._show_observers_debug = false;
      this._show_configurables_debug = false;
      this._show_objective_functions_debug = false;
      this._show_displayers_debug = false;
      this._show_resetables_debug = false;
      this._show_listeners_debug = false;
    }

    bool AreAllChecked() {
      if (this._show_simulation_manager_debug
          && this._show_player_reactions_debug
          && this._show_environments_debug
          && this._show_actors_debug
          && this._show_motors_debug
          && this._show_observers_debug
          && this._show_configurables_debug
          && this._show_objective_functions_debug
          && this._show_displayers_debug
          && this._show_resetables_debug
          && this._show_listeners_debug) {
        return true;
      }

      return false;
    }

    void OnGUI() {
      this.FindObjects();

      var prev_debug_all = this._debug_all;
      this._debug_all = EditorGUILayout.Toggle(
          "Debug everything",
          this._debug_all);
      if (this._debug_all != prev_debug_all) {
        if(this._debug_all) {
          this.EnableAll();
        } else {
          this.DisableAll();
        }
      }

      EditorGUILayout.Separator();

      this._show_simulation_manager_debug = EditorGUILayout.Toggle(
          "Debug simulation manager",
          this._show_simulation_manager_debug);
      this._show_player_reactions_debug = EditorGUILayout.Toggle(
          "Debug player reactions",
          this._show_player_reactions_debug);
      this._show_environments_debug = EditorGUILayout.Toggle(
          "Debug all environments",
          this._show_environments_debug);
      this._show_actors_debug = EditorGUILayout.Toggle("Debug all actors", this._show_actors_debug);
      this._show_motors_debug = EditorGUILayout.Toggle("Debug all motors", this._show_motors_debug);
      this._show_observers_debug = EditorGUILayout.Toggle("Debug all observers", this._show_observers_debug);
      this._show_configurables_debug = EditorGUILayout.Toggle(
          "Debug all configurables",
          this._show_configurables_debug);
      this._show_objective_functions_debug = EditorGUILayout.Toggle(
          "Debug all objective functions",
          this._show_objective_functions_debug);
      this._show_displayers_debug = EditorGUILayout.Toggle(
          "Debug all displayers",
          this._show_displayers_debug);

      this._show_resetables_debug = EditorGUILayout.Toggle(
          "Debug all resetables",
          this._show_resetables_debug);
      this._show_listeners_debug = EditorGUILayout.Toggle(
          "Debug all listeners",
          this._show_listeners_debug);

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

        foreach (var motor in this._motors) {
          motor.Debugging = this._show_motors_debug;
        }

        foreach (var observer in this._observers) {
          observer.Debugging = this._show_observers_debug;
        }

        foreach (var configurable in this._configurables) {
          configurable.Debugging = this._show_configurables_debug;
        }

        foreach (var objective_functions in this._objective_functions) {
          objective_functions.Debugging = this._show_objective_functions_debug;
        }

        foreach (var displayer in this._displayers) {
          displayer.Debugging = this._show_displayers_debug;
        }

        foreach (var resetable in this._resetables) {
          resetable.Debugging = this._show_resetables_debug;
        }

        foreach (var listener in this._listeners) {
          listener.Debugging = this._show_listeners_debug;
        }
      }

      /*if (GUI.changed) {
      EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
      // Unity not tracking changes to properties of gameobject made through this window automatically and
      // are not saved unless other changes are made from a working inpector window
      }*/
    }

    public void OnInspectorUpdate() { this.Repaint(); }
  }
}
#endif
