using System;
using UnityEngine;

namespace Neodroid.Utilities.Structs {
  [Serializable]
  public struct DoubleVector3 {
    [SerializeField] public double _X;
    [SerializeField] public double _Y;
    [SerializeField] public double _Z;

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

    public Double x { get { return this._X; } set { this._X = value; } }

    public Double y { get { return this._Y; } set { this._Y = value; } }

    public Double z { get { return this._Z; } set { this._Z = value; } }
  }
}
