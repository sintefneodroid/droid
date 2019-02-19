using UnityEngine;

namespace droid.Runtime.Utilities.NeodroidCamera.Segmentation.Obsolete {
  /// <inheritdoc cref="MonoBehaviour" />
  /// <summary>
  /// </summary>
  public abstract class ObsoleteSegmenter : Segmenter {

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
