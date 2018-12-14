using System;
using Random = UnityEngine.Random;
using UnityEngine;

namespace Neodroid.Runtime.Utilities.Structs {
  [Serializable]
  public struct ValueSpace {
    public int _Decimal_Granularity;
    public float _Min_Value;
    public float _Max_Value;

    public ValueSpace(int decimal_granularity = 2) {
      this._Decimal_Granularity = decimal_granularity;
      this._Min_Value = -1f; //float.NegativeInfinity;
      this._Max_Value = 1f; //float.PositiveInfinity;
    }

    public float RandomValue() {
      var x = Random.Range(this._Min_Value, this._Max_Value);

      return x;
    }

    public float Sample() { return this.RandomValue(); }

    public float Span { get { return this._Max_Value - this._Min_Value; } }

    public float ClipNormaliseRound(float v) {
      if (v > this._Max_Value) {
        v = this._Max_Value;
      } else if (v < this._Min_Value) {
        v = this._Min_Value;
      }

      return this.Round((v - this._Min_Value) / this.Span);
    }

    public Vector2 ToVector2(){
      return new Vector2(this._Min_Value,this._Max_Value);
    }
    
    public Vector3 ToVector3(){
      return new Vector3(this._Min_Value,this._Decimal_Granularity,this._Max_Value);
    }

    public float Round(float v) { return (float)Math.Round(v, this._Decimal_Granularity); }
  }
}