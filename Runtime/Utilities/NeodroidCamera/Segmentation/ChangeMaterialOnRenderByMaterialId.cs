using System;
using System.Collections.Generic;
using droid.Runtime.Utilities.NeodroidCamera.Synthesis;
using droid.Runtime.Utilities.Structs;
using UnityEngine;
using UnityEngine.Serialization;
using Object = System.Object;
using Random = UnityEngine.Random;

namespace droid.Runtime.Utilities.NeodroidCamera.Segmentation {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public class ChangeMaterialOnRenderByMaterialId : Segmenter {
    /// <summary>
    /// </summary>
    Renderer[] _all_renders;

    /// <summary>
    /// </summary>
    MaterialPropertyBlock _block;

    [SerializeField] Shader segmentation_shader;
    [SerializeField] Camera _camera;

    /// <summary>
    /// </summary>
    public Dictionary<Material, Color> ColorsDictGameObject { get; set; } = new Dictionary<Material, Color>();

    /// <summary>
    /// </summary>
    public override Dictionary<String, Color> ColorsDict {
      get {
        var colors = new Dictionary<String, Color>();
        foreach (var key_val in this.ColorsDictGameObject) {
          colors.Add(key_val.Key.name, key_val.Value);
        }

        return colors;
      }
    }

    // Use this for initialization
    /// <summary>
    /// </summary>
    void Start() {
      this.Setup();
    }

    /// <summary>
    /// </summary>
    void Awake() { //this.Setup();
                   }

    /// <summary>
    /// </summary>
    void Update() { }

    void CheckBlock() {
      if (this._block == null) {
        this._block = new MaterialPropertyBlock();
      }
    }

    SynthesisUtils.CapturePass[] _capture_passes = {
                                           new SynthesisUtils.CapturePass {
                                                                              _Name = "_material_id",
                                                                              ReplacementMode =
                                                                                  SynthesisUtils
                                                                                      .ReplacementModes
                                                                                      .Material_id_,
                                                                              _SupportsAntialiasing = false
                                                                          }
                                       };

    /// <summary>
    /// </summary>
    void Setup() {
      this._all_renders = FindObjectsOfType<Renderer>();
      this._block = new MaterialPropertyBlock();

      this._camera = this.GetComponent<Camera>();
      SynthesisUtils.SetupCapturePassesReplacementShader(this._camera,
                                                         this.segmentation_shader,
                                                         ref this._capture_passes);

      this.ColorsDictGameObject = new Dictionary<Material, Color>();
      this.CheckBlock();
      foreach (var r in this._all_renders) {
        var sm = r.sharedMaterial;
        if (sm) {
          var id = sm.GetInstanceID();
          var color = ColorEncoding.EncodeIdAsColor(id);
          if(!this.ColorsDictGameObject.ContainsKey(sm)) {
            this.ColorsDictGameObject.Add(sm, color);
          }

          this._block.SetColor("_MaterialIdColor", color);
          r.SetPropertyBlock(this._block);
        }
      }
    }
  }
}
