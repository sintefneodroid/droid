using System.Collections.Generic;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Utilities.Segmentation {
  /// <summary>
  ///
  /// </summary>
  [ExecuteInEditMode]
  public class ChangeMaterialOnRenderByTag : MonoBehaviour {
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
    public ColorByTag[] _Colors_By_Tag;

    /// <summary>
    ///
    /// </summary>
    LinkedList<Color>[] _original_colors;

    /// <summary>
    ///
    /// </summary>
    public bool _Replace_Untagged_Color = true;

    /// <summary>
    ///
    /// </summary>
    Dictionary<string, Color> _tag_colors;

    /// <summary>
    ///
    /// </summary>
    public Color _Untagged_Color = Color.black;

    /// <summary>
    ///
    /// </summary>
    public ColorByTag[] ColorsByTag { get { return this._Colors_By_Tag; } }

    /// <summary>
    ///
    /// </summary>
    void Awake() {
      this._block = new MaterialPropertyBlock();
      this._tag_colors = new Dictionary<string, Color>();
      if (this._Colors_By_Tag.Length > 0) {
        foreach (var tag_color in this._Colors_By_Tag) {
          if (!this._tag_colors.ContainsKey(tag_color._Tag)) {
            this._tag_colors.Add(tag_color._Tag, tag_color._Col);
          }
        }
      }

      this.Setup();
    }

    /// <summary>
    ///
    /// </summary>
    void Update() {
      this.Setup(); // renderes maybe be disable and enabled, that is why every update we find all renderers again
    }

    /// <summary>
    ///
    /// </summary>
    void Setup() {
      if (this._block == null) {
        this._block = new MaterialPropertyBlock();
      }

      this._all_renders = FindObjectsOfType<Renderer>();
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
          if (this._tag_colors != null && this._tag_colors.ContainsKey(this._all_renders[i].tag)) {
            foreach (var mat in this._all_renders[i].sharedMaterials) {
              if (mat != null && mat.HasProperty("_Color")) {
                this._original_colors[i].AddFirst(mat.color);
              }

              this._block.SetColor("_Color", this._tag_colors[this._all_renders[i].tag]);
              this._all_renders[i].SetPropertyBlock(this._block);
            }
          } else if (this._Replace_Untagged_Color) {
            foreach (var mat in this._all_renders[i].sharedMaterials) {
              if (mat != null && mat.HasProperty("_Color")) {
                this._original_colors[i].AddFirst(mat.color);
              }

              this._block.SetColor("_Color", this._Untagged_Color);
              this._all_renders[i].SetPropertyBlock(this._block);
            }
          }
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    void Restore() {
      for (var i = 0; i < this._all_renders.Length; i++) {
        if (this._all_renders[i]) {
          foreach (var mat in this._all_renders[i].sharedMaterials) {
            if (mat != null
                && mat.HasProperty("_Color")
                && this._original_colors != null
                && i < this._original_colors.Length) {
              this._block.SetColor("_Color", this._original_colors[i].Last.Value);
              this._original_colors[i].RemoveLast();
              this._all_renders[i].SetPropertyBlock(this._block);
            }
          }
        }
      }
    }

    /*void OnPreCull() {
      // change
    }*/

    void OnPreRender() {
      // change
      this.Change();
    }

    void OnPostRender() {
      // change back
      this.Restore();
    }
  }
}
