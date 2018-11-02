using System;
using Neodroid.Runtime.Utilities.Misc.SearchableEnum;
using UnityEngine;

namespace Neodroid.Runtime.Utilities.ScriptableObjects {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [CreateAssetMenu(
      fileName = "PlayerMotions",
      menuName = "Neodroid/ScriptableObjects/PlayerMotions",
      order = 1)]
  public class PlayerMotions : ScriptableObject {
    /// <summary>
    /// </summary>
    public PlayerMotion[] _Motions;
  }

  /// <summary>
  /// </summary>
  [Serializable]
  public struct PlayerMotion {
    /// <summary>
    /// </summary>
    [SearchableEnum]
    public KeyCode _Key;

    /// <summary>
    /// </summary>
    public string _Actor;

    /// <summary>
    /// </summary>
    public string _Motor;

    /// <summary>
    /// </summary>
    public float _Strength;
  }
}