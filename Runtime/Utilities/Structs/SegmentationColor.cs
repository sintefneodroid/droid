using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace droid.Runtime.Utilities.Structs {
  /// <summary>
  /// </summary>
  [Serializable]
  public struct ColorByCategory {
    /// <summary>
    /// </summary>
    [FormerlySerializedAs("_Tag")]
    public string _Category_Name;

    /// <summary>
    /// </summary>
    [FormerlySerializedAs("_Col")]
    public Color _Color;
  }

  /// <summary>
  /// </summary>
  [Serializable]
  public struct ColorByInstance {
    /// <summary>
    /// </summary>
    [FormerlySerializedAs("_Obj")]
    public GameObject _Game_Object;

    /// <summary>
    /// </summary>
    [FormerlySerializedAs("_Col")]
    public Color _Color;
  }
}
