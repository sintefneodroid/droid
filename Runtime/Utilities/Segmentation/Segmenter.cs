using System;
using System.Collections.Generic;
using Neodroid.Runtime.Interfaces;
using UnityEngine;

namespace Neodroid.Runtime.Utilities.Segmentation {

  class Seri {

  }

  /// <inheritdoc cref="MonoBehaviour" />
  /// <summary>
  /// </summary>
  public abstract class Segmenter : MonoBehaviour,
                                    IMaterialManipulator {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract Dictionary<String, Color> ColorsDict { get; }
  }
}
