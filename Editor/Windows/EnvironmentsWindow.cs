#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Neodroid.Editor.Utilities;
using Neodroid.Runtime;
using Neodroid.Runtime.Environments;
using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.InternalReactions;
using Neodroid.Runtime.Managers;
using Neodroid.Runtime.Prototyping.Actors;
using Neodroid.Runtime.Prototyping.Configurables;
using Neodroid.Runtime.Prototyping.Displayers;
using Neodroid.Runtime.Prototyping.Evaluation;
using Neodroid.Runtime.Prototyping.Internals;
using Neodroid.Runtime.Prototyping.Motors;
using Neodroid.Runtime.Prototyping.Observers;
using Neodroid.Runtime.Utilities.Enums;
using Neodroid.Runtime.Utilities.GameObjects;
using Neodroid.Runtime.Utilities.Misc;
using Neodroid.Runtime.Utilities.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Neodroid.Editor.Windows {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class EnvironmentsWindow : EditorWindow {
    const int _logo_image_size = 100;

    const string _neodroid_url_text = "Documentation";
    const string _neodroid_url = "https://neodroid.ml/documentation";

    /// <summary>
    /// </summary>
    const bool _refresh_enabled = false;

    PrototypingEnvironment[] _environments;
    Texture _icon;
    Texture _neodroid_icon;

    PlayerReactions _player_reactions;
    Vector2 _scroll_position;
    bool[] _show_environment_properties = new bool[1];
    bool _show_detailed_descriptions = false;

    /// <summary>
    /// </summary>
    NeodroidManager _simulation_manager;

    /// <summary>
    /// </summary>
    [MenuItem(EditorWindowMenuPath._WindowMenuPath + "EnvironmentsWindow")]
    [MenuItem(EditorWindowMenuPath._ToolMenuPath + "EnvironmentsWindow")]
    public static void ShowWindow() {
      GetWindow(typeof(EnvironmentsWindow)); //Show existing window instance. If one doesn't exist, make one.
      //window.Show();
    }

    /// <summary>
    /// </summary>
    void OnEnable() {
      this._icon = (Texture2D)AssetDatabase.LoadAssetAtPath(
          NeodroidInfo._ImportLocation + "Gizmos/Icons/world.png",
          typeof(Texture2D));
      this._neodroid_icon = (Texture)AssetDatabase.LoadAssetAtPath(
          NeodroidInfo._ImportLocation + "Gizmos/Icons/neodroid_favicon_cut.png",
          typeof(Texture));
      this.titleContent = new GUIContent("Neo:Env", this._icon, "Window for configuring environments");
      this.Setup();
    }

    /// <summary>
    /// </summary>
    void Setup() {
      if (this._environments != null) {
        this._show_environment_properties = new bool[this._environments.Length];
      }
    }

    /// <summary>
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

        this._show_detailed_descriptions = EditorGUILayout.Toggle(
            "Show Details",
          this._show_detailed_descriptions);
  
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        this._environments = NeodroidUtilities.FindAllObjectsOfTypeInScene<PrototypingEnvironment>();
        if (this._show_environment_properties.Length != this._environments.Length) {
          this.Setup();
        }

        this._scroll_position = EditorGUILayout.BeginScrollView(this._scroll_position);

        EditorGUILayout.BeginVertical("Box");
        var num_active_environments = this._environments.Length;
        var num_inactive_environments = this._environments.Length - num_active_environments;
  
        //EditorGUILayout.BeginHorizontal();
  
          GUILayout.Label(
            $"Environments - Active({num_active_environments}), Inactive({num_inactive_environments}), Total({this._environments.Length})");
  
        //EditorGUILayout.EndHorizontal();
  
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

                this.DrawActors(actors);

                this.DrawObservers(observers);

                this.DrawConfigurables(configurables);

                this.DrawDisplayers(displayers);

                //this.DrawInternals(resetables, listeners);

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
        }
      }
    }

    #region GUIDRAWS

    void DrawInternals(
        Dictionary<String, IResetable> resetables,
        Dictionary<String, IEnvironmentListener> listeners) {
      EditorGUILayout.BeginVertical("Box");

      GUILayout.Label("Internals");
      foreach (var resetable in resetables) {
        var resetable_value = (Resetable)resetable.Value;
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

      foreach (var listener in listeners) {
        var listener_value_value = (EnvironmentListener)listener.Value;
        if (listener_value_value != null) {
          EditorGUILayout.BeginVertical("Box");
          listener_value_value.enabled = EditorGUILayout.BeginToggleGroup(
              listener.Key,
              listener_value_value.enabled && listener_value_value.gameObject.activeSelf);
          EditorGUILayout.ObjectField(listener_value_value, typeof(EnvironmentListener), true);
          EditorGUILayout.EndToggleGroup();
          EditorGUILayout.EndVertical();
        }
      }

      EditorGUILayout.EndVertical();
    }

    void DrawActors(Dictionary<string, IActor> actors) {
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

          this.DrawMotors(motors);

          EditorGUILayout.EndToggleGroup();

          EditorGUILayout.EndVertical();
        }
      }

      EditorGUILayout.EndVertical();
    }

    void DrawObservers(SortedDictionary<string, IObserver> observers) {
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
    }

    void DrawDisplayers(Dictionary<string, IDisplayer> displayers) {
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
    }

    void DrawConfigurables(Dictionary<string, IConfigurable> configurables) {
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
    }

    void DrawMotors(Dictionary<string, IMotor> motors) {
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
  
          if (this._show_detailed_descriptions) {
            EditorGUILayout.Vector3Field("Motion Space (min,gran,max)", motor_value.MotionValueSpace.ToVector3());
          }
  
          EditorGUILayout.EndToggleGroup();

          EditorGUILayout.EndVertical();
        }
      }

      EditorGUILayout.EndVertical();
    }

    #endregion

    /// <summary>
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
    /// </summary>
    void OnValidate() {
      if (EditorApplication.isPlaying || !_refresh_enabled) {
        return;
      }

      this.Refresh();
    }

    /// <summary>
    /// </summary>
    void OnHierarchyChange() { this.Refresh(); }

    /// <summary>
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