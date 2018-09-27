#if UNITY_EDITOR
using Neodroid.Editor.Utilities;
using Neodroid.Runtime;
using Neodroid.Runtime.Environments;
using Neodroid.Runtime.Managers;
using Neodroid.Runtime.PlayerControls;
using Neodroid.Runtime.Prototyping.Actors;
using Neodroid.Runtime.Prototyping.Configurables;
using Neodroid.Runtime.Prototyping.Displayers;
using Neodroid.Runtime.Prototyping.Evaluation;
using Neodroid.Runtime.Prototyping.Internals;
using Neodroid.Runtime.Prototyping.Motors;
using Neodroid.Runtime.Prototyping.Observers;
using Neodroid.Runtime.Utilities.Enums;
using Neodroid.Runtime.Utilities.GameObjects;
using Neodroid.Runtime.Utilities.Misc.Drawing;
using Neodroid.Runtime.Utilities.Misc.Grasping;
using Neodroid.Runtime.Utilities.ScriptableObjects;
using UnityEditor;
using UnityEngine;
using NeodroidUtilities = Neodroid.Runtime.Utilities.Misc.NeodroidUtilities;

namespace Neodroid.Editor.Windows {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class SimulationWindow : EditorWindow {
    const int _logo_image_size = 100;
    PrototypingEnvironment[] _environments;
    Texture _icon;
    Texture _neodroid_icon;
    Vector2 _scroll_position;
    bool[] _show_environment_properties = new bool[1];

    /// <summary>
    ///
    /// </summary>
    NeodroidManager _simulation_manager;

    PlayerReactions _player_reactions;

    const string _neodroid_url_text = "Documentation";
    const string _neodroid_url = "https://github.com/sintefneodroid/droid";

    /// <summary>
    ///
    /// </summary>
    const bool _refresh_enabled = false;

    /// <summary>
    ///
    /// </summary>
    [MenuItem(EditorWindowMenuPath._WindowMenuPath + "SimulationWindow"),
     MenuItem(EditorWindowMenuPath._ToolMenuPath + "SimulationWindow")]
    public static void ShowWindow() {
      GetWindow(typeof(SimulationWindow)); //Show existing window instance. If one doesn't exist, make one.
      //window.Show();
    }

    /// <summary>
    ///
    /// </summary>
    void OnEnable() {
      this._icon = (Texture2D)AssetDatabase.LoadAssetAtPath(
          NeodroidInfo._ImportLocation + "Gizmos/Icons/world.png",
          typeof(Texture2D));
      this._neodroid_icon = (Texture)AssetDatabase.LoadAssetAtPath(
          NeodroidInfo._ImportLocation + "Gizmos/Icons/neodroid_favicon_cut.png",
          typeof(Texture));
      this.titleContent = new GUIContent("Neo:Sim", this._icon, "Window for configuring simulation");
      this.Setup();
    }

    /// <summary>
    ///
    /// </summary>
    void Setup() {
      if (this._environments != null) {
        this._show_environment_properties = new bool[this._environments.Length];
      }
    }

    /// <summary>
    ///
    /// </summary>
    void OnGUI() {
      var serialised_object = new SerializedObject(this);
      this._simulation_manager = FindObjectOfType<PausableManager>();
      if (this._simulation_manager) {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();
        GUILayout.Label(
            this._neodroid_icon,
            GUILayout.Width(_logo_image_size),
            GUILayout.Height(_logo_image_size));

        if (NeodroidEditorUtilities.LinkLabel(new GUIContent(_neodroid_url_text))) {
          Application.OpenURL(_neodroid_url);
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.ObjectField(this._simulation_manager, typeof(NeodroidManager), true);

        this._simulation_manager.Configuration = (SimulatorConfiguration)EditorGUILayout.ObjectField(
            (SimulatorConfiguration)this._simulation_manager.Configuration,
            typeof(SimulatorConfiguration),
            true);

        this._simulation_manager.Configuration.FrameSkips = EditorGUILayout.IntField(
            "Frame Skips",
            this._simulation_manager.Configuration.FrameSkips);
        this._simulation_manager.Configuration.ResetIterations = EditorGUILayout.IntField(
            "Reset Iterations",
            this._simulation_manager.Configuration.ResetIterations);
        this._simulation_manager.Configuration.SimulationType = (SimulationType)EditorGUILayout.EnumPopup(
            "Simulation Type",
            this._simulation_manager.Configuration.SimulationType);
        this._simulation_manager.TestMotors = EditorGUILayout.Toggle(
            "Test Motors",
            this._simulation_manager.TestMotors);

        this._player_reactions = FindObjectOfType<PlayerReactions>();
        EditorGUILayout.ObjectField(this._player_reactions, typeof(PlayerReactions), true);

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        this._environments = NeodroidUtilities.FindAllObjectsOfTypeInScene<PrototypingEnvironment>();
        if (this._show_environment_properties.Length != this._environments.Length) {
          this.Setup();
        }

        this._scroll_position = EditorGUILayout.BeginScrollView(this._scroll_position);

        EditorGUILayout.BeginVertical("Box");
        var num_active_environments = this._environments.Length; //TODO: Calculate actual number
        var num_inactive_environments =
            this._environments.Length - num_active_environments; //TODO: Calculate actual number
        GUILayout.Label(
            $"Environments - Active({num_active_environments}), Inactive({num_inactive_environments}), Total({this._environments.Length})");
        if (this._show_environment_properties != null) {
          for (var i = 0; i < this._show_environment_properties.Length; i++) {
            if (this._environments[i].isActiveAndEnabled) {
              this._show_environment_properties[i] = EditorGUILayout.Foldout(
                  this._show_environment_properties[i],
                  $"{this._environments[i].Identifier}");
              if (this._show_environment_properties[i]) {
                var actors = this._environments[i].Actors;
                var observers = this._environments[i].Observers;
                var configurables = this._environments[i].Configurables;
                var resetables = this._environments[i].Resetables;
                var listeners = this._environments[i].Listeners;
                var displayers = this._environments[i].Displayers;

                EditorGUILayout.BeginVertical("Box");
                this._environments[i].enabled = EditorGUILayout.BeginToggleGroup(
                    this._environments[i].Identifier,
                    this._environments[i].enabled && this._environments[i].gameObject.activeSelf);
                EditorGUILayout.ObjectField(this._environments[i], typeof(PrototypingEnvironment), true);
                this._environments[i].CoordinateSystem = (CoordinateSystem)EditorGUILayout.EnumPopup(
                    "Coordinate system",
                    this._environments[i].CoordinateSystem);
                EditorGUI.BeginDisabledGroup(
                    this._environments[i].CoordinateSystem != CoordinateSystem.Relative_to_reference_point_);
                this._environments[i].CoordinateReferencePoint = (Transform)EditorGUILayout.ObjectField(
                    "Reference point",
                    this._environments[i].CoordinateReferencePoint,
                    typeof(Transform),
                    true);
                EditorGUI.EndDisabledGroup();
                this._environments[i].ObjectiveFunction = (ObjectiveFunction)EditorGUILayout.ObjectField(
                    "Objective function",
                    (ObjectiveFunction)this._environments[i].ObjectiveFunction,
                    typeof(ObjectiveFunction),
                    true);
                this._environments[i].EpisodeLength = EditorGUILayout.IntField(
                    "Episode Length",
                    this._environments[i].EpisodeLength);

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.LabelField("Info:");
                EditorGUILayout.Toggle("Terminated", this._environments[i].Terminated);
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.BeginVertical("Box");
                GUILayout.Label("Actors");
                foreach (var actor in actors) {
                  var actor_value = (Actor)actor.Value;
                  if (actor_value != null) {
                    var motors = actor_value.Motors;

                    EditorGUILayout.BeginVertical("Box");

                    actor_value.enabled = EditorGUILayout.BeginToggleGroup(
                        actor.Key,
                        actor_value.enabled && actor_value.gameObject.activeSelf);
                    EditorGUILayout.ObjectField(actor_value, typeof(Actor), true);

                    EditorGUILayout.BeginVertical("Box");
                    GUILayout.Label("Motors");
                    foreach (var motor in motors) {
                      var motor_value = (Motor)motor.Value;
                      if (motor_value != null) {
                        EditorGUILayout.BeginVertical("Box");
                        motor_value.enabled = EditorGUILayout.BeginToggleGroup(
                            motor.Key,
                            motor_value.enabled && motor_value.gameObject.activeSelf);
                        EditorGUILayout.ObjectField(motor_value, typeof(Motor), true);
                        EditorGUILayout.EndToggleGroup();

                        EditorGUILayout.EndVertical();
                      }
                    }

                    EditorGUILayout.EndVertical();

                    EditorGUILayout.EndToggleGroup();

                    EditorGUILayout.EndVertical();
                  }
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("Box");
                GUILayout.Label("Observers");
                foreach (var observer in observers) {
                  var observer_value = (Observer)observer.Value;
                  if (observer_value != null) {
                    EditorGUILayout.BeginVertical("Box");
                    observer_value.enabled = EditorGUILayout.BeginToggleGroup(
                        observer.Key,
                        observer_value.enabled && observer_value.gameObject.activeSelf);
                    EditorGUILayout.ObjectField(observer_value, typeof(Observer), true);
                    EditorGUILayout.EndToggleGroup();
                    EditorGUILayout.EndVertical();
                  }
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("Box");
                GUILayout.Label("Configurables");
                foreach (var configurable in configurables) {
                  var configurable_value = (Configurable)configurable.Value;
                  if (configurable_value != null) {
                    EditorGUILayout.BeginVertical("Box");
                    configurable_value.enabled = EditorGUILayout.BeginToggleGroup(
                        configurable.Key,
                        configurable_value.enabled && configurable_value.gameObject.activeSelf);
                    EditorGUILayout.ObjectField(configurable_value, typeof(Configurable), true);
                    EditorGUILayout.EndToggleGroup();
                    EditorGUILayout.EndVertical();
                  }
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("Box");
                GUILayout.Label("Displayers");
                foreach (var displayer in displayers) {
                  var displayer_value = (Displayer)displayer.Value;
                  if (displayer_value != null) {
                    EditorGUILayout.BeginVertical("Box");
                    displayer_value.enabled = EditorGUILayout.BeginToggleGroup(
                        displayer.Key,
                        displayer_value.enabled && displayer_value.gameObject.activeSelf);
                    EditorGUILayout.ObjectField(displayer_value, typeof(Displayer), true);
                    EditorGUILayout.EndToggleGroup();
                    EditorGUILayout.EndVertical();
                  }
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("Box");
                /*
                GUILayout.Label("Internals");
                foreach (var resetable in resetables) {
                  var resetable_value = (Resetable) resetable.Value;
                  if (resetable_value != null) {

                    EditorGUILayout.BeginVertical("Box");
                    resetable_value.enabled = EditorGUILayout.BeginToggleGroup(
                        resetable.Key,
                        resetable_value.enabled && resetable_value.gameObject.activeSelf);
                    EditorGUILayout.ObjectField(resetable_value, typeof(Resetable), true);
                    EditorGUILayout.EndToggleGroup();
                    EditorGUILayout.EndVertical();
                  }
                }
                */

/*
                foreach (var listener in listeners) {
                  if (listener.Value != null) {
                    EditorGUILayout.BeginVertical("Box");
                    listener.Value.enabled = EditorGUILayout.BeginToggleGroup(
                        listener.Key,
                        listener.Value.enabled && listener.Value.gameObject.activeSelf);
                    EditorGUILayout.ObjectField(listener.Value, typeof(EnvironmentListener), true);
                    EditorGUILayout.EndToggleGroup();
                    EditorGUILayout.EndVertical();
                  }
                }
*/
                EditorGUILayout.EndVertical();

                EditorGUILayout.EndToggleGroup();
                EditorGUILayout.EndVertical();
              }
            }
          }

          EditorGUILayout.EndVertical();

          EditorGUILayout.BeginVertical("Box");
          GUILayout.Label("Disabled environments");
          for (var i = 0; i < this._show_environment_properties.Length; i++) {
            if (!this._environments[i].isActiveAndEnabled) {
              EditorGUILayout.ObjectField(this._environments[i], typeof(NeodroidEnvironment), true);
            }
          }

          EditorGUILayout.EndVertical();

          EditorGUILayout.EndScrollView();
          serialised_object.ApplyModifiedProperties();

          /*
          if (GUILayout.Button("Refresh")) {
            this.Refresh();
          }

          EditorGUI.BeginDisabledGroup(!Application.isPlaying);

          if (GUILayout.Button("Step")) {
            this._simulation_manager.ReactAndCollectStates(
                new Reaction(
                    new ReactionParameters(true, true, episode_count:true),
                    null,
                    null,
                    null,
                    null,
                    ""));
          }

          if (GUILayout.Button("Reset")) {
            this._simulation_manager.ReactAndCollectStates(
                new Reaction(
                    new ReactionParameters(true, false, true, episode_count:true),
                    null,
                    null,
                    null,
                    null,
                    ""));
          }

          EditorGUI.EndDisabledGroup();
          */
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    void Refresh() {
      if (this._simulation_manager) {
        this._simulation_manager.Clear();
      }

      var prototyping_game_objects = FindObjectsOfType<PrototypingGameObject>();
      foreach (var obj in prototyping_game_objects) {
        obj.RefreshAwake();
      }

      foreach (var obj in prototyping_game_objects) {
        obj.RefreshStart();
      }
    }

    /// <summary>
    ///
    /// </summary>
    void OnValidate() {
      if (EditorApplication.isPlaying || !_refresh_enabled) {
        return;
      }

      this.Refresh();
    }

    /// <summary>
    ///
    /// </summary>
    void OnHierarchyChange() { this.Refresh(); }

    /// <summary>
    ///
    /// </summary>
    public void OnInspectorUpdate() {
      this.Repaint();
      if (GUI.changed) {
        this.Refresh();
      }
    }
  }
}
#endif
