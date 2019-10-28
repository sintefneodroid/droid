using UnityEngine;
using UnityEngine.Events;

namespace droid.Runtime.GameObjects.StatusDisplayer.EventRecipients {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public abstract class DataPoller : MonoBehaviour {
    [SerializeField] UnityEvent _poll_event;

    [SerializeField] bool _invoke_on_validate = false;

    /// <summary>
    /// </summary>
    public UnityEvent PollEvent { get { return this._poll_event; } set { this._poll_event = value; } }

    /// <summary>
    /// </summary>
    /// <param name="data"></param>
    public abstract void PollData(dynamic data);

    void OnValidate() {
      if (this._invoke_on_validate) {
        this._poll_event?.Invoke();
      }
    }

    void OnEnable() { this._poll_event?.Invoke(); }

    // Update is called once per frame
    /// <summary>
    /// </summary>
    void Update() {
      if (!this._invoke_on_validate) {
        this._poll_event?.Invoke();
      }
    }
  }
}
