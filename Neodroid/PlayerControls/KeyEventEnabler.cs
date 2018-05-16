using UnityEngine;

namespace Neodroid.PlayerControls {
  /// <summary>
  ///
  /// </summary>
  public class KeyEventEnabler : MonoBehaviour {
    [SerializeField] GameObject _game_object;

    [SerializeField] KeyCode _key;

    /// <summary>
    ///
    /// </summary>
    void Update() {
      if (Input.GetKeyDown(this._key)) {
        this._game_object.SetActive(!this._game_object.activeSelf);
      }
    }
  }
}
