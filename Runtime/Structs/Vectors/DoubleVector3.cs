using System;
using UnityEngine;

namespace droid.Runtime.Structs.Vectors {
  [Serializable]
  public struct DoubleVector3 {
    [SerializeField] double _X;
    [SerializeField] double _Y;
    [SerializeField] double _Z;

    public DoubleVector3(Vector3 vec3) {
      this._X = vec3.x;
      this._Y = vec3.y;
      this._Z = vec3.z;
    }

    public static DoubleVector3 operator+(DoubleVector3 a, DoubleVector3 b) {
      a._X += b._X;
      a._Y += b._Y;
      a._Z += b._Z;
      return a;
    }

    public DoubleVector3(double x, double y, double z) {
      this._X = x;
      this._Y = y;
      this._Z = z;
    }

    public Double X { get { return this._X; } set { this._X = value; } }

    public Double Y { get { return this._Y; } set { this._Y = value; } }

    public Double Z { get { return this._Z; } set { this._Z = value; } }
  }
}
