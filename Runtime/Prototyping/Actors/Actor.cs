using System.Collections.Generic;
using droid.Runtime.Environments.Prototyping;
using droid.Runtime.GameObjects;
using droid.Runtime.GameObjects.BoundingBoxes.Experimental;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities;
using UnityEditor;
using UnityEngine;

namespace droid.Runtime.Prototyping.Actors {
  /// <inheritdoc cref="PrototypingGameObject" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(menuName : ActorComponentMenuPath._ComponentMenuPath
                               + "Base"
                               + ActorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  public class Actor : PrototypingGameObject,
                       IActor {
    /// <summary>
    /// </summary>
    [SerializeField]
    Bounds _bounds;

    /// <summary>
    /// </summary>
    [SerializeField]
    bool _draw_bounds = false;

    /// <summary>
    /// </summary>
    public Bounds ActorBounds {
      get {
        var col = this.GetComponent<BoxCollider>();
        this._bounds = new Bounds(center : this.transform.position, size : Vector3.zero); // position and size

        if (col) {
          this._bounds.Encapsulate(bounds : col.bounds);
        }

        var cols = this.GetComponentsInChildren<Collider>();
        for (var index = 0; index < cols.Length; index++) {
          var child_col = cols[index];
          if (child_col != col) {
            this._bounds.Encapsulate(bounds : child_col.bounds);
          }
        }

        return this._bounds;
      }
    }

    SortedDictionary<string, IActuator> IActor.Actuators { get { return this._Actuators; } }

    public Transform CachedTransform { get { return this.transform; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    public virtual void ApplyMotion(IMotion motion) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log(message : $"Applying {motion} To {this.name}'s Actuators");
      }
      #endif

      var motion_actuator_name = motion.ActuatorName;
      if (this._Actuators.ContainsKey(key : motion_actuator_name)
          && this._Actuators[key : motion_actuator_name] != null) {
        this._Actuators[key : motion_actuator_name].ApplyMotion(motion : motion);
      } else {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(message :
                    $"Could find not Actuator with the specified name: {motion_actuator_name} on actor {this.name}");
        }
        #endif
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PrototypingReset() {
      if (this._Actuators != null) {
        foreach (var actuator in this._Actuators.Values) {
          actuator?.PrototypingReset();
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="actuator"></param>
    /// <param name="identifier"></param>
    public void UnRegister(IActuator actuator, string identifier) {
      if (this._Actuators != null) {
        if (this._Actuators.ContainsKey(key : identifier)) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log(message : $"Actor {this.name} unregistered Actuator {identifier}");
          }
          #endif

          this._Actuators.Remove(key : identifier);
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="actuator"></param>
    public void UnRegister(IActuator actuator) {
      this.UnRegister(actuator : actuator, identifier : actuator.Identifier);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() {
      #if UNITY_EDITOR
      if (!Application.isPlaying) {
        var manager_script = MonoScript.FromMonoBehaviour(behaviour : this);
        if (MonoImporter.GetExecutionOrder(script : manager_script) != _script_execution_order) {
          MonoImporter.SetExecutionOrder(script : manager_script,
                                         order :
                                         _script_execution_order); // Ensures that PreStep is called first, before all other scripts.
          Debug.LogWarning("Execution Order changed, you will need to press play again to make everything function correctly!");
          EditorApplication.isPlaying = false;
          //TODO: UnityEngine.Experimental.LowLevel.PlayerLoop.SetPlayerLoop(new UnityEngine.Experimental.LowLevel.PlayerLoopSystem());
        }
      }
      #endif
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Clear() { this._Actuators.Clear(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment, c : this);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() { this._environment?.UnRegister(actor : this); }

    /// <summary>
    /// </summary>
    void Update() {
      if (this._draw_bounds) {
        var corners = Corners.ExtractCorners(v3_center : this.ActorBounds.center,
                                             v3_extents : this.ActorBounds.extents,
                                             reference_transform : this.transform);

        Corners.DrawBox(v3_front_top_left : corners[0],
                        v3_front_top_right : corners[1],
                        v3_front_bottom_left : corners[2],
                        v3_front_bottom_right : corners[3],
                        v3_back_top_left : corners[4],
                        v3_back_top_right : corners[5],
                        v3_back_bottom_left : corners[6],
                        v3_back_bottom_right : corners[7],
                        color : Color.gray);
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="actuator"></param>
    /// <param name="identifier"></param>
    public void RegisterActuator(IActuator actuator, string identifier) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log(message : "Actor " + this.name + " has Actuator " + identifier);
      }
      #endif

      if (!this._Actuators.ContainsKey(key : identifier)) {
        this._Actuators.Add(key : identifier, value : actuator);
      } else {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(message : $"A Actuator with the identifier {identifier} is already registered");
        }
        #endif
      }
    }

    #region Fields

    /// <summary>
    /// </summary>
    [Header("References", order = 99)]
    [SerializeField]
    ActorisedPrototypingEnvironment _environment;

    /// <summary>
    /// </summary>
    [Header("General", order = 101)]
    [SerializeField]
    protected SortedDictionary<string, IActuator> _Actuators = new SortedDictionary<string, IActuator>();

    #if UNITY_EDITOR
    const int _script_execution_order = -10;
    #endif

    #endregion

    #region Getters

    /// <summary>
    /// </summary>
    /// <param name="actuator"></param>
    public void Register(IActuator actuator) {
      this.RegisterActuator(actuator : actuator, identifier : actuator.Identifier);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="actuator"></param>
    /// <param name="identifier"></param>
    public void Register(IActuator actuator, string identifier) {
      this.RegisterActuator(actuator : actuator, identifier : identifier);
    }

    /// <summary>
    /// </summary>
    public SortedDictionary<string, IActuator> Actuators { get { return this._Actuators; } }

    /// <summary>
    /// </summary>
    public ActorisedPrototypingEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    #endregion
  }
}
