﻿using System;
using System.Text.RegularExpressions;
using droid.Runtime.Utilities.Misc.SearchableEnum;
using UnityEngine;

namespace droid.Runtime.Utilities.ScriptableObjects {
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
        var actor = copy[i]._Actor;
        copy[i]._Actor = Regex.Replace(actor, "[^\\w\\._]", "");

        var motor= copy[i]._Motor;
        copy[i]._Motor = Regex.Replace(motor, "[^\\w\\._]", "");
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
