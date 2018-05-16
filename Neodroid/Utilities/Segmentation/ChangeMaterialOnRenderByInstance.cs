using System.Collections.Generic;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Utilities.Segmentation {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public class ChangeMaterialOnRenderByInstance : MonoBehaviour {
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

    /// <summary>
    ///
    /// </summary>
    public Dictionary<GameObject, Color> InstanceColorsDict { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public ColorByInstance[] InstanceColors {
      get {
        if (this.InstanceColorsDict != null) {
          var instance_color_array = new ColorByInstance[this.InstanceColorsDict.Keys.Count];
          var i = 0;
          foreach (var key in this.InstanceColorsDict.Keys) {
            var seg = new ColorByInstance {_Obj = key, _Col = this.InstanceColorsDict[key]};
            instance_color_array[i] = seg;
            i++;
          }

          return instance_color_array;
        }

        return null;
      }
      set {
        foreach (var seg in value) {
          this.InstanceColorsDict[seg._Obj] = seg._Col;
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
      if (this.InstanceColorsDict == null) {
        this.Setup();
      } else if (this.InstanceColorsDict.Keys.Count != renderers.Length) {
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

      this.InstanceColorsDict = new Dictionary<GameObject, Color>(this._all_renders.Length);
      foreach (var rend in this._all_renders) {
        if (rend) {
          this.InstanceColorsDict.Add(rend.gameObject, Random.ColorHSV());
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

            this._block.SetColor("_Color", this.InstanceColorsDict[c_renderer.gameObject]);
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
