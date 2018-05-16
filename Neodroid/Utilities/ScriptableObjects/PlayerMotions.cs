using System;
using UnityEngine;

namespace Neodroid.Utilities.ScriptableObjects {
  [CreateAssetMenu(
      fileName = "PlayerMotions",
      menuName = "Neodroid/ScriptableObjects/PlayerMotions",
      order = 1)]
  public class PlayerMotions : ScriptableObject {
    public PlayerMotion[] _Motions;
  }

  [Serializable]
  public struct PlayerMotion {
    public KeyCode _Key;
    public string _Actor;
    public string _Motor;
    public float _Strength;
  }
}
