using System.Runtime.CompilerServices;
using UnityEngine;

namespace droid.Runtime.GameObjects.BoundingBoxes.Experimental {
  /// <summary>
  ///
  /// </summary>
  public static class VectorSpace {
    /// <summary>
    ///
    /// </summary>
    /// <param name="point"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetMinMax(this Vector2 point, ref Vector2 min, ref Vector2 max) {
      min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
      max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="point"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetMinMax(this Vector3 point, ref Vector3 min, ref Vector3 max) {
      min = new Vector3(min.x >= point.x ? point.x : min.x,
                        min.y >= point.y ? point.y : min.y,
                        min.z >= point.z ? point.z : min.z);
      max = new Vector3(max.x <= point.x ? point.x : max.x,
                        max.y <= point.y ? point.y : max.y,
                        max.z <= point.z ? point.z : max.z);
    }
  }
}
