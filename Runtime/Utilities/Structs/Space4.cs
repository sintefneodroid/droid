using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Neodroid.Runtime.Utilities.Structs {
  /// <summary>
  /// </summary>
  [Serializable]
  public struct Space4 {
    public int _DecimalGranularity;
    public Vector4 _Min_Values;
    public Vector4 _Max_Values;

    public Space4(int decimal_granularity = int.MaxValue) {
      this._DecimalGranularity = decimal_granularity;
      this._Min_Values = Vector4.negativeInfinity;
      this._Max_Values = Vector4.positiveInfinity;
    }

    public Vector4 Span { get { return this._Max_Values - this._Min_Values; } }

    public Vector4 RandomVector4() {
      var x = Random.Range(this._Min_Values.x, this._Max_Values.x);
      var y = Random.Range(this._Min_Values.y, this._Max_Values.y);
      var z = Random.Range(this._Min_Values.z, this._Max_Values.z);
      var w = Random.Range(this._Min_Values.w, this._Max_Values.w);

      return new Vector4(x, y, z, w);
    }

    public Quaternion RandomQuaternion() {
      var vector = this.RandomVector4();
      return new Quaternion(vector.x, vector.y, vector.z, vector.w);
    }

    public Vector4 ClipNormaliseRound(Vector4 v) {
      if (v.x > this._Max_Values.x) {
        v.x = this._Max_Values.x;
      } else if (v.x < this._Min_Values.x) {
        v = this._Min_Values;
      }

      if (this.Span.x > 0) {
        v.x = this.Round((v.x - this._Min_Values.x) / this.Span.x);
      } else {
        v.x = 0;
      }

      if (v.y > this._Max_Values.y) {
        v.y = this._Max_Values.y;
      } else if (v.y < this._Min_Values.y) {
        v = this._Min_Values;
      }

      if (this.Span.y > 0) {
        v.y = this.Round((v.y - this._Min_Values.y) / this.Span.y);
      } else {
        v.y = 0;
      }

      if (v.z > this._Max_Values.z) {
        v.z = this._Max_Values.z;
      } else if (v.z < this._Min_Values.z) {
        v = this._Min_Values;
      }

      if (this.Span.z > 0) {
        v.z = this.Round((v.z - this._Min_Values.z) / this.Span.z);
      } else {
        v.z = 0;
      }

      if (v.w > this._Max_Values.w) {
        v.w = this._Max_Values.w;
      } else if (v.w < this._Min_Values.w) {
        v = this._Min_Values;
      }

      if (this.Span.w > 0) {
        v.w = this.Round((v.w - this._Min_Values.w) / this.Span.w);
      } else {
        v.w = 0;
      }

      return v;
    }

    public float Round(float v) { return (float)Math.Round(v, this._DecimalGranularity); }
  }
}