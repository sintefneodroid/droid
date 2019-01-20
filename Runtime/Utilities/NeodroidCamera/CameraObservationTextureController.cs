using System;
using System.Collections.Generic;
using UnityEngine;

namespace droid.Runtime.Utilities.NeodroidCamera {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  [Serializable]
  public class CameraObservationTextureController : MonoBehaviour {
    [SerializeField] Camera[] _cameras;
    [SerializeField] FilterMode _filter_mode = FilterMode.Bilinear;

    [SerializeField] Vector2Int _size = new Vector2Int(84, 84);
    [SerializeField] TextureFormat _texture_format = TextureFormat.ARGB32;

    [SerializeField] Texture[] _textures;
    [SerializeField] TextureWrapMode _wrap_mode = TextureWrapMode.Clamp;

    void Awake() {
      this._cameras = FindObjectsOfType<Camera>();

      var textures = new List<Texture>();

      foreach (var camera1 in this._cameras) {
        var target = camera1.targetTexture;
        if (target) {
          textures.Add(target);
        }
      }

      this._textures = textures.ToArray();

      foreach (var texture in this._textures) {
        if (texture) {
          texture.height = this._size.y;
          texture.width = this._size.x;
          texture.filterMode = this._filter_mode;
          texture.wrapMode = this._wrap_mode;
        }
      }
    }
  }
}
