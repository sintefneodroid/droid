using UnityEngine;
using UnityEngine.Rendering;

namespace droid.Runtime.Utilities.NeodroidCamera.Synthesis {
  /// <summary>
  ///
  /// </summary>
  public static class SynthesisUtils {
    static readonly int _sensitivity = Shader.PropertyToID("_Sensitivity");

    public static CapturePass[] capture_passes = {
                                                     new SynthesisUtils.CapturePass {_Name = "_img"},
                                                     new SynthesisUtils.CapturePass {
                                                                                        _Name = "_id",
                                                                                        _SupportsAntialiasing
                                                                                            = false
                                                                                    },
                                                     new SynthesisUtils.CapturePass {
                                                                                        _Name = "_layer",
                                                                                        _SupportsAntialiasing
                                                                                            = false
                                                                                    },
                                                     new SynthesisUtils.CapturePass {_Name = "_depth"},
                                                     new SynthesisUtils.CapturePass {_Name = "_normals"},
                                                     new SynthesisUtils.CapturePass {
                                                                                        _Name = "_flow",
                                                                                        _SupportsAntialiasing
                                                                                            = false,
                                                                                        _NeedsRescale = true
                                                                                    }
                                                 }; // pass configuration

    /// <summary>
    ///
    /// </summary>
    public struct CapturePass { // configuration
      public string _Name;
      public bool _SupportsAntialiasing;
      public bool _NeedsRescale;
      public Camera _Camera;
    }

    /// <summary>
    ///
    /// </summary>
    public enum ReplacementModes {
      Object_id_ = 0,
      Category_id_ = 1,
      Depth_compressed_ = 2,
      Depth_multichannel_ = 3,
      Normals_ = 4
    }

    /// <summary>
    ///
    /// </summary>
    public static void OnCameraChangeFull(Camera main_camera,
                                          Shader segmentation_shader,
                                          Shader optical_flow_shader,
                                          Material optical_flow_material,
                                          float optical_flow_sensitivity) {
      PreOnCameraChange(main_camera);

      // cache materials and setup material properties
      if (!optical_flow_material || optical_flow_material.shader != optical_flow_shader) {
        optical_flow_material = new Material(optical_flow_shader);
      }

      optical_flow_material.SetFloat(_sensitivity, optical_flow_sensitivity);

      // setup command buffers and replacement shaders
      SetupCameraWithReplacementShader(capture_passes[1]._Camera,
                                       segmentation_shader,
                                       ReplacementModes.Object_id_);
      SetupCameraWithReplacementShader(capture_passes[2]._Camera,
                                       segmentation_shader,
                                       ReplacementModes.Category_id_);
      SetupCameraWithReplacementShader(capture_passes[3]._Camera,
                                       segmentation_shader,
                                       ReplacementModes.Depth_compressed_,
                                       Color.white);
      SetupCameraWithReplacementShader(capture_passes[4]._Camera,
                                       segmentation_shader,
                                       ReplacementModes.Normals_);
      SetupCameraWithPostShader(capture_passes[5]._Camera,
                                optical_flow_material,
                                DepthTextureMode.Depth | DepthTextureMode.MotionVectors);
    }

    /// <summary>
    ///
    /// </summary>
    public static void SetupOnCameraChangeObjectId(Camera main_camera, Shader segmentation_shader) {
      PreOnCameraChange(main_camera);

      // setup command buffers and replacement shaders
      SetupCameraWithReplacementShader(capture_passes[1]._Camera,
                                       segmentation_shader,
                                       ReplacementModes.Object_id_);
      SetupCameraWithReplacementShader(capture_passes[2]._Camera,
                                       segmentation_shader,
                                       ReplacementModes.Category_id_);
    }

    public static void PreOnCameraChange(Camera main_camera) {
      var target_display = 1;
      foreach (var pass in capture_passes) {
        if (pass._Camera == main_camera) {
          continue;
        }

        // cleanup capturing camera
        pass._Camera.RemoveAllCommandBuffers();

        // copy all "main" camera parameters into capturing camera
        pass._Camera.CopyFrom(main_camera);

        // set targetDisplay here since it gets overriden by CopyFrom()
        pass._Camera.targetDisplay = target_display++;
      }
    }

    static void SetupCameraWithReplacementShader(Camera cam,
                                                 Shader shader,
                                                 SynthesisUtils.ReplacementModes mode) {
      SetupCameraWithReplacementShader(cam, shader, mode, Color.black);
    }

    static void SetupCameraWithReplacementShader(Camera cam,
                                                 Shader shader,
                                                 SynthesisUtils.ReplacementModes mode,
                                                 Color clear_color) {
      var cb = new CommandBuffer();
      //cb.SetGlobalFloat("_OutputMode", (int)mode);
      cb.SetGlobalInt("_OutputMode", (int)mode);
      cam.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, cb);
      cam.AddCommandBuffer(CameraEvent.BeforeFinalPass, cb);
      cam.SetReplacementShader(shader, "");
      cam.backgroundColor = clear_color;
      cam.clearFlags = CameraClearFlags.SolidColor;
    }

    static void SetupCameraWithPostShader(Camera cam,
                                          Material material,
                                          DepthTextureMode depth_texture_mode = DepthTextureMode.None) {
      var cb = new CommandBuffer();
      cb.Blit(null, BuiltinRenderTextureType.CurrentActive, material);
      cam.AddCommandBuffer(CameraEvent.AfterEverything, cb);
      cam.depthTextureMode = depth_texture_mode;
    }

    static Camera CreateHiddenCamera(string cam_name, Transform parent) {
      var go = new GameObject(cam_name, typeof(Camera)) {hideFlags = HideFlags.HideAndDontSave};
      go.transform.parent = parent;

      var new_camera = go.GetComponent<Camera>();
      return new_camera;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="main_camera"></param>
    public static void Setup(Camera main_camera) { // use real camera to capture final image
      capture_passes[0]._Camera = main_camera;
      for (var q = 1; q < capture_passes.Length; q++) {
        capture_passes[q]._Camera = CreateHiddenCamera(capture_passes[q]._Name, main_camera.transform);
      }
    }
  }
}
