using System;
using System.Collections.Generic;
using droid.Runtime.Structs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace droid.Runtime.GameObjects.NeodroidCamera.Segmentation.Obsolete {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public class ChangeMaterialOnRenderByInstance : ObsoleteSegmenter {
    /// <summary>
    /// </summary>
    Renderer[] _all_renders;

    /// <summary>
    /// </summary>
    MaterialPropertyBlock _block;

    /// <summary>
    /// </summary>
    LinkedList<Color>[] _original_colors;

    [SerializeField] ColorByInstance[] instanceColorArray;

    /// <summary>
    /// </summary>
    public Dictionary<GameObject, Color> ColorsDictGameObject { get; } = new Dictionary<GameObject, Color>();

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

    /// <summary>
    /// </summary>
    public ColorByInstance[] InstanceColors {
      get {
        if (this.ColorsDictGameObject != null) {
          this.instanceColorArray = new ColorByInstance[this.ColorsDictGameObject.Keys.Count];
          var i = 0;
          foreach (var key in this.ColorsDictGameObject.Keys) {
            var seg = new ColorByInstance {_Game_Object = key, _Color = this.ColorsDictGameObject[key]};
            this.instanceColorArray[i] = seg;
            i++;
          }

          return this.instanceColorArray;
        }

        return null;
      }
      set {
        foreach (var seg in value) {
          this.ColorsDictGameObject[seg._Game_Object] = seg._Color;
        }
      }
    }

    // Use this for initialization
    /// <summary>
    /// </summary>
    void Start() { this.Setup(); }

    /// <summary>
    /// </summary>
    void Awake() {
      this._all_renders = FindObjectsOfType<Renderer>();
      this._block = new MaterialPropertyBlock();
      this.Setup();
    }

    // Update is called once per frame
    /// <summary>
    /// </summary>
    void Update() {
      var renderers = FindObjectsOfType<Renderer>();
      if (this.ColorsDictGameObject == null) {
        this.Setup();
      } else if (this.ColorsDictGameObject.Keys.Count != renderers.Length) {
        this._all_renders = renderers;
        this.Setup();
      }
    }

    void CheckBlock() {
      if (this._block == null) {
        this._block = new MaterialPropertyBlock();
      }
    }

    /// <summary>
    /// </summary>
    void Setup() {
      this.CheckBlock();

      this.ColorsDictGameObject.Clear();
      foreach (var rend in this._all_renders) {
        if (rend) {
          this.ColorsDictGameObject.Add(rend.gameObject, Random.ColorHSV());
        }
      }
    }

    /// <summary>
    /// </summary>
    protected override void Change() {
      this.CheckBlock();
      this._original_colors = new LinkedList<Color>[this._all_renders.Length];

      for (var i = 0; i < this._original_colors.Length; i++) {
        this._original_colors[i] = new LinkedList<Color>();
      }

      for (var i = 0; i < this._all_renders.Length; i++) {
        var c_renderer = this._all_renders[i];
        if (c_renderer) {
          foreach (var mat in c_renderer.sharedMaterials) {
            if (mat != null && mat.HasProperty(this._Default_Color_Tag)) {
              this._original_colors[i].AddFirst(mat.color);
            }

            if (this.ColorsDictGameObject.ContainsKey(c_renderer.gameObject)) {
              var val = this.ColorsDictGameObject[c_renderer.gameObject];
              this._block.SetColor(this._Segmentation_Color_Tag, val);
              this._block.SetColor(this._Outline_Color_Tag, this._Outline_Color);
              this._block.SetFloat(this._Outline_Width_Factor_Tag, this._Outline_Width_Factor);
            }

            c_renderer.SetPropertyBlock(this._block);
          }
        }
      }
    }

    /// <summary>
    /// </summary>
    protected override void Restore() {
      this.CheckBlock();
      for (var i = 0; i < this._all_renders.Length; i++) {
        var c_renderer = this._all_renders[i];
        if (c_renderer) {
          foreach (var mat in c_renderer.sharedMaterials) {
            if (mat != null && this._original_colors != null && i < this._original_colors.Length) {
              var c_original_color = this._original_colors[i];
              if (c_original_color != null) {
                var c = this._original_colors[i];
                var last = c?.Last;
                if (last != null) {
                  var last_val = last.Value;
                  this._block.SetColor(this._Default_Color_Tag, last_val);
                  c_original_color.RemoveLast();
                  c_renderer.SetPropertyBlock(this._block);
                }
              }
            }
          }
        }
      }
    }
  }
}
