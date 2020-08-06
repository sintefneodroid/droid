using System;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities.Extensions;
using UnityEngine;
using Object = System.Object;

namespace droid.Runtime.Structs.Space {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [Serializable]
  public struct Space4 : ISpace {
    #region Fields

    public ProjectionEnum Normalised { get { return this.normalised; } set { this.normalised = value; } }

    /// <summary>
    ///
    /// </summary>
    [Header("Space", order = 103)]
    [SerializeField]
    Vector4 _min;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    Vector4 _max;

    /// <summary>
    ///
    /// </summary>
    [Range(-1, 15)]
    [SerializeField]
    int _decimal_granularity;

    [SerializeField] ProjectionEnum normalised;

    #endregion

    /// <summary>
    ///
    /// </summary>
    public Vector4 Span { get { return this._max - this._min; } }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <param name="v"></param>
    ///  <returns></returns>
    ///  <exception cref="T:System.ArgumentOutOfRangeException"></exception>
    public dynamic Project(dynamic v) {
      if (this.Clipped) {
        v = this.Clip(v : v);
      }

      switch (this.Normalised) {
        case ProjectionEnum.None_:
          return v;
        case ProjectionEnum.Zero_one_:
          return ClipNormalise01Round(v : v);
        case ProjectionEnum.Minus_one_one_:
          return ClipNormaliseMinusOneOneRound(v : v);
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
        case ProjectionEnum.Zero_one_:
          v = ClipRoundDenormalise01Clip(v : v);
          break;
        case ProjectionEnum.Minus_one_one_:
          v = ClipRoundDenormaliseMinusOneOneClip(configuration_configurable_value : v);
          break;
        case ProjectionEnum.None_: break;
        default: throw new ArgumentOutOfRangeException();
      }

      if (this.Clipped) {
        v = this.Clip(v : v);
      }

      return v;
    }

    public bool Clipped { get { return this._clipped; } set { this._clipped = value; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
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
    ///
    /// </summary>
    public Space1 Zspace {
      get {
        return new Space1 {
                              Min = this._min.z,
                              Max = this._max.z,
                              DecimalGranularity = this.DecimalGranularity
                          };
      }
    }

    /// <summary>
    ///
    /// </summary>
    public Space1 Wspace {
      get {
        return new Space1 {
                              Min = this._min.w,
                              Max = this._max.w,
                              DecimalGranularity = this.DecimalGranularity
                          };
      }
    }

    [SerializeField] ProjectionEnum _projection; //TODO use!
    [SerializeField] bool _clipped;

    /// <summary>
    ///
    /// </summary>
    public bool NormalisedBool {
      get { return this._projection == ProjectionEnum.Zero_one_; }
      set { this._projection = value ? ProjectionEnum.Zero_one_ : ProjectionEnum.None_; }
    }

    /// <summary>
    /// If max is less than min, no clipping is performed.
    /// </summary>
    /// <param name="v"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public Vector4 Clip(Vector4 v, Vector4 min, Vector4 max) {
      return new Vector4(x : max.x < min.x ? v.x : Mathf.Clamp(value : v.x, min : min.x, max : max.x),
                         y : max.y < min.y ? v.y : Mathf.Clamp(value : v.y, min : min.y, max : max.y),
                         z : max.z < min.z ? v.z : Mathf.Clamp(value : v.z, min : min.z, max : max.z),
                         w : max.w < min.w ? v.w : Mathf.Clamp(value : v.w, min : min.w, max : max.w));
    }

    public Vector4 Clip(Vector4 v) { return this.Clip(v : v, min : this._min, max : this._max); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    Vector4 ClipRound(Vector4 v) { return this.Clip(v : this.Round(v : v)); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Vector4 Round(Vector4 v) {
      v.x = this.Round(v : v.x);
      v.y = this.Round(v : v.y);
      v.w = this.Round(v : v.z);
      v.z = this.Round(v : v.w);
      return v;
    }

    dynamic ClipRoundDenormalise01Clip(dynamic v) {
      #if ALWAYS_PRE_CLIP_PROJECTIONS
      v = Clip(v : configuration_configurable_value,
                                                                  min : Vector4.zero,
                                                                  max : Vector4.one);
      #endif

      return this.Clip(v : this.Round(this.Denormalise01(v : v)));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    dynamic ClipNormalise01Round(dynamic v) {
      #if ALWAYS_PRE_CLIP_PROJECTIONS
      v = Clip(v : v);
      #endif

      return this.Round(Normalise01(v : v));
    }

    public dynamic Mean { get { return (this.Max + this.Min) * 0.5f; } }

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
    public static Space4 ZeroOne {
      get {
        return new Space4 {
                              _min = Vector4.zero,
                              Max = Vector4.one,
                              DecimalGranularity = 4,
                              Normalised = ProjectionEnum.Zero_one_,
                              Clipped = true
                          };
      }
    }

    /// <summary>
    ///
    /// </summary>
    public static Space4 TwentyEighty {
      get {
        return new Space4 {
                              _min = Vector4.one * 0.2f,
                              Max = Vector4.one * 0.8f,
                              DecimalGranularity = 4,
                              Normalised = ProjectionEnum.Zero_one_,
                              Clipped = true
                          };
      }
    }

    /// <summary>
    ///
    /// </summary>
    public static Space4 MinusOneOne {
      get {
        return new Space4 {
                              _min = -Vector4.one,
                              Max = Vector4.one,
                              DecimalGranularity = 4,
                              Normalised = ProjectionEnum.Zero_one_,
                              Clipped = true
                          };
      }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public dynamic Max { get { return this._max; } set { this._max = value; } }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public dynamic Min { get { return this._min; } set { this._min = value; } }

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Space4 operator*(Space4 a, float b) {
      a.Max *= b;
      a.Min *= b;
      return a;
    }

    dynamic ClipNormaliseMinusOneOneRound(dynamic v) {
      #if ALWAYS_PRE_CLIP_PROJECTIONS
      v = Clip(v : v);
      #endif

      return this.Round(NormaliseMinusOneOne(v : v));
    }

    dynamic ClipRoundDenormaliseMinusOneOneClip(dynamic configuration_configurable_value) {
      #if ALWAYS_PRE_CLIP_PROJECTIONS
configuration_configurable_value = Clip(v : configuration_configurable_value,
                                                                  min : Vector4.zero,
                                                                  max : Vector4.one)
      #endif

      return this.Clip(v : this.Round(this.DenormaliseMinusOneOne(v : configuration_configurable_value)));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    Vector4 Denormalise01(Vector4 v) {
      if (v.x > 1 || v.y > 1 || v.z > 1 || v.w > 1 || v.x < 0 || v.y < 0 || v.z < 0 || v.w < 0) {
        throw new ArgumentException(message : $"Value was {v}, min:0, max:1");
      }

      return Normalisation.Denormalise01_(v : v, min : this._min, span : this.Span);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    Vector4 Normalise01(Vector4 v) {
      if (v.x > this._max.x
          || v.y > this._max.y
          || v.z > this._max.z
          || v.w > this._max.w
          || v.x < this._min.x
          || v.y < this._min.y
          || v.z < this._min.z
          || v.w < this._min.w) {
        throw new ArgumentException(message : $"Value was {v}, min:{this._min}, max:{this._max}");
      }

      return Normalisation.Normalise01_(v : v, min : this._min, span : this.Span);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    Vector4 NormaliseMinusOneOne(Vector4 v) {
      if (v.x > this._max.x
          || v.y > this._max.y
          || v.z > this._max.z
          || v.w > this._max.w
          || v.x < this._min.x
          || v.y < this._min.y
          || v.z < this._min.z
          || v.w < this._min.w) {
        throw new ArgumentException(message : $"Value was {v}, min:{this._min}, max:{this._max}");
      }

      if (this.Span.x > 0 && this.Span.y > 0 && this.Span.z > 0 && this.Span.w > 0) { //TODO: FINISH cases
        v = Normalisation.NormaliseMinusOneOne_(v : v, min : this._min, span : this.Span);
      } else if (this.Span.x > 0 && this.Span.y <= 0) {
        v.x = Normalisation.NormaliseMinusOneOne_(v : v.x, min : this._min.x, span : this.Span.x);
        v.y = 0;
      } else if (this.Span.x <= 0 && this.Span.y >= 0) {
        v.x = 0;
        v.y = Normalisation.NormaliseMinusOneOne_(v : v.y, min : this._min.y, span : this.Span.y);
      } else {
        v.x = 0;
        v.y = 0;
        v.z = 0;
        v.w = 0;
      }

      return v;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    Vector4 DenormaliseMinusOneOne(Vector4 v) {
      if (v.x > 1 || v.y > 1 || v.z > 1 || v.w > 1 || v.x < -1 || v.y < -1 || v.z < -1 || v.w < -1) {
        throw new ArgumentException(message : $"Value was {v}, min:-1, max:1");
      }

      if (this.Span.x <= 0) { //TODO: FINISH cases
        if (this.Span.y <= 0) {
          return new Vector4(0, 0);
        }

        return new Vector4(0,
                           y : Normalisation.DenormaliseMinusOneOne_(v : v.y,
                                                                     min : this._min.y,
                                                                     span : this.Span.y));
      }

      if (this.Span.y <= 0) {
        if (this.Span.x <= 0) {
          return new Vector4(0, 0);
        }

        return new Vector4(x : Normalisation.DenormaliseMinusOneOne_(v : v.x,
                                                                     min : this._min.x,
                                                                     span : this.Span.x),
                           0);
      }

      return Normalisation.DenormaliseMinusOneOne_(v : v, min : this._min, span : this.Span);
    }
  }
}