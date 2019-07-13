using UnityEngine;

namespace droid.Runtime.Utilities.InternalReactions {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class KeyEventEnabler : MonoBehaviour {
    [SerializeField] GameObject _game_object = null;

    [SerializeField] [SearchableEnum] KeyCode _key = KeyCode.None;

    /// <summary>
    /// </summary>
    void Update() {
      if (Input.GetKeyDown(this._key)) {
        this._game_object?.SetActive(!this._game_object.activeSelf);
      }
    }
  }
}
