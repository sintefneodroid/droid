using System;
using UnityEngine;

namespace Neodroid.Utilities.Structs {
  [Serializable]
  public struct Space2 {
    public int _Decimal_Granularity;
    public Vector2 _Min_Values;
    public Vector2 _Max_Values;

    public Space2(Int32 decimal_granularity = 10) : this() {
      this._Min_Values = Vector2.one * -100f; //Vector2.negativeInfinity;
      this._Max_Values = Vector2.one * 100f; //Vector2.positiveInfinity;
      this._Decimal_Granularity = decimal_granularity;
    }

    public Vector2 Span { get { return this._Max_Values - this._Min_Values; } }

    public Vector2 ClipNormalise(Vector2 v) {
      if (v.x > this._Max_Values.x) {
        v.x = this._Max_Values.x;
      } else if (v.x < this._Min_Values.x) {
        v.x = this._Min_Values.x;
      }

      v.x = (v.x - this._Min_Values.x) / this.Span.x;

      if (v.y > this._Max_Values.y) {
        v.y = this._Max_Values.y;
      } else if (v.y < this._Min_Values.y) {
        v.y = this._Min_Values.y;
      }

      v.y = (v.y - this._Min_Values.y) / this.Span.y;

      return v;
    }

    public float Round(float v) { return (float)Math.Round(v, this._Decimal_Granularity); }
  }
}
