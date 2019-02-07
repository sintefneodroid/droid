using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Utilities.NeodroidCamera.Segmentation {
  /// <inheritdoc cref="MonoBehaviour" />
  /// <summary>
  /// </summary>
  public abstract class Segmenter : MonoBehaviour,
                                    IMaterialManipulator {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract Dictionary<String, Color> ColorsDict { get; }

    /// <summary>
    ///
    /// </summary>

    protected int _Default_Color_Tag = Shader.PropertyToID("_Color");

     protected int _Segmentation_Color_Tag = Shader.PropertyToID("_SegmentationColor");
     protected int _Outline_Color_Tag = Shader.PropertyToID("_OutlineColor");
     protected int _Outline_Width_Factor_Tag = Shader.PropertyToID("_OutlineWidthFactor");
     protected int _Skip_Outline_Tag = Shader.PropertyToID("_SkipOutline");

    [SerializeField, Range(0, 1)] protected float _Outline_Width_Factor = 0.05f;
    [SerializeField] protected Color _Outline_Color = Color.magenta;

    public Color OutlineColor { get { return this._Outline_Color; } }
  }
}
