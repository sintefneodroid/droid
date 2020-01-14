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

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override Dictionary<String, Color> ColorsDict {
      get {
        var colors = new Dictionary<String, Color>();
        foreach (var key_val in this.ColorsDictGameObject) {
          colors.Add(key_val.Key.GetInstanceID().ToString(), value : key_val.Value);
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
            var seg = new ColorByInstance {_Game_Object = key, _Color = this.ColorsDictGameObject[key : key]};
            this.instanceColorArray[i] = seg;
            i++;
          }

          return this.instanceColorArray;
        }

        return null;
      }
      set {
        for (var index = 0; index < value.Length; index++) {
          var seg = value[index];
          this.ColorsDictGameObject[key : seg._Game_Object] = seg._Color;
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
      for (var index = 0; index < this._all_renders.Length; index++) {
        var rend = this._all_renders[index];
        if (rend) {
          this.ColorsDictGameObject.Add(key : rend.gameObject, Random.ColorHSV());
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
          for (var index = 0; index < c_renderer.sharedMaterials.Length; index++) {
            var mat = c_renderer.sharedMaterials[index];
            if (mat != null && mat.HasProperty(nameID : this._Default_Color_Tag)) {
              this._original_colors[i].AddFirst(value : mat.color);
            }

            if (this.ColorsDictGameObject.ContainsKey(key : c_renderer.gameObject)) {
              var val = this.ColorsDictGameObject[key : c_renderer.gameObject];
              this._block.SetColor(nameID : this._Segmentation_Color_Tag, value : val);
              this._block.SetColor(nameID : this._Outline_Color_Tag, value : this._Outline_Color);
              this._block.SetFloat(nameID : this._Outline_Width_Factor_Tag, value : this._Outline_Width_Factor);
            }

            c_renderer.SetPropertyBlock(properties : this._block);
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
          for (var index = 0; index < c_renderer.sharedMaterials.Length; index++) {
            var mat = c_renderer.sharedMaterials[index];
            if (mat != null && this._original_colors != null && i < this._original_colors.Length) {
              var c_original_color = this._original_colors[i];
              if (c_original_color != null) {
                var c = this._original_colors[i];
                var last = c?.Last;
                if (last != null) {
                  var last_val = last.Value;
                  this._block.SetColor(nameID : this._Default_Color_Tag, value : last_val);
                  c_original_color.RemoveLast();
                  c_renderer.SetPropertyBlock(properties : this._block);
                }
              }
            }
          }
        }
      }
    }
  }
}
