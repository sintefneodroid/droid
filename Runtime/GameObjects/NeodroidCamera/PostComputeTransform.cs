using UnityEngine;
using UnityEngine.Rendering;

namespace droid.Runtime.GameObjects.NeodroidCamera {
  /// <summary>
  ///
  /// </summary>
  [RequireComponent(typeof(Camera))]
  [ExecuteInEditMode]
  public class PostComputeTransform : MonoBehaviour {
    /// <summary>
    ///
    /// </summary>
    public ComputeShader _TransformationComputeShader;

    //[SerializeField] Material _post_material;

    [Header("Specific", order = 102)]
    [SerializeField]
    Camera _camera = null;

    ComputeBuffer _transformation_compute_buffer;
    CommandBuffer _transformation_command_buffer;

    void Awake() {
      if (this._camera == null) {
        this._camera = this.GetComponent<Camera>();
      }

      this.MyRenderTexture = new RenderTexture(256, 256, 0) {enableRandomWrite = true};
      this.MyRenderTexture.Create();

      if (this._TransformationComputeShader) {
        var kernel_id = this._TransformationComputeShader.FindKernel("CSMain");

        this._transformation_command_buffer = new CommandBuffer();

        /*
      var target_texture = this._camera.targetTexture;
      this._TransformationComputeShader.SetTexture(kernel_id,
                                                   "Result",
                                                   target_texture);

      this._transformation_compute_buffer = new ComputeBuffer(target_texture.width * target_texture.height * target_texture.depth,
                                        sizeof(float)) {name = "My Buffer"};

*/

        //Graphics.SetRandomWriteTarget(1, my_buffer);

        //myBuffer.SetData(minMaxHeight);
        //this.my_buffer.targetTexture.GetNativeDepthBufferPtr().GetBuffer(0, "minMax", minMaxBuffer);

        //_TransformationComputeShader.SetFloat("gamma", 2.2);
        //_TransformationComputeShader.Dispatch(0, map.Length, 1, 1);

        //my_buffer.SetData(*target_texture.GetNativeTexturePtr());

//      this._TransformationComputeShader.SetTexture(kernelHandle, "Result", target_texture);

        this._transformation_command_buffer.SetComputeTextureParam(this._TransformationComputeShader,
                                                                   kernel_id,
                                                                   "Result",
                                                                   this.MyRenderTexture);
        //this._transformation_command_buffer.SetComputeBufferParam(this._TransformationComputeShader, kernel_id,"Result",this._transformation_compute_buffer);
        this._transformation_command_buffer.DispatchCompute(this._TransformationComputeShader,
                                                            kernel_id,
                                                            256 / 32,
                                                            256 / 32,
                                                            1);
        //this._camera.AddCommandBuffer(CameraEvent.AfterEverything, this._transformation_command_buffer);
      }
    }

    void Update() {
      if (this._TransformationComputeShader) {
        this._TransformationComputeShader.SetTexture(0, "Result", this.MyRenderTexture);
        //this._TransformationComputeShader.SetBuffer(0,"",this._transformation_compute_buffer);
        this._TransformationComputeShader.Dispatch(0,
                                                   256 / 32,
                                                   256 / 32,
                                                   1);
      }
    }

    /// <summary>
    ///
    /// </summary>
    public RenderTexture MyRenderTexture { get; set; }

// Postprocess the image
/*
  void OnRenderImage(RenderTexture source, RenderTexture destination) {



    Graphics.Blit(source, destination);
  }
*/

    /// <summary>
    ///
    /// </summary>
    public void OnDisable() { this.Cleanup(); }

    void Cleanup() {
      if (this._transformation_command_buffer != null) {
        this._camera.RemoveCommandBuffer(CameraEvent.AfterEverything, this._transformation_command_buffer);
      }
    }

    void OnDestroy() {
      this.Cleanup();

//DestroyImmediate(this._GammaCommandBuffer);
    }
  }
}
