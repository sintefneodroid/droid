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
    Vector2Int _size = new Vector2Int(NeodroidConstants._Default_Observation_Texture_Xy_Size,
                                      NeodroidConstants._Default_Observation_Texture_Xy_Size);
    //[SerializeField] GraphicsFormat _texture_format = GraphicsFormat.R8G8B8A8_UNorm;

    [SerializeField] Texture[] _textures = null;
    [SerializeField] TextureWrapMode _wrap_mode = TextureWrapMode.Clamp;

    void Awake() {
      this._cameras = FindObjectsOfType<Camera>();

      var textures = new List<Texture>();

      foreach (var a_camera in this._cameras) {
        var target = a_camera.targetTexture;
        if (target) {
          textures.Add(target);
        }
      }

      this._textures = textures.ToArray();

      foreach (var texture in this._textures) {
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
