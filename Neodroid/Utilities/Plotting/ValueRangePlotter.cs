using UnityEngine;

namespace Neodroid.Utilities.Plotting {
  [ExecuteInEditMode]
  public class ValueRangePlotter : MonoBehaviour {
    [SerializeField] Shader _shader;
    [SerializeField] Bounds _value_range = new Bounds(Vector3.zero, Vector3.one * 2);

    Material _material;

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

      this._material.SetVector(
          "_Range",
          new Vector4(
              this._value_range.min.x,
              this._value_range.max.x,
              this._value_range.center.y,
              this._value_range.extents.y + this._value_range.center.y));

      this._material.SetPass(0);
      Graphics.DrawProcedural(MeshTopology.LineStrip, 512, 1);
    }
  }
}
