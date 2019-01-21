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
    [SerializeField]
    protected int _Default_Color_Tag = Shader.PropertyToID("_Color");

    [SerializeField] protected int _Segmentation_Color_Tag = Shader.PropertyToID("_SegmentationColor");
    [SerializeField] protected int _Outline_Color_Tag = Shader.PropertyToID("_OutlineColor");
    [SerializeField] protected int _Outline_Width_Factor_Tag = Shader.PropertyToID("_OutlineWidthFactor");
    [SerializeField] protected int _Skip_Outline_Tag = Shader.PropertyToID("_SkipOutline");

    [SerializeField, Range(0, 1)] protected float _Outline_Width_Factor = 0.05f;
    [SerializeField] protected Color _Outline_Color = Color.magenta;

    public Color OutlineColor { get { return this._Outline_Color; } }
  }
}
