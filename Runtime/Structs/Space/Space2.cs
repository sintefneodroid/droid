using System;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using UnityEngine;
using Object = System.Object;

namespace droid.Runtime.Structs.Space {
  /// <inheritdoc />
  ///  <summary>
  ///  </summary>
  [Serializable]
  public struct Space2 : ISpace {
    #region Fields

    public ProjectionEnum Normalised { get { return this.normalised; } set { this.normalised = value; } }

    /// <summary>
    ///
    /// </summary>
    [Header("Space", order = 103)]
    [SerializeField]
    Vector2 _min;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    Vector2 _max;

    [Range(-1, 15)] [SerializeField] int _decimal_granularity;
    [SerializeField] ProjectionEnum normalised;

    #endregion

    /// <summary>
    ///
    /// </summary>
    public Vector2 Span { get { return this._max - this._min; } }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public int DecimalGranularity {
      get { return this._decimal_granularity; }
      set { this._decimal_granularity = value; }
    }

    /// <summary>
    ///
    /// </summary>
    public Space1 Xspace {
      get {
        return new Space1 {
                              Min = this._min.x,
                              Max = this._max.x,
                              DecimalGranularity = this.DecimalGranularity
                          };
      }
    }

    /// <summary>
    ///
    /// </summary>
    public Space1 Yspace {
      get {
        return new Space1 {
                              Min = this._min.y,
                              Max = this._max.y,
                              DecimalGranularity = this.DecimalGranularity
                          };
      }
    }

    /// <summary>
    /// If max is less than min, no clipping is performed.
    /// </summary>
    /// <param name="v"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public Vector2 Clip(Vector2 v, Vector2 min, Vector2 max) {
      return new Vector2(x : max.x < min.x ? v.x : Mathf.Clamp(value : v.x, min : min.x, max : max.x),
                         y : max.y < min.y ? v.y : Mathf.Clamp(value : v.y, min : min.y, max : max.y));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Vector2 Clip(Vector2 v) { return this.Clip(v : v, min : this._min, max : this._max); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    Vector2 ClipRound(Vector2 v) { return this.Clip(v : this.Round(v : v)); }

    dynamic ClipRoundDenormalise01Clip(dynamic configuration_configurable_value) {
      #if PRE_CLIP_PROJECTIONS
      configuration_configurable_value = Clip(v : configuration_configurable_value,
                                                                  min : Vector2.zero,
                                                                  max : Vector2.one)
      #endif

      return this.Clip(v : this.Round(this.Denormalise01(v : configuration_configurable_value)));
    }

    dynamic ClipNormaliseMinusOneOneRound(dynamic v) {
      #if PRE_CLIP_PROJECTIONS
      v = Clip(v : v)
      #endif


      return this.Round(this.NormaliseMinusOneOne(v : v));
    }



    dynamic ClipRoundDenormaliseMinusOneOneClip(dynamic configuration_configurable_value) {
      #if PRE_CLIP_PROJECTIONS
      configuration_configurable_value = Clip(v : configuration_configurable_value,
                                                                  min : -Vector2.one,
                                                                  max : Vector2.one)
      #endif

      return this.Clip(v : this.Round(this.DenormaliseMinusOneOne(v : configuration_configurable_value)));
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <param name="v"></param>
    ///  <returns></returns>
    ///  <exception cref="T:System.ArgumentOutOfRangeException"></exception>
    public dynamic Project(dynamic v) {
      switch (this.Normalised) {
        case ProjectionEnum.None_:
          return v;
        case ProjectionEnum.Zero_one_:
          return ClipNormalise01Round(v : v);
        case ProjectionEnum.Minus_one_one_:
          return ClipNormaliseMinusOneOneRound(v : v);
        case ProjectionEnum.Clipped_: return ClipRound(v : v);
        default: throw new ArgumentOutOfRangeException();
      }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <param name="v"></param>
    ///  <returns></returns>
    ///  <exception cref="T:System.ArgumentOutOfRangeException"></exception>
    public dynamic Reproject(dynamic v) {
      switch (this.Normalised) {
        case ProjectionEnum.None_:
          return v;
        case ProjectionEnum.Zero_one_:
          return ClipRoundDenormalise01Clip(configuration_configurable_value : v);
        case ProjectionEnum.Minus_one_one_:
          return ClipRoundDenormaliseMinusOneOneClip(configuration_configurable_value : v);
        case ProjectionEnum.Clipped_:
          return ClipRound(v : v);
        default: throw new ArgumentOutOfRangeException();
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    dynamic ClipNormalise01Round(dynamic v) {
      #if PRE_CLIP_PROJECTIONS
      v = Clip(v : v)
      #endif

      return this.Round(Normalise01(v));
    }

    /// <summary>
    /// if Decimal granularity is negative no rounding is performed
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public float Round(float v) {
      return this.DecimalGranularity >= 0
                 ? (float)Math.Round(value : v, digits : this.DecimalGranularity)
                 : v;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Vector2 Round(Vector2 v) {
      v.x = this.Round(v : v.x);
      v.y = this.Round(v : v.y);
      return v;
    }

    /// <summary>
    ///
    /// </summary>
    public static Space2 ZeroOne {
      get {
        return new Space2 {
                              _min = Vector2.zero,
                              Max = Vector2.one,
                              DecimalGranularity = 4,
                              Normalised = ProjectionEnum.Zero_one_
                          };
      }
    }

    /// <summary>
    ///
    /// </summary>
    public static Space2 TwentyEighty {
      get {
        return new Space2 {
                              _min = Vector2.one * 0.2f,
                              Max = Vector2.one * 0.8f,
                              DecimalGranularity = 4,
                              Normalised = ProjectionEnum.Zero_one_
                          };
      }
    }

    /// <summary>
    ///
    /// </summary>
    public static Space2 MinusOneOne {
      get {
        return new Space2 {
                              _min = -Vector2.one,
                              Max = Vector2.one,
                              DecimalGranularity = 4,
                              Normalised = ProjectionEnum.Zero_one_
                          };
      }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public dynamic Min { get { return this._min; } set { this._min = value; } }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public dynamic Max { get { return this._max; } set { this._max = value; } }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public dynamic Mean { get { return (this.Max + this.Min) * 0.5f; } }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    Vector2 Denormalise01(Vector2 v) {
      if (v.x > 1 || v.y > 1 || v.x < 0 || v.y < 0) {
        throw new ArgumentException();
      }

      if (this.Span.x <= 0) {
        if (this.Span.y <= 0) {
          return new Vector2(0, 0);
        }

        return new Vector2(0,
                           y : Normalisation.Denormalise01_(v : v.y, min : this._min.y, span : this.Span.y));
      }

      if (this.Span.y <= 0) {
        if (this.Span.x <= 0) {
          return new Vector2(0, 0);
        }

        return new Vector2(x : Normalisation.Denormalise01_(v : v.x, min : this._min.x, span : this.Span.x),
                           0);
      }

      return Normalisation.Denormalise01_(v : v, min : this._min, span : this.Span);
    }

    Vector2 DenormaliseMinusOneOne(Vector2 v) {
      if (v.x > 1 || v.y > 1 || v.x < -1 || v.y < -1) {
        throw new ArgumentException();
      }

      if (this.Span.x <= 0) {
        if (this.Span.y <= 0) {
          return new Vector2(0, 0);
        }

        return new Vector2(0,
                           y : Normalisation.DenormaliseMinusOneOne_(v : v.y,
                                                                     min : this._min.y,
                                                                     span : this.Span.y));
      }

      if (this.Span.y <= 0) {
        if (this.Span.x <= 0) {
          return new Vector2(0, 0);
        }

        return new Vector2(x : Normalisation.DenormaliseMinusOneOne_(v : v.x,
                                                                     min : this._min.x,
                                                                     span : this.Span.x),
                           0);
      }

      return Normalisation.DenormaliseMinusOneOne_(v : v, min : this._min, span : this.Span);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    Vector2 Normalise01(Vector2 v) {
      if (v.x > this._max.x || v.y > this._max.y || v.x < this._min.x || v.y < this._min.y) {
        throw new ArgumentException();
      }

      if (this.Span.x <= 0) {
        if (this.Span.y <= 0) {
          return new Vector2(0, 0);
        }

        return new Vector2(0, y : Normalisation.Normalise01_(v : v.y, min : this._min.y, span : this.Span.y));
      }

      if (this.Span.y <= 0) {
        if (this.Span.x <= 0) {
          return new Vector2(0, 0);
        }

        return new Vector2(x : Normalisation.Normalise01_(v : v.x, min : this._min.x, span : this.Span.x), 0);
      }

      return Normalisation.Normalise01_(v : v, min : this._min, span : this.Span);
    }

    dynamic NormaliseMinusOneOne(dynamic v) {
      if (v.x > this._max.x || v.y > this._max.y || v.x < this._min.x || v.y < this._min.y) {
        throw new ArgumentException();
      }

      if (this.Span.x > 0 && this.Span.y > 0) {
        v = this.Round(Normalisation.NormaliseMinusOneOne_(v : v, min : this._min, span : this.Span));
      } else if (this.Span.x > 0 && this.Span.y <= 0) {
        v.x = this.Round(Normalisation.NormaliseMinusOneOne_(v : v.x, min : this._min.x, span : this.Span.x));
        v.y = 0;
      } else if (this.Span.x <= 0 && this.Span.y >= 0) {
        v.x = 0;
        v.y = this.Round(Normalisation.NormaliseMinusOneOne_(v : v.y, min : this._min.y, span : this.Span.y));
      } else {
        v.x = 0;
        v.y = 0;
      }

      return v;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Space2 operator*(Space2 a, float b) {
      a.Max *= b;
      a.Min *= b;
      return a;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="bounds_extents"></param>
    /// <param name="normalised"></param>
    /// <param name="decimal_granularity"></param>
    /// <returns></returns>
    public static Space2 FromCenterExtents(Vector2 bounds_extents,
                                           ProjectionEnum normalised = ProjectionEnum.Zero_one_,
                                           int decimal_granularity = 4) {
      return new Space2 {
                            _min = -bounds_extents,
                            Max = bounds_extents,
                            normalised = normalised,
                            _decimal_granularity = decimal_granularity
                        };
    }
  }
}
