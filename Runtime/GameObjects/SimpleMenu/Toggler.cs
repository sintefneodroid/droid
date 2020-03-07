using UnityEngine;

namespace droid.Runtime.GameObjects.SimpleMenu {
  public class Toggler : MonoBehaviour {
    [SerializeField] GameObject togglee;

    void Update() {
      if (Input.GetKey("escape")) {
        if (this.togglee) {
          this.togglee.SetActive(value : !this.togglee.activeSelf);
        }
      }
    }
  }
}
