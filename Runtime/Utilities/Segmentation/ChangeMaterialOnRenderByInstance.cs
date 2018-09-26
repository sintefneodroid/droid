using System;
using System.Collections.Generic;
using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Utilities.Structs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Neodroid.Runtime.Utilities.Segmentation {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public class ChangeMaterialOnRenderByInstance : Segmenter{
    /// <summary>
    ///
    /// </summary>
    Renderer[] _all_renders;

    /// <summary>
    ///
    /// </summary>
    MaterialPropertyBlock _block;

    /// <summary>
    ///
    /// </summary>
    LinkedList<Color>[] _original_colors;

    ///  <summary>
    ///  </summary>
    public Dictionary<GameObject, Color> ColorsDictGameObject { get; private set; } =
      new Dictionary<GameObject, Color>();


    /// <summary>
    /// 
    /// </summary>
    public override Dictionary<String, Color> ColorsDict {
      get {
        var colors = new Dictionary<String, Color>();
        foreach (var key_val in this.ColorsDictGameObject) {
          colors.Add(key_val.Key.GetInstanceID().ToString(),key_val.Value);
        }

        return colors;
      }
    }

    /// <summary>
    ///
    /// </summary>
    public ColorByInstance[] InstanceColors {
      get {
        if (this.ColorsDictGameObject != null) {
          var instance_color_array = new ColorByInstance[this.ColorsDictGameObject.Keys.Count];
          var i = 0;
          foreach (var key in this.ColorsDictGameObject.Keys) {
            var seg = new ColorByInstance {_Obj = key, _Col = this.ColorsDictGameObject[key]};
            instance_color_array[i] = seg;
            i++;
          }

          return instance_color_array;
        }

        return null;
      }
      set {
        foreach (var seg in value) {
          this.ColorsDictGameObject[seg._Obj] = seg._Col;
        }
      }
    }

    // Use this for initialization
    /// <summary>
    ///
    /// </summary>
    void Start() { this.Setup(); }

    /// <summary>
    ///
    /// </summary>
    void Awake() {
      this._all_renders = FindObjectsOfType<Renderer>();
      this._block = new MaterialPropertyBlock();
      this.Setup();
    }

    // Update is called once per frame
    /// <summary>
    ///
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

    /// <summary>
    ///
    /// </summary>
    void Setup() {
      if (this._block == null) {
        this._block = new MaterialPropertyBlock();
      }

      this.ColorsDictGameObject.Clear();
      foreach (var rend in this._all_renders) {
        if (rend) {
          this.ColorsDictGameObject.Add(rend.gameObject, Random.ColorHSV());
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    void Change() {
      this._original_colors = new LinkedList<Color>[this._all_renders.Length];

      for (var i = 0; i < this._original_colors.Length; i++) {
        this._original_colors[i] = new LinkedList<Color>();
      }

      for (var i = 0; i < this._all_renders.Length; i++) {
        var c_renderer = this._all_renders[i];
        if (c_renderer) {
          foreach (var mat in c_renderer.sharedMaterials) {
            if (mat != null && mat.HasProperty("_Color")) {
              this._original_colors[i].AddFirst(mat.color);
            }

            this._block.SetColor("_Color", this.ColorsDictGameObject[c_renderer.gameObject]);
            c_renderer.SetPropertyBlock(this._block);
          }
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    void Restore() {
      for (var i = 0; i < this._all_renders.Length; i++) {
        var c_renderer = this._all_renders[i];
        if (c_renderer) {
          foreach (var mat in c_renderer.sharedMaterials) {
            if (mat != null
                && mat.HasProperty("_Color")
                && this._original_colors != null
                && i < this._original_colors.Length) {
              var c_original_color = this._original_colors[i];
              if (c_original_color != null) {
                this._block.SetColor("_Color", c_original_color.Last.Value);
                c_original_color.RemoveLast();
                c_renderer.SetPropertyBlock(this._block);
              }
            }
          }
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    void OnPreRender() {
      // change
      this.Change();
    }

    /// <summary>
    ///
    /// </summary>
    void OnPostRender() {
      // change back
      this.Restore();
    }

  }
}