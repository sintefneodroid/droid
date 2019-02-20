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
  public class CategorySegmenter : Segmenter {
    /// <summary>
    /// </summary>
    Renderer[] _all_renders;

    /// <summary>
    /// </summary>
    MaterialPropertyBlock _block;

    [SerializeField] Shader segmentation_shader;
    [SerializeField] Camera _camera;

    enum SegmentationMode {
      Tag,
      Layer
    }

    [SerializeField] ScriptableObjects.Segmentation _segmentation;
    /// <summary>
    /// </summary>
    public bool _Replace_Untagged_Color = true;
    /// <summary>
    /// </summary>
    public Color _Untagged_Color = Color.black;
    /// <summary>
    /// </summary>
    public ColorByCategory[] ColorsByCategory { get { return this._colors_by_category; } }

    [SerializeField]
    protected ColorByCategory[] _colors_by_category;

    [SerializeField] SegmentationMode _segmentation_mode;

    /// <summary>
    /// </summary>
    public Dictionary<string, Color> ColorsDictGameObject { get; set; } = new Dictionary<string, Color>();

    /// <summary>
    /// </summary>
    public override Dictionary<String, Color> ColorsDict {
      get {
        var colors = new Dictionary<String, Color>();
        foreach (var key_val in this.ColorsDictGameObject) {
          colors.Add(key_val.Key, key_val.Value);
        }

        return colors;
      }
    }

    // Use this for initialization
    /// <summary>
    /// </summary>
    void Start() { this.Setup(); }

    /// <summary>
    /// </summary>
    void Awake() {
      /*
      var colors_by_tag = this._colors_by_category;
      if (colors_by_tag != null && colors_by_tag.Length > 0) {
        foreach (var tag_color in this._colors_by_category) {
          if (!this.ColorsDictGameObject.ContainsKey(tag_color._Tag)) {
            this.ColorsDictGameObject.Add(tag_color._Tag, tag_color._Col);
          }
        }
      }

      if (this._segmentation) {
        var segmentation_color_by_tags = this._segmentation._color_by_categories;
        if (segmentation_color_by_tags != null) {
          foreach (var tag_color in segmentation_color_by_tags) {
            if (!this.ColorsDictGameObject.ContainsKey(tag_color._Tag)) {
              this.ColorsDictGameObject.Add(tag_color._Tag, tag_color._Col);
            }
          }
        }
      }*/
    }


    void CheckBlock() {
      if (this._block == null) {
        this._block = new MaterialPropertyBlock();
      }
    }

    SynthesisUtils.CapturePass[] _capture_passes = {
                                                       new SynthesisUtils.CapturePass {
                                                                                          _Name = "_tag_id",
                                                                                          _ReplacementMode =
                                                                                              SynthesisUtils
                                                                                                  .ReplacementModes
                                                                                                  .Tag_id_,
                                                                                          _SupportsAntialiasing
                                                                                              = false
                                                                                      }
                                                   };

    /// <summary>
    /// </summary>
    void Setup() {
      this._all_renders = FindObjectsOfType<Renderer>();

      switch (this._segmentation_mode) {
        case SegmentationMode.Tag:
          this._capture_passes = new[] {
                                           new SynthesisUtils.CapturePass {
                                                                              _Name = "_tag_id",
                                                                              _ReplacementMode =
                                                                                  SynthesisUtils
                                                                                      .ReplacementModes
                                                                                      .Tag_id_,
                                                                              _SupportsAntialiasing = false
                                                                          }
                                       };
          break;
        case SegmentationMode.Layer:
          this._capture_passes = new[] {
                                           new SynthesisUtils.CapturePass {
                                                                              _Name = "_layer_id",
                                                                              _ReplacementMode =
                                                                                  SynthesisUtils
                                                                                      .ReplacementModes
                                                                                      .Layer_id_,
                                                                              _SupportsAntialiasing = false
                                                                          }
                                       };
          break;
        default: throw new ArgumentOutOfRangeException();
      }

      this._camera = this.GetComponent<Camera>();
      SynthesisUtils.SetupCapturePassesReplacementShader(this._camera,
                                                         this.segmentation_shader,
                                                         ref this._capture_passes);

      this.ColorsDictGameObject = new Dictionary<string, Color>();
      this.CheckBlock();
      foreach (var r in this._all_renders) {
        r.GetPropertyBlock(this._block);
        string category_name;
        Color color;
        switch (this._segmentation_mode) {
          case SegmentationMode.Tag:
            category_name = r.tag;
            color = ColorEncoding.EncodeIdAsColor(category_name.GetHashCode());
            this._block.SetColor(SynthesisUtils._Shader_Tag_Color_Name, color);
            break;
          case SegmentationMode.Layer:
            var layer_int = r.gameObject.layer;
            color = ColorEncoding.EncodeLayerAsColor(layer_int);

            category_name = LayerMask.LayerToName(layer_int);
            this._block.SetColor(SynthesisUtils._Shader_Layer_Color_Name, color);
            break;
          default: throw new ArgumentOutOfRangeException();
        }

        if (!this.ColorsDictGameObject.ContainsKey(category_name)) {
          this.ColorsDictGameObject.Add(category_name, color);
        }

        /*
switch (this._segmentation_mode) {
  case SegmentationMode.Tag:
    this._block?.SetInt(SynthesisUtils._Shader_OutputMode_Name,(int) SynthesisUtils.ReplacementModes
                                                                                   .Tag_id_);
    break;
  case SegmentationMode.Layer:
    this._block?.SetInt(SynthesisUtils._Shader_OutputMode_Name,(int) SynthesisUtils.ReplacementModes
                                                                                   .Layer_id_);
    break;
  default: throw new ArgumentOutOfRangeException();
}
*/

        r.SetPropertyBlock(this._block);
      }
    }
  }
}
