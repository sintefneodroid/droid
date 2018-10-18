using System;
using System.Collections.Generic;
using Neodroid.Runtime.Interfaces;
using UnityEngine;

namespace Neodroid.Runtime.Utilities.NeodroidCamera.Segmentation {


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
    protected const string _Default_Color_Tag = "_Color";
    protected const string _Segmentation_Color_Tag = "_SegmentationColor";
    protected const string _Outline_Color_Tag = "_OutlineColor";
    protected const string _Outline_Width_Factor_Tag = "_OutlineWidthFactor";
    [SerializeField, Range(0,1)]protected  float _Outline_Width_Factor = 0.05f;
    [SerializeField]protected  Color _Outline_Color = Color.magenta;
    protected const string _Skip_Outline_Tag = "_SkipOutline";

    public Color OutlineColor { get { return this._Outline_Color; } }
  }
}
