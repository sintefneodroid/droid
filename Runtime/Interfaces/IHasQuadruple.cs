using Neodroid.Runtime.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Runtime.Interfaces {
  /// <summary>
  ///
  /// </summary>
  public interface IHasQuadruple {
    /// <summary>
    ///
    /// </summary>
    Quaternion ObservationValue { get; }

    /// <summary>
    /// 
    /// </summary>
    Space4 QuadSpace { get; }
  }
}