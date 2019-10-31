using UnityEngine;
using UnityEngine.UI;

namespace droid.Runtime.GameObjects.StatusDisplayer.EventRecipients {
  /// <inheritdoc cref="DataPoller" />
  /// <summary>
  /// </summary>
  [RequireComponent(typeof(InputField))]
  [ExecuteInEditMode]
  public class InputFieldUpdater : DataPoller {
    /// <summary>
    /// </summary>
    [SerializeField]
    InputField _input_field;

    /// <summary>
    /// </summary>
    void Start() { this._input_field = this.GetComponent<InputField>(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="data"></param>
    public override void PollData(dynamic data) {
      if (data is string) {
        if (this._input_field) {
          this._input_field.text = data;
        }
      }
    }
  }
}
