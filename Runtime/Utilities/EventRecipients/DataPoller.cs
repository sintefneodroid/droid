using UnityEngine;
using UnityEngine.Events;

namespace Neodroid.Runtime.Utilities.EventRecipients {
  namespace droid.Neodroid.Utilities.Unsorted {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract class DataPoller : MonoBehaviour {
      [SerializeField] UnityEvent _poll_event;

      /// <summary>
      /// </summary>
      public UnityEvent PollEvent { get { return this._poll_event; } set { this._poll_event = value; } }

      /// <summary>
      /// </summary>
      /// <param name="data"></param>
      public abstract void PollData(dynamic data);

      // Update is called once per frame
      /// <summary>
      /// </summary>
      void Update() { this._poll_event?.Invoke(); }
    }
  }
}