#if UNITY_EDITOR
using UnityEditor.Events;
using Neodroid.Runtime.Environments;
using Neodroid.Runtime.Managers;
using Neodroid.Runtime.Prototyping.Evaluation;
using Neodroid.Runtime.Utilities.EventRecipients;
using Neodroid.Runtime.Utilities.EventRecipients.droid.Neodroid.Utilities.Unsorted;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Neodroid.Runtime.Utilities.StatusDisplayer {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public class AutoSetupStatusDisplayer : MonoBehaviour {
    [SerializeField] NeodroidManager _manager;
    [SerializeField] NeodroidEnvironment _environment;
    [SerializeField] ObjectiveFunction _evaluation_function;

    [SerializeField] TextUpdater _environment_text;
    [SerializeField] TextUpdater _environment_frame;
    [SerializeField] TextUpdater _environment_obs;
    [SerializeField] TextUpdater _signal;
    [SerializeField] ToggleUpdater _terminated;
    [SerializeField] TextUpdater _status_text;
    [SerializeField] Toggle _testing_toggle;
    [SerializeField] Button _reset_button;
    [SerializeField] bool _clean_empty_no_target_events = true;
    [SerializeField] bool _debugging;
    [SerializeField] UnityEventCallState _unity_event_call_state = UnityEventCallState.RuntimeOnly;

    void TryRegister(DataPoller poller, UnityAction<DataPoller> f) {
      if (poller) {
        var count = poller.PollEvent.GetPersistentEventCount();
        if (this._clean_empty_no_target_events && count > 0) {
          //poller.PollEvent.RemoveAllListeners(); // Only non-persistant listeners.
          for (var i = 0; i < count; i++) {
            if (poller.PollEvent.GetPersistentTarget(i) == null
                || poller.PollEvent.GetPersistentMethodName(i) == null) {
              UnityEventTools.RemovePersistentListener(poller.PollEvent, i);
            }
          }
        }

        count = poller.PollEvent.GetPersistentEventCount();
        if (count == 0) {
          UnityEventTools.AddObjectPersistentListener(poller.PollEvent, f, poller);
          poller.PollEvent.SetPersistentListenerState(0, this._unity_event_call_state);
        } else if (count > 0 && poller.PollEvent.GetPersistentTarget(0) != poller) {
          if (this.Debugging) {
            Debug.Log($"PollEvent on {poller} already has a listeners");
          }
        }
      }
    }

    void TryRegisterVoid(UnityEventBase poller, UnityAction f) {
      var count = poller.GetPersistentEventCount();
      if (this._clean_empty_no_target_events && count > 0) {
        //poller.PollEvent.RemoveAllListeners(); // Only non-persistant listeners.
        for (var i = 0; i < count; i++) {
          if (poller.GetPersistentTarget(i) == null || poller.GetPersistentMethodName(i) == null) {
            UnityEventTools.RemovePersistentListener(poller, i);
          }
        }
      }

      count = poller.GetPersistentEventCount();
      if (count == 0) {
        UnityEventTools.AddVoidPersistentListener(poller, f);
        poller.SetPersistentListenerState(0, this._unity_event_call_state);
      } else if (count > 0) {
        if (this.Debugging) {
          Debug.Log($"PollEvent on {poller} already has a listeners");
        }
      }
    }

    void TryRegisterProperty(Toggle.ToggleEvent poller, UnityAction<bool> f) {
      var count = poller.GetPersistentEventCount();
      if (this._clean_empty_no_target_events && count > 0) {
        //poller.PollEvent.RemoveAllListeners(); // Only non-persistent listeners.
        for (var i = 0; i < count; i++) {
          if (poller.GetPersistentTarget(i) == null || poller.GetPersistentMethodName(i) == null) {
            UnityEventTools.RemovePersistentListener(poller, i);
          }
        }
      }

      count = poller.GetPersistentEventCount();
      if (count == 0) {
        UnityEventTools.AddPersistentListener(poller, f);
        poller.SetPersistentListenerState(0, this._unity_event_call_state);
      } else if (count > 0) {
        if (this.Debugging) {
          Debug.Log($"PollEvent on {poller} already has a listeners");
        }
      }
    }

    bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    void Start() {
      if (!this._environment) {
        this._environment = FindObjectOfType<NeodroidEnvironment>();
      }

      if (this._environment) {
        this.TryRegister(this._environment_text, this._environment.IdentifierString);
        this.TryRegister(this._environment_frame, this._environment.FrameString);
        this.TryRegister(this._environment_obs, this._environment.ObservationsString);
        this.TryRegister(this._terminated, this._environment.TerminatedBoolean);
      }

      if (!this._evaluation_function) {
        this._evaluation_function = FindObjectOfType<ObjectiveFunction>();
      }

      if (this._evaluation_function) {
        this.TryRegister(this._signal, this._evaluation_function.SignalString);
      }

      if (!this._manager) {
        this._manager = FindObjectOfType<NeodroidManager>();
      }

      if (this._manager) {
        if (this._status_text) {
          this.TryRegister(this._status_text, this._manager.StatusString);
        }

        if (this._testing_toggle) {
          this.TryRegisterProperty(this._testing_toggle.onValueChanged, this._manager.SetTesting);
        }
      }

      if (this._reset_button) {
        this.TryRegisterVoid(this._reset_button.onClick, this._manager.ResetAllEnvironments);
      }
    }
  }
}
#endif
