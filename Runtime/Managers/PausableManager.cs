using System;
using droid.Runtime.Utilities.Enums;
using UnityEngine;

namespace droid.Runtime.Managers {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu("Neodroid/Managers/PausableManager")]
  public class PausableManager : NeodroidManager {
    #region Fields

    //[SerializeField] bool _blocked;
    #if UNITY_EDITOR
    /// <summary>
    /// </summary>
    [Header("Warning! Will block editor requiring a restart, if not terminated while receiving.",
        order = 120)]
    [SerializeField]
    bool _allow_in_editor_blockage = false;
    #endif

    #endregion

    #region UnityCallbacks

    /// <summary>
    /// </summary>
    protected new void Awake() {
      base.Awake();
      if (this.Configuration.SimulationType == SimulationType.Frame_dependent_) {
        this.EarlyUpdateEvent += this.PauseSimulation;
        this.UpdateEvent += this.MaybeResume;
      } else if (this.Configuration.SimulationType == SimulationType.Physics_dependent_) {
        #if UNITY_EDITOR
        if (this._allow_in_editor_blockage) {
          this.EarlyFixedUpdateEvent +=
              this.Receive; // Receive blocks the main thread and therefore also the unity editor.
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
    void MaybeResume() {
      if (this.Stepping) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("Resuming simulation because of stepping");
        }
        #endif

        this.ResumeSimulation(this.Configuration.TimeScale);
      } else if (this.TestMotors) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("Resuming simulation because of TestMotors");
        }
        #endif

        this.ResumeSimulation(this.Configuration.TimeScale);
      } else {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("Not resuming simulation because of stepping");
        }
        #endif
      }
    }

    /// <summary>
    /// </summary>
    public bool IsSimulationPaused { get { return !(this.SimulationTimeScale > 0); } }

    /// <summary>
    /// </summary>
    void PauseSimulation() {
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

    void SetAnimationSpeeds(float speed) {
      var animators = FindObjectsOfType<Animator>();
      foreach (var animator in animators) {
        animator.speed = speed;
      }
    }

    /// <summary>
    /// </summary>
    void Receive() {
      var reaction = this._Message_Server.Receive(TimeSpan.Zero);
      this.SetReactionsFromExternalSource(reaction);
    }

    #endregion
  }
}
