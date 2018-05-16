using UnityEngine;

namespace Neodroid.Utilities.Structs {
	public class Points : MonoBehaviour {

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
