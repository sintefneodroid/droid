using System;
using UnityEngine;

namespace Neodroid.Utilities.NeodroidCamera {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [RequireComponent(typeof(Camera))]
  [ExecuteInEditMode]
  [Serializable]
  public class SynchroniseCameraProperties : MonoBehaviour {
    /// <summary>
    ///
    /// </summary>
    const double _tolerance = Double.Epsilon;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    SynchroniseCameraProperties _camera;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    SynchroniseCameraProperties[] _cameras;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    int _old_culling_mask;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    float _old_far_clip_plane;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    float _old_near_clip_plane;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    float _old_orthographic_size;

    [SerializeField] bool _sync_culling_mask = true;
    [SerializeField] bool _sync_far_clip_plane = true;
    [SerializeField] bool _sync_near_clip_plane = true;

    [SerializeField] bool _sync_orthographic_size = true;

    /// <summary>
    ///
    /// </summary>
    public Boolean SyncOrthographicSize {
      get { return this._sync_orthographic_size; }
      set { this._sync_orthographic_size = value; }
    }

    public Boolean SyncNearClipPlane {
      get { return this._sync_near_clip_plane; }
      set { this._sync_near_clip_plane = value; }
    }

    public Boolean SyncFarClipPlane {
      get { return this._sync_far_clip_plane; }
      set { this._sync_far_clip_plane = value; }
    }

    public Boolean SyncCullingMask {
      get { return this._sync_culling_mask; }
      set { this._sync_culling_mask = value; }
    }

    /// <summary>
    ///
    /// </summary>
    public void Awake() {
      this._camera = this.GetComponent<SynchroniseCameraProperties>();
      if (this._camera) {
        this._old_orthographic_size = this._camera.GetComponent<Camera>().orthographicSize;
        this._old_near_clip_plane = this._camera.GetComponent<Camera>().nearClipPlane;
        this._old_far_clip_plane = this._camera.GetComponent<Camera>().farClipPlane;
        this._old_culling_mask = this._camera.GetComponent<Camera>().cullingMask;

        this._cameras = FindObjectsOfType<SynchroniseCameraProperties>();
      } else {
        Debug.Log("No camera component found on gameobject");
      }
    }

    public void Update() {
      if (this._camera) {
        if (Math.Abs(this._old_orthographic_size - this._camera.GetComponent<Camera>().orthographicSize)
            > _tolerance) {
          if (this._sync_culling_mask) {
            this._old_orthographic_size = this._camera.GetComponent<Camera>().orthographicSize;
            foreach (var cam in this._cameras) {
              if (cam != this._camera) {
                cam.GetComponent<Camera>().orthographicSize =
                    this._camera.GetComponent<Camera>().orthographicSize;
              }
            }
          }
        }

        if (Math.Abs(this._old_near_clip_plane - this._camera.GetComponent<Camera>().nearClipPlane)
            > _tolerance) {
          if (this._sync_culling_mask) {
            this._old_near_clip_plane = this._camera.GetComponent<Camera>().nearClipPlane;
            foreach (var cam in this._cameras) {
              if (cam != this._camera) {
                cam.GetComponent<Camera>().nearClipPlane = this._camera.GetComponent<Camera>().nearClipPlane;
              }
            }
          }
        }

        if (Math.Abs(this._old_far_clip_plane - this._camera.GetComponent<Camera>().farClipPlane)
            > _tolerance) {
          if (this._sync_culling_mask) {
            this._old_far_clip_plane = this._camera.GetComponent<Camera>().farClipPlane;
            foreach (var cam in this._cameras) {
              if (cam != this._camera) {
                cam.GetComponent<Camera>().farClipPlane = this._camera.GetComponent<Camera>().farClipPlane;
              }
            }
          }
        }

        if (this._old_culling_mask != this._camera.GetComponent<Camera>().cullingMask) {
          if (this._sync_culling_mask) {
            this._old_culling_mask = this._camera.GetComponent<Camera>().cullingMask;
            foreach (var cam in this._cameras) {
              if (cam != this._camera) {
                cam.GetComponent<Camera>().cullingMask = this._camera.GetComponent<Camera>().cullingMask;
              }
            }
          }
        }
      } else {
        Debug.Log("No camera component found on gameobject");
      }
    }
  }
}
