using System;
using droid.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Object = System.Object;
using Random = UnityEngine.Random;

namespace droid.Runtime.Utilities.Structs {
  /// <summary>
  ///
  /// </summary>
  [Serializable]
  public struct Space3:ISpace {
    public int DecimalGranularity {
      get { return this._decimal_granularity; }
      set { this._decimal_granularity = value; }
    }

    public bool IsNormalised { get { return this.normalised; } set { this.normalised = value; } }
    public Vector3 _Min_Values;
    public Vector3 _Max_Values;
    public int _decimal_granularity;
    [SerializeField]  bool normalised;

    public Space3(int decimal_granularity = 1) :this(){
      this._decimal_granularity = decimal_granularity;
      this._Min_Values = Vector3.one * -100f;
      this._Max_Values = Vector3.one * 100f; //Vector3.positiveInfinity;
    }

    public Vector3 Span { get { return this._Max_Values - this._Min_Values; } }

    public Vector3 Sample() {
      var x = Random.Range(this._Min_Values.x, this._Max_Values.x);
      var y = Random.Range(this._Min_Values.y, this._Max_Values.y);
      var z = Random.Range(this._Min_Values.z, this._Max_Values.z);

      return new Vector3(x, y, z);
    }

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

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public float Round(float v) { return (float)Math.Round(v, this.DecimalGranularity); }

    public Space1 Xspace {
      get {
        return new Space1(this.DecimalGranularity) {
                                                         _Min_Value = this._Min_Values.x,
                                                         _Max_Value = this._Max_Values.x
                                                     };
      }
    }

    public Space1 Yspace {
      get {
        return new Space1(this.DecimalGranularity) {
                                                         _Min_Value = this._Min_Values.y,
                                                         _Max_Value = this._Max_Values.y
                                                     };
      }
    }

    public Space1 Zspace {
      get {
        return new Space1(this.DecimalGranularity) {
                                                         _Min_Value = this._Min_Values.z,
                                                         _Max_Value = this._Max_Values.z
                                                     };
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static Space3 ZeroOne {
      get { return new Space3(1) {_Min_Values = Vector3.zero, _Max_Values = Vector3.one}; }
    }

    public static Space3 TwentyEighty {
      get { return new Space3(1) {_Min_Values = Vector3.one*0.2f, _Max_Values = Vector3.one*0.8f}; }
    }


    public static Space3 MinusOneOne {
      get { return new Space3(1) {_Min_Values = -Vector3.one, _Max_Values = Vector3.one}; }
    }
  }
}
