using UnityEngine;

namespace droid.Runtime.GameObjects.Flipping {
  /// <summary>
  ///
  /// </summary>
  public class Flipper : MonoBehaviour {
    [SerializeField] ComputeShader _shader;
    [SerializeField] Texture2D _texture_2d;

    void Start() { this._texture_2d = new Texture2D(256, 256); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="my_texture"></param>
    /// <param name="result"></param>
    public void FlipImage(Texture my_texture, Texture2D result) {
      var kernel_handle = this._shader.FindKernel("Flip");
      var tex =
          new RenderTexture(width : my_texture.width, height : my_texture.height, 24) {
                                                                                          enableRandomWrite =
                                                                                              true
                                                                                      };
      tex.Create();

      this._shader.SetTexture(kernelIndex : kernel_handle, "Result", texture : tex);
      this._shader.SetTexture(kernelIndex : kernel_handle, "ImageInput", texture : my_texture);
      this._shader.SetInt("width", val : my_texture.width);
      this._shader.SetInt("height", val : my_texture.height);
      this._shader.Dispatch(kernelIndex : kernel_handle,
                            threadGroupsX : my_texture.width / 8,
                            threadGroupsY : my_texture.height / 8,
                            1);

      RenderTexture.active = tex;
      result.ReadPixels(source : new Rect(0,
                                          0,
                                          width : tex.width,
                                          height : tex.height),
                        destX : 0,
                        destY : 0);
      result.Apply();
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest) {
      this.FlipImage(my_texture : src, result : this._texture_2d);

      Graphics.Blit(source : this._texture_2d, dest : dest);
    }
  }
}
