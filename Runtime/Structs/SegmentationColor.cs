using System;
using UnityEngine;

namespace droid.Runtime.Structs {
  /// <summary>
  /// </summary>
  [Serializable]
  public struct ColorByCategory {
    /// <summary>
    /// </summary>
    public string _Category_Name;

    /// <summary>
    /// </summary>
    public Color _Color;
  }

  /// <summary>
  /// </summary>
  [Serializable]
  public struct ColorByInstance {
    /// <summary>
    /// </summary>
    public GameObject _Game_Object;

    /// <summary>
    /// </summary>
    public Color _Color;
  }
}
