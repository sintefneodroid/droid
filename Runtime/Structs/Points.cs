using UnityEngine;

namespace droid.Runtime.Structs {
  /// <summary>
  /// 
  /// </summary>
  public class Points : MonoBehaviour {
    /// <summary>
    /// 
    /// </summary>
    public struct StringPoint {
      /// <summary>
      ///
      /// </summary>
      public Vector3 _Pos;

      /// <summary>
      ///
      /// </summary>
      public float _Size;

      /// <summary>
      ///
      /// </summary>
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
      /// <summary>
      ///
      /// </summary>
      public Vector3 _Pos;

      /// <summary>
      ///
      /// </summary>
      public float _Size;

      /// <summary>
      ///
      /// </summary>
      public float _Val;

      public ValuePoint(Vector3 pos, float val, float size) {
        this._Pos = pos;
        this._Val = val;
        this._Size = size;
      }
    }
  }
}
