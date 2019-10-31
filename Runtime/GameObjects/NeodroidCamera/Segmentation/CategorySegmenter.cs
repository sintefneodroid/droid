using System;
using System.Collections.Generic;
using droid.Runtime.GameObjects.NeodroidCamera.Synthesis;
using droid.Runtime.Structs;
using UnityEngine;

namespace droid.Runtime.GameObjects.NeodroidCamera.Segmentation {
  /// <summary>
  ///
  /// </summary>
  enum SegmentationMode {
    Tag_,
    Layer_
  }

  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public class CategorySegmenter : Segmenter {
    /// <summary>
    /// </summary>
    Renderer[] _all_renders = null;

    /// <summary>
    /// </summary>
    MaterialPropertyBlock _block = null;

    [SerializeField] Shader segmentation_shader = null;
    [SerializeField] Camera _camera = null;

    [SerializeField] protected ColorByCategory[] _colors_by_category = null;

    [SerializeField] SegmentationMode _segmentation_mode = SegmentationMode.Tag_;

    [SerializeField] ScriptableObjects.Segmentation _segmentation_preset = null;

    /// <summary>
    /// </summary>
    public bool _Replace_Untagged_Color = true;

    /// <summary>
    /// </summary>
    public Color _Untagged_Color = Color.black;

    /// <summary>
    /// </summary>
    public ColorByCategory[] ColorsByCategory { get { return this._colors_by_category; } }

    /// <summary>
    /// </summary>
    public Dictionary<string, Color> ColorsDictGameObject { get; set; } = new Dictionary<string, Color>();

    /// <summary>
    /// </summary>
    public override Dictionary<string, Color> ColorsDict {
      get {
        var colors = new Dictionary<string, Color>();
        foreach (var key_val in this.ColorsDictGameObject) {
          colors.Add(key_val.Key, key_val.Value);
        }

        return colors;
      }
    }

    /// <summary>
    /// </summary>
    void Start() {
      //this.Setup();
    }

    /// <summary>
    /// </summary>
    void Awake() { this.Setup(); }

    void CheckBlock() {
      if (this._block == null) {
        this._block = new MaterialPropertyBlock();
      }
    }

    SynthesisUtilities.CapturePass[] _capture_passes;

    /// <summary>
    /// </summary>
    void Setup() {
      if (this._colors_by_category != null && this._colors_by_category.Length > 0) {
        foreach (var tag_color in this._colors_by_category) {
          if (!this.ColorsDictGameObject.ContainsKey(tag_color._Category_Name)) {
            this.ColorsDictGameObject.Add(tag_color._Category_Name, tag_color._Color);
          }
        }
      }

      if (this._segmentation_preset) {
        var segmentation_color_by_tags = this._segmentation_preset._color_by_categories;
        if (segmentation_color_by_tags != null) {
          foreach (var tag_color in segmentation_color_by_tags) {
            if (!this.ColorsDictGameObject.ContainsKey(tag_color._Category_Name)) {
              this.ColorsDictGameObject.Add(tag_color._Category_Name, tag_color._Color);
            }
          }
        }
      }

      this._all_renders = FindObjectsOfType<Renderer>();
      if (!this._camera) {
        this._camera = this.GetComponent<Camera>();
      }

      if (this.ColorsDictGameObject == null) {
        this.ColorsDictGameObject = new Dictionary<string, Color>();
      }

      switch (this._segmentation_mode) {
        case SegmentationMode.Tag_:
          this._capture_passes = new[] {
                                           new SynthesisUtilities.CapturePass {
                                                                                  _Name = "_tag_id",
                                                                                  _ReplacementMode =
                                                                                      SynthesisUtilities.ReplacementModes
                                                                                          .Tag_id_,
                                                                                  _SupportsAntialiasing =
                                                                                      false
                                                                              }
                                       };
          break;
        case SegmentationMode.Layer_:
          this._capture_passes = new[] {
                                           new SynthesisUtilities.CapturePass {
                                                                                  _Name = "_layer_id",
                                                                                  _ReplacementMode =
                                                                                      SynthesisUtilities.ReplacementModes
                                                                                          .Layer_id_,
                                                                                  _SupportsAntialiasing =
                                                                                      false
                                                                              }
                                       };
          break;
        default: throw new ArgumentOutOfRangeException();
      }

      SynthesisUtilities.SetupCapturePassesReplacementShader(this._camera,
                                                             this.segmentation_shader,
                                                             ref this._capture_passes);

      this.CheckBlock();
      foreach (var a_renderer in this._all_renders) {
        a_renderer.GetPropertyBlock(this._block);
        string category_name;
        var category_int = 0;
        Color color;
        string shader_data_name;
        switch (this._segmentation_mode) {
          case SegmentationMode.Tag_:
            category_name = a_renderer.tag;
            shader_data_name = SynthesisUtilities._Shader_Tag_Color_Name;
            break;
          case SegmentationMode.Layer_:
            category_int = a_renderer.gameObject.layer;
            category_name = LayerMask.LayerToName(category_int);
            shader_data_name = SynthesisUtilities._Shader_Layer_Color_Name;
            break;
          default: throw new ArgumentOutOfRangeException();
        }

        if (!this.ColorsDictGameObject.ContainsKey(category_name)) {
          if (!this._Replace_Untagged_Color) {
            switch (this._segmentation_mode) {
              case SegmentationMode.Tag_:
                category_int = category_name.GetHashCode();
                color = ColorEncoding.EncodeTagHashCodeAsColor(category_int);
                //color = ColorEncoding.EncodeIdAsColor(category_int);
                break;
              case SegmentationMode.Layer_:
                color = ColorEncoding.EncodeLayerAsColor(category_int);
                break;
              default:
                //color = ColorEncoding.EncodeIdAsColor(category_int);
                throw new ArgumentOutOfRangeException();
            }
          } else {
            color = this._Untagged_Color;
          }

          this.ColorsDictGameObject.Add(category_name, color);
        } else {
          color = this.ColorsDictGameObject[category_name];
        }

        this._block.SetColor(shader_data_name, color);

        a_renderer.SetPropertyBlock(this._block);
      }
    }
  }
}
