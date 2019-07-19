using System;
using UnityEngine;

#if UNITY_2019_1_OR_NEWER

namespace droid.Runtime.GameObjects.Plotting {
  [ExecuteInEditMode]
  public class ValueRangePlotter : MonoBehaviour {
    Material _material;
    [SerializeField] Shader _shader = null;
    [SerializeField] Bounds _value_range = new Bounds(Vector3.zero, Vector3.one * 2);
    static readonly Int32 _range = Shader.PropertyToID("_Range");

    void OnDestroy() {
      if (this._material != null) {
        if (Application.isPlaying) {
          Destroy(this._material);
        } else {
          DestroyImmediate(this._material);
        }
      }
    }

    public void OnRenderObject() {
      if (this._material == null) {
        this._material = new Material(this._shader);
        this._material.hideFlags = HideFlags.DontSave;
      }

      this._material.SetVector(_range,
                               new Vector4(this._value_range.min.x,
                                           this._value_range.max.x,
                                           this._value_range.center.y,
                                           this._value_range.extents.y + this._value_range.center.y));

      this._material.SetPass(0);
      Graphics.DrawProceduralNow(MeshTopology.LineStrip, 512);
    }
  }
}
#endif
