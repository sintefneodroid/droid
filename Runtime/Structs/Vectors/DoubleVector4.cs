using System;
using UnityEngine;

namespace droid.Runtime.Structs.Vectors {
  [Serializable]
  public struct DoubleVector4 {
    [SerializeField] double _X;
    [SerializeField] double _Y;
    [SerializeField] double _Z;
    [SerializeField] double _W;

    public DoubleVector4(Vector4 vec3) {
      this._X = vec3.x;
      this._Y = vec3.y;
      this._Z = vec3.z;
      this._W = vec3.w;
    }

    public static DoubleVector4 operator+(DoubleVector4 a, DoubleVector4 b) {
      a._X += b._X;
      a._Y += b._Y;
      a._Z += b._Z;
      a._W += b._W;
      return a;
    }

    public DoubleVector4(double x, double y, double z, double w) {
      this._X = x;
      this._Y = y;
      this._Z = z;
      this._W = w;
    }

    public Double X { get { return this._X; } set { this._X = value; } }

    public Double Y { get { return this._Y; } set { this._Y = value; } }

    public Double Z { get { return this._Z; } set { this._Z = value; } }

    public Double W { get { return this._W; } set { this._W = value; } }

    /// <summary>
    ///
    /// </summary>
    public static DoubleVector4 Zero {
      get {
        return new DoubleVector4(0,
                                 0,
                                 0,
                                 0);
      }
    }
  }
}
