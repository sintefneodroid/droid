using UnityEngine;

namespace droid.Runtime.GameObjects.NeodroidCamera.Segmentation.Obsolete {
  /// <inheritdoc cref="MonoBehaviour" />
  /// <summary>
  /// </summary>
  public abstract class ObsoleteSegmenter : Segmenter {
    /// <summary>
    ///
    /// </summary>
    protected int _Default_Color_Tag = Shader.PropertyToID("_Color");

    protected int _Segmentation_Color_Tag = Shader.PropertyToID("_SegmentationColor");
    protected int _Outline_Color_Tag = Shader.PropertyToID("_OutlineColor");
    protected int _Outline_Width_Factor_Tag = Shader.PropertyToID("_OutlineWidthFactor");

    [SerializeField, Range(0, 2)] protected float _Outline_Width_Factor = 0.05f;
    [SerializeField] protected Color _Outline_Color = Color.magenta;

    public Color OutlineColor { get { return this._Outline_Color; } }

    void OnPreRender() {
      // change
      this.Change();
    }

    protected abstract void Change();

    /*void OnPreCull() {
  // change
}*/
    /// <summary>
    /// </summary>
    void OnPostRender() {
      // change back
      this.Restore();
    }

    protected abstract void Restore();
  }
}
