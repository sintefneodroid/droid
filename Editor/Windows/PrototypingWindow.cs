using System;
using System.Collections.Generic;
using droid.Editor.Utilities;
using droid.Runtime.Enums;
using droid.Runtime.Environments;
using droid.Runtime.GameObjects;
using droid.Runtime.Interfaces;
using droid.Runtime.Managers;
using droid.Runtime.Prototyping.Actors;
using droid.Runtime.Prototyping.Actuators;
using droid.Runtime.Prototyping.Configurables;
using droid.Runtime.Prototyping.Displayers;
using droid.Runtime.Prototyping.ObjectiveFunctions;
using droid.Runtime.Prototyping.Sensors;
using droid.Runtime.Prototyping.Unobservables;
using droid.Runtime.Structs;
using droid.Runtime.Structs.Space;
using droid.Runtime.Utilities;
using droid.Runtime.Utilities.InternalReactions;
using UnityEngine;
#if UNITY_EDITOR
using droid.Runtime.Environments.Prototyping;
using UnityEditor;

namespace droid.Editor.Windows {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class PrototypingWindow : EditorWindow {
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
    bool _show_detailed_descriptions;

    /// <summary>
    /// </summary>
    AbstractNeodroidManager _simulation_manager;

    /// <summary>
    /// </summary>
    [MenuItem(EditorWindowMenuPath._WindowMenuPath + "PrototypingWindow")]
    [MenuItem(EditorWindowMenuPath._ToolMenuPath + "PrototypingWindow")]
    public static void ShowWindow() {
      GetWindow(typeof(PrototypingWindow)); //Show existing window instance. If one doesn't exist, make one.
      //window.Show();
    }

    /// <summary>
    /// </summary>
    void OnEnable() {
      this._icon =
          (Texture2D)AssetDatabase.LoadAssetAtPath(NeodroidSettings.Current.NeodroidImportLocationProp
                                                   + "Gizmos/Icons/world.png",
                                                   typeof(Texture2D));
      this._neodroid_icon =
          (Texture)AssetDatabase.LoadAssetAtPath(NeodroidSettings.Current.NeodroidImportLocationProp
                                                 + "Gizmos/Icons/neodroid_favicon_cut.png",
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
      this._simulation_manager = FindObjectOfType<NeodroidManager>();
      if (this._simulation_manager) {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();
        GUILayout.Label(this._neodroid_icon,
                        GUILayout.Width(_logo_image_size),
                        GUILayout.Height(_logo_image_size));

        if (NeodroidEditorUtilities.LinkLabel(new GUIContent(_neodroid_url_text))) {
          Application.OpenURL(_neodroid_url);
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.ObjectField(this._simulation_manager, typeof(AbstractNeodroidManager), true);

        this._simulation_manager.Configuration =
            (SimulatorConfiguration)
            EditorGUILayout.ObjectField((SimulatorConfiguration)this._simulation_manager.Configuration,
                                        typeof(SimulatorConfiguration),
                                        true);

        this._simulation_manager.Configuration.FrameSkips =
            EditorGUILayout.IntField("Frame Skips", this._simulation_manager.Configuration.FrameSkips);

        this._simulation_manager.Configuration.SimulationType =
            (SimulationType)EditorGUILayout.EnumPopup("Simulation Type",
                                                      this._simulation_manager.Configuration.SimulationType);


        this._player_reactions = FindObjectOfType<PlayerReactions>();
        EditorGUILayout.ObjectField(this._player_reactions, typeof(PlayerReactions), true);

        this._show_detailed_descriptions =
            EditorGUILayout.Toggle("Show Details", this._show_detailed_descriptions);

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        this._environments =
            NeodroidSceneUtilities.FindAllObjectsOfTypeInScene<PrototypingEnvironment>();
        if (this._show_environment_properties.Length != this._environments.Length) {
          this.Setup();
        }

        this._scroll_position = EditorGUILayout.BeginScrollView(this._scroll_position);

        EditorGUILayout.BeginVertical("Box");
        var num_active_environments = this._environments.Length;
        var num_inactive_environments = this._environments.Length - num_active_environments;

        //EditorGUILayout.BeginHorizontal();

        GUILayout.Label($"Environments - Active({num_active_environments}), Inactive({num_inactive_environments}), Total({this._environments.Length})");

        //EditorGUILayout.EndHorizontal();

        if (this._show_environment_properties != null) {
          for (var i = 0; i < this._show_environment_properties.Length; i++) {
            if (this._environments[i].isActiveAndEnabled) {
              this._show_environment_properties[i] =
                  EditorGUILayout.Foldout(this._show_environment_properties[i],
                                          $"{this._environments[i].Identifier}");
              if (this._show_environment_properties[i]) {
                var sensors = this._environments[i].Sensors;
                var configurables = this._environments[i].Configurables;
                var listeners = this._environments[i].Listeners;
                var displayers = this._environments[i].Displayers;

                EditorGUILayout.BeginVertical("Box");
                this._environments[i].enabled =
                    EditorGUILayout.BeginToggleGroup(this._environments[i].Identifier,
                                                     this._environments[i].enabled
                                                     && this._environments[i].gameObject.activeSelf);

                EditorGUILayout.ObjectField(this._environments[i], typeof(PrototypingEnvironment), true);

                if (this._show_detailed_descriptions) {
                  this._environments[i].CoordinateSpace =
                      (CoordinateSpace)EditorGUILayout.EnumPopup("Coordinate system",
                                                                  this._environments[i].CoordinateSpace);
                  EditorGUI.BeginDisabledGroup(this._environments[i].CoordinateSpace
                                               != CoordinateSpace.Environment_);
                  this._environments[i].CoordinateReferencePoint =
                      (Transform)EditorGUILayout.ObjectField("Reference point",
                                                             this._environments[i].CoordinateReferencePoint,
                                                             typeof(Transform),
                                                             true);
                  EditorGUI.EndDisabledGroup();
                  if (this._environments[i].ObjectiveFunction != null) {
                    this._environments[i].ObjectiveFunction =
                        (EpisodicObjective)EditorGUILayout.ObjectField("Objective function",
                                                                       (EpisodicObjective)this
                                                                                          ._environments[i]
                                                                                          .ObjectiveFunction,
                                                                       typeof(EpisodicObjective),
                                                                       true);
                    EditorGUILayout.LabelField("Signal: "
                                               + ((EpisodicObjective)this._environments[i].ObjectiveFunction).LastSignal);
                    this._environments[i].ObjectiveFunction.SignalSpace
                        .FromVector3(EditorGUILayout.Vector3Field(Space1.Vector3Description(),
                                                                  this._environments[i].ObjectiveFunction
                                                                      .SignalSpace.ToVector3()));
                    this._environments[i].ObjectiveFunction.EpisodeLength =
                        EditorGUILayout.IntField("Episode Length",
                                                 this._environments[i].ObjectiveFunction.EpisodeLength);
                  }
                  //EditorGUILayout.BeginHorizontal("Box");
                  #if NEODROID_DEBUG
                  this._environments[i].Debugging =
                      EditorGUILayout.Toggle("Debugging", this._environments[i].Debugging);
                  #endif
                  //EditorGUILayout.EndHorizontal();

                  EditorGUI.BeginDisabledGroup(true);
                  EditorGUILayout.LabelField("Info:");
                  EditorGUILayout.Toggle("Terminated", this._environments[i].Terminated);
                  EditorGUI.EndDisabledGroup();
                }

                var s = this._environments[i] as AbstractSpatialPrototypingEnvironment as ActorisedPrototypingEnvironment;
                if (s) {
                  var actors = s.Actors;
                  this.DrawActors(actors);
                } else {
                  var c = this._environments[i] as PrototypingEnvironment;
                  if (c) {
                    this.DrawActuators(c.Actuators);
                  }
                }

                this.DrawSensors(sensors);

                this.DrawConfigurables(configurables);

                this.DrawDisplayers(displayers);

                this.DrawListeners(listeners);

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

    void DrawListeners(SortedDictionary<String, IUnobservable> listeners) {
      EditorGUILayout.BeginVertical("Box");

      GUILayout.Label("Listeners");
      foreach (var resetable in listeners) {
        var resetable_value = (Unobservable)resetable.Value;
        if (resetable_value != null) {
          EditorGUILayout.BeginVertical("Box");
          resetable_value.enabled =
              EditorGUILayout.BeginToggleGroup(resetable.Key,
                                               resetable_value.enabled
                                               && resetable_value.gameObject.activeSelf);
          EditorGUILayout.ObjectField(resetable_value, typeof(Unobservable), true);
          if (this._show_detailed_descriptions) {
            //EditorGUILayout.BeginHorizontal("Box");
            #if NEODROID_DEBUG
            resetable_value.Debugging = EditorGUILayout.Toggle("Debugging", resetable_value.Debugging);
            #endif
            //EditorGUILayout.EndHorizontal();
          }

          EditorGUILayout.EndToggleGroup();
          EditorGUILayout.EndVertical();
        }
      }

      EditorGUILayout.EndVertical();
    }

    void DrawActors(SortedDictionary<string, IActor> actors) {
      EditorGUILayout.BeginVertical("Box");
      GUILayout.Label("Actors");
      foreach (var actor in actors) {
        var actor_value = (Actor)actor.Value;
        if (actor_value != null) {
          var actuators = actor_value.Actuators;

          EditorGUILayout.BeginVertical("Box");

          actor_value.enabled =
              EditorGUILayout.BeginToggleGroup(actor.Key,
                                               actor_value.enabled && actor_value.gameObject.activeSelf);
          EditorGUILayout.ObjectField(actor_value, typeof(Actor), true);
          if (this._show_detailed_descriptions) {
            //EditorGUILayout.BeginHorizontal("Box");
            #if NEODROID_DEBUG
            actor_value.Debugging = EditorGUILayout.Toggle("Debugging", actor_value.Debugging);
            #endif
            //EditorGUILayout.EndHorizontal();
          }

          this.DrawActuators(actuators);

          EditorGUILayout.EndToggleGroup();

          EditorGUILayout.EndVertical();
        }
      }

      EditorGUILayout.EndVertical();
    }

    void DrawSensors(SortedDictionary<string, ISensor> sensors) {
      EditorGUILayout.BeginVertical("Box");
      GUILayout.Label("Sensors");
      foreach (var sensor in sensors) {
        var sensor_value = (Sensor)sensor.Value;
        if (sensor_value != null) {
          EditorGUILayout.BeginVertical("Box");
          sensor_value.enabled =
              EditorGUILayout.BeginToggleGroup(sensor.Key,
                                               sensor_value.enabled && sensor_value.gameObject.activeSelf);
          EditorGUILayout.ObjectField(sensor_value, typeof(Sensor), true);
          if (this._show_detailed_descriptions) {
            //EditorGUILayout.BeginHorizontal("Box");
            #if NEODROID_DEBUG
            sensor_value.Debugging = EditorGUILayout.Toggle("Debugging", sensor_value.Debugging);
            EditorGUILayout.LabelField("Observables: [" + sensor_value.ToString() + "]");
            #endif
            //EditorGUILayout.EndHorizontal();
          }

          EditorGUILayout.EndToggleGroup();
          EditorGUILayout.EndVertical();
        }
      }

      EditorGUILayout.EndVertical();
    }

    void DrawDisplayers(SortedDictionary<string, IDisplayer> displayers) {
      EditorGUILayout.BeginVertical("Box");
      GUILayout.Label("Displayers");
      foreach (var displayer in displayers) {
        var displayer_value = (Displayer)displayer.Value;
        if (displayer_value != null) {
          EditorGUILayout.BeginVertical("Box");
          displayer_value.enabled =
              EditorGUILayout.BeginToggleGroup(displayer.Key,
                                               displayer_value.enabled
                                               && displayer_value.gameObject.activeSelf);
          EditorGUILayout.ObjectField(displayer_value, typeof(Displayer), true);
          if (this._show_detailed_descriptions) {
            //EditorGUILayout.BeginHorizontal("Box");
            #if NEODROID_DEBUG
            displayer_value.Debugging = EditorGUILayout.Toggle("Debugging", displayer_value.Debugging);
            #endif
            //EditorGUILayout.EndHorizontal();
          }

          EditorGUILayout.EndToggleGroup();
          EditorGUILayout.EndVertical();
        }
      }

      EditorGUILayout.EndVertical();
    }

    void DrawConfigurables(SortedDictionary<string, IConfigurable> configurables) {
      EditorGUILayout.BeginVertical("Box");
      GUILayout.Label("Configurables");
      foreach (var configurable in configurables) {
        var configurable_value = (Configurable)configurable.Value;
        if (configurable_value != null) {
          EditorGUILayout.BeginVertical("Box");
          configurable_value.enabled = EditorGUILayout.BeginToggleGroup(configurable.Key,
                                                                        configurable_value.enabled
                                                                        && configurable_value
                                                                           .gameObject.activeSelf);
          EditorGUILayout.ObjectField(configurable_value, typeof(Configurable), true);
          if (this._show_detailed_descriptions) {
            //EditorGUILayout.BeginHorizontal("Box");
            #if NEODROID_DEBUG
            configurable_value.Debugging = EditorGUILayout.Toggle("Debugging", configurable_value.Debugging);
            #endif
            //EditorGUILayout.EndHorizontal();
          }

          EditorGUILayout.EndToggleGroup();
          EditorGUILayout.EndVertical();
        }
      }

      EditorGUILayout.EndVertical();
    }

    void DrawActuators(SortedDictionary<string, IActuator> actuators) {
      EditorGUILayout.BeginVertical("Box");
      GUILayout.Label("Actuators");
      foreach (var actuator in actuators) {
        var actuator_value = (Actuator)actuator.Value;
        if (actuator_value != null) {
          EditorGUILayout.BeginVertical("Box");
          actuator_value.enabled =
              EditorGUILayout.BeginToggleGroup(actuator.Key,
                                               actuator_value.enabled
                                               && actuator_value.gameObject.activeSelf);
          EditorGUILayout.ObjectField(actuator_value, typeof(Actuator), true);

          if (this._show_detailed_descriptions) {
            actuator_value.MotionSpace.FromVector3(EditorGUILayout.Vector3Field(Space1.Vector3Description(),
                                                                                actuator_value
                                                                                    .MotionSpace
                                                                                    .ToVector3()));
            //EditorGUILayout.BeginHorizontal("Box");
            #if NEODROID_DEBUG
            actuator_value.Debugging = EditorGUILayout.Toggle("Debugging", actuator_value.Debugging);
            #endif
            //EditorGUILayout.EndHorizontal();
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
