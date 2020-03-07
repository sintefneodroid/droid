using System;
using System.Collections.Generic;
using droid.Runtime.GameObjects.NeodroidCamera.Synthesis;
using UnityEngine;

namespace droid.Runtime.GameObjects.NeodroidCamera.Segmentation {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public class MaterialIdSegmenter : Segmenter {
    /// <summary>
    /// </summary>
    Renderer[] _all_renders = null;

    /// <summary>
    /// </summary>
    MaterialPropertyBlock _block = null;

    [SerializeField] Shader segmentation_shader = null;
    [SerializeField] Camera _camera = null;

    /// <summary>
    /// </summary>
    public Dictionary<Material, Color> ColorsDictGameObject { get; set; } = new Dictionary<Material, Color>();

    /// <summary>
    /// </summary>
    public override Dictionary<String, Color> ColorsDict {
      get {
        var colors = new Dictionary<String, Color>();
        foreach (var key_val in this.ColorsDictGameObject) {
          if (!colors.ContainsKey(key : key_val.Key.name)) {
            colors.Add(key : key_val.Key.name, value : key_val.Value);
          }
        }

        return colors;
      }
    }

    // Use this for initialization
    /// <summary>
    /// </summary>
    void Start() { this.Setup(); }

    void CheckBlock() {
      if (this._block == null) {
        this._block = new MaterialPropertyBlock();
      }
    }

    SynthesisUtilities.CapturePass[] _capture_passes = {
                                                           new SynthesisUtilities.CapturePass {
                                                                                                  _Name =
                                                                                                      "_material_id",
                                                                                                  _ReplacementMode
                                                                                                      = SynthesisUtilities
                                                                                                        .ReplacementModes
                                                                                                        .Material_id_,
                                                                                                  _SupportsAntialiasing
                                                                                                      = false
                                                                                              }
                                                       };

    /// <summary>
    /// </summary>
    void Setup() {
      this._all_renders = FindObjectsOfType<Renderer>();

      this._camera = this.GetComponent<Camera>();
      SynthesisUtilities.SetupCapturePassesReplacementShader(camera : this._camera,
                                                             replacement_shader : this.segmentation_shader,
                                                             capture_passes : ref this._capture_passes);

      this.ColorsDictGameObject = new Dictionary<Material, Color>();
      this.CheckBlock();
      for (var index = 0; index < this._all_renders.Length; index++) {
        var r = this._all_renders[index];
        r.GetPropertyBlock(properties : this._block);
        var sm = r.sharedMaterial;
        if (sm) {
          var id = sm.GetInstanceID();
          var color = ColorEncoding.EncodeIdAsColor(instance_id : id);
          if (!this.ColorsDictGameObject.ContainsKey(key : sm)) {
            this.ColorsDictGameObject.Add(key : sm, value : color);
          }

          this._block.SetColor(name : SynthesisUtilities._Shader_MaterialId_Color_Name, value : color);
          r.SetPropertyBlock(properties : this._block);
        }
      }
    }
  }
}
