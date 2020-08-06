//#define ALWAYS_PRE_CLIP_PROJECTIONS
//#define ZERO_RETURN_NEGATIVE_SPAN
using System;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Structs.Space {
  /// <inheritdoc cref="ISpace" />
  ///  <summary>
  ///  </summary>
  [Serializable]
  public struct Space1 : ISpace {
    #region Fields

    /// <summary>
    ///
    /// </summary>
    [Header("Space", order = 103)]
    [SerializeField]
    float _min;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    float _max;

    /// <summary>
    ///
    /// </summary>
    ///
    [Range(-1, 15)]
    [SerializeField]
    int _decimal_granularity;

    [SerializeField] ProjectionEnum _projection;
    [SerializeField] bool _clipped;

    #endregion

    /// <summary>
    ///
    /// </summary>
    public float Span { get { return this._max - this._min; } }

    /// <summary>
    /// If max is less than min, no clipping is performed.
    /// </summary>
    /// <param name="v"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    static float Clip(float v, float min, float max) {
      #if ZERO_RETURN_NEGATIVE_SPAN
        return max < min ? 0 : Mathf.Clamp(value : v, min : min, max : max);
      #else
        return max < min ? v : Mathf.Clamp(value : v, min : min, max : max);
      #endif
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    float Clip(float v) { return Clip(v : v, min : this._min, max : this._max); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    float RoundClip(float v) {
      #if ALWAYS_PRE_CLIP_PROJECTIONS
      v = Clip(v : v);
      #endif

      return this.Round(v : v);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    float ClipRound(float v) {
      #if ALWAYS_PRE_CLIP_PROJECTIONS
      v = Clip(v : v);
      #endif

      return this.Round(v : v);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    dynamic ClipNormaliseRound(dynamic v) {
      #if ALWAYS_PRE_CLIP_PROJECTIONS
      v = this.Clip(v);
      #endif

      return this.Round(v : this.Normalise01(v : v));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    dynamic ClipNormaliseMinusOneOneRound(dynamic v) {
      #if ALWAYS_PRE_CLIP_PROJECTIONS
      v = this.Clip( v);
      #endif

      return this.Round(v : this.NormaliseMinusOneOne(v : v));
    }

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
          return ClipNormaliseRound(v : v);
        case ProjectionEnum.Minus_one_one_:
          return ClipNormaliseMinusOneOneRound(v : v);
        default: throw new ArgumentOutOfRangeException();
      }
    }

    public bool Clipped { get { return this._clipped; } set { this._clipped = value; } }

    public dynamic Mean { get { return (this.Max + this.Min) * 0.5f; } }

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
          v = ClipRoundDenormaliseMinusOneOneClip(v : v);
          break;
        case ProjectionEnum.None_: break;
        default: throw new ArgumentOutOfRangeException();
      }

      if (this.Clipped) {
        v = this.Clip(v : v);
      }

      return v;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Vector2 ToVector2() { return new Vector2(x : this._min, y : this._max); }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Vector3 ToVector3() {
      return new Vector3(x : this._min, y : this._max, z : this._decimal_granularity);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    dynamic ClipRoundDenormalise01Clip(dynamic v) {
      #if ALWAYS_PRE_CLIP_PROJECTIONS
      v = Clip(v : v, min : 0, max : 1);
      #endif

      return this.Clip(v : this.Round(v : this.Denormalise01(v : v)));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    dynamic ClipRoundDenormaliseMinusOneOneClip(dynamic v) {
      #if ALWAYS_PRE_CLIP_PROJECTIONS
      v = Clip(v : v, min : -1, max : 1);
      #endif

      return this.Clip(v : this.Round(v : this.DenormaliseMinusOneOne(v : v)));
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static string Vector3Description() { return "Space (min, max, granularity)"; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public float Round(float v) {
      return this.DecimalGranularity >= 0
                 ? (float)Math.Round(value : v, digits : this._decimal_granularity)
                 : v;
    }

    /// <summary>
    ///
    /// </summary>
    public static Space1 TwentyEighty {
      get {
        return new Space1 {
                              _min = 0.2f,
                              _max = 0.8f,
                              DecimalGranularity = 4,
                              Normalised = ProjectionEnum.Zero_one_,
                              Clipped = true
                          };
      }
    }

    /// <summary>
    ///
    /// </summary>
    public static Space1 ZeroOne {
      get {
        return new Space1 {
                              _min = 0,
                              _max = 1,
                              DecimalGranularity = 4,
                              Normalised = ProjectionEnum.Zero_one_,
                              Clipped = true
                          };
      }
    }

    /// <summary>
    ///
    /// </summary>
    public static Space1 MinusOneOne {
      get {
        return new Space1 {
                              _min = -1,
                              _max = 1,
                              DecimalGranularity = 4,
                              Normalised = ProjectionEnum.Zero_one_,
                              Clipped = true
                          };
      }
    }

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
    public bool NormalisedBool {
      get { return this._projection == ProjectionEnum.Zero_one_; }
      set { this._projection = value ? ProjectionEnum.Zero_one_ : ProjectionEnum.None_; }
    }

    /// <summary>
    ///
    /// </summary>
    public static Space1 DiscreteMinusOneOne {
      get {
        return new Space1 {
                              _min = -1,
                              _max = 1,
                              DecimalGranularity = 0,
                              Normalised = ProjectionEnum.None_,
                              Clipped = false
                          };
      }
    }

    /// <summary>
    ///
    /// </summary>
    public static Space1 DiscreteZeroOne {
      get {
        return new Space1 {
                              _min = 0,
                              _max = 1,
                              DecimalGranularity = 0,
                              Normalised = ProjectionEnum.None_,
                              Clipped = false
                          };
      }
    }

    /// <summary>
    ///
    /// </summary>
    public dynamic Precision {
      get {
        if (this._decimal_granularity < 0) {
          return float.PositiveInfinity;
        }

        return 1.0f / (this._decimal_granularity + 1.0f);
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
    public ProjectionEnum Normalised { get { return this._projection; } set { this._projection = value; } }

    /// <summary>
    ///
    /// </summary>
    /// <param name="vector3_field"></param>
    public void FromVector3(Vector3 vector3_field) {
      this._decimal_granularity = (int)vector3_field.z;
      this._max = vector3_field.y;
      this._min = vector3_field.x;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Space1 operator*(Space1 a, float b) {
      a.Max *= b;
      a.Min *= b;
      return a;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="extent"></param>
    /// <param name="projection_enum"></param>
    /// <param name="decimal_granularity"></param>
    /// <returns></returns>
    public static Space1 FromCenterExtent(float extent,
                                          ProjectionEnum projection_enum = ProjectionEnum.Zero_one_,
                                          int decimal_granularity = 4) {
      return new Space1 {
                            _min = -extent,
                            Max = extent,
                            _projection = projection_enum,
                            DecimalGranularity = decimal_granularity
                        };
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    float Normalise01(float v) {
      if (v > this._max || v < this._min) {
        throw new ArgumentException(message : $"Value was {v}, min:{this._min}, max:{this._max}");
      }

      if (this.Span <= 0) {
        return 0;
      }

      return Normalisation.Normalise01_(v : v, min : this._min, span : this.Span);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    float NormaliseMinusOneOne(float v) {
      if (v > this._max || v < this._min) {
        throw new ArgumentException(message : $"Value was {v}, min:{this._min}, max:{this._max}");
      }

      if (this.Span <= 0) {
        return 0;
      }

      return Normalisation.NormaliseMinusOneOne_(v : v, min : this._min, span : this.Span);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    float Denormalise01(float v) {
      if (v > 1 || v < 0) {
        throw new ArgumentException(message : $"Value was {v}, min:0, max:1");
      }

      if (this.Span <= 0) {
        return 0;
      }

      return Normalisation.Denormalise01_(v : v, min : this._min, span : this.Span);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    float DenormaliseMinusOneOne(float v) {
      if (v > 1 || v < -1) {
        throw new ArgumentException(message : $"Value was {v}, min:-1, max:1");
      }

      if (this.Span <= 0) {
        return 0;
      }

      return Normalisation.DenormaliseMinusOneOne_(v : v, min : this._min, span : this.Span);
    }
  }
}