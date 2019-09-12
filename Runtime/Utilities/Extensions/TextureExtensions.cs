using System.Runtime.InteropServices;
using UnityEngine;

namespace droid.Runtime.Utilities.Extensions {
  /// <summary>
  ///
  /// </summary>
  public static class TextureExtentions {
    /// <summary>
    ///
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct Color32Array {
      /// <summary>
      ///
      /// </summary>
      [FieldOffset(0)]
      public byte[] byteArray;

      /// <summary>
      ///
      /// </summary>
      [FieldOffset(0)]
      public Color32[] colors;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="texture"></param>
    /// <returns></returns>
    public static Texture2D ToTexture2D(this WebCamTexture texture) {
      var color_array = new Color32Array {colors = new Color32[texture.width * texture.height]};
      texture.GetPixels32(color_array.colors);
      var tex = new Texture2D(2, 2);
      tex.LoadRawTextureData(color_array.byteArray);
      tex.Apply();

      return tex;

      /*
      var ntv_p = texture.GetNativeTexturePtr();

      return Texture2D.CreateExternalTexture(texture.width,
                                             texture.height,
                                             TextureFormat.RGBAFloat,
                                             false,
                                             true,
                                             ntv_p);
    */
    }
  }
}
