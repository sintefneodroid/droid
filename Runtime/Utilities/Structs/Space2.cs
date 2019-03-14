using System;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;

namespace droid.Runtime.Utilities.Structs {
  /// <summary>
  /// 
  /// </summary>
  [Serializable]
  public struct Space2 {
    /// <summary>
    /// 
    /// </summary>
    public int _Decimal_Granularity;
    /// <summary>
    /// 
    /// </summary>
    public Vector2 _Min_Values;
    /// <summary>
    /// 
    /// </summary>
    public Vector2 _Max_Values;

    public Space2(int decimal_granularity = 2) : this() {
      this._Min_Values = Vector2.one * -100f; //Vector2.negativeInfinity;
      this._Max_Values = Vector2.one * 100f; //Vector2.positiveInfinity;
      this._Decimal_Granularity = decimal_granularity;
    }

    /// <summary>
    /// 
    /// </summary>
    public Vector2 Span { get { return this._Max_Values - this._Min_Values; } }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Vector2 Sample() {
      var x = Random.Range(this._Min_Values.x, this._Max_Values.x);
      var y = Random.Range(this._Min_Values.y, this._Max_Values.y);

      return new Vector3(x, y);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Vector2 ClipNormaliseRound(Vector2 v) {
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

      return v;
    }

    /// <summary>
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public float Round(float v) { return (float)Math.Round(v, this._Decimal_Granularity); }

    /// <summary>
    /// 
    /// </summary>
    public static Space2 ZeroOne {
      get { return new Space2(1) {_Min_Values = Vector2.zero, _Max_Values = Vector2.one}; }
    }
  }
}
