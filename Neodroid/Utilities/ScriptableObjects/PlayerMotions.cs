using System;
using UnityEngine;

namespace Neodroid.Utilities.ScriptableObjects {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [CreateAssetMenu(
      fileName = "PlayerMotions",
      menuName = "Neodroid/ScriptableObjects/PlayerMotions",
      order = 1)]
  public class PlayerMotions : ScriptableObject {
    /// <summary>
    /// 
    /// </summary>
    public PlayerMotion[] _Motions;
  }

  /// <summary>
  /// 
  /// </summary>
  [Serializable]
  public struct PlayerMotion {
    /// <summary>
    /// 
    /// </summary>
    public KeyCode _Key;

    /// <summary>
    /// 
    /// </summary>
    public string _Actor;

    /// <summary>
    /// 
    /// </summary>
    public string _Motor;

    /// <summary>
    /// 
    /// </summary>
    public float _Strength;
  }
}
