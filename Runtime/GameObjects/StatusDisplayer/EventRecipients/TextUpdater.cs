using UnityEngine;
using UnityEngine.UI;

namespace droid.Runtime.GameObjects.StatusDisplayer.EventRecipients {
  /// <inheritdoc cref="DataPoller" />
  /// <summary>
  /// </summary>
  [RequireComponent(typeof(Text))]
  [ExecuteInEditMode]
  public class TextUpdater : DataPoller {
    /// <summary>
    /// </summary>
    [SerializeField]
    Text _text;

    /// <summary>
    /// </summary>
    void Start() { this._text = this.GetComponent<Text>(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="data"></param>
    public override void PollData(dynamic data) {
      if (data is string) {
        if (this._text) {
          this._text.text = data;
        }
      }
    }
  }
}
