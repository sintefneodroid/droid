using Neodroid.Utilities.Unsorted.SearchableEnum;
using UnityEngine;

namespace Neodroid.PlayerControls {
  /// <inheritdoc />
  ///  <summary>
  ///  </summary>
  public class KeyEventEnabler : MonoBehaviour {
    [SerializeField] GameObject _game_object;

    [SerializeField] [SearchableEnum] KeyCode _key;

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
