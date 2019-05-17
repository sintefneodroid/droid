using System;
using System.Collections.Generic;
using System.Linq;
using droid.Runtime.Interfaces;
using droid.Runtime.Managers;
using droid.Runtime.Utilities.Enums;
using droid.Runtime.Utilities.Misc;
using droid.Runtime.Utilities.Structs;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace droid.Runtime.Prototyping.Sensors.Camera {
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "ByteArrayCamera"
                    + SensorComponentMenuPath._Postfix)]
  public class ByteArrayCameraSensor : Sensor,
                                       IHasByteArray {
    [Header("Observation", order = 103)] byte[] byte_array;

    [Header("Specific", order = 102)]
    [SerializeField]
    UnityEngine.Camera _camera = null;

    bool _grab = true;

    IManager _manager = null;

    [SerializeField] Texture2D _texture = null;

    /// <summary>
    ///
    /// </summary>
    public Space1[] ObservationSpace { get { return new[] {Space1.ZeroOne}; } }

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
                                      false);
      } else {
        this._texture = new Texture2D(target_texture.width,
                                      target_texture.height,
                                      target_texture.graphicsFormat,
                                      this.flags);
      }
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void OnPostRender() {
      #if NEODROID_DEBUG
      if(this.Debugging){
            this._grab = true;
      }
      #endif
      if (this._camera.targetTexture) {
        this.UpdateArray();
      }
      #if NEODROID_DEBUG
      if(this.Debugging){
      Graphics.DrawTexture(new Rect(new Vector2(0,0),new Vector2(128,128)), this._texture);
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

        this._texture.ReadPixels(new Rect(0, 0, this._texture.width, this._texture.height), 0, 0);

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

    /// <summary>
    ///
    /// </summary>
    public GraphicsFormat DataType { get { return this._texture.graphicsFormat; } }
  }
}
