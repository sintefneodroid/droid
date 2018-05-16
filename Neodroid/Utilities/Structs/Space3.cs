using System;
using UnityEngine;

namespace Neodroid.Utilities.Structs {
  [Serializable]
  public struct Space3 {
    public int _Decimal_Granularity;
    public Vector3 _Min_Values;
    public Vector3 _Max_Values;

    public Space3(int decimal_granularity = 10) {
      this._Decimal_Granularity = decimal_granularity;
      this._Min_Values = Vector3.one * -100f;
      this._Max_Values = Vector3.one * 100f; //Vector3.positiveInfinity;
    }

    public Vector3 Span { get { return this._Max_Values - this._Min_Values; } }

    public Vector3 ClipNormaliseRound(Vector3 v) {
      if (v.x > this._Max_Values.x) {
        v.x = this._Max_Values.x;
      } else if (v.x < this._Min_Values.x) {
        v.x = this._Min_Values.x;
      }

      if (this.Span.x > 0) {
        v.x = this.Round((v.x - this._Min_Values.x) / this.Span.x);
      } else {
        v.x = 0;
      }

      if (v.y > this._Max_Values.y) {
        v.y = this._Max_Values.y;
      } else if (v.y < this._Min_Values.y) {
        v.y = this._Min_Values.y;
      }

      if (this.Span.y > 0) {
        v.y = this.Round((v.y - this._Min_Values.y) / this.Span.y);
      } else {
        v.y = 0;
      }

      if (v.z > this._Max_Values.z) {
        v.z = this._Max_Values.z;
      } else if (v.z < this._Min_Values.z) {
        v.z = this._Min_Values.z;
      }

      if (this.Span.z > 0) {
        v.z = this.Round((v.z - this._Min_Values.z) / this.Span.z);
      } else {
        v.z = 0;
      }

      return v;
    }

    public float Round(float v) { return (float)Math.Round(v, this._Decimal_Granularity); }
  }
}
