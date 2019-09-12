using UnityEngine;

namespace droid.Runtime.Utilities.Extensions {
  /// <summary>
  ///
  /// </summary>
  public static partial class NeodroidUtilities {
    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Vector4 Divide(this Vector4 a, Vector4 b) {
      return new Vector4(a.x / b.x,
                         a.y / b.y,
                         a.z / b.z,
                         a.w / b.w);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Vector4 Multiply(this Vector4 a, Vector4 b) {
      return new Vector4(a.x * b.x,
                         a.y * b.y,
                         a.z * b.z,
                         a.w * b.w);
    }
  }
}
