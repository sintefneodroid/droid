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
      var tex = new RenderTexture(my_texture.width, my_texture.height, 24) {enableRandomWrite = true};
      tex.Create();

      this._shader.SetTexture(kernel_handle, "Result", tex);
      this._shader.SetTexture(kernel_handle, "ImageInput", my_texture);
      this._shader.SetInt("width", my_texture.width);
      this._shader.SetInt("height", my_texture.height);
      this._shader.Dispatch(kernel_handle,
                            my_texture.width / 8,
                            my_texture.height / 8,
                            1);

      RenderTexture.active = tex;
      result.ReadPixels(new Rect(0,
                                 0,
                                 tex.width,
                                 tex.height),
                        0,
                        0);
      result.Apply();
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest) {
      this.FlipImage(src, this._texture_2d);

      Graphics.Blit(this._texture_2d, dest);
    }
  }
}
