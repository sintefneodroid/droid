using System;
using System.Collections.Generic;
using droid.Runtime.GameObjects.NeodroidCamera.Synthesis;
using UnityEngine;

namespace droid.Runtime.GameObjects.NeodroidCamera.Segmentation {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public class ObjectIdSegmenter : Segmenter {
    /// <summary>
    /// </summary>
    Renderer[] _all_renders;

    /// <summary>
    /// </summary>
    MaterialPropertyBlock _block;

    [SerializeField] Shader segmentation_shader = null;
    [SerializeField] Camera _camera;

    /// <summary>
    /// </summary>
    public Dictionary<GameObject, Color> ColorsDictGameObject { get; set; } =
      new Dictionary<GameObject, Color>();

    /// <summary>
    /// </summary>
    public override Dictionary<string, Color> ColorsDict {
      get {
        var colors = new Dictionary<string, Color>();
        foreach (var key_val in this.ColorsDictGameObject) {
          colors.Add(key : key_val.Key.GetInstanceID().ToString(), value : key_val.Value);
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
                                                                                                      "_object_id",
                                                                                                  _ReplacementMode
                                                                                                      = SynthesisUtilities
                                                                                                        .ReplacementModes
                                                                                                        .Object_id_,
                                                                                                  _SupportsAntialiasing
                                                                                                      = false
                                                                                              }
                                                       };

    /// <summary>
    /// </summary>
    void Setup() {
      this._camera = this.GetComponent<Camera>();
      SynthesisUtilities.SetupCapturePassesReplacementShader(camera : this._camera,
                                                             replacement_shader : this.segmentation_shader,
                                                             capture_passes : ref this._capture_passes);
      this.ColorsDictGameObject = new Dictionary<GameObject, Color>();
      this._all_renders = FindObjectsOfType<Renderer>();
      this.CheckBlock();
      for (var index = 0; index < this._all_renders.Length; index++) {
        var r = this._all_renders[index];
        r.GetPropertyBlock(properties : this._block);
        var game_object = r.gameObject;
        var id = game_object.GetInstanceID();
        var layer = game_object.layer;
        var go_tag = game_object.tag;

        if (!this.ColorsDictGameObject.ContainsKey(key : game_object)) {
          this.ColorsDictGameObject.Add(key : game_object,
                                        value : ColorEncoding.EncodeIdAsColor(instance_id : id));
        } else {
          #if NEODROID_DEBUG
          if (true) {
            Debug.LogWarning(message : $"ColorDict Duplicate {game_object}");
          }
          #endif
        }

        this._block.SetColor(name : SynthesisUtilities._Shader_ObjectId_Color_Name,
                             value : ColorEncoding.EncodeIdAsColor(instance_id : id));
/*
this._block?.SetInt(SynthesisUtils._Shader_OutputMode_Name,(int) SynthesisUtils.ReplacementModes
                                                                          .Object_id_);
                                                                          */
        //this._block.SetColor("_CategoryIdColor", ColorEncoding.EncodeLayerAsColor(layer));
        //this._block.SetColor("_MaterialIdColor", ColorEncoding.EncodeIdAsColor(id));
        //this._block.SetColor("_CategoryColor", ColorEncoding.EncodeTagHashCodeAsColor(go_tag));
        r.SetPropertyBlock(properties : this._block);
      }
    }
  }
}
