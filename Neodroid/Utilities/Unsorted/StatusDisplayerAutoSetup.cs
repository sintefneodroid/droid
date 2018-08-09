#if UNITY_EDITOR
using Neodroid.Environments;
using Neodroid.Managers;
using Neodroid.Prototyping.Evaluation;
using Neodroid.Utilities.EventRecipients;
using Neodroid.Utilities.EventRecipients.droid.Neodroid.Utilities.Unsorted;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Neodroid.Utilities.Unsorted {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public class StatusDisplayerAutoSetup : MonoBehaviour {
    [SerializeField] NeodroidManager _manager;
    [SerializeField] NeodroidEnvironment _environment;
    [SerializeField] ObjectiveFunction _evaluation_function;

    [SerializeField] TextUpdater _enviroment_text;
    [SerializeField] TextUpdater _enviroment_frame;
    [SerializeField] TextUpdater _signal;
    [SerializeField] ToggleUpdater _terminated;
    [SerializeField] TextUpdater _status_text;
    [SerializeField] Toggle _testing_toggle;
    [SerializeField] Button _reset_button;
    [SerializeField] bool _clean_empty_no_target_events = true;
    [SerializeField] bool _debugging;
    [SerializeField] UnityEventCallState _unity_event_call_state = UnityEventCallState.RuntimeOnly;

    void RegisterIfSomething(DataPoller poller, UnityAction<DataPoller> f) {
      if (poller) {
        var count = poller.PollEvent.GetPersistentEventCount();
        if (this._clean_empty_no_target_events && count > 0) {
          //poller.PollEvent.RemoveAllListeners(); // Only non-persistant listeners.
          for (var i = 0; i < count; i++) {
            if (poller.PollEvent.GetPersistentTarget(i) == null
                || poller.PollEvent.GetPersistentMethodName(i) == null) {
              UnityEditor.Events.UnityEventTools.RemovePersistentListener(poller.PollEvent, i);
            }
          }
        }

        count = poller.PollEvent.GetPersistentEventCount();
        if (count == 0) {
          UnityEditor.Events.UnityEventTools.AddObjectPersistentListener(poller.PollEvent, f, poller);
          poller.PollEvent.SetPersistentListenerState(0, this._unity_event_call_state);
        } else if (count > 0 && poller.PollEvent.GetPersistentTarget(0) != poller) {
          if (this.Debugging) {
            Debug.Log($"PollEvent on {poller} already has a listeners");
          }
        }
      }
    }

    void RegisterIfSomethingVoid(UnityEventBase poller, UnityAction f) {
      var count = poller.GetPersistentEventCount();
      if (this._clean_empty_no_target_events && count > 0) {
        //poller.PollEvent.RemoveAllListeners(); // Only non-persistant listeners.
        for (var i = 0; i < count; i++) {
          if (poller.GetPersistentTarget(i) == null || poller.GetPersistentMethodName(i) == null) {
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(poller, i);
          }
        }
      }

      count = poller.GetPersistentEventCount();
      if (count == 0) {
        UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(poller, f);
        poller.SetPersistentListenerState(0, this._unity_event_call_state);
      } else if (count > 0) {
        if (this.Debugging) {
          Debug.Log($"PollEvent on {poller} already has a listeners");
        }
      }
    }

    void RegisterIfSomethingProperty(Toggle.ToggleEvent poller, UnityAction<bool> f) {
      var count = poller.GetPersistentEventCount();
      if (this._clean_empty_no_target_events && count > 0) {
        //poller.PollEvent.RemoveAllListeners(); // Only non-persistant listeners.
        for (var i = 0; i < count; i++) {
          if (poller.GetPersistentTarget(i) == null || poller.GetPersistentMethodName(i) == null) {
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(poller, i);
          }
        }
      }

      count = poller.GetPersistentEventCount();
      if (count == 0) {
        UnityEditor.Events.UnityEventTools.AddPersistentListener(poller, f);
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
        this.RegisterIfSomething(this._enviroment_text, this._environment.IdentifierString);
        this.RegisterIfSomething(this._enviroment_frame, this._environment.FrameString);
        this.RegisterIfSomething(this._terminated, this._environment.TerminatedBoolean);
      }

      if (!this._evaluation_function) {
        this._evaluation_function = FindObjectOfType<ObjectiveFunction>();
      }

      if (this._evaluation_function) {
        this.RegisterIfSomething(this._signal, this._evaluation_function.SignalString);
      }

      if (!this._manager) {
        this._manager = FindObjectOfType<NeodroidManager>();
      }

      if (this._manager) {
        if (this._status_text) {
          this.RegisterIfSomething(this._status_text, this._manager.StatusString);
        }

        if (this._testing_toggle) {
          this.RegisterIfSomethingProperty(this._testing_toggle.onValueChanged, this._manager.SetTesting);
        }
      }

      if (this._reset_button) {
        this.RegisterIfSomethingVoid(this._reset_button.onClick, this._manager.ResetAllEnvironments);
      }
    }
  }
}
#endif
