using UnityEngine;

namespace droid.Runtime.Utilities.Structs {
  /// <summary>
  /// 
  /// </summary>
  public class Points : MonoBehaviour {
    /// <summary>
    /// 
    /// </summary>
    public struct StringPoint {
      public Vector3 _Pos;
      public float _Size;
      public string _Val;

      public StringPoint(Vector3 pos, string val, float size) {
        this._Pos = pos;
        this._Val = val;
        this._Size = size;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public struct ValuePoint {
      public Vector3 _Pos;
      public float _Size;
      public float _Val;

      public ValuePoint(Vector3 pos, float val, float size) {
        this._Pos = pos;
        this._Val = val;
        this._Size = size;
      }
    }
  }
}
