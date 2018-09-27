using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Neodroid.Runtime.Utilities.NeodroidCamera {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [ExecuteInEditMode, Serializable]
    public class CameraObservationResolutionController : MonoBehaviour
    {
        [SerializeField]
        Camera[] _cameras;

        [SerializeField] Texture[] _textures;

        [SerializeField] Vector2Int _size = new Vector2Int(84,84);
        
        void Awake() {
            this._cameras = FindObjectsOfType<Camera>();

            var textures = new List<Texture>();
            
            foreach (var camera1 in this._cameras) {
                var target = camera1.targetTexture;
                if(target) {
                    textures.Add(target);
                }
            }

            this._textures = textures.ToArray();

            foreach (var texture in this._textures) {
                if (texture) {
                    texture.height = this._size.y;
                    texture.width = this._size.x;
                }
            }
        }
    }
}
