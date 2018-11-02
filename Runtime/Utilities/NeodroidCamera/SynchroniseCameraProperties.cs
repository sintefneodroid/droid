using System;
using UnityEngine;

namespace Neodroid.Runtime.Utilities.NeodroidCamera {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [RequireComponent(typeof(Camera))]
  [ExecuteInEditMode]
  [Serializable]
  public class SynchroniseCameraProperties : MonoBehaviour {
    /// <summary>
    /// </summary>
    const double _tolerance = double.Epsilon;

    /// <summary>
    /// </summary>
    [SerializeField]
    SynchroniseCameraProperties _camera;

    /// <summary>
    /// </summary>
    [SerializeField]
    SynchroniseCameraProperties[] _cameras;

    /// <summary>
    /// </summary>
    [SerializeField]
    int _old_culling_mask;

    /// <summary>
    /// </summary>
    [SerializeField]
    float _old_far_clip_plane;

    [SerializeField] float _old_fov;

    /// <summary>
    /// </summary>
    [SerializeField]
    float _old_near_clip_plane;

    [SerializeField] bool _old_orthographic_projection;

    /// <summary>
    /// </summary>
    [SerializeField]
    float _old_orthographic_size;

    [SerializeField] bool _only_run_in_edit_mode = true;

    [SerializeField] bool _only_run_on_awake; //TODO: Does nothing as of right now

    [SerializeField] bool _sync_culling_mask = true;
    [SerializeField] bool _sync_far_clip_plane = true;
    [SerializeField] bool _sync_fov = true;
    [SerializeField] bool _sync_near_clip_plane = true;
    [SerializeField] bool _sync_orthographic_projection = true;
    [SerializeField] bool _sync_orthographic_size = true;

    /// <summary>
    /// </summary>
    public bool SyncOrthographicSize {
      get { return this._sync_orthographic_size; }
      set { this._sync_orthographic_size = value; }
    }

    /// <summary>
    /// </summary>
    public bool SyncOrthographicProjection {
      get { return this._sync_orthographic_projection; }
      set { this._sync_orthographic_projection = value; }
    }

    /// <summary>
    /// </summary>
    public bool SyncNearClipPlane {
      get { return this._sync_near_clip_plane; }
      set { this._sync_near_clip_plane = value; }
    }

    /// <summary>
    /// </summary>
    public bool SyncFarClipPlane {
      get { return this._sync_far_clip_plane; }
      set { this._sync_far_clip_plane = value; }
    }

    /// <summary>
    /// </summary>
    public bool SyncCullingMask {
      get { return this._sync_culling_mask; }
      set { this._sync_culling_mask = value; }
    }

    /// <summary>
    /// </summary>
    public bool SyncFov { get { return this._sync_fov; } set { this._sync_fov = value; } }

    /// <summary>
    /// </summary>
    public void Awake() {
      this._camera = this.GetComponent<SynchroniseCameraProperties>();
      if (this._camera) {
        var cam = this._camera.GetComponent<Camera>();
        this._old_orthographic_size = cam.orthographicSize;
        this._old_near_clip_plane = cam.nearClipPlane;
        this._old_far_clip_plane = cam.farClipPlane;
        this._old_culling_mask = cam.cullingMask;
        this._old_orthographic_projection = cam.orthographic;
        this._old_fov = cam.fieldOfView;

        this._cameras = FindObjectsOfType<SynchroniseCameraProperties>();
      } else {
        Debug.Log("No camera component found on GameObject");
      }

      this.Sync_Cameras();
    }

    void Sync_Cameras() {
      if (this._camera) {
        var this_camera = this._camera.GetComponent<Camera>();
        if (this_camera) {
          if (this._sync_orthographic_size) {
            var orthographic_size = this_camera.orthographicSize;
            if (Math.Abs(this._old_orthographic_size - orthographic_size) > _tolerance) {
              this._old_orthographic_size = orthographic_size;
              foreach (var cam in this._cameras) {
                if (cam != this._camera) {
                  if (cam.SyncOrthographicSize) {
                    var other_cam = cam.GetComponent<Camera>();
                    if (other_cam) {
                      other_cam.orthographicSize = orthographic_size;
                    }
                  }
                }
              }
            }
          }

          if (this._sync_near_clip_plane) {
            var near_clip_plane = this_camera.nearClipPlane;
            if (Math.Abs(this._old_near_clip_plane - near_clip_plane) > _tolerance) {
              this._old_near_clip_plane = near_clip_plane;
              foreach (var cam in this._cameras) {
                if (cam != this._camera) {
                  if (cam.SyncNearClipPlane) {
                    var other_cam = cam.GetComponent<Camera>();
                    if (other_cam) {
                      other_cam.nearClipPlane = near_clip_plane;
                    }
                  }
                }
              }
            }
          }

          if (this._sync_far_clip_plane) {
            var far_clip_plane = this_camera.farClipPlane;
            if (Math.Abs(this._old_far_clip_plane - far_clip_plane) > _tolerance) {
              this._old_far_clip_plane = far_clip_plane;
              foreach (var cam in this._cameras) {
                if (cam != this._camera) {
                  if (cam.SyncFarClipPlane) {
                    var other_cam = cam.GetComponent<Camera>();
                    if (other_cam) {
                      other_cam.farClipPlane = far_clip_plane;
                    }
                  }
                }
              }
            }
          }

          if (this._sync_culling_mask) {
            var culling_mask = this_camera.cullingMask;
            if (this._old_culling_mask != culling_mask) {
              this._old_culling_mask = culling_mask;
              foreach (var cam in this._cameras) {
                if (cam != this._camera) {
                  if (cam.SyncCullingMask) {
                    var other_cam = cam.GetComponent<Camera>();
                    if (other_cam) {
                      other_cam.cullingMask = culling_mask;
                    }
                  }
                }
              }
            }
          }

          if (this._sync_orthographic_projection) {
            var orthographic = this_camera.orthographic;
            if (this._old_orthographic_projection != orthographic) {
              this._old_orthographic_projection = orthographic;
              foreach (var cam in this._cameras) {
                if (cam != this._camera) {
                  if (cam.SyncOrthographicProjection) {
                    var other_cam = cam.GetComponent<Camera>();
                    if (other_cam) {
                      other_cam.orthographic = orthographic;
                    }
                  }
                }
              }
            }
          }

          if (this._sync_fov) {
            var fov = this_camera.fieldOfView;
            if (Math.Abs(this._old_fov - fov) > _tolerance) {
              this._old_fov = fov;
              foreach (var cam in this._cameras) {
                if (cam != this._camera) {
                  if (cam.SyncFov) {
                    var other_cam = cam.GetComponent<Camera>();
                    if (other_cam) {
                      other_cam.fieldOfView = fov;
                    }
                  }
                }
              }
            }
          }
        } else {
          Debug.Log("No Camera component found on GameObject");
        }
      } else {
        Debug.Log("No SyncCameraProperties component found on GameObject");
      }
    }

    /// <summary>
    /// </summary>
    public void Update() {
      if (!this._only_run_on_awake) {
        if (this._only_run_in_edit_mode) {
          #if UNITY_EDITOR
          if (!Application.isPlaying) {
            this.Sync_Cameras();
          }
          #endif
        } else {
          this.Sync_Cameras();
        }
      }
    }
  }
}