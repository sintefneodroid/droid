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
    public static Vector3 Divide(this Vector3 a, Vector3 b){
      return new Vector3(a.x / b.x, a.y / b.y,a.z / b.z);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Vector3 Multiply(this Vector3 a, Vector3 b){
      return new Vector3(a.x * b.x, a.y * b.y,a.z * b.z);
    }


  }
}
