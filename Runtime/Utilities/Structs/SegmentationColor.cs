using System;
using UnityEngine;

namespace Neodroid.Runtime.Utilities.Structs {
  /// <summary>
  /// 
  /// </summary>
  [Serializable]
  public struct ColorByTag {
    /// <summary>
    /// 
    /// </summary>
    public string _Tag;

    /// <summary>
    /// 
    /// </summary>
    public Color _Col;
  }

  /// <summary>
  /// 
  /// </summary>
  [Serializable]
  public struct ColorByInstance {
    /// <summary>
    /// 
    /// </summary>
    public GameObject _Obj;

    /// <summary>
    /// 
    /// </summary>
    public Color _Col;
  }
}
