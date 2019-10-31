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
    public override Dictionary<String, Color> ColorsDict {
      get {
        var colors = new Dictionary<String, Color>();
        foreach (var key_val in this.ColorsDictGameObject) {
          colors.Add(key_val.Key.GetInstanceID().ToString(), key_val.Value);
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
                                                                                                      = SynthesisUtilities.ReplacementModes
                                                                                                        .Object_id_,
                                                                                                  _SupportsAntialiasing
                                                                                                      = false
                                                                                              }
                                                       };

    /// <summary>
    /// </summary>
    void Setup() {
      this._camera = this.GetComponent<Camera>();
      SynthesisUtilities.SetupCapturePassesReplacementShader(this._camera,
                                                             this.segmentation_shader,
                                                             ref this._capture_passes);
      this.ColorsDictGameObject = new Dictionary<GameObject, Color>();
      this._all_renders = FindObjectsOfType<Renderer>();
      this.CheckBlock();
      foreach (var r in this._all_renders) {
        r.GetPropertyBlock(this._block);
        var game_object = r.gameObject;
        var id = game_object.GetInstanceID();
        var layer = game_object.layer;
        var go_tag = game_object.tag;

        if (!this.ColorsDictGameObject.ContainsKey(game_object)) {
          this.ColorsDictGameObject.Add(game_object, ColorEncoding.EncodeIdAsColor(id));
        } else {
          #if NEODROID_DEBUG
          if (true) {
            Debug.LogWarning($"ColorDict Duplicate {game_object}");
          }
          #endif
        }

        this._block.SetColor(SynthesisUtilities._Shader_ObjectId_Color_Name,
                             ColorEncoding.EncodeIdAsColor(id));
/*
this._block?.SetInt(SynthesisUtils._Shader_OutputMode_Name,(int) SynthesisUtils.ReplacementModes
                                                                          .Object_id_);
                                                                          */
        //this._block.SetColor("_CategoryIdColor", ColorEncoding.EncodeLayerAsColor(layer));
        //this._block.SetColor("_MaterialIdColor", ColorEncoding.EncodeIdAsColor(id));
        //this._block.SetColor("_CategoryColor", ColorEncoding.EncodeTagHashCodeAsColor(go_tag));
        r.SetPropertyBlock(this._block);
      }
    }
  }
}
