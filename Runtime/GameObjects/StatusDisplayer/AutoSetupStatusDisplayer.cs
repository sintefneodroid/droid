#if UNITY_EDITOR
using droid.Runtime.GameObjects.StatusDisplayer.EventRecipients;
using droid.Runtime.Managers;
using droid.Runtime.Prototyping.ObjectiveFunctions;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEditor.Events;
using droid.Runtime.Environments;
using UnityEngine;

namespace droid.Runtime.GameObjects.StatusDisplayer {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public class AutoSetupStatusDisplayer : MonoBehaviour {
    [SerializeField] bool _clean_empty_no_target_events = true;

    [SerializeField] NeodroidEnvironment _environment = null;
    [SerializeField] TextUpdater _environment_frame = null;
    [SerializeField] TextUpdater _environment_obs = null;

    [SerializeField] TextUpdater _environment_text = null;
    [SerializeField] ObjectiveFunction _evaluation_function = null;
    [SerializeField] AbstractNeodroidManager _manager = null;
    [SerializeField] Button _reset_button = null;
    [SerializeField] TextUpdater _signal = null;
    [SerializeField] TextUpdater _episode_length = null;
    [SerializeField] TextUpdater _status_text = null;
    [SerializeField] ToggleUpdater _terminated = null;
    [SerializeField] Toggle _testing_toggle = null;
    [SerializeField] UnityEventCallState _unity_event_call_state = UnityEventCallState.RuntimeOnly;

    #if NEODROID_DEBUG
    bool Debugging { get { return this._debugging; } set { this._debugging = value; } }
    [SerializeField] bool _debugging;
    #endif

    void TryRegister(DataPoller poller, UnityAction<DataPoller> f) {
      if (poller) {
        var count = poller.PollEvent.GetPersistentEventCount();
        if (this._clean_empty_no_target_events && count > 0) {
          //poller.PollEvent.RemoveAllListeners(); // Only non-persistant listeners.
          for (var i = 0; i < count; i++) {
            if (poller.PollEvent.GetPersistentTarget(index : i) == null
                || poller.PollEvent.GetPersistentMethodName(index : i) == null) {
              UnityEventTools.RemovePersistentListener(unityEvent : poller.PollEvent, index : i);
            }
          }
        }

        count = poller.PollEvent.GetPersistentEventCount();
        if (count == 0) {
          UnityEventTools.AddObjectPersistentListener(unityEvent : poller.PollEvent,
                                                      call : f,
                                                      argument : poller);
          poller.PollEvent.SetPersistentListenerState(0, state : this._unity_event_call_state);
        } else if (count > 0 && poller.PollEvent.GetPersistentTarget(0) != poller) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.Log(message : $"PollEvent on {poller} already has a listeners");
          }
          #endif
        }
      }
    }

    void TryRegisterVoid(UnityEventBase poller, UnityAction f) {
      var count = poller.GetPersistentEventCount();
      if (this._clean_empty_no_target_events && count > 0) {
        //poller.PollEvent.RemoveAllListeners(); // Only non-persistant listeners.
        for (var i = 0; i < count; i++) {
          if (poller.GetPersistentTarget(index : i) == null
              || poller.GetPersistentMethodName(index : i) == null) {
            UnityEventTools.RemovePersistentListener(unityEvent : poller, index : i);
          }
        }
      }

      count = poller.GetPersistentEventCount();
      if (count == 0) {
        UnityEventTools.AddVoidPersistentListener(unityEvent : poller, call : f);
        poller.SetPersistentListenerState(0, state : this._unity_event_call_state);
      } else if (count > 0) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(message : $"PollEvent on {poller} already has a listeners");
        }
        #endif
      }
    }

    void TryRegisterProperty(Toggle.ToggleEvent poller, UnityAction<bool> f) {
      var count = poller.GetPersistentEventCount();
      if (this._clean_empty_no_target_events && count > 0) {
        //poller.PollEvent.RemoveAllListeners(); // Only non-persistent listeners.
        for (var i = 0; i < count; i++) {
          if (poller.GetPersistentTarget(index : i) == null
              || poller.GetPersistentMethodName(index : i) == null) {
            UnityEventTools.RemovePersistentListener(unityEvent : poller, index : i);
          }
        }
      }

      count = poller.GetPersistentEventCount();
      if (count == 0) {
        UnityEventTools.AddPersistentListener(unityEvent : poller, call : f);
        poller.SetPersistentListenerState(0, state : this._unity_event_call_state);
      } else if (count > 0) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(message : $"PollEvent on {poller} already has a listeners");
        }
        #endif
      }
    }

    void Start() {
      if (!this._environment) {
        this._environment = FindObjectOfType<NeodroidEnvironment>();
      }

      var neodroid_environment = this._environment;

      if (neodroid_environment != null) {
        this.TryRegister(poller : this._environment_text, f : neodroid_environment.IdentifierString);
        this.TryRegister(poller : this._environment_frame, f : neodroid_environment.FrameString);
        this.TryRegister(poller : this._environment_obs, f : neodroid_environment.ObservationsString);
        this.TryRegister(poller : this._terminated, f : neodroid_environment.TerminatedBoolean);
      }

      if (!this._evaluation_function) {
        this._evaluation_function = FindObjectOfType<ObjectiveFunction>();
      }

      var evaluation_function = this._evaluation_function;
      if (evaluation_function != null) {
        this.TryRegister(poller : this._signal, f : evaluation_function.SignalString);
        this.TryRegister(poller : this._episode_length, f : evaluation_function.EpisodeLengthString);
      }

      if (!this._manager) {
        this._manager = FindObjectOfType<AbstractNeodroidManager>();
      }

      if (this._manager) {
        if (this._status_text) {
          this.TryRegister(poller : this._status_text, f : this._manager.StatusString);
        }

        if (this._testing_toggle) {
          this.TryRegisterProperty(poller : this._testing_toggle.onValueChanged,
                                   f : this._manager.SetTesting);
        }
      }

      if (this._reset_button) {
        this.TryRegisterVoid(poller : this._reset_button.onClick, f : this._manager.ResetAllEnvironments);
      }
    }
  }
}
#endif
