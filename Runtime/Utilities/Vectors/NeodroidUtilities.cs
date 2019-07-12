using System.Collections.Generic;
using System.IO;
using droid.Runtime.Environments;
using droid.Runtime.Interfaces;
using droid.Runtime.Prototyping.Actors;
using droid.Runtime.Utilities.GameObjects.ChildSensors;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace droid.Runtime.Utilities {
  /// <summary>
  /// </summary>
  public static partial class NeodroidUtilities {
    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static Vector3 BroadcastVector3(this float a) { return new Vector3 {x = a, y = a, z = a}; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static Vector2 BroadcastVector2(this float a) { return new Vector2 {x = a, y = a}; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static Vector4 BroadcastVector4(this float a) { return new Vector4 {x = a, y = a, z = a}; }
  }
}
