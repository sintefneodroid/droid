using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace droid.Runtime.Utilities.NeodroidCamera.Synthesis
{
  /// <summary>
  ///
  /// </summary>
  public static class SynthesisUtils
  {
    static readonly int _sensitivity = Shader.PropertyToID("_Sensitivity");

    public static CapturePass[] default_capture_passes =
    {
      new CapturePass {_Name = "_img", ReplacementMode = ReplacementModes.None},
      new CapturePass
      {
        _Name = "_id",
        _SupportsAntialiasing
          = false,
        ReplacementMode = ReplacementModes.Object_id_
      },

      new CapturePass
      {
        _Name = "_layer",
        _SupportsAntialiasing
          = false
        ,ReplacementMode = ReplacementModes.Category_id_
      },
      new CapturePass {_Name = "_depth",ReplacementMode = ReplacementModes.Depth_compressed_},
      new CapturePass {_Name = "_normals", ReplacementMode = ReplacementModes.Normals_},
      new CapturePass
      {
        _Name = "_flow",
        _SupportsAntialiasing
          = false,
        _NeedsRescale = true,ReplacementMode = ReplacementModes.Flow
      },
      new CapturePass
      {
        _Name = "_mat_id",
        _SupportsAntialiasing
          = false,
        ReplacementMode = ReplacementModes.Material_id_
      }
    };

    /// <summary>
    ///
    /// </summary>
    public struct CapturePass
    {
      // configuration
      public string _Name;
      public bool _SupportsAntialiasing;
      public bool _NeedsRescale;
      public Camera _Camera;
      public ReplacementModes ReplacementMode;
    }

    /// <summary>
    ///
    /// </summary>
    public enum ReplacementModes
    {
      Object_id_ = 0,
      Category_id_ = 1,
      Depth_compressed_ = 2,
      Depth_multichannel_ = 3,
      Normals_ = 4,
      Material_id_ = 5,
      None = 6,

      Flow = 7,
    }

    /// <summary>
    ///
    /// </summary>
    public static void SetupCapturePassesFull(Camera camera,
      Shader replacement_shader,
      Shader optical_flow_shader,
      Material optical_flow_material,
      float optical_flow_sensitivity,
      ref CapturePass[] capture_passes)
    {
      SetupHiddenCapturePassCameras(camera, ref capture_passes);
      CleanRefreshPassCameras(camera, ref capture_passes);

      // cache materials and setup material properties
      if (!optical_flow_material || optical_flow_material.shader != optical_flow_shader)
      {
        optical_flow_material = new Material(optical_flow_shader);
      }

      optical_flow_material.SetFloat(_sensitivity, optical_flow_sensitivity);


      // setup command buffers and replacement shaders
      AddReplacementShaderCommandBufferOnCamera(capture_passes[1]._Camera,
        replacement_shader,
        capture_passes[1].ReplacementMode);
      AddReplacementShaderCommandBufferOnCamera(capture_passes[2]._Camera,
        replacement_shader,
        capture_passes[2].ReplacementMode);
      AddReplacementShaderCommandBufferOnCamera(capture_passes[6]._Camera,
        replacement_shader,
        capture_passes[6].ReplacementMode);

      AddReplacementShaderCommandBufferOnCamera(capture_passes[3]._Camera,
        replacement_shader,
        capture_passes[3].ReplacementMode,
        Color.white);
      AddReplacementShaderCommandBufferOnCamera(capture_passes[4]._Camera,
        replacement_shader,
        capture_passes[4].ReplacementMode);
      SetupCameraWithPostShader(capture_passes[5]._Camera,
        optical_flow_material,
        DepthTextureMode.Depth | DepthTextureMode.MotionVectors);
    }

    public static void SetupCapturePassesReplacementShader(Camera camera,
      Shader replacement_shader,
      ref CapturePass[] capture_passes)
    {
      SetupHiddenCapturePassCameras(camera, ref capture_passes);
      CleanRefreshPassCameras(camera,ref capture_passes);

      foreach (var capture_pass in capture_passes)
      {
        AddReplacementShaderCommandBufferOnCamera(capture_pass._Camera,
          replacement_shader,
          capture_pass.ReplacementMode);
      }
    }

    static void CleanRefreshPassCameras(Camera camera,ref CapturePass[] capture_passes)
    {
      var target_display = 1;
      foreach (var pass in capture_passes){
        if (pass._Camera == camera){
          continue;
        }

        pass._Camera.RemoveAllCommandBuffers(); // cleanup capturing camera
        pass._Camera.CopyFrom(camera); // copy all "main" camera parameters into capturing camera
        pass._Camera.targetDisplay =
          target_display++; // set targetDisplay here since it gets overriden by CopyFrom()
      }
    }

    static void AddReplacementShaderCommandBufferOnCamera(Camera cam,
      Shader shader,
      ReplacementModes mode)
    {
      AddReplacementShaderCommandBufferOnCamera(cam, shader, mode, Color.black);
    }

    static void AddReplacementShaderCommandBufferOnCamera(Camera camera,
      Shader shader,
      ReplacementModes mode,
      Color clear_color)
    {
      var cb = new CommandBuffer();
      cb.SetGlobalInt("_OutputMode", (int) mode);
      camera.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, cb);
      camera.AddCommandBuffer(CameraEvent.BeforeFinalPass, cb);
      camera.SetReplacementShader(shader, "");
      camera.backgroundColor = clear_color;
      camera.clearFlags = CameraClearFlags.SolidColor;
    }

    static void SetupCameraWithPostShader(Camera cam,
      Material material,
      DepthTextureMode depth_texture_mode = DepthTextureMode.None)
    {
      var cb = new CommandBuffer();
      cb.Blit(null, BuiltinRenderTextureType.CurrentActive, material);
      cam.AddCommandBuffer(CameraEvent.AfterEverything, cb);
      cam.depthTextureMode = depth_texture_mode;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="camera"></param>
    static void SetupHiddenCapturePassCameras(Camera camera,ref  CapturePass[] capture_passes)
    {
      capture_passes[0]._Camera = camera;
      for (var q = 1; q < capture_passes.Length; q++)
      {
        capture_passes[q]._Camera = CreateHiddenCamera(capture_passes[q]._Name, camera.transform);
      }
    }

    static Camera CreateHiddenCamera(string cam_name, Transform parent)
    {
      var go = new GameObject(cam_name, typeof(Camera)) {hideFlags = HideFlags.HideAndDontSave};
      go.transform.parent = parent;

      var new_camera = go.GetComponent<Camera>();
      return new_camera;
    }
  }
}