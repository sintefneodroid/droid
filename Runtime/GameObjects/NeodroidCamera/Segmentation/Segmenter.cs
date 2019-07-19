using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.GameObjects.NeodroidCamera.Segmentation {
  /// <inheritdoc cref="MonoBehaviour" />
  /// <summary>
  /// </summary>
  public abstract class Segmenter : MonoBehaviour,
                                    IColorProvider {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract Dictionary<String, Color> ColorsDict { get; }
  }
}
