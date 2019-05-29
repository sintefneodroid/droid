using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Managers;
using droid.Runtime.Utilities.Enums;
using droid.Runtime.Utilities.Misc;
using droid.Runtime.Utilities.Structs;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace droid.Runtime.Prototyping.Sensors.Camera {
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "ByteArrayCamera"
                    + SensorComponentMenuPath._Postfix)]
  public class ByteArrayCameraSensor : Sensor,
                                       IHasByteArray {
    //[Header("Observation", order = 103)]
    byte[] byte_array;

    [Header("Specific", order = 102)]
    [SerializeField]
    UnityEngine.Camera _camera = null;

    bool _grab = true;

    //[SerializeField] bool linear_space;
    IManager _manager = null;

    [SerializeField] Texture2D _texture = null;

    [SerializeField]ComputeShader _TransformationComputeShader;
    [SerializeField]CommandBuffer _TransformationCommandBuffer;
    Material _post_material;
    float gamma = 2.2f;


    protected override void PreSetup() {
      if (this._manager == null) {
        this._manager = FindObjectOfType<NeodroidManager>();
      }

      if (this._camera == null) {
        this._camera = this.GetComponent<UnityEngine.Camera>();
      }

      var target_texture = this._camera.targetTexture;
      if (!target_texture) {
        #if NEODROID_DEBUG
        Debug.LogWarning("Texture not available!");
        #endif
        this._texture = new Texture2D(NeodroidConstants._Default_Width,
                                      NeodroidConstants._Default_Height,
                                      NeodroidConstants._Default_TextureFormat,
                                      false
                                      //,this.linear_space
                                     );
      } else {
        this._texture = new Texture2D(target_texture.width,
                                      target_texture.height,
                                      target_texture.graphicsFormat,
                                      this.flags);
      }

        /*
      if(this._TransformationComputeShader){

        this._TransformationCommandBuffer = new CommandBuffer();
        this._TransformationCommandBuffer.DispatchCompute(this._TransformationComputeShader);
        this._camera.AddCommandBuffer(CameraEvent.AfterEverything,this._TransformationCommandBuffer);

        int[] minMaxHeight = { floatToIntMultiplier * numOctaves, 0 };
        ComputeBuffer minMaxBuffer = new ComputeBuffer (minMaxHeight.Length, sizeof (int));
        minMaxBuffer.SetData (minMaxHeight);
        heightMapComputeShader.SetBuffer (0, "minMax", minMaxBuffer);

        heightMapComputeShader.SetInt ("mapSize", mapSize);
        heightMapComputeShader.SetInt ("octaves", numOctaves);
        heightMapComputeShader.SetFloat ("lacunarity", lacunarity);
        heightMapComputeShader.SetFloat ("persistence", persistence);
        heightMapComputeShader.SetFloat ("scaleFactor", initialScale);
        heightMapComputeShader.SetInt ("floatToIntMultiplier", floatToIntMultiplier);

        heightMapComputeShader.Dispatch (0, map.Length, 1, 1);

        mapBuffer.GetData (map);
        minMaxBuffer.GetData (minMaxHeight);
      }
      */

        _post_material = new Material( Shader.Find("Neodroid/Gamma") );

  }

  // Postprocess the image
  void OnRenderImage(RenderTexture source, RenderTexture destination) {

    this._post_material.SetFloat("_gamma", gamma);
    Graphics.Blit(source, destination, _post_material);

  }

  void OnDestroy() {

      /*if (this._TransformationCommandBuffer!=null) {
        this._camera.RemoveCommandBuffer(CameraEvent.AfterEverything,this._TransformationCommandBuffer);
      }*/
      //DestroyImmediate(this._GammaCommandBuffer);
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void OnPostRender() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        this._grab = true;
      }
      #endif
      if (this._camera.targetTexture) {
        this.UpdateArray();
      }
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Graphics.DrawTexture(new Rect(new Vector2(0, 0), new Vector2(0, 0)), this._texture);
      }
      #endif
    }

    TextureCreationFlags flags;

    /// <summary>
    ///
    /// </summary>
    protected virtual void UpdateArray() {
      if (!this._grab) {
        return;
      }

      this._grab = false;

      if (this._camera) {
        var current_render_texture = RenderTexture.active;
        var texture = this._camera.targetTexture;
        //texture.GetNativeTexturePtr()
        RenderTexture.active = texture;

        this._texture.ReadPixels(new Rect(0, 0, this._texture.width, this._texture.height),
                                 0,
                                 0,
                                 recalculateMipMaps : false);

        //this._texture.Apply();

        this.Bytes = this._texture.GetRawTextureData();
        //Debug.Log($"{this.Identifier}:{this.Bytes.Length}");

        RenderTexture.active = current_render_texture;
      } else {
        Debug.LogWarning($"No camera found on {this}");
      }
    }

    public override String PrototypingTypeName { get { return ""; } }

    public override IEnumerable<float> FloatEnumerable {
      get { return null; } //this.ObservationArray; }
    }

    public override void UpdateObservation() {
      this._grab = true;
      if (this._manager?.SimulatorConfiguration?.SimulationType != SimulationType.Frame_dependent_) {
        #if NEODROID_DEBUG
        Debug.Log($"{this._manager?.SimulatorConfiguration?.SimulationType}");
        #endif
        if (Application.isPlaying) {
          this._camera.Render();
        }

        this.UpdateArray();
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      var rep = $"Very Large Float Array (Length: {this.Bytes.Length}), "
                + $"Sample [{Mathf.Clamp01(this.byte_array[0])}.."
                + $"{Mathf.Clamp01(this.byte_array[this.byte_array.Length - 1])}]";

      return rep;
    }

    /// <summary>
    ///
    /// </summary>
    public Byte[] Bytes { get; set; }

    public Int32[] Shape {
      get {
        int channels;
        switch (this._texture.graphicsFormat) {
          case GraphicsFormat.R8_UNorm:
          case GraphicsFormat.R16_SFloat:
          case GraphicsFormat.R32_SFloat:
            channels = 1;
            break;
          //case GraphicsFormat.R32G32B32A32_SFloat:
          //case GraphicsFormat.B8G8R8A8_UNorm:
          //case GraphicsFormat.R16G16B16A16_SFloat:
          default:
            channels = 4;
            break;
        }

        return new[] {this._texture.width, this._texture.height, channels};
      }
    }

    /// <summary>
    ///
    /// </summary>
    public String ArrayEncoding {
      get {
        string s;
        switch (this._texture.graphicsFormat) {
          case GraphicsFormat.R8G8B8A8_UNorm:
          case GraphicsFormat.R8_UInt:
            s = "UINT8";
            break;
          case GraphicsFormat.R16_SFloat:
          case GraphicsFormat.R16G16B16A16_SFloat:
            s = "FLOAT16";
            break;
          case GraphicsFormat.R32_SFloat:
          case GraphicsFormat.R32G32B32A32_SFloat:
            s = "FLOAT32";
            break;
          default:
            s = "Unknown";
            break;
        }

        return s;
      }
    }
  }
}
