using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Utilities.Interfaces {
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
