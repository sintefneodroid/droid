using UnityEngine;

namespace droid.Runtime.GameObjects.NeodroidCamera.Synthesis {
  /// <summary>
  ///
  /// </summary>
  public static class ColorEncoding {
    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static byte ReverseBits(byte value) {
      return (byte)((value * 0x0202020202 & 0x010884422010) % 1023);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <param name="sparse"></param>
    /// <returns></returns>
    public static int SparsifyBits(byte value, int sparse) {
      var ret_val = 0;
      for (var bits = 0; bits < 8; bits++, value >>= 1) {
        ret_val |= value & 1;
        ret_val <<= sparse;
      }

      return ret_val >> sparse;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="instance_id"></param>
    /// <returns></returns>
    public static Color EncodeIdAsColor(int instance_id) {
      var uid = instance_id * 2;
      if (uid < 0) {
        uid = -uid + 1;
      }

      var sid = (SparsifyBits((byte)(uid >> 16), 3) << 2)
                | (SparsifyBits((byte)(uid >> 8), 3) << 1)
                | SparsifyBits((byte)uid, 3);
      //Debug.Log(uid + " >>> " + System.Convert.ToString(sid, 2).PadLeft(24, '0'));

      var r = (byte)(sid >> 8);
      var g = (byte)(sid >> 16);
      var b = (byte)sid;

      //Debug.Log(r + " " + g + " " + b);
      return new Color32(r,
                         g,
                         b,
                         255);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="hash"></param>
    /// <returns></returns>
    public static Color EncodeTagHashCodeAsColor(int hash) {
      //var a = (byte)(hash >> 24);
      var r = (byte)(hash >> 16);
      var g = (byte)(hash >> 8);
      var b = (byte)hash;
      return new Color32(r,
                         g,
                         b,
                         255);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public static Color EncodeLayerAsColor(int layer, float z = .7f) {
      // z value must be in the range (0.5 .. 1.0)
      // in order to avoid color overlaps when using 'divider' in this func

      // First 8 layers are Unity Builtin layers
      // Unity supports up to 32 layers in total

      // Lets create palette of unique 16 colors
      var unique_colors = new[] {
                                    new Color(1,
                                              1,
                                              1,
                                              1),
                                    new Color(z,
                                              z,
                                              z,
                                              1), // 0
                                    new Color(1,
                                              1,
                                              z,
                                              1),
                                    new Color(1,
                                              z,
                                              1,
                                              1),
                                    new Color(z,
                                              1,
                                              1,
                                              1), //
                                    new Color(1,
                                              z,
                                              0,
                                              1),
                                    new Color(z,
                                              0,
                                              1,
                                              1),
                                    new Color(0,
                                              1,
                                              z,
                                              1), // 7

                                    new Color(1,
                                              0,
                                              0,
                                              1),
                                    new Color(0,
                                              1,
                                              0,
                                              1),
                                    new Color(0,
                                              0,
                                              1,
                                              1), // 8
                                    new Color(1,
                                              1,
                                              0,
                                              1),
                                    new Color(1,
                                              0,
                                              1,
                                              1),
                                    new Color(0,
                                              1,
                                              1,
                                              1), //
                                    new Color(1,
                                              z,
                                              z,
                                              1),
                                    new Color(z,
                                              1,
                                              z,
                                              1) // 15
                                };

      // Create as many colors as necessary by using base 16 color palette
      // To create more than 16 - will simply adjust brightness with 'divider'
      var color = unique_colors[layer % unique_colors.Length];
      var divider = 1.0f + Mathf.Floor(layer / unique_colors.Length);
      color /= divider;

      return color;
    }
  }
}
