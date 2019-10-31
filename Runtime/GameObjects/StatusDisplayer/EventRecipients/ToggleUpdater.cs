using UnityEngine;
using UnityEngine.UI;

namespace droid.Runtime.GameObjects.StatusDisplayer.EventRecipients {
  /// <inheritdoc cref="DataPoller" />
  /// <summary>
  /// </summary>
  [RequireComponent(typeof(Toggle))]
  [ExecuteInEditMode]
  public class ToggleUpdater : DataPoller {
    /// <summary>
    /// </summary>
    [SerializeField]
    Toggle _toggle;

    // Use this for initialization
    /// <summary>
    /// </summary>
    void Start() { this._toggle = this.GetComponent<Toggle>(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="data"></param>
    public override void PollData(dynamic data) {
      if (data is bool) {
        if (this._toggle) {
          this._toggle.isOn = data;
        }
      }
    }
  }
}
