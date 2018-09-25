#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Neodroid.Runtime.Utilities.Unsorted.SceneReference.Editor {
  /// <summary>
  /// Class used to serialize a reference to a scene asset that can be used
  /// at runtime in a build, when the asset can no longer be directly
  /// referenced. This caches the scene name based on the SceneAsset to use
  /// at runtime to load.
  /// </summary>
  [Serializable]
  public class SceneReference : ISerializationCallbackReceiver {
    /// <summary>
    /// Exception that is raised when there is an issue resolving and
    /// loading a scene reference.
    /// </summary>
    public class SceneLoadException : Exception {
      public SceneLoadException(string message) : base(message) { }
    }

    #if UNITY_EDITOR
    /// <summary>
    /// 
    /// </summary>
    public SceneAsset _Scene;
    #endif

    /// <summary>
    /// 
    /// </summary>
    [Tooltip("The name of the referenced scene. This may be used at runtime to load the scene.")]
    public string _SceneName;

    [SerializeField] int _scene_index = -1;

    [SerializeField] bool _scene_enabled;

    void ValidateScene() {
      if (string.IsNullOrEmpty(this._SceneName)) {
        throw new SceneLoadException("No scene specified.");
      }

      if (this._scene_index < 0) {
        throw new SceneLoadException("Scene " + this._SceneName + " is not in the build settings");
      }

      if (!this._scene_enabled) {
        throw new SceneLoadException("Scene " + this._SceneName + " is not enabled in the build settings");
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mode"></param>
    public void LoadScene(LoadSceneMode mode = LoadSceneMode.Single) {
      this.ValidateScene();
      SceneManager.LoadScene(this._SceneName, mode);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public void OnBeforeSerialize() {
      #if UNITY_EDITOR
      if (this._Scene != null) {
        var scene_asset_path = AssetDatabase.GetAssetPath(this._Scene);
        var scene_asset_guid = AssetDatabase.AssetPathToGUID(scene_asset_path);

        var scenes = EditorBuildSettings.scenes;

        this._scene_index = -1;
        for (var i = 0; i < scenes.Length; i++) {
          if (scenes[i].guid.ToString() == scene_asset_guid) {
            this._scene_index = i;
            this._scene_enabled = scenes[i].enabled;
            if (scenes[i].enabled) {
              this._SceneName = this._Scene.name;
            }

            break;
          }
        }
      } else {
        this._SceneName = "";
      }
      #endif
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public void OnAfterDeserialize() { }
  }
}
#endif
