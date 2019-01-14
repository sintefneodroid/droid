using System;
using Neodroid.Runtime.Utilities.Misc.SearchableEnum;
using UnityEditor.UI;
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

    void OnValidate() {
/*
      foreach (var motion in this._Motions) {
        foreach (var motion2 in this._Motions) {
          if (motion._Key == motion2._Key) {
            Debug.LogWarning($"{motion} and {motion2} has the same Key");
          }
        }
      }
  */
      var copy = this._Motions;
      for (var i = 0; i < copy.Length; i++) {
        copy[i]._Actor = copy[i]._Actor.Trim(' ');
        copy[i]._Motor = copy[i]._Motor.Trim(' ');
      }
      this._Motions = copy;
    }
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
