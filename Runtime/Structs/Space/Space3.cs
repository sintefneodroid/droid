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
  public struct Space3 : ISpace {
    #region Fields

    public ProjectionEnum Normalised { get { return this.normalised; } set { this.normalised = value; } }

    [Header("Space", order = 103)]
    [SerializeField]
    Vector3 _min;

    [SerializeField] Vector3 _max;
    [Range(-1, 15)] [SerializeField] int _decimal_granularity;
    [SerializeField] ProjectionEnum normalised;

    #endregion

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
    public Vector3 Span { get { return this._max - this._min; } }

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

    public bool Clipped { get { return this._clipped; } set { this._clipped = value; } }

    dynamic ClipNormaliseMinusOneOneRound(dynamic v) {
      #if ALWAYS_PRE_CLIP_PROJECTIONS
      v = Clip(v : v);
      #endif

      return this.Round(NormaliseMinusOneOne(v : v));
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
          v = ClipRoundDenormalise01Clip(configuration_configurable_value : v);
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

    dynamic ClipRoundDenormaliseMinusOneOneClip(dynamic configuration_configurable_value) {
      #if ALWAYS_PRE_CLIP_PROJECTIONS
configuration_configurable_value = Clip(v : configuration_configurable_value,
                                                                  min : Vector3.zero,
                                                                  max : Vector3.one)
      #endif

      return this.Clip(v : this.Round(this.DenormaliseMinusOneOne(v : configuration_configurable_value)));
    }

    /// <summary>
    /// If max is  less than min, no clipping is performed.
    /// </summary>
    /// <param name="v"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static Vector3 Clip(Vector3 v, Vector3 min, Vector3 max) {
      return new Vector3(x : max.x < min.x ? v.x : Mathf.Clamp(value : v.x, min : min.x, max : max.x),
                         y : max.y < min.y ? v.y : Mathf.Clamp(value : v.y, min : min.y, max : max.y),
                         z : max.z < min.z ? v.z : Mathf.Clamp(value : v.z, min : min.z, max : max.z));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Vector3 Clip(Vector3 v) { return Clip(v : v, min : this._min, max : this._max); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    Vector3 ClipRound(Vector3 v) { return this.Clip(v : this.Round(v : v)); }

    dynamic ClipRoundDenormalise01Clip(dynamic configuration_configurable_value) {
      #if ALWAYS_PRE_CLIP_PROJECTIONS
configuration_configurable_value = Clip(v : configuration_configurable_value,
                                                                  min : Vector3.zero,
                                                                  max : Vector3.one)
      #endif

      return this.Clip(v : this.Round(this.Denormalise01(v : configuration_configurable_value)));
    }

    public dynamic Mean { get { return (this.Max + this.Min) * 0.5f; } }

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
    public Vector3 Round(Vector3 v) {
      v.x = this.Round(v : v.x);
      v.y = this.Round(v : v.y);
      v.z = this.Round(v : v.z);
      return v;
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
    ///
    /// </summary>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static Space3 operator+(Space3 b, Vector3 c) {
      b._min += c;
      b._max += c;
      return b;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static Space3 operator-(Space3 b, Vector3 c) {
      b._min -= c;
      b._max -= c;
      return b;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static Space3 operator-(Vector3 c, Space3 b) {
      b._min -= c;
      b._max -= c;
      return b;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="c"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Space3 operator+(Vector3 c, Space3 b) {
      b._min += c;
      b._max += c;
      return b;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Space3 operator*(Space3 a, float b) {
      a.Max *= b;
      a.Min *= b;
      return a;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static Space3 ZeroOne {
      get {
        return new Space3 {
                              _min = Vector3.zero,
                              Max = Vector3.one,
                              DecimalGranularity = 4,
                              Normalised = ProjectionEnum.Zero_one_,
                              Clipped = true
                          };
      }
    }

    /// <summary>
    ///
    /// </summary>
    public static Space3 TwentyEighty {
      get {
        return new Space3 {
                              Min = Vector3.one * 0.2f,
                              _max = Vector3.one * 0.8f,
                              DecimalGranularity = 4,
                              Normalised = ProjectionEnum.Zero_one_,
                              Clipped = true
                          };
      }
    }

    /// <summary>
    ///
    /// </summary>
    public static Space3 MinusOneOne {
      get {
        return new Space3 {
                              _min = -Vector3.one,
                              Max = Vector3.one,
                              DecimalGranularity = 4,
                              Normalised = ProjectionEnum.Zero_one_,
                              Clipped = true
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

    /// <summary>
    /// Return Space3 with the negative and positive extents respectively as min and max for each dimension
    /// </summary>
    /// <param name="bounds_extents"></param>
    /// <param name="normalised"></param>
    /// <param name="decimal_granularity"></param>
    /// <returns></returns>
    public static Space3 FromCenterExtents(Vector3 bounds_extents,
                                           ProjectionEnum normalised = ProjectionEnum.Zero_one_,
                                           int decimal_granularity = 4) {
      return new Space3 {
                            _min = -bounds_extents,
                            Max = bounds_extents,
                            normalised = normalised,
                            _decimal_granularity = decimal_granularity
                        };
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    dynamic NormaliseMinusOneOne(dynamic v) { // TODO: Finish cases
      if (v.x > this._max.x
          || v.y > this._max.y
          || v.z > this._max.z
          || v.x < this._min.x
          || v.y < this._min.y
          || v.z < this._min.z) {
        throw new ArgumentException(message : $"Value was {v}, min:{this._min}, max:{this._max}");
      }

      if (this.Span.x > 0 && this.Span.y > 0 && this.Span.z > 0) {
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
      }

      return v;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    Vector3 DenormaliseMinusOneOne(Vector3 v) {
      if (v.x > 1 || v.y > 1 || v.z > 1 || v.x < -1 || v.y < -1 || v.z < -1) {
        throw new ArgumentException(message : $"Value was {v}, min:-1, max:1");
      }

      if (this.Span.x <= 0) { //TODO: FINISH cases
        if (this.Span.y <= 0) {
          return new Vector3(0, 0);
        }

        return new Vector3(0,
                           y : Normalisation.DenormaliseMinusOneOne_(v : v.y,
                                                                     min : this._min.y,
                                                                     span : this.Span.y));
      }

      if (this.Span.y <= 0) {
        if (this.Span.x <= 0) {
          return new Vector3(0, 0);
        }

        return new Vector3(x : Normalisation.DenormaliseMinusOneOne_(v : v.x,
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
    Vector3 Denormalise01(Vector3 v) {
      if (v.x > 1 || v.y > 1 || v.z > 1 || v.x < 0 || v.y < 0 || v.z < 0) {
        throw new ArgumentException(message : $"Value was {v}, min:0, max:1");
      }

      return Normalisation.Denormalise01_(v : v, min : this._min, span : this.Span);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    Vector3 Normalise01(Vector3 v) {
      if (v.x > this._max.x
          || v.y > this._max.y
          || v.z > this._max.z
          || v.x < this._min.x
          || v.y < this._min.y
          || v.z < this._min.z) {
        throw new ArgumentException(message : $"Value was {v}, min:{this._min}, max:{this._max}");
      }

      if (this.Span.x > 0 && this.Span.y > 0 && this.Span.z > 0) { //TODO: Complete variations
        v = this.Round(v : Normalisation.Normalise01_(v : v, min : this._min, span : this.Span));
      } else if (this.Span.x > 0 && this.Span.y > 0 && this.Span.z <= 0) {
        v.x = this.Round(v : Normalisation.Normalise01_(v : v.x, min : this._min.x, span : this.Span.x));
        v.y = this.Round(v : Normalisation.Normalise01_(v : v.y, min : this._min.y, span : this.Span.y));
        v.z = 0;
      } else if (this.Span.x > 0 && this.Span.y <= 0 && this.Span.z <= 0) {
        v.x = this.Round(v : Normalisation.Normalise01_(v : v.x, min : this._min.x, span : this.Span.x));
        v.y = 0;
        v.z = 0;
      } else {
        v.x = 0;
        v.y = 0;
        v.z = 0;
      }

      return v;
    }
  }
}