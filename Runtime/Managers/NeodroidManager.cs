using System;
using droid.Runtime.Enums;
using droid.Runtime.GameObjects;
using UnityEngine;
using Object = System.Object;

namespace droid.Runtime.Managers {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu("Neodroid/Managers/NeodroidManager")]
  public class NeodroidManager : AbstractNeodroidManager {
    [SerializeField] Animator[] animators;

    #region Fields

    #if UNITY_EDITOR
    /// <summary>
    /// </summary>
    [field :
        Header("Warning! Will block editor requiring a restart, if not terminated while receiving.",
               order = 120)]
    [field : SerializeField]
    public bool AllowInEditorBlockage { get; set; } = false;
    #endif

    /// <summary>
    /// </summary>
    public bool IsSimulationPaused { get { return !(this.SimulationTimeScale > 0); } }

    #endregion

    #region UnityCallbacks

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() {
      if (this.Configuration.SimulationType == SimulationTypeEnum.Frame_dependent_) {
        this.EarlyUpdateEvent += this.Pause;
        this.UpdateEvent += this.Resume;
      } else if (this.Configuration.SimulationType == SimulationTypeEnum.Physics_dependent_) {
        #if UNITY_EDITOR
        if (this.AllowInEditorBlockage) {
          // Receive blocks the main thread and therefore also the unity editor.
          this.EarlyFixedUpdateEvent += this.Receive;
        } else {
          Debug.LogWarning("Physics dependent blocking in editor is not enabled and is therefore not receiving or sending anything.");
        }
        #else
        this.EarlyFixedUpdateEvent += this.Receive;
        #endif
      }
    }

    #endregion

    #region PrivateMethods

    /// <summary>
    /// </summary>
    void Resume() {
      if (this.ShouldResume) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("Resuming simulation because of stepping");
        }
        #endif

        this.ResumeSimulation(simulation_time_scale : this.Configuration.TimeScale);
      } else if (this.TestActuators) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("Resuming simulation because of TestActuators");
        }
        #endif

        this.ResumeSimulation(simulation_time_scale : this.Configuration.TimeScale);
      } else {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("Not resuming simulation");
        }
        #endif
      }
    }

    /// <summary>
    /// </summary>
    void Pause() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Pausing simulation");
      }
      #endif
      this.SimulationTimeScale = 0;
    }

    /// <summary>
    /// </summary>
    /// <param name="simulation_time_scale"></param>
    void ResumeSimulation(float simulation_time_scale) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Resuming simulation");
      }
      #endif
      this.SimulationTimeScale = simulation_time_scale > 0 ? simulation_time_scale : 1;
    }

    void SetAnimationSpeeds(float speed, bool recache_animators = false) {
      if (this.animators == null || recache_animators) {
        this.animators = FindObjectsOfType<Animator>();
      }

      foreach (var animator in this.animators) {
        animator.speed = speed;
      }
    }

    /// <summary>
    /// </summary>
    void Receive() {
      var reaction = this._Message_Server.Receive(wait_time : TimeSpan.Zero);
      this.SetReactions(reactions : reaction);
    }

    #endregion
  }
}
