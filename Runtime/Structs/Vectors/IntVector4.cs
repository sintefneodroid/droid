using System;
using UnityEngine;

namespace droid.Runtime.Structs.Vectors {
  [Serializable]
  public struct IntVector4 {
    [SerializeField] public int _X;
    [SerializeField] public int _Y;
    [SerializeField] public int _Z;
    [SerializeField] public int _W;

    public IntVector4(Vector4 vec3) {
      this._X = (int)vec3.x;
      this._Y = (int)vec3.y;
      this._Z = (int)vec3.z;
      this._W = (int)vec3.w;
    }

    public static IntVector4 operator+(IntVector4 a, IntVector4 b) {
      a._X += b._X;
      a._Y += b._Y;
      a._Z += b._Z;
      a._W += b._W;
      return a;
    }

    public IntVector4(int x, int y, int z, int w) {
      this._X = x;
      this._Y = y;
      this._Z = z;
      this._W = w;
    }

    public IntVector4(float x, float y, float z, float w) {
      this._X = (int)x;
      this._Y = (int)y;
      this._Z = (int)z;
      this._W = (int)w;
    }

    public int X { get { return this._X; } set { this._X = value; } }

    public int Y { get { return this._Y; } set { this._Y = value; } }

    public int Z { get { return this._Z; } set { this._Z = value; } }

    public int W { get { return this._W; } set { this._W = value; } }

    /// <summary>
    ///
    /// </summary>
    public static IntVector4 Zero {
      get {
        return new IntVector4(0,
                              0,
                              0,
                              0);
      }
    }
  }
}
