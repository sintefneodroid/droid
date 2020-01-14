using System;
using System.Collections.Generic;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.GameObjects.NeodroidCamera {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  [Serializable]
  public class CameraObservationTextureController : MonoBehaviour {
    [SerializeField] Camera[] _cameras = null;
    [SerializeField] FilterMode _filter_mode = FilterMode.Bilinear;

    [SerializeField]
    Vector2Int _size = new Vector2Int(x : NeodroidConstants._Default_Observation_Texture_Xy_Size,
                                      y : NeodroidConstants._Default_Observation_Texture_Xy_Size);
    //[SerializeField] GraphicsFormat _texture_format = GraphicsFormat.R8G8B8A8_UNorm;

    [SerializeField] Texture[] _textures = null;
    [SerializeField] TextureWrapMode _wrap_mode = TextureWrapMode.Clamp;

    void Awake() {
      this._cameras = FindObjectsOfType<Camera>();

      var textures = new List<Texture>();

      for (var index = 0; index < this._cameras.Length; index++) {
        var a_camera = this._cameras[index];
        var target = a_camera.targetTexture;
        if (target) {
          textures.Add(item : target);
        }
      }

      this._textures = textures.ToArray();

      for (var index = 0; index < this._textures.Length; index++) {
        var texture = this._textures[index];
        if (texture) {
          //texture.height = this._size.y;
          //texture.width = this._size.x;
          texture.filterMode = this._filter_mode;
          texture.wrapMode = this._wrap_mode;
          //texture.graphicsFormat = this._texture_format;
        }
      }
    }
  }
}
