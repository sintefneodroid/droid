using UnityEngine;

#if UNITY_EDITOR

#endif

namespace droid.Runtime.GameObjects.SimpleMenu {
  public class Quitter : MonoBehaviour {
    public void Quit() {
      #if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
      #else
      Application.Quit();
      #endif
    }
  }
}
