using UnityEngine;

namespace Neodroid.Utilities.Structs {
  /// <summary>
  ///
  /// </summary>
  [System.Serializable]
  public struct Space4 {
    public int _DecimalGranularity;
    public Vector4 _MinValues;
    public Vector4 _MaxValues;

    public Space4(int decimal_granularity = System.Int32.MaxValue) {
      this._DecimalGranularity = decimal_granularity;
      this._MinValues = Vector4.negativeInfinity;
      this._MaxValues = Vector4.positiveInfinity;
    }

    public Vector4 Span { get { return this._MaxValues - this._MinValues; } }

    public Vector4 ClipNormalise(Vector4 v) {
      if (v.x > this._MaxValues.x) {
        v.x = this._MaxValues.x;
      } else if (v.x < this._MinValues.x) {
        v = this._MinValues;
      }

      v.x = (v.x - this._MinValues.x) / this.Span.x;

      if (v.y > this._MaxValues.y) {
        v.y = this._MaxValues.y;
      } else if (v.y < this._MinValues.y) {
        v = this._MinValues;
      }

      v.y = (v.y - this._MinValues.y) / this.Span.y;

      if (v.z > this._MaxValues.z) {
        v.z = this._MaxValues.z;
      } else if (v.z < this._MinValues.z) {
        v = this._MinValues;
      }

      v.z = (v.z - this._MinValues.z) / this.Span.z;

      if (v.w > this._MaxValues.w) {
        v.w = this._MaxValues.w;
      } else if (v.w < this._MinValues.w) {
        v = this._MinValues;
      }

      v.w = (v.w - this._MinValues.w) / this.Span.w;

      return v;
    }

    public float Round(float v) { return (float)System.Math.Round(v, this._DecimalGranularity); }
  }
}
