using System.Collections.Generic;
using UnityEngine;

namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IColorProvider {
    /// <summary>
    /// </summary>
    Dictionary<string, Color> ColorsDict { get; }
  }
}
