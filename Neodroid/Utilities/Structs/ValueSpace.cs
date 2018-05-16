using System;

namespace Neodroid.Utilities.Structs {
  [Serializable]
  public struct ValueSpace {
    public int _Decimal_Granularity;
    public float _Min_Value;
    public float _Max_Value;

    public ValueSpace(int decimal_granularity = 10) {
      this._Decimal_Granularity = decimal_granularity;
      this._Min_Value = -100f; //float.NegativeInfinity;
      this._Max_Value = 100f; //float.PositiveInfinity;
    }

    public float Span { get { return this._Max_Value - this._Min_Value; } }

    public float ClipNormaliseRound(float v) {
      if (v > this._Max_Value) {
        v = this._Max_Value;
      } else if (v < this._Min_Value) {
        v = this._Min_Value;
      }

      return this.Round((v - this._Min_Value) / this.Span);
    }

    public float Round(float v) { return (float)Math.Round(v, this._Decimal_Granularity); }
  }
}
